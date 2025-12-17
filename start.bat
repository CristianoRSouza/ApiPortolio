@echo off
echo 🚀 Iniciando ambiente de PRODUÇÃO...
echo 📦 Subindo PostgreSQL + API completa...

docker-compose up --build

echo ✅ Aplicação completa rodando!
echo 🌐 API: http://localhost:8080/swagger
echo 📊 PostgreSQL: localhost:5432
pause
