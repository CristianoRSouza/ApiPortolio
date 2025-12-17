-- BANCO COMPLETO BASEADO EM TODAS AS ENTIDADES

-- Tabela Roles
CREATE TABLE IF NOT EXISTS "Roles" (
    "Id" SERIAL PRIMARY KEY,
    "Name" VARCHAR(50) NOT NULL,
    "Description" VARCHAR(255),
    "IsActive" BOOLEAN DEFAULT true,
    "CreatedAt" TIMESTAMPTZ NOT NULL DEFAULT NOW()
);

-- Tabela Adresses
CREATE TABLE IF NOT EXISTS "Adresses" (
    "Id" SERIAL PRIMARY KEY,
    "City" TEXT NOT NULL,
    "Neighborhood" TEXT NOT NULL,
    "Street" TEXT NOT NULL,
    "PostalCode" TEXT NOT NULL,
    "Country" TEXT NOT NULL,
    "Phone" TEXT NOT NULL
);

-- Tabela Championships
CREATE TABLE IF NOT EXISTS "Championships" (
    "Id" SERIAL PRIMARY KEY,
    "Name" TEXT NOT NULL,
    "Country" TEXT NOT NULL,
    "Logo" TEXT,
    "IsActive" BOOLEAN DEFAULT true,
    "CreatedAt" TIMESTAMPTZ NOT NULL DEFAULT NOW()
);

-- Tabela Teams
CREATE TABLE IF NOT EXISTS "Teams" (
    "Id" SERIAL PRIMARY KEY,
    "Name" TEXT NOT NULL,
    "Logo" TEXT,
    "Country" TEXT NOT NULL,
    "CreatedAt" TIMESTAMPTZ NOT NULL DEFAULT NOW()
);

-- Tabela Users (COM AdressId devido à relação em Adress)
CREATE TABLE IF NOT EXISTS "Users" (
    "Id" SERIAL PRIMARY KEY,
    "Email" VARCHAR(255) NOT NULL UNIQUE,
    "Nickname" VARCHAR(255) NOT NULL,
    "FullName" VARCHAR(100),
    "Phone" VARCHAR(20),
    "Cpf" VARCHAR(14),
    "PasswordHash" VARCHAR(255) NOT NULL,
    "Balance" DECIMAL(10,2) DEFAULT 0.00,
    "IsVerified" BOOLEAN DEFAULT false,
    "IsActive" BOOLEAN DEFAULT true,
    "CreatedAt" TIMESTAMPTZ NOT NULL DEFAULT NOW(),
    "LastLogin" TIMESTAMPTZ,
    "UpdatedAt" TIMESTAMPTZ NOT NULL DEFAULT NOW(),
    "AdressId" INTEGER REFERENCES "Adresses"("Id")
);

-- Tabela Matches
CREATE TABLE IF NOT EXISTS "Matches" (
    "Id" SERIAL PRIMARY KEY,
    "Team1Id" INTEGER NOT NULL REFERENCES "Teams"("Id"),
    "Team2Id" INTEGER NOT NULL REFERENCES "Teams"("Id"),
    "ChampionshipId" INTEGER NOT NULL REFERENCES "Championships"("Id"),
    "MatchDate" TIMESTAMPTZ NOT NULL,
    "Status" TEXT NOT NULL,
    "Team1Score" INTEGER,
    "Team2Score" INTEGER,
    "Team1Odd" DECIMAL(5,2) NOT NULL,
    "DrawOdd" DECIMAL(5,2) NOT NULL,
    "Team2Odd" DECIMAL(5,2) NOT NULL,
    "CreatedAt" TIMESTAMPTZ NOT NULL DEFAULT NOW(),
    "UpdatedAt" TIMESTAMPTZ NOT NULL DEFAULT NOW()
);

-- Tabela Odds
CREATE TABLE IF NOT EXISTS "Odds" (
    "Id" SERIAL PRIMARY KEY,
    "MatchId" INTEGER NOT NULL REFERENCES "Matches"("Id"),
    "BetType" TEXT NOT NULL,
    "Selection" TEXT NOT NULL,
    "Value" DECIMAL(5,2) NOT NULL,
    "IsActive" BOOLEAN DEFAULT true,
    "CreatedAt" TIMESTAMPTZ NOT NULL DEFAULT NOW(),
    "UpdatedAt" TIMESTAMPTZ NOT NULL DEFAULT NOW()
);

-- Tabela Bets
CREATE TABLE IF NOT EXISTS "Bets" (
    "Id" SERIAL PRIMARY KEY,
    "TicketId" TEXT NOT NULL,
    "UserId" INTEGER NOT NULL REFERENCES "Users"("Id"),
    "MatchId" INTEGER NOT NULL REFERENCES "Matches"("Id"),
    "BetType" TEXT NOT NULL,
    "Selection" TEXT NOT NULL,
    "BetAmount" DECIMAL(10,2) NOT NULL,
    "SelectedOdd" DECIMAL(5,2) NOT NULL,
    "ResultAmount" DECIMAL(10,2) NOT NULL,
    "Status" TEXT NOT NULL,
    "CreatedAt" TIMESTAMPTZ NOT NULL DEFAULT NOW(),
    "UpdatedAt" TIMESTAMPTZ NOT NULL DEFAULT NOW()
);

-- Tabela Transactions
CREATE TABLE IF NOT EXISTS "Transactions" (
    "Id" SERIAL PRIMARY KEY,
    "UserId" INTEGER NOT NULL REFERENCES "Users"("Id"),
    "Type" TEXT NOT NULL,
    "Amount" DECIMAL(10,2) NOT NULL,
    "Status" TEXT NOT NULL,
    "Description" TEXT,
    "PixKey" TEXT,
    "QrCode" TEXT,
    "ExpiresAt" TIMESTAMPTZ,
    "CreatedAt" TIMESTAMPTZ NOT NULL DEFAULT NOW(),
    "UpdatedAt" TIMESTAMPTZ NOT NULL DEFAULT NOW()
);

-- Tabela Notifications
CREATE TABLE IF NOT EXISTS "Notifications" (
    "Id" SERIAL PRIMARY KEY,
    "UserId" INTEGER NOT NULL REFERENCES "Users"("Id"),
    "Type" TEXT NOT NULL,
    "Title" TEXT NOT NULL,
    "Message" TEXT NOT NULL,
    "IsRead" BOOLEAN DEFAULT false,
    "CreatedAt" TIMESTAMPTZ NOT NULL DEFAULT NOW()
);

-- Tabela UserRoles (COM AssignedAt)
CREATE TABLE IF NOT EXISTS "UserRoles" (
    "Id" SERIAL PRIMARY KEY,
    "UserId" INTEGER NOT NULL REFERENCES "Users"("Id"),
    "RoleId" INTEGER NOT NULL REFERENCES "Roles"("Id"),
    "AssignedAt" TIMESTAMPTZ NOT NULL DEFAULT NOW(),
    "AssignedBy" INTEGER REFERENCES "Users"("Id")
);

-- Tabela MatchStats
CREATE TABLE IF NOT EXISTS "MatchStats" (
    "Id" SERIAL PRIMARY KEY,
    "MatchId" INTEGER NOT NULL REFERENCES "Matches"("Id"),
    "Corners" INTEGER NOT NULL DEFAULT 0,
    "Fouls" INTEGER NOT NULL DEFAULT 0,
    "YellowCards" INTEGER NOT NULL DEFAULT 0,
    "RedCards" INTEGER NOT NULL DEFAULT 0,
    "Offsides" INTEGER NOT NULL DEFAULT 0,
    UNIQUE("MatchId")
);

-- Inserir dados de teste
INSERT INTO "Roles" ("Name", "Description") VALUES 
('Admin', 'Administrador do sistema'),
('User', 'Usuário comum'),
('Manager', 'Gerente')
ON CONFLICT DO NOTHING;

-- Inserir usuário de teste
INSERT INTO "Users" ("Email", "Nickname", "FullName", "PasswordHash", "Balance", "IsActive", "IsVerified") VALUES 
('admin@soccerbet.com', 'admin', 'Administrador', '$2a$10$92IXUNpkjO0rOQ5byMi.Ye4oKoEa3Ro9llC/.og/at2.uheWG/igi', 1000.00, true, true),
('user@test.com', 'testuser', 'Usuário Teste', '$2a$10$92IXUNpkjO0rOQ5byMi.Ye4oKoEa3Ro9llC/.og/at2.uheWG/igi', 500.00, true, true)
ON CONFLICT DO NOTHING;

-- Associar usuários às roles
INSERT INTO "UserRoles" ("UserId", "RoleId") VALUES 
(1, 1), -- admin como Admin
(2, 2)  -- testuser como User
ON CONFLICT DO NOTHING;
