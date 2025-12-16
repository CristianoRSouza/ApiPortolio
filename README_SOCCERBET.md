# 🚀 SoccerBet API - ASP.NET Core

## 📋 Funcionalidades Implementadas

### 🔐 **Autenticação**
- `POST /api/auth/login` - Login com email/senha
- `POST /api/auth/register` - Registro de novo usuário

### 👤 **Usuário**
- `GET /api/usuario/perfil` - Obter dados do perfil
- `PUT /api/usuario/perfil` - Atualizar dados pessoais
- `POST /api/usuario/alterar-senha` - Alterar senha
- `GET /api/usuario/estatisticas` - Estatísticas de apostas

### 💰 **Transações**
- `GET /api/transacao` - Listar transações do usuário
- `POST /api/transacao/deposito` - Criar depósito PIX
- `POST /api/transacao/confirmar-deposito/{id}` - Confirmar pagamento PIX
- `POST /api/transacao/saque` - Solicitar saque PIX

### 🎲 **Apostas**
- `GET /api/aposta` - Listar apostas do usuário
- `POST /api/aposta` - Criar nova aposta

## 🗄️ **Entidades do Banco**

### Usuario
- Id, Email, SenhaHash, Apelido, NomeCompleto
- Telefone, CPF, Saldo, Verificado, Ativo
- CriadoEm, AtualizadoEm, UltimoLogin

### Transacao
- Id, UsuarioId, Tipo, Valor, SaldoAnterior, SaldoPosterior
- Status, Descricao, MetodoPagamento, ChavePix
- CriadoEm, AtualizadoEm

### Aposta
- Id, BilheteId, UsuarioId, PartidaId, TipoAposta
- Selecao, Quantidade, ValorOdd, ValorAposta
- GanhoPotencial, Status, ValorResultado
- CriadoEm, LiquidadaEm

### Partida
- Id, CampeonatoId, Time1Id, Time2Id, DataPartida
- Status, Placares, Estatísticas (escanteios, cartões, etc.)

### Campeonato
- Id, Nome, Slug, Descricao, Pais, Temporada

### Time
- Id, Nome, NomeCurto, LogoUrl, Pais

### Notificacao
- Id, UsuarioId, Tipo, Titulo, Mensagem, Lida

## 🔧 **Como usar**

### 1. **Configurar Banco PostgreSQL**
```bash
# No diretório SoccerBet
docker-compose up -d
```

### 2. **Executar Migrations**
```bash
cd GordoMentoria/ApiEntregasMentoria
dotnet ef database update
```

### 3. **Executar API**
```bash
dotnet run
```

### 4. **Acessar Swagger**
```
https://localhost:7000/swagger
```

## 📱 **Integração com React Native**

### Configuração Base URL
```typescript
const API_BASE_URL = 'https://localhost:7000/api';
```

### Exemplo de Login
```typescript
const login = async (email: string, password: string) => {
  const response = await fetch(`${API_BASE_URL}/auth/login`, {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json',
    },
    body: JSON.stringify({ email, password }),
  });
  
  const data = await response.json();
  return data; // { token, usuario }
};
```

### Exemplo de Depósito
```typescript
const criarDeposito = async (valor: number, token: string) => {
  const response = await fetch(`${API_BASE_URL}/transacao/deposito`, {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json',
      'Authorization': `Bearer ${token}`,
    },
    body: JSON.stringify({ valor, metodoPagamento: 'pix' }),
  });
  
  const data = await response.json();
  return data; // { transacaoId, codigoPix, qrCodeUrl, valor, expiresAt }
};
```

## 🔑 **Autenticação JWT**

Todas as rotas (exceto login/register) requerem token JWT no header:
```
Authorization: Bearer {seu-token-jwt}
```

## 📊 **Status Codes**

- `200` - Sucesso
- `400` - Erro de validação
- `401` - Não autorizado
- `404` - Não encontrado
- `500` - Erro interno

## 🎯 **Próximos Passos**

1. **Executar a API**: `dotnet run`
2. **Testar no Swagger**: Criar usuário, fazer login, testar endpoints
3. **Integrar com React Native**: Substituir dados mockados por chamadas reais
4. **Implementar notificações push**
5. **Adicionar validação de documentos (KYC)**

A API está pronta para integração completa com o app SoccerBet! 🚀