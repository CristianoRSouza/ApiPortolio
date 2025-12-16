@echo off
echo 🚀 Iniciando ambiente local SoccerBet...

echo 📦 Subindo PostgreSQL com Docker...
docker-compose -f docker-compose.local.yml up -d

echo ⏳ Aguardando PostgreSQL inicializar...
timeout /t 10

echo ✅ PostgreSQL rodando em localhost:5432
echo 📊 Banco: soccerbet
echo 👤 Usuário: postgres  
echo 🔑 Senha: 123456

echo.
echo 🎯 Agora você pode:
echo   1. Rodar a aplicação no Visual Studio (F5)
echo   2. Acessar: https://localhost:7207/swagger
echo   3. Testar os endpoints

echo.
echo 🛑 Para parar o PostgreSQL: docker-compose -f docker-compose.local.yml down
pause
