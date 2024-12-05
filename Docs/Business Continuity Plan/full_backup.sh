#!/bin/bash

# Definir variáveis
DATE=$(date +%Y-%m-%d_%H-%M-%S)
BACKUP_DIR="/root/backups/FullBackups/$DATE"  # Novo diretório para armazenar os backups com data
SQL_SERVER="vs1314.dei.isep.ipp.pt"  # Endereço do servidor SQL
SQL_USER="sa"
SQL_PASS="giIl5CHJSA==Xa5"
SQL_DB="BackendDB"  # Nome do banco de dados
LAST_BACKUP_FILE="/root/backups/last_backup.txt"  # Arquivo para armazenar a data do último backup

# Criar o diretório de backup com data no nome
mkdir -p "$BACKUP_DIR"

# Criar ficheiro database_scheme
bash ./generate_datascheme.sh "$BACKUP_DIR"

# Obter a data do último backup
if [ -f "$LAST_BACKUP_FILE" ]; then
  LAST_BACKUP_DATE=$(cat "$LAST_BACKUP_FILE")
else
  # Se o arquivo não existir, definir uma data antiga
  LAST_BACKUP_DATE="2000-01-01"
fi

# Obter todas as tabelas do banco de dados
TABLES=$(sqlcmd -S $SQL_SERVER -U $SQL_USER -P $SQL_PASS -d $SQL_DB -Q "SELECT name FROM sys.tables" -h -1 -W)


# Loop para exportar todas as tabelas
for TABLE in $TABLES
do
  # Pular qualquer linha vazia (como título ou separador)
  if [ -z "$TABLE" ] || [[ "$TABLE" == *"__"* ]]; then
    continue
  fi

  # Definir o nome do arquivo de backup dentro da pasta com a data
  BACKUP_FILE="$BACKUP_DIR/${TABLE}_$DATE.bak"

  # Realizar o backup da tabela
  echo "Fazendo backup da tabela $TABLE..."
  bcp "$SQL_DB.dbo.$TABLE" out "$BACKUP_FILE" -S $SQL_SERVER -U $SQL_USER -P $SQL_PASS -c
  
  # Verificar se o backup foi bem-sucedido
  if [ $? -eq 0 ]; then
    echo "Backup da tabela $TABLE realizado com sucesso em $DATE"
  else
    echo "Erro ao realizar backup da tabela $TABLE em $DATE"
  fi
done

# Atualizar a data do último backup
echo "$DATE" > "$LAST_BACKUP_FILE"