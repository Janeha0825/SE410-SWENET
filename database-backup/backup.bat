net use Z: \\pintsize\home\misc\swenet-backup OJ32Simpson /user:se\swenet-backup
C:\"Program Files"\"Microsoft SQL Server"\MSSQL\Binn\sqlmaint -D swenet -BkUpDB C:\database-backup -BkUpMedia DISK -DelBkUps 30minutes
C:\"Program Files"\"Microsoft SQL Server"\MSSQL\Binn\sqlmaint -D UserAccounts -BkUpDB C:\database-backup -BkUpMedia DISK -DelBkUps 30minutes
C:\"Program Files"\"Microsoft SQL Server"\MSSQL\Binn\sqlmaint -D AspNetForums -BkUpDB C:\database-backup -BkUpMedia DISK -DelBkUps 30minutes
C:\"Documents and Settings"\Administrator\Desktop\SwenetBackup\SwenetBackup.exe
net use Z: /D