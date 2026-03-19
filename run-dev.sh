#!/usr/bin/env bash

set -euo pipefail

ROOT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
cd "$ROOT_DIR"

CONTAINER_NAME="${SQL_CONTAINER_NAME:-identity-sql}"
SQL_IMAGE="${SQL_IMAGE:-mcr.microsoft.com/mssql/server:2022-latest}"
SQL_HOST_PORT="${SQL_HOST_PORT:-11433}"
SQL_CONTAINER_PORT="1433"
DB_NAME="${DB_NAME:-identitydb}"
SQL_MAX_WAIT_SECONDS="${SQL_MAX_WAIT_SECONDS:-180}"
APP_LAUNCH_PROFILE="${APP_LAUNCH_PROFILE:-https}"

if ! command -v docker >/dev/null 2>&1; then
    echo "Error: docker command not found."
    exit 1
fi

if ! command -v dotnet >/dev/null 2>&1; then
    echo "Error: dotnet command not found."
    exit 1
fi

if [[ -z "${DB_PASSWORD:-}" ]]; then
    read -r -s -p "Enter DB_PASSWORD (sa password): " DB_PASSWORD
    echo
fi

if [[ -z "${DB_PASSWORD:-}" ]]; then
    echo "Error: DB_PASSWORD cannot be empty."
    exit 1
fi

if docker context ls --format '{{.Name}}' | grep -qx 'default'; then
    docker context use default >/dev/null
fi

if docker ps -a --format '{{.Names}}' | grep -qx "$CONTAINER_NAME"; then
    if ! docker ps --format '{{.Names}}' | grep -qx "$CONTAINER_NAME"; then
        echo "Starting existing SQL container: $CONTAINER_NAME"
        docker start "$CONTAINER_NAME" >/dev/null
    fi
else
    echo "Creating SQL container: $CONTAINER_NAME"
    docker run \
        -e ACCEPT_EULA=Y \
        -e SA_PASSWORD="$DB_PASSWORD" \
        -p "${SQL_HOST_PORT}:${SQL_CONTAINER_PORT}" \
        --name "$CONTAINER_NAME" \
        -d "$SQL_IMAGE" >/dev/null
fi

PORT_BINDING="$(docker port "$CONTAINER_NAME" ${SQL_CONTAINER_PORT}/tcp 2>/dev/null | head -n 1 || true)"
if [[ -z "$PORT_BINDING" ]]; then
    echo "Error: could not detect host port mapping for $CONTAINER_NAME."
    exit 1
fi

SQL_HOST_PORT="${PORT_BINDING##*:}"
echo "SQL container is reachable on localhost:${SQL_HOST_PORT}"

echo "Waiting for SQL Server readiness..."
elapsed=0
while ! docker logs "$CONTAINER_NAME" 2>&1 | grep -q "SQL Server is now ready for client connections"; do
    sleep 3
    elapsed=$((elapsed + 3))
    if (( elapsed >= SQL_MAX_WAIT_SECONDS )); then
        echo "Error: SQL Server was not ready in ${SQL_MAX_WAIT_SECONDS}s."
        docker logs "$CONTAINER_NAME" --tail 80 || true
        exit 1
    fi
done

if command -v dotnet-ef >/dev/null 2>&1; then
    EF_CMD="dotnet-ef"
elif [[ -x "$HOME/.dotnet/tools/dotnet-ef" ]]; then
    EF_CMD="$HOME/.dotnet/tools/dotnet-ef"
    export DOTNET_ROOT="${DOTNET_ROOT:-/usr/share/dotnet}"
else
    echo "Installing dotnet-ef tool (8.0.11)..."
    dotnet tool install --global dotnet-ef --version 8.0.11 >/dev/null
    EF_CMD="$HOME/.dotnet/tools/dotnet-ef"
    export DOTNET_ROOT="${DOTNET_ROOT:-/usr/share/dotnet}"
fi

export DB_PASSWORD
export ConnectionStrings__SqlVerbindung="Server=localhost,${SQL_HOST_PORT};Database=${DB_NAME};User Id=sa;Password={DB_PASSWORD};TrustServerCertificate=True;"

echo "Applying EF Core migrations..."
"$EF_CMD" database update \
    --project AspNetCoreIdentity.Repository/AspNetCoreIdentity.Repository.csproj \
    --startup-project AspNetCoreIdentity.Web/AspNetCoreIdentity.Web.csproj

echo "Starting web app..."
if [[ "$APP_LAUNCH_PROFILE" == "https" ]]; then
    echo "Open: https://localhost:7296 (or http://localhost:5282)"
else
    echo "Open: http://localhost:5282"
fi

dotnet run \
    --project AspNetCoreIdentity.Web/AspNetCoreIdentity.Web.csproj \
    --launch-profile "$APP_LAUNCH_PROFILE"
