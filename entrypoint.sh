#!/bin/bash
set -e

echo "🔵 Waiting for database to be ready..."

# Extract database connection info from DATABASE_URL
DB_HOST=$(echo $DATABASE_URL | sed -n 's/.*@\([^:]*\):.*/\1/p')
DB_PORT=$(echo $DATABASE_URL | sed -n 's/.*:\([0-9]*\)\/.*/\1/p')
DB_USER=$(echo $DATABASE_URL | sed -n 's/.*\/\/\([^:]*\):.*/\1/p')

echo "🔍 Connecting to: $DB_HOST:$DB_PORT as $DB_USER"

# Wait for database to be ready
until pg_isready -h $DB_HOST -p $DB_PORT -U $DB_USER; do
  echo "⏳ Database is unavailable - sleeping"
  sleep 2
done

echo "✅ Database is ready!"

# Execute SQL script to create tables
if [ -f "/app/init-db.sql" ]; then
    echo "🔧 Creating database tables..."
    psql $DATABASE_URL -f /app/init-db.sql
    echo "✅ Tables created successfully!"
else
    echo "⚠️ No init-db.sql found, skipping table creation"
fi

echo "🚀 Starting SoccerBet API..."
exec dotnet ApiEntregasMentoria.dll
