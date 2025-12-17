@echo off
echo 🔧 Iniciando ambiente de DESENVOLVIMENTO...
echo 📦 Subindo apenas PostgreSQL...

docker-compose -f docker-compose.dev.yml up -d

echo ⏳ Aguardando PostgreSQL inicializar...
timeout /t 10

echo ✅ PostgreSQL rodando em localhost:5432
echo 🎯 Agora você pode:
echo   1. Rodar a aplicação no Visual Studio (F5)
echo   2. Acessar: https://localhost:7207/swagger
echo   3. Testar e debuggar normalmente

echo.
echo 🛑 Para parar o banco: docker-compose -f docker-compose.dev.yml down
pause
