#!/bin/bash

# Definir variáveis
DATE=$(date +%Y-%m-%d)
BACKUP_DIR="/root/backups/IncrementalBackups/$DATE"  # Diretório de backup com a data
SQL_SERVER="vs1314.dei.isep.ipp.pt"  # Endereço do servidor SQL
SQL_USER="sa"
SQL_PASS="giIl5CHJSA==Xa5"
SQL_DB="BackendDB"  # Nome do banco de dados que você quer fazer backup
LAST_BACKUP_FILE="/root/backups/last_backup.txt"  # Caminho para o arquivo de data do último backup

# Ler a última data de backup, se o arquivo existir
if [ -f "$LAST_BACKUP_FILE" ]; then
  LAST_BACKUP_DATE=$(cat "$LAST_BACKUP_FILE")
else
  # Se não existir, use uma data inicial muito antiga
  LAST_BACKUP_DATE="2000-01-01"
fi

# Criar o diretório de backup com a data (caso não exista)
mkdir -p "$BACKUP_DIR"

# Obter todas as tabelas do banco de dados
TABLES=$(sqlcmd -S $SQL_SERVER -U $SQL_USER -P $SQL_PASS -d $SQL_DB -Q "SET NOCOUNT ON; SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE = 'BASE TABLE'" -h -1)

# Loop para fazer backup de todas as tabelas com base na coluna last_modified
for TABLE in $TABLES
do
  echo "Fazendo backup da tabela $TABLE..."

  # Definir o caminho do arquivo de backup dentro da pasta com a data
  BACKUP_FILE="$BACKUP_DIR/${TABLE}_$DATE.bak"

  # Realiza o backup da tabela filtrando pelos dados modificados após o último backup
  sqlcmd -S $SQL_SERVER -U $SQL_USER -P $SQL_PASS -d $SQL_DB -Q "SET NOCOUNT ON; SELECT * FROM $SQL_DB.dbo.$TABLE WHERE last_modified > '$LAST_BACKUP_DATE'" | bcp "$SQL_DB.dbo.$TABL>
  
  # Verificar se o backup foi bem-sucedido
  if [ $? -eq 0 ]; then
    echo "Backup incremental da tabela $TABLE realizado com sucesso!"
  else
    echo "Erro ao realizar backup incremental da tabela $TABLE."
  fi
done

# Atualizar a data do último backup
echo "$DATE" > "$LAST_BACKUP_FILE"

