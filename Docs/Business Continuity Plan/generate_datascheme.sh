#!/bin/bash

# Definir variáveis
DATE=$(date +%Y-%m-%d)
DAY_MONTH_YEAR=$(date +%d-%m-%Y)

# Verificar se o argumento foi passado; caso contrário, usar o valor padrão
BACKUP_DIR="${1:-/root/backups/SchemaBackup_$DAY_MONTH_YEAR}"

SQL_SERVER="vs1314.dei.isep.ipp.pt"  # Endereço do servidor SQL
SQL_USER="sa"
SQL_PASS="giIl5CHJSA==Xa5"
SQL_DB="BackendDB"  # Nome do banco de dados

# Criar o diretório de backup com o formato desejado
mkdir -p "$BACKUP_DIR"

# Definir o nome do arquivo de backup do esquema
BACKUP_FILE="$BACKUP_DIR/schema_backup_$DATE.sql"

# Gerar o esquema de dados (estruturas de tabelas)
sqlcmd -S $SQL_SERVER -U $SQL_USER -P $SQL_PASS -d $SQL_DB -Q "
  SELECT 'CREATE TABLE [' + TABLE_SCHEMA + '].[' + TABLE_NAME + '] (' 
  + STRING_AGG('[' + COLUMN_NAME + '] ' + DATA_TYPE, ', ') 
  + ');' AS CreateTableStatements
  FROM INFORMATION_SCHEMA.COLUMNS
  GROUP BY TABLE_SCHEMA, TABLE_NAME
  ORDER BY TABLE_SCHEMA, TABLE_NAME
" -o "$BACKUP_FILE"

# Verificar se o backup foi bem-sucedido
if [ $? -eq 0 ]; then
  echo "Esquema de dados gerado com sucesso em $BACKUP_FILE"
else
  echo "Erro ao gerar esquema de dados"
fi
