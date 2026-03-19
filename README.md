# AspNetCoreIdentity.Web

## One-Command Start

Run the project with a single script:

```bash
chmod +x ./run-dev.sh
DB_PASSWORD='YOUR_SA_PASSWORD' ./run-dev.sh
```

Notes:

- The script starts (or creates) a SQL Server container named `identity-sql`.
- It applies EF Core migrations automatically.
- It then runs the web project with the `https` launch profile by default.
- If `identity-sql` is already mapped to a host port, the script uses that port.

Optional environment variables:

- `SQL_CONTAINER_NAME` (default: `identity-sql`)
- `SQL_HOST_PORT` (default: `11433`, used when container is created first time)
- `DB_NAME` (default: `identitydb`)
- `SQL_MAX_WAIT_SECONDS` (default: `180`)
- `APP_LAUNCH_PROFILE` (default: `https`, set `http` if needed)