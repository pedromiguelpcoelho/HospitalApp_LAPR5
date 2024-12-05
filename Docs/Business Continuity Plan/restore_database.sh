#!/bin/bash

# Definir variáveis
DATE=$(date +%Y-%m-%d_%H-%M-%S)
BACKUP_DIR="/root/backups/FullBackups/$DATE"  # Diretório onde os backups estão armazenados
SQL_SERVER="vs1314.dei.isep.ipp.pt"  # Endereço do servidor SQL
SQL_USER="sa"
SQL_PASS="giIl5CHJSA==Xa5"
SQL_DB="BackendDB"  # Nome do banco de dados
LAST_BACKUP_FILE="/root/backups/last_backup.txt"  # Caminho para o arquivo que armazena a data do último backup

# Função para restaurar o banco de dados
restore_database() {
    # Recebe o arquivo de backup como argumento
    BACKUP_FILE="$1"
    
    echo "Iniciando a restauração do banco de dados a partir do backup: $BACKUP_FILE"
    
    # Restaurar usando o BCP (Bulk Copy Program) para cada tabela
    TABLES=$(sqlcmd -S $SQL_SERVER -U $SQL_USER -P $SQL_PASS -d $SQL_DB -Q "SELECT name FROM sys.tables" -h -1 -W)
    
    for TABLE in $TABLES
    do
        # Pular qualquer linha vazia
        if [ -z "$TABLE" ]; then
            continue
        fi
        
        # Definir o nome do arquivo de backup para a tabela
        BACKUP_FILE="$BACKUP_DIR/${TABLE}_$DATE.bak"
        
        echo "Restaurando a tabela $TABLE..."

        # Comando BCP para restaurar a tabela
        bcp "$SQL_DB.dbo.$TABLE" in "$BACKUP_FILE" -S $SQL_SERVER -U $SQL_USER -P $SQL_PASS -c
        
        # Verificar se a restauração foi bem-sucedida
        if [ $? -eq 0 ]; then
            echo "Restauração da tabela $TABLE realizada com sucesso."
        else
            echo "Erro ao restaurar a tabela $TABLE."
        fi
    done
}

# Verificar se o diretório de backup existe
if [ ! -d "$BACKUP_DIR" ]; then
    echo "Erro: O diretório de backup não foi encontrado."
    exit 1
fi

# Verificar se o diretório contém backups comprimidos (tar.gz ou gz)
echo "Verificando se o backup está comprimido..."

# Descompactar arquivos .tar.gz, caso existam
for BACKUP_FILE in $BACKUP_DIR/*.tar.gz; do
    if [ -f "$BACKUP_FILE" ]; then
        echo "Descompactando arquivo $BACKUP_FILE..."
        tar -xzf "$BACKUP_FILE" -C "$BACKUP_DIR"
    fi
done

# Descompactar arquivos .gz, caso existam
for BACKUP_FILE in $BACKUP_DIR/*.gz; do
    if [ -f "$BACKUP_FILE" ]; then
        echo "Descompactando arquivo $BACKUP_FILE..."
        gunzip "$BACKUP_FILE"
    fi
done

# Iniciar a restauração das tabelas
echo "O diretório de backup existe. Tentando restaurar a partir dos backups completos ou incrementais."
restore_database "$BACKUP_DIR"

# Atualizar a data do último backup no arquivo
echo "$DATE" > "$LAST_BACKUP_FILE"
