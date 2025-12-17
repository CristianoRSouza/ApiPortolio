# 🚀 SoccerBet - Como Usar

## 🔧 Desenvolvimento (Visual Studio + PostgreSQL)

**Para desenvolver e debuggar:**

1. Execute: `start-dev.bat`
2. Aguarde PostgreSQL inicializar
3. Rode a aplicação no Visual Studio (F5)
4. Acesse: `https://localhost:7207/swagger`

**Vantagens:**
- ✅ Debug no Visual Studio
- ✅ Hot reload
- ✅ Breakpoints funcionam
- ✅ Banco PostgreSQL real

---

## 🐳 Produção (Docker Completo)

**Para testar ambiente completo:**

1. Execute: `start.bat`
2. Aguarde build e inicialização
3. Acesse: `http://localhost:8080/swagger`

**Vantagens:**
- ✅ Ambiente idêntico ao Railway
- ✅ Teste de deploy
- ✅ Containerização completa

---

## 👤 Usuários de Teste

```json
{
  "email": "admin@soccerbet.com",
  "password": "password"
}
```

```json
{
  "email": "user@test.com",
  "password": "password"
}
```

---

## 🛑 Para Parar

**Desenvolvimento:**
```bash
docker-compose -f docker-compose.dev.yml down
```

**Produção:**
```bash
docker-compose down
```
