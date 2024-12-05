#!/bin/sh

# Caminho para a pasta Docs
basePath="Docs"

echo "LOG: Generate PlantUML Diagrams"
exportFormat="svg"
extra="-SdefaultFontSize=20"

# Encontra todas as pastas 'puml' dentro da pasta 'Docs'
find "$basePath" -type d -name 'puml' | while read -r pumlDir; do
  
  # Define o diretório de saída como 'svg' no diretório pai
  parentDir=$(dirname "$pumlDir")
  outputDir="$parentDir/svg"

  # Cria a pasta de saída se não existir
  mkdir -p "$outputDir"

  # Processa todos os arquivos .puml na pasta atual
  for plantuml_file in "$pumlDir"/*.puml; do
    if [ -f "$plantuml_file" ]; then
      echo "Processing file: $plantuml_file"
      #echo "Output folder: $outputDir"

      # Gera o diagrama usando o PlantUML em um diretório temporário, suprimindo logs do Java
      tempDir=$(mktemp -d)
      java -jar libs/plantuml-1.2024.7.jar $extra -t$exportFormat -o "$tempDir" "$plantuml_file" >/dev/null 2>&1

      # Move os arquivos SVG do diretório temporário para o diretório de saída
      mv "$tempDir"/*.svg "$outputDir" 2>/dev/null

      # Remove o diretório temporário
      rmdir "$tempDir"
    fi
  done
done

echo "Finished"
