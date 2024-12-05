# Configuração do Serviço Kestrel e Nginx para Spot.NET SDK no Servidor Ubuntu

Este guia descreve como configurar um serviço Kestrel para rodar a aplicação .NET, gerenciar variáveis de ambiente, e configurar o Nginx para redirecionar requisições HTTP. 

## 1. Configuração do Serviço Kestrel

O serviço Kestrel é responsável por hospedar a aplicação .NET no servidor. 

### Passo 1: Criar o Arquivo de Configuração do Serviço

Crie o arquivo de configuração do serviço Kestrel:

```bash
sudo nano /etc/systemd/system/kestrel-spotnetsdk.service
```

### Passo 2: Adicionar o Seguinte Conteúdo ao Arquivo

Adicione o seguinte conteúdo ao arquivo de configuração do serviço Kestrel:

```ini
[Unit]
Description=Spot.NET SDK Web App running on Ubuntu Server

[Service]
WorkingDirectory=/var/www/spot_ubuntu
ExecStart=/usr/bin/dotnet /var/www/spot_ubuntu/BackEnd.dll
Restart=no
SyslogIdentifier=spot_ubuntu
Environment=ASPNETCORE_ENVIRONMENT=Production
Environment=DOTNET_PRINT_TELEMETRY_MESSAGE=false
Environment=AWS_ACCESS_KEY_ID=API_KEY
Environment=AWS_SECRET_ACCESS_KEY=API_KEY

[Install]
WantedBy=multi-user.target

```

### Passo 3: Atualizar e Iniciar o Serviço Kestrel
Após configurar o arquivo do serviço, execute os comandos abaixo para carregar e iniciar o serviço Kestrel.

```bash
# Recarregar os serviços
sudo systemctl daemon-reload

# Iniciar o serviço Kestrel
sudo systemctl start kestrel-spotnetsdk.service

# Configurar o serviço Kestrel para iniciar automaticamente no boot
sudo systemctl enable kestrel-spotnetsdk.service
```

## 2. Configuração do Nginx

O Nginx será usado como um servidor proxy reverso para redirecionar as requisições HTTP para o serviço Kestrel.

### Passo 1: Criar o Arquivo de Configuração do Nginx
Abra o arquivo de configuração do Nginx:

```bash
sudo nano /etc/nginx/sites-available/spotnetsdk
```

Adicione a configuração para redirecionar as requisições para o Kestrel rodando no localhost na porta 5000. Certifique-se de que o domínio ou IP do servidor está correto.

### Passo 2: Adicionar o Seguinte Conteúdo ao Arquivo

Adicione o seguinte conteúdo ao arquivo de configuração do Nginx:

```bash
server {
    listen 80;
    server_name vs598.dei.isep.ipp.pt; # Hostname ou IP do servidor

    # Define o local para onde o Nginx enviará as requisições
    location / {
        proxy_pass http://localhost:5002;  # Endereço onde o Kestrel está a ouvir
        proxy_http_version 1.1;
        proxy_set_header Upgrade $http_upgrade;
        proxy_set_header Connection keep-alive;
        proxy_set_header Host $host;
        proxy_cache_bypass $http_upgrade;
        proxy_set_header X-Forwarded-For $proxy_add_x_forwarded_for;
        proxy_set_header X-Forwarded-Proto $scheme;
    }

    # Opcional: Configuração para servir arquivos estáticos diretamente pelo Nginx
    location /static/ {
        alias /var/www/spot_ubuntu/wwwroot/static/;
        autoindex on;
    }

    # Configurações de erro personalizadas
    error_page 502 /502.html;
    location = /502.html {
        internal;
        root /usr/share/nginx/html;
    }
}

# Log de erro
error_log /var/log/nginx/spotnetsdk_error.log;
access_log /var/log/nginx/spotnetsdk_access.log;


```

### Passo 3: Ativar a Configuração do Nginx
Ative a configuração do site e reinicie o Nginx para aplicar as mudanças:

```bash

# Criar um link simbólico para sites-enabled
sudo ln -s /etc/nginx/sites-available/spotnetsdk /etc/nginx/sites-enabled/

# Reiniciar o Nginx
sudo systemctl restart nginx
```



### Passo 4: Recarregar os serviços:

```bash
# Recarregar os serviços
sudo systemctl daemon-reload

# Iniciar o Path Unit
sudo systemctl start kestrel-spotnetsdk.path

# Reiniciar o serviço Kestrel
sudo systemctl restart kestrel-spotnetsdk.service

# Status do serviço Kestrel
sudo systemctl status kestrel-spotnetsdk.service

# Verificar o status do Nginx
sudo systemctl status nginx

# Reiniciar o Nginx
sudo systemctl restart nginx

# Verificar o status do Nginx
sudo systemctl status nginx
```

Após seguir esses passos, o serviço Kestrel e o Nginx devem estar configurados corretamente para rodar a aplicação .NET no servidor Ubuntu.

## Deploy Workflow

```bash

name: Deploy to Self-Hosted Runner

on:
  push:
    paths:
      - 'Backend/**'
    branches:
      - main
      - development

  workflow_dispatch:

jobs:
  deploy:
    runs-on: self-hosted

    steps:
    - name: Set HOME environment variable
      run: echo "HOME=/root" >> $GITHUB_ENV
      
    - name: Check out code
      uses: actions/checkout@v3

    - name: Verify repository structure
      run: |
        echo "Listing the directory structure"
        ls $GITHUB_WORKSPACE
        ls $GITHUB_WORKSPACE/Backend

    - name: Build the backend
      run: |
        cd $GITHUB_WORKSPACE/Backend
        dotnet build || exit 1

    - name: Clean the /var/www/spot_ubuntu folder
      run: |
        sudo rm -rf /var/www/spot_ubuntu/*

    - name: Copy the Confidential folder to /var/www/spot_ubuntu
      run: |
        sudo cp -r /root/actions-runner/Confidential /var/www/spot_ubuntu

    - name: Copy the contents of bin/Debug/net8.0 to /var/www/spot_ubuntu
      run: |
        sudo cp -r /root/actions-runner/_work/SEM5_PI_GRUPO44/SEM5_PI_GRUPO44/Backend/bin/Debug/net8.0/* /var/www/spot_ubuntu/

    - name: Stop the kestrel-spotnetsdk service if running
      run: |
        if systemctl is-active --quiet kestrel-spotnetsdk.service; then
          echo "Stopping the kestrel-spotnetsdk service..."
          sudo systemctl stop kestrel-spotnetsdk.service
        else
          echo "kestrel-spotnetsdk service is not running, skipping stop."
        fi

    - name: Start the kestrel-spotnetsdk service
      run: |
        echo "Starting the kestrel-spotnetsdk service..."
        sudo systemctl start kestrel-spotnetsdk.service


```