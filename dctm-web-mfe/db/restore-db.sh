#!/bin/bash
# Restores the legacy DCTrack backup into the sqlserver container as database "dctrack".
# Idempotent: skips the restore when the database already exists.
set -euo pipefail

SQLCMD=/opt/mssql-tools18/bin/sqlcmd
BACKUP=/backups/dctrack_25Aug2019.bak

run_sql() { "$SQLCMD" -C -S sqlserver -U sa -P "$MSSQL_SA_PASSWORD" -b "$@"; }

echo "Waiting for SQL Server..."
for i in $(seq 1 30); do
  run_sql -Q "SELECT 1" >/dev/null 2>&1 && break
  sleep 2
done

if run_sql -h -1 -Q "SET NOCOUNT ON; SELECT COUNT(*) FROM sys.databases WHERE name='dctrack'" | tr -d '[:space:]' | grep -q '^1'; then
  echo "Database 'dctrack' already exists — skipping restore."
  exit 0
fi

if [ ! -f "$BACKUP" ]; then
  echo "ERROR: backup not found at $BACKUP" >&2
  exit 1
fi

echo "Restoring dctrack from $BACKUP ..."
# Logical file names inside the backup vary, so build MOVE clauses dynamically.
run_sql -Q "
DECLARE @moves NVARCHAR(MAX) = N'';
DECLARE @files TABLE (LogicalName NVARCHAR(128), PhysicalName NVARCHAR(260), Type CHAR(1),
    FileGroupName NVARCHAR(128), Size NUMERIC(20,0), MaxSize NUMERIC(20,0), FileID BIGINT,
    CreateLSN NUMERIC(25,0), DropLSN NUMERIC(25,0), UniqueID UNIQUEIDENTIFIER, ReadOnlyLSN NUMERIC(25,0),
    ReadWriteLSN NUMERIC(25,0), BackupSizeInBytes BIGINT, SourceBlockSize INT, FileGroupID INT,
    LogGroupGUID UNIQUEIDENTIFIER, DifferentialBaseLSN NUMERIC(25,0), DifferentialBaseGUID UNIQUEIDENTIFIER,
    IsReadOnly BIT, IsPresent BIT, TDEThumbprint VARBINARY(32), SnapshotURL NVARCHAR(360));
INSERT INTO @files EXEC('RESTORE FILELISTONLY FROM DISK = ''/backups/dctrack_25Aug2019.bak''');
SELECT @moves = @moves + N', MOVE N''' + LogicalName + N''' TO N''/var/opt/mssql/data/dctrack_' +
    CAST(FileID AS NVARCHAR(10)) + CASE WHEN Type = 'L' THEN N'.ldf''' ELSE N'.mdf''' END
FROM @files;
EXEC(N'RESTORE DATABASE [dctrack] FROM DISK = N''/backups/dctrack_25Aug2019.bak'' WITH RECOVERY' + @moves);
"
echo "Restore complete."
