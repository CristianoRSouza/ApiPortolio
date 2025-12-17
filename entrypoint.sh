#!/bin/bash
set -e

echo "🚀 Iniciando aplicação SoccerBet..."

# Aguardar PostgreSQL estar disponível
echo "⏳ Aguardando PostgreSQL..."
until pg_isready -h $(echo $DATABASE_URL | cut -d'@' -f2 | cut -d'/' -f1) -p 5432; do
  echo "PostgreSQL não está pronto - aguardando..."
  sleep 2
done

echo "✅ PostgreSQL está pronto!"

# Extrair dados da DATABASE_URL para executar script SQL
DB_HOST=$(echo $DATABASE_URL | sed 's/.*@\([^:]*\):.*/\1/')
DB_PORT=$(echo $DATABASE_URL | sed 's/.*:\([0-9]*\)\/.*/\1/')
DB_NAME=$(echo $DATABASE_URL | sed 's/.*\/\([^?]*\).*/\1/')
DB_USER=$(echo $DATABASE_URL | sed 's/.*\/\/\([^:]*\):.*/\1/')
DB_PASS=$(echo $DATABASE_URL | sed 's/.*:\/\/[^:]*:\([^@]*\)@.*/\1/')

echo "🗄️ Executando script de inicialização do banco..."
PGPASSWORD=$DB_PASS psql -h $DB_HOST -p $DB_PORT -U $DB_USER -d $DB_NAME -f init-db.sql || echo "⚠️ Script já executado ou erro (continuando...)"

echo "🎯 Iniciando aplicação .NET..."
exec dotnet ApiEntregasMentoria.dll
