-- Criar tabelas do banco de dados
CREATE TABLE IF NOT EXISTS "Adresses" (
    "Id" SERIAL PRIMARY KEY,
    "City" TEXT NOT NULL,
    "Neighborhood" TEXT NOT NULL,
    "Street" TEXT NOT NULL,
    "PostalCode" TEXT NOT NULL,
    "Country" TEXT NOT NULL,
    "Phone" TEXT NOT NULL
);

CREATE TABLE IF NOT EXISTS "Championships" (
    "Id" SERIAL PRIMARY KEY,
    "Name" TEXT NOT NULL,
    "Country" TEXT NOT NULL,
    "Logo" TEXT,
    "IsActive" BOOLEAN NOT NULL,
    "CreatedAt" TIMESTAMPTZ NOT NULL
);

CREATE TABLE IF NOT EXISTS "Teams" (
    "Id" SERIAL PRIMARY KEY,
    "Name" TEXT NOT NULL,
    "Logo" TEXT,
    "Country" TEXT NOT NULL,
    "CreatedAt" TIMESTAMPTZ NOT NULL
);

CREATE TABLE IF NOT EXISTS "Users" (
    "Id" SERIAL PRIMARY KEY,
    "Email" TEXT NOT NULL,
    "Nickname" TEXT NOT NULL,
    "FullName" TEXT,
    "Phone" TEXT,
    "Cpf" TEXT,
    "PasswordHash" TEXT NOT NULL,
    "Balance" NUMERIC NOT NULL,
    "IsVerified" BOOLEAN NOT NULL,
    "IsActive" BOOLEAN NOT NULL,
    "CreatedAt" TIMESTAMPTZ NOT NULL,
    "LastLogin" TIMESTAMPTZ,
    "UpdatedAt" TIMESTAMPTZ NOT NULL,
    "AdressId" INTEGER REFERENCES "Adresses"("Id")
);

CREATE TABLE IF NOT EXISTS "Matches" (
    "Id" SERIAL PRIMARY KEY,
    "Team1Id" INTEGER NOT NULL REFERENCES "Teams"("Id"),
    "Team2Id" INTEGER NOT NULL REFERENCES "Teams"("Id"),
    "ChampionshipId" INTEGER NOT NULL REFERENCES "Championships"("Id"),
    "MatchDate" TIMESTAMPTZ NOT NULL,
    "Status" TEXT NOT NULL,
    "Team1Score" INTEGER,
    "Team2Score" INTEGER,
    "Team1Odd" NUMERIC NOT NULL,
    "DrawOdd" NUMERIC NOT NULL,
    "Team2Odd" NUMERIC NOT NULL,
    "CreatedAt" TIMESTAMPTZ NOT NULL,
    "UpdatedAt" TIMESTAMPTZ NOT NULL
);

CREATE TABLE IF NOT EXISTS "Notifications" (
    "Id" SERIAL PRIMARY KEY,
    "UserId" INTEGER NOT NULL REFERENCES "Users"("Id"),
    "Type" TEXT NOT NULL,
    "Title" TEXT NOT NULL,
    "Message" TEXT NOT NULL,
    "IsRead" BOOLEAN NOT NULL,
    "CreatedAt" TIMESTAMPTZ NOT NULL
);

CREATE TABLE IF NOT EXISTS "Transactions" (
    "Id" SERIAL PRIMARY KEY,
    "UserId" INTEGER NOT NULL REFERENCES "Users"("Id"),
    "Type" TEXT NOT NULL,
    "Amount" NUMERIC NOT NULL,
    "Status" TEXT NOT NULL,
    "Description" TEXT,
    "PixKey" TEXT,
    "QrCode" TEXT,
    "ExpiresAt" TIMESTAMPTZ,
    "CreatedAt" TIMESTAMPTZ NOT NULL,
    "UpdatedAt" TIMESTAMPTZ NOT NULL
);

CREATE TABLE IF NOT EXISTS "Bets" (
    "Id" SERIAL PRIMARY KEY,
    "TicketId" TEXT NOT NULL,
    "UserId" INTEGER NOT NULL REFERENCES "Users"("Id"),
    "MatchId" INTEGER NOT NULL REFERENCES "Matches"("Id"),
    "BetType" TEXT NOT NULL,
    "Selection" TEXT NOT NULL,
    "BetAmount" NUMERIC NOT NULL,
    "SelectedOdd" NUMERIC NOT NULL,
    "ResultAmount" NUMERIC NOT NULL,
    "Status" TEXT NOT NULL,
    "CreatedAt" TIMESTAMPTZ NOT NULL,
    "UpdatedAt" TIMESTAMPTZ NOT NULL
);

CREATE TABLE IF NOT EXISTS "MatchStats" (
    "Id" SERIAL PRIMARY KEY,
    "MatchId" INTEGER NOT NULL REFERENCES "Matches"("Id"),
    "Corners" INTEGER NOT NULL,
    "Fouls" INTEGER NOT NULL,
    "YellowCards" INTEGER NOT NULL,
    "RedCards" INTEGER NOT NULL,
    "Offsides" INTEGER NOT NULL
);

CREATE TABLE IF NOT EXISTS "Odds" (
    "Id" SERIAL PRIMARY KEY,
    "MatchId" INTEGER NOT NULL REFERENCES "Matches"("Id"),
    "BetType" TEXT NOT NULL,
    "Selection" TEXT NOT NULL,
    "Value" NUMERIC NOT NULL,
    "IsActive" BOOLEAN NOT NULL,
    "CreatedAt" TIMESTAMPTZ NOT NULL,
    "UpdatedAt" TIMESTAMPTZ NOT NULL
);

-- Criar índices
CREATE INDEX IF NOT EXISTS "IX_Users_AdressId" ON "Users" ("AdressId");
CREATE INDEX IF NOT EXISTS "IX_Matches_Team1Id" ON "Matches" ("Team1Id");
CREATE INDEX IF NOT EXISTS "IX_Matches_Team2Id" ON "Matches" ("Team2Id");
CREATE INDEX IF NOT EXISTS "IX_Matches_ChampionshipId" ON "Matches" ("ChampionshipId");
CREATE INDEX IF NOT EXISTS "IX_Notifications_UserId" ON "Notifications" ("UserId");
CREATE INDEX IF NOT EXISTS "IX_Transactions_UserId" ON "Transactions" ("UserId");
CREATE INDEX IF NOT EXISTS "IX_Bets_UserId" ON "Bets" ("UserId");
CREATE INDEX IF NOT EXISTS "IX_Bets_MatchId" ON "Bets" ("MatchId");
CREATE INDEX IF NOT EXISTS "IX_MatchStats_MatchId" ON "MatchStats" ("MatchId");
CREATE INDEX IF NOT EXISTS "IX_Odds_MatchId" ON "Odds" ("MatchId");
