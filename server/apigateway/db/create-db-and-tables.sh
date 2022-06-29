sleep 30s

/opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P AdminAdmin123 -d master -i create-db-and-tables.sql