-- Criar tabelas do banco de dados
CREATE TABLE IF NOT EXISTS "Roles" (
    "Id" SERIAL PRIMARY KEY,
    "Name" VARCHAR(50) NOT NULL,
    "Description" VARCHAR(255)
);

CREATE TABLE IF NOT EXISTS "Users" (
    "Id" SERIAL PRIMARY KEY,
    "Email" VARCHAR(255) NOT NULL UNIQUE,
    "Nickname" VARCHAR(255) NOT NULL,
    "FullName" VARCHAR(100),
    "PasswordHash" VARCHAR(255) NOT NULL,
    "Phone" VARCHAR(20),
    "Cpf" VARCHAR(14),
    "DateOfBirth" DATE,
    "Balance" DECIMAL(10,2) DEFAULT 0.00,
    "CreatedAt" TIMESTAMPTZ NOT NULL DEFAULT NOW(),
    "UpdatedAt" TIMESTAMPTZ NOT NULL DEFAULT NOW()
);

CREATE TABLE IF NOT EXISTS "UserRoles" (
    "Id" SERIAL PRIMARY KEY,
    "UserId" INTEGER NOT NULL REFERENCES "Users"("Id") ON DELETE CASCADE,
    "RoleId" INTEGER NOT NULL REFERENCES "Roles"("Id") ON DELETE CASCADE,
    "CreatedAt" TIMESTAMPTZ NOT NULL DEFAULT NOW()
);

-- Inserir dados de teste
INSERT INTO "Roles" ("Name", "Description") VALUES 
('Admin', 'Administrador do sistema'),
('User', 'Usuário comum'),
('Manager', 'Gerente')
ON CONFLICT DO NOTHING;

-- Inserir usuário de teste (senha: password)
INSERT INTO "Users" ("Email", "Nickname", "FullName", "PasswordHash", "Balance") VALUES 
('admin@soccerbet.com', 'admin', 'Administrador', '$2a$11$8K1p/a0dL2LkqvQDfuHoUOZShUy63TXz5B0qX5n2y4HvAQJbYdvTi', 1000.00),
('user@test.com', 'testuser', 'Usuário Teste', '$2a$11$8K1p/a0dL2LkqvQDfuHoUOZShUy63TXz5B0qX5n2y4HvAQJbYdvTi', 500.00)
ON CONFLICT DO NOTHING;

-- Associar usuários às roles
INSERT INTO "UserRoles" ("UserId", "RoleId") VALUES 
(1, 1), -- admin como Admin
(1, 3), -- admin como Manager  
(2, 2)  -- testuser como User
ON CONFLICT DO NOTHING;

PRINT 'Banco de dados inicializado com sucesso!';
