# DCTrack Mobile REST API — Administrator & Deployment Guide

Audience: system administrators and DevOps engineers deploying and operating
**DCTMRestAPI** (.NET 10 / ASP.NET Core).

Covers:

1. [Prerequisites](#1-prerequisites)
2. [Configuration reference](#2-configuration-reference)
3. [Building & publishing](#3-building--publishing)
4. [Deployment A — Windows Server + IIS](#4-deployment-a--windows-server--iis)
5. [Deployment B — Containers (on-prem & cloud)](#5-deployment-b--containers-on-prem--cloud)
6. [Database](#6-database)
7. [Security hardening](#7-security-hardening)
8. [Operations: logging, monitoring, backup](#8-operations-logging-monitoring-backup)
9. [Upgrades & rollback](#9-upgrades--rollback)
10. [Troubleshooting](#10-troubleshooting)

---

## 1. Prerequisites

### Common

| Requirement | Notes |
|---|---|
| SQL Server with the `DCTrack` database | 2016 or later; the API is database-first against ~87 existing tables and stored procedures. |
| A SQL login for the API | Least privilege — see §6.2. |
| TLS certificate | The API refuses plain HTTP outside `Development`. |
| JWT signing key | ≥ 32 bytes of random data. **The app fails to start without it.** |

### Windows / IIS host

| Requirement | Notes |
|---|---|
| Windows Server 2019 / 2022 / 2025 | |
| IIS with **ASP.NET Core Hosting Bundle for .NET 10** | Installs the runtime + `AspNetCoreModuleV2`. Install **after** IIS. |
| IIS role services | Web Server, Static Content, plus Windows Authentication only if you use it for the app-pool identity. |
| .NET 10 SDK | Only on the build machine, not the target server. |

### Container host

| Requirement | Notes |
|---|---|
| Docker / containerd, or Kubernetes 1.27+ | |
| Registry | ACR, ECR, GAR, Harbor, or any OCI registry. |
| Secret store | Environment variables at minimum; Key Vault / Secrets Manager / K8s Secrets preferred. |

---

## 2. Configuration reference

Sources are read in this order, later winning:
`appsettings.json` → `appsettings.{Environment}.json` → user secrets (Development only)
→ environment variables → command line.

Environment-variable form uses `__` (double underscore) for the `:` separator.

| Key | Env var | Required | Default | Purpose |
|---|---|---|---|---|
| `Jwt:SigningKey` | `Jwt__SigningKey` | **Yes** | — | HS256 signing key, **≥ 32 bytes**. Startup fails fast if missing or short. |
| `ConnectionStrings:DCTrackDatabase` | `ConnectionStrings__DCTrackDatabase` | **Yes** | — | SQL Server connection string. Plaintext (must contain `source`) or AES-encrypted (see §7.3). |
| `ASPNETCORE_ENVIRONMENT` | same | Yes in prod | `Production` | Anything other than `Development` enables HTTPS enforcement and disables the developer exception page. |
| `Security:UpgradePasswordHashesOnLogin` | `Security__UpgradePasswordHashesOnLogin` | No | `false` | Rehash legacy password hashes to PBKDF2 on successful login. **Keep off** until the main DCTrack application understands the new hash format — `tblUser` is shared. |
| `PageConfiguration:PageSize` | `PageConfiguration__PageSize` | No | `10` | Default page size for `/api/Assets/pages`. |
| `Serilog:*` | — | No | console + daily file | Logging. See §8.1. |
| `ASPNETCORE_URLS` | same | Container only | `http://+:8080` in the image | Kestrel bind addresses. Not used under IIS in-process hosting. |

### 2.1 Minimum production configuration

```powershell
# Windows / IIS — set on the app pool or as machine environment variables
setx /M Jwt__SigningKey "<32+ bytes of base64 random>"
setx /M ConnectionStrings__DCTrackDatabase "Server=sql01;Database=DCTrack;User ID=dctm_api;Password=<pwd>;Encrypt=True;TrustServerCertificate=False;"
setx /M ASPNETCORE_ENVIRONMENT "Production"
```

Generate a signing key:

```powershell
[Convert]::ToBase64String((1..48 | ForEach-Object { Get-Random -Max 256 }))
```

> **Never** put the real signing key or connection string in a committed
> `appsettings.json`. The shipped file contains `XXXXXX` placeholders — that is
> deliberate.

---

## 3. Building & publishing

On a machine with the .NET 10 SDK:

```powershell
# from the solution root
dotnet restore
dotnet build -c Release
dotnet test                      # optional but recommended

# publish the API
dotnet publish DCTMRestAPI\DCTMRestAPI.csproj -c Release `
  /p:CopyOutputSymbolsToPublishDirectory=false `
  -o .\Publish

# publish the connection-string tool alongside it (optional)
dotnet publish EncryptDecrypt\EncryptDecrypt.csproj -c Release `
  /p:CopyOutputSymbolsToPublishDirectory=false `
  -o .\Publish
```

The repository contains a `publish.bat` that does the same thing but with **hard-coded
absolute paths from a developer machine**. Use the commands above, or edit `publish.bat`
for your environment before running it.

The `Publish` folder contains `DCTMRestAPI.dll`, its dependencies, `web.config`,
`appsettings.json`, `wwwroot`, and `DCTMRestAPI.xml` (the XML doc file that populates
Swagger descriptions — keep it).

### 3.1 Release vs Debug Swagger path

The Swagger UI endpoint differs by build configuration:

| Build | Swagger JSON path |
|---|---|
| Debug | `/swagger/v1.2/swagger.json` |
| Release | `/dctmrest/swagger/v1.2/swagger.json` |

The Release path assumes the app is hosted as an **IIS application named `dctmrest`**
under a site. If you host it at a different path or at the site root, Swagger UI will
fail to load its document — either name the IIS application `dctmrest`, or change the
`SwaggerEndpoint` path in `Startup.cs` and rebuild.

---

## 4. Deployment A — Windows Server + IIS

### 4.1 Install the hosting bundle

1. Install/enable the IIS role first.
2. Download and install the **ASP.NET Core 10.0 Hosting Bundle** (`dotnet-hosting-10.0.x-win.exe`).
3. Restart IIS:
   ```powershell
   net stop was /y
   net start w3svc
   ```
4. Verify `AspNetCoreModuleV2` appears in IIS Manager → server node → **Modules**.

### 4.2 Update `web.config`

The `web.config` in the repository still references the **v1** module:

```xml
<add name="aspNetCore" path="*" verb="*" modules="AspNetCoreModule" resourceType="Unspecified"/>
```

`AspNetCoreModule` v1 is not installed by the .NET 10 hosting bundle. Replace the
published `web.config` with:

```xml
<?xml version="1.0" encoding="utf-8"?>
<configuration>
  <system.webServer>
    <handlers>
      <remove name="aspNetCore" />
      <add name="aspNetCore" path="*" verb="*" modules="AspNetCoreModuleV2" resourceType="Unspecified" />
    </handlers>
    <aspNetCore processPath="dotnet"
                arguments=".\DCTMRestAPI.dll"
                stdoutLogEnabled="false"
                stdoutLogFile=".\logs\stdout"
                hostingModel="inprocess">
      <environmentVariables>
        <environmentVariable name="ASPNETCORE_ENVIRONMENT" value="Production" />
      </environmentVariables>
    </aspNetCore>
    <security>
      <requestFiltering>
        <!-- log upload endpoint accepts device log files -->
        <requestLimits maxAllowedContentLength="52428800" />
      </requestFiltering>
    </security>
  </system.webServer>
</configuration>
```

> If you publish framework-dependent (the default), `processPath="dotnet"` is correct.
> For a self-contained publish use `processPath=".\DCTMRestAPI.exe"` and drop `arguments`.

### 4.3 Create the site and application pool

```powershell
Import-Module WebAdministration

# App pool: "No Managed Code" — ASP.NET Core runs outside the CLR pipeline
New-WebAppPool -Name 'DCTMRestAPI'
Set-ItemProperty IIS:\AppPools\DCTMRestAPI -Name managedRuntimeVersion -Value ''
Set-ItemProperty IIS:\AppPools\DCTMRestAPI -Name startMode              -Value 'AlwaysRunning'
Set-ItemProperty IIS:\AppPools\DCTMRestAPI -Name processModel.idleTimeout -Value '00:00:00'
Set-ItemProperty IIS:\AppPools\DCTMRestAPI -Name recycling.periodicRestart.time -Value '00:00:00'

# Deploy files
New-Item -ItemType Directory 'C:\inetpub\dctm\dctmrest' -Force
Copy-Item .\Publish\* 'C:\inetpub\dctm\dctmrest' -Recurse -Force

# Application named 'dctmrest' under the Default Web Site (matches the Release Swagger path)
New-WebApplication -Site 'Default Web Site' -Name 'dctmrest' `
  -PhysicalPath 'C:\inetpub\dctm\dctmrest' -ApplicationPool 'DCTMRestAPI'
```

`AlwaysRunning` + no idle timeout keeps first-request latency low; combine it with IIS
Application Initialization if you want warm-up on recycle.

### 4.4 Bind HTTPS

```powershell
# Import the certificate into LocalMachine\My first, then:
New-WebBinding -Name 'Default Web Site' -Protocol https -Port 443 -HostHeader 'dctm.contoso.com' -SslFlags 1
$cert = Get-ChildItem Cert:\LocalMachine\My | Where-Object Subject -like '*dctm.contoso.com*'
New-Item -Path "IIS:\SslBindings\!443!dctm.contoso.com" -Value $cert -SSLFlags 1
```

The API enforces HTTPS itself outside Development, so an HTTP binding is optional; if
you keep one, it will 301-redirect to HTTPS.

### 4.5 Configuration and secrets on IIS

Three options, in increasing order of safety:

1. **App-pool environment variables** — IIS Manager → Application Pools → *DCTMRestAPI*
   → Advanced Settings → *Environment Variables*. Scoped to the pool, not the whole machine.
2. **`web.config` `<environmentVariables>`** — as shown in §4.2. Convenient but the
   values sit in a file on disk; ACL the directory.
3. **Azure Key Vault / HashiCorp Vault** via a startup-injected environment variable —
   preferred for the JWT signing key.

Grant the app-pool identity (`IIS AppPool\DCTMRestAPI`) read access to the application
folder and **write** access to the `logs` subfolder:

```powershell
icacls 'C:\inetpub\dctm\dctmrest' /grant 'IIS AppPool\DCTMRestAPI:(OI)(CI)RX'
New-Item -ItemType Directory 'C:\inetpub\dctm\dctmrest\logs' -Force
icacls 'C:\inetpub\dctm\dctmrest\logs' /grant 'IIS AppPool\DCTMRestAPI:(OI)(CI)M'
```

### 4.6 Data Protection keys

ASP.NET Core Data Protection defaults to a per-machine key ring under the app-pool
profile. If the pool has no user profile, or you scale out to a web farm, keys are lost
on recycle. For multi-server deployments, persist the key ring to a shared UNC path or
SQL and protect it with a certificate:

```csharp
services.AddDataProtection()
        .PersistKeysToFileSystem(new DirectoryInfo(@"\\fileserver\dctm-keys"))
        .ProtectKeysWithCertificate(thumbprint)
        .SetApplicationName("DCTMRestAPI");
```

Also set `Load User Profile = True` on the app pool.

### 4.7 Reverse proxy / load balancer

The pipeline already calls `UseForwardedHeaders` for `X-Forwarded-For` and
`X-Forwarded-Proto`, so TLS termination at a load balancer or ARR front-end works —
`Request.IsHttps` and client IPs resolve correctly. Ensure the proxy actually sets both
headers, and restrict `KnownProxies`/`KnownNetworks` if the front-end is untrusted.

### 4.8 Verify

```powershell
Invoke-WebRequest https://dctm.contoso.com/dctmrest/api-docs -UseBasicParsing | Select StatusCode

$body = @{ UserName='Admin'; Password='<pwd>'; DeviceID='' } | ConvertTo-Json
$tok  = (Invoke-RestMethod -Method Post -Uri https://dctm.contoso.com/dctmrest/api/auth/token `
         -ContentType 'application/json' -Body $body).token
Invoke-RestMethod https://dctm.contoso.com/dctmrest/api/Country -Headers @{ Authorization = "Bearer $tok" }
```

---

## 5. Deployment B — Containers (on-prem & cloud)

### 5.1 Replace the shipped Dockerfile

`DCTMRestAPI/dockerfile` targets the retired `microsoft/dotnet` images and will not
build. Use this instead (place at the **solution root** as `Dockerfile`):

```dockerfile
# syntax=docker/dockerfile:1
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY DCTMRestAPI/DCTMRestAPI.csproj DCTMRestAPI/
RUN dotnet restore DCTMRestAPI/DCTMRestAPI.csproj
COPY DCTMRestAPI/ DCTMRestAPI/
RUN dotnet publish DCTMRestAPI/DCTMRestAPI.csproj -c Release -o /app --no-restore \
    /p:CopyOutputSymbolsToPublishDirectory=false

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
COPY --from=build /app .
ENV ASPNETCORE_URLS=http://+:8080 \
    ASPNETCORE_ENVIRONMENT=Production
EXPOSE 8080
USER app
ENTRYPOINT ["dotnet", "DCTMRestAPI.dll"]
```

Build and push:

```bash
docker build -t dctm-rest-api:1.2.0 .
docker tag  dctm-rest-api:1.2.0 registry.contoso.com/dctm/rest-api:1.2.0
docker push registry.contoso.com/dctm/rest-api:1.2.0
```

> **TLS in containers.** The container listens on plain HTTP on 8080. Because the app
> enforces HTTPS outside `Development`, you **must** terminate TLS in front of it
> (ingress, App Gateway, ALB, nginx) and have that proxy send
> `X-Forwarded-Proto: https`. Without the header every request is redirected in a loop.
> Alternatively run the container with `ASPNETCORE_URLS=https://+:8443` and a mounted
> PFX (`ASPNETCORE_Kestrel__Certificates__Default__Path` / `__Password`).

### 5.2 Run standalone (on-prem Docker)

```bash
docker run -d --name dctm-api -p 8080:8080 \
  -e ASPNETCORE_ENVIRONMENT=Production \
  -e Jwt__SigningKey="$JWT_KEY" \
  -e ConnectionStrings__DCTrackDatabase="Server=sql01;Database=DCTrack;User ID=dctm_api;Password=$SQL_PWD;Encrypt=True;TrustServerCertificate=False;" \
  -v /var/log/dctm:/app/logs \
  --restart unless-stopped \
  registry.contoso.com/dctm/rest-api:1.2.0
```

`docker-compose.yml` with an nginx TLS terminator:

```yaml
services:
  api:
    image: registry.contoso.com/dctm/rest-api:1.2.0
    environment:
      ASPNETCORE_ENVIRONMENT: Production
      Jwt__SigningKey: ${JWT_SIGNING_KEY:?set JWT_SIGNING_KEY}
      ConnectionStrings__DCTrackDatabase: ${DCTRACK_CONN:?set DCTRACK_CONN}
    volumes:
      - ./logs:/app/logs
    restart: unless-stopped

  proxy:
    image: nginx:alpine
    ports: ["443:443"]
    volumes:
      - ./nginx.conf:/etc/nginx/conf.d/default.conf:ro
      - ./certs:/etc/nginx/certs:ro
    depends_on: [api]
```

The nginx server block must forward the scheme:

```nginx
location / {
    proxy_pass         http://api:8080;
    proxy_set_header   Host              $host;
    proxy_set_header   X-Real-IP         $remote_addr;
    proxy_set_header   X-Forwarded-For   $proxy_add_x_forwarded_for;
    proxy_set_header   X-Forwarded-Proto $scheme;   # required
    client_max_body_size 50m;                        # device log uploads
}
```

### 5.3 Kubernetes (on-prem or cloud)

```yaml
apiVersion: v1
kind: Secret
metadata: { name: dctm-api-secrets }
type: Opaque
stringData:
  Jwt__SigningKey: "<32+ bytes>"
  ConnectionStrings__DCTrackDatabase: "Server=sql01;Database=DCTrack;User ID=dctm_api;Password=…;Encrypt=True;"
---
apiVersion: apps/v1
kind: Deployment
metadata: { name: dctm-api }
spec:
  replicas: 2
  selector: { matchLabels: { app: dctm-api } }
  template:
    metadata: { labels: { app: dctm-api } }
    spec:
      containers:
        - name: api
          image: registry.contoso.com/dctm/rest-api:1.2.0
          ports: [{ containerPort: 8080 }]
          env:
            - { name: ASPNETCORE_ENVIRONMENT, value: Production }
          envFrom:
            - secretRef: { name: dctm-api-secrets }
          resources:
            requests: { cpu: "100m", memory: "256Mi" }
            limits:   { cpu: "1",    memory: "1Gi"  }
          securityContext:
            runAsNonRoot: true
            allowPrivilegeEscalation: false
            readOnlyRootFilesystem: false   # Serilog writes ./logs
---
apiVersion: v1
kind: Service
metadata: { name: dctm-api }
spec:
  selector: { app: dctm-api }
  ports: [{ port: 80, targetPort: 8080 }]
---
apiVersion: networking.k8s.io/v1
kind: Ingress
metadata:
  name: dctm-api
  annotations:
    nginx.ingress.kubernetes.io/proxy-body-size: "50m"
spec:
  ingressClassName: nginx
  tls: [{ hosts: [dctm.contoso.com], secretName: dctm-tls }]
  rules:
    - host: dctm.contoso.com
      http:
        paths:
          - path: /
            pathType: Prefix
            backend: { service: { name: dctm-api, port: { number: 80 } } }
```

**Probes.** This version exposes **no dedicated health endpoint**. Until one is added,
use a TCP socket probe, or an HTTP probe against a cheap authenticated-free path:

```yaml
readinessProbe:
  tcpSocket: { port: 8080 }
  initialDelaySeconds: 10
livenessProbe:
  tcpSocket: { port: 8080 }
  periodSeconds: 30
```

Adding `app.MapHealthChecks("/healthz").AllowAnonymous()` with an EF Core DB check is a
small, worthwhile change for container deployments.

**Scaling.** The API is stateless apart from the Data Protection key ring and local log
files. Before scaling past one replica, either mount a shared volume for Data Protection
keys or configure a shared key store (§4.6), and ship logs to a collector instead of a
per-pod file (§8.1).

### 5.4 Cloud-managed container services

| Platform | Notes |
|---|---|
| **Azure App Service (Linux container)** | Set app settings for `Jwt__SigningKey` and the connection string (or use Key Vault references). App Service terminates TLS and sends `X-Forwarded-Proto` — no extra work. Enable *Always On*. |
| **Azure Container Apps** | Ingress `external: true`, target port `8080`; secrets via Container Apps secrets or Key Vault. Built-in TLS + forwarded headers. |
| **AWS ECS / Fargate** | Task definition with an ALB target group on 8080; secrets from Secrets Manager via `secrets:` valueFrom. ALB adds `X-Forwarded-Proto`. |
| **Google Cloud Run** | Container must listen on `$PORT` — set `ASPNETCORE_URLS=http://+:8080` and Cloud Run's default port 8080 matches. Secrets from Secret Manager. |
| **AKS / EKS / GKE / OpenShift** | Use the manifests in §5.3. |

In every case the SQL Server must be reachable from the container network — for cloud
deployments against an on-prem `DCTrack`, that means VPN/ExpressRoute/Direct Connect and
a firewall rule for TCP 1433.

---

## 6. Database

### 6.1 Restore (non-production environments)

```sql
RESTORE FILELISTONLY FROM DISK = N'D:\backups\dctrack.bak';

RESTORE DATABASE [DCTrack] FROM DISK = N'D:\backups\dctrack.bak'
WITH MOVE N'DCTrack'     TO N'D:\SQLData\DCTrack.mdf',
     MOVE N'DCTrack_log' TO N'D:\SQLData\DCTrack_log.ldf',
     RECOVERY, REPLACE;
```

### 6.2 Service account permissions

The API is database-first: it does **not** create or migrate schema. Grant only what it
needs.

```sql
CREATE LOGIN [dctm_api] WITH PASSWORD = N'<strong>';
USE [DCTrack];
CREATE USER [dctm_api] FOR LOGIN [dctm_api];
ALTER ROLE db_datareader ADD MEMBER [dctm_api];
ALTER ROLE db_datawriter ADD MEMBER [dctm_api];
GRANT EXECUTE TO [dctm_api];   -- the API calls DCTrack stored procedures
```

Do **not** grant `db_owner`. If you enable
`Security:UpgradePasswordHashesOnLogin`, the account also needs `UPDATE` on
`dbo.tblUser` (already covered by `db_datawriter`).

### 6.3 Notes for DBAs

- Reads use `AsNoTracking` and async EF Core queries.
- Several tables carry triggers (notably `tblUser`); the API writes to those with raw
  parameterized SQL because EF Core's `UPDATE … OUTPUT` is rejected on trigger tables.
  Do not "clean up" those triggers without testing the API.
- `GET /api/Assets` and the search endpoints call stored procedures that can return the
  full asset table — expect heavy reads if clients use them instead of `/pages`.
- Use `Encrypt=True` in the connection string; only set `TrustServerCertificate=True`
  when the SQL Server certificate is not chain-trusted, and prefer fixing the certificate.

---

## 7. Security hardening

### 7.1 Checklist

- [ ] `Jwt:SigningKey` is ≥ 32 bytes of cryptographic randomness, unique per environment,
      stored in a secret store — never in source control.
- [ ] Connection string held in a secret store; SQL account is least-privilege (§6.2).
- [ ] `ASPNETCORE_ENVIRONMENT=Production` (enables HTTPS enforcement, disables the
      developer exception page).
- [ ] TLS 1.2+ only at the edge; HSTS enabled.
- [ ] `logs/` directory is not web-servable and is ACL'd to the service identity.
- [ ] Swagger UI (`/api-docs`) restricted to internal networks in production if you do
      not want the surface published — it is served unconditionally by this build.
- [ ] `Security:UpgradePasswordHashesOnLogin` left `false` until the main DCTrack
      application understands PBKDF2 v2 hashes.
- [ ] Data Protection key ring persisted and protected for multi-instance deployments (§4.6).

### 7.2 Token characteristics to be aware of

- Issuer and audience validation are **disabled**; only the signature and lifetime are
  checked. Any token signed with the same key is accepted — so the key must not be
  shared across environments or applications.
- Lifetime is ~1 day with 5 minutes of clock skew. There is **no revocation list**;
  rotating `Jwt:SigningKey` invalidates all outstanding tokens immediately (and forces
  every client to re-authenticate).

### 7.3 Connection-string encryption tool (legacy)

`Cryptosettings.exe` (the `EncryptDecrypt` project) encrypts/decrypts the
`DCTrackDatabase` value inside `appsettings.json`. Run it from the folder containing
that file:

```powershell
Cryptosettings -pe ConnectionStrings -key DCTrackDatabase   # encrypt
Cryptosettings -pd ConnectionStrings -key DCTrackDatabase   # decrypt
```

At startup the API decrypts the value automatically when it does not look like
plaintext (the heuristic is whether it contains `source`).

> This scheme uses an **in-source symmetric key** and is being retired. Prefer a
> plaintext connection string held in a proper secret store. See
> [`security-key-migration-plan.md`](security-key-migration-plan.md).

---

## 8. Operations: logging, monitoring, backup

### 8.1 Logging

Serilog writes to the console and to daily rolling files at `./logs/log-<date>.log`
relative to the content root. Configure via the `Serilog` section:

```json
"Serilog": {
  "MinimumLevel": { "Default": "Information", "Override": { "Microsoft": "Warning" } },
  "WriteTo": [
    { "Name": "Console" },
    { "Name": "File", "Args": { "path": "./logs/log-.log", "rollingInterval": "Day", "retainedFileCountLimit": 31 } }
  ],
  "Enrich": [ "FromLogContext", "WithMachineName", "WithThreadId" ]
}
```

- **IIS:** ensure the app-pool identity has write access to `logs` (§4.5). Keep
  `stdoutLogEnabled="false"` in `web.config` except when diagnosing a startup crash —
  it grows without bound.
- **Containers:** the console sink is what your log driver collects. Either drop the
  File sink (recommended) or mount a volume at `/app/logs`; otherwise logs are lost when
  the container is replaced.
- For centralised logging, add a sink package (`Serilog.Sinks.Seq`,
  `Serilog.Sinks.Elasticsearch`, …) and a `WriteTo` entry — no code change needed, the
  configuration is read from `appsettings.json`.

Authentication failures are logged at `Critical` with the message
`"Token validation failed"` — a useful alert signal for credential-stuffing attempts.

### 8.2 Monitoring

Minimum signals to alert on:

| Signal | Why |
|---|---|
| Process/pod restarts | A missing `Jwt:SigningKey` causes an immediate startup crash loop. |
| HTTP 5xx rate | Usually SQL connectivity or stored-procedure failures. |
| HTTP 401/403 rate | Expired tokens (expected daily churn) vs. credential attacks. |
| p95 latency on `/api/Assets*` | Full-table exports; a spike usually means a client stopped paging. |
| SQL connection-pool exhaustion | Symptom of the above. |
| Disk free on the log volume | Rolling files have no retention limit unless configured. |

### 8.3 Backup and DR

- **Database** — the only stateful component. Use the DCTrack database's existing backup
  policy; the API adds no schema of its own.
- **Application** — stateless; redeploy from the artifact or image. Keep the published
  artifact / image tag for every release.
- **Configuration** — back up the secret store entries (signing key, connection string).
  Losing the signing key is not fatal: generate a new one; all clients simply
  re-authenticate.
- **Uploaded device logs** — retained wherever `LogUpload` writes them; include that
  path in the file-server backup if they are needed for audit.

---

## 9. Upgrades & rollback

**IIS**

1. Publish to a new folder (e.g. `…\dctmrest_v1.2.1`).
2. Drop an `app_offline.htm` into the live folder (IIS drains and stops the app).
3. Swap the physical path of the IIS application to the new folder, or copy files in.
4. Remove `app_offline.htm`.
5. Smoke-test (§4.8). To roll back, point the physical path at the previous folder.

Using two folders and repointing the application gives you an atomic switch and an
instant rollback.

**Containers**

```bash
kubectl set image deployment/dctm-api api=registry.contoso.com/dctm/rest-api:1.2.1
kubectl rollout status  deployment/dctm-api
kubectl rollout undo    deployment/dctm-api      # rollback
```

Because the API only reads an existing schema, application rollback is safe as long as
no DCTrack schema change accompanied the release.

---

## 10. Troubleshooting

| Symptom | Cause / fix |
|---|---|
| Site returns **HTTP 500.30 / 500.31** on IIS | The app failed to start. Temporarily set `stdoutLogEnabled="true"` in `web.config`, reproduce, then read `logs\stdout_*.log`. Most common cause: missing `Jwt__SigningKey`. |
| Startup log: *"JWT signing key is missing or too short"* | `Jwt:SigningKey` absent or < 32 bytes. This fail-fast is intentional — set it via app-pool env var, `web.config`, or container secret. |
| **HTTP 500.19** | `web.config` references `AspNetCoreModule` (v1) but only `AspNetCoreModuleV2` is installed. Apply §4.2. |
| **HTTP 502.5** | Hosting bundle missing or wrong `processPath`/`arguments`. Reinstall the .NET 10 Hosting Bundle and restart WAS. |
| Infinite redirect loop in a container | TLS terminated upstream but `X-Forwarded-Proto` is not being sent. Configure the proxy/ingress to set it. |
| Swagger UI loads but shows "Failed to load API definition" | Release build expects `/dctmrest/swagger/v1.2/swagger.json`. Name the IIS application `dctmrest`, or change the `SwaggerEndpoint` path in `Startup.cs`. |
| Every request → 401 after a deployment | The signing key changed (or differs between instances behind a load balancer). Make the key identical across replicas. |
| Intermittent 401s across replicas | Same as above — a per-instance generated key. Set `Jwt__SigningKey` explicitly everywhere. |
| SQL: *"Cannot open database"* / login failure | Connection string points at the wrong instance, or the login lacks a `DCTrack` user (§6.2). Check `Encrypt`/`TrustServerCertificate` if the error mentions a certificate chain. |
| Connection string looks like gibberish and the app starts anyway | It is AES-encrypted; the API decrypts it at startup (§7.3). Use `Cryptosettings -pd` to read it. |
| `POST /api/LogUpload` → 413 | Raise `maxAllowedContentLength` (IIS, §4.2) or `client_max_body_size` / `proxy-body-size` (nginx/ingress). |
| Logs directory empty | The service identity cannot write to `logs`. Grant Modify (§4.5) or mount a writable volume. |
| Slow responses / SQL CPU spike | A client is calling `GET /api/Assets` (full export) in a loop. Point it at `/api/Assets/pages` or `/api/Assets/search`. |

---

## 11. Related documents

- [User Guide](USER-GUIDE.md) — endpoint reference, authentication, client workflows.
- [Security key migration plan](security-key-migration-plan.md) — retiring the in-source
  encryption key and completing the password-hash migration.
- Project [README](../README.md) — developer setup, tests, solution layout.
