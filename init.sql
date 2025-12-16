-- Initial database setup for SoccerBet
-- This will be executed when the PostgreSQL container starts

CREATE TABLE IF NOT EXISTS Roles (
    Id SERIAL PRIMARY KEY,
    Name VARCHAR(50) NOT NULL UNIQUE,
    Description VARCHAR(255),
    IsActive BOOLEAN DEFAULT TRUE,
    CreatedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

CREATE TABLE IF NOT EXISTS Users (
    Id SERIAL PRIMARY KEY,
    Email VARCHAR(255) UNIQUE NOT NULL,
    PasswordHash VARCHAR(255) NOT NULL,
    Nickname VARCHAR(100) NOT NULL,
    FullName VARCHAR(255),
    Phone VARCHAR(20),
    CPF VARCHAR(14) UNIQUE,
    Balance DECIMAL(10, 2) DEFAULT 0.00 NOT NULL CHECK (Balance >= 0),
    IsVerified BOOLEAN DEFAULT FALSE,
    IsActive BOOLEAN DEFAULT TRUE,
    CreatedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    UpdatedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    LastLogin TIMESTAMP
);

CREATE TABLE IF NOT EXISTS Championships (
    Id SERIAL PRIMARY KEY,
    Name VARCHAR(100) NOT NULL,
    Slug VARCHAR(100) UNIQUE NOT NULL,
    Description TEXT,
    Country VARCHAR(100),
    Season VARCHAR(20),
    IsActive BOOLEAN DEFAULT TRUE,
    CreatedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    UpdatedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

CREATE TABLE IF NOT EXISTS Teams (
    Id SERIAL PRIMARY KEY,
    Name VARCHAR(100) NOT NULL,
    ShortName VARCHAR(50),
    LogoUrl TEXT,
    Country VARCHAR(100),
    IsActive BOOLEAN DEFAULT TRUE,
    CreatedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    UpdatedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

CREATE TABLE IF NOT EXISTS Matches (
    Id SERIAL PRIMARY KEY,
    ChampionshipId INTEGER NOT NULL REFERENCES Championships(Id) ON DELETE CASCADE,
    Team1Id INTEGER NOT NULL REFERENCES Teams(Id),
    Team2Id INTEGER NOT NULL REFERENCES Teams(Id),
    MatchDate TIMESTAMP NOT NULL,
    Status VARCHAR(20) DEFAULT 'scheduled' CHECK (Status IN ('scheduled', 'live', 'finished', 'cancelled')),
    Team1Score INTEGER,
    Team2Score INTEGER,
    RoundNumber INTEGER,
    Venue VARCHAR(255),
    CreatedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    UpdatedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    CHECK (Team1Id != Team2Id)
);

CREATE TABLE IF NOT EXISTS Odds (
    Id SERIAL PRIMARY KEY,
    MatchId INTEGER NOT NULL REFERENCES Matches(Id) ON DELETE CASCADE,
    BetType VARCHAR(50) NOT NULL,
    Selection VARCHAR(100) NOT NULL,
    Quantity INTEGER,
    OddValue DECIMAL(5, 2) NOT NULL CHECK (OddValue >= 1.00),
    IsActive BOOLEAN DEFAULT TRUE,
    CreatedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    UpdatedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

CREATE TABLE IF NOT EXISTS Bets (
    Id SERIAL PRIMARY KEY,
    TicketId VARCHAR(20) UNIQUE NOT NULL,
    UserId INTEGER NOT NULL REFERENCES Users(Id) ON DELETE CASCADE,
    MatchId INTEGER NOT NULL REFERENCES Matches(Id),
    OddId INTEGER NOT NULL REFERENCES Odds(Id),
    BetType VARCHAR(50) NOT NULL,
    Selection VARCHAR(100) NOT NULL,
    Quantity INTEGER,
    OddValue DECIMAL(5, 2) NOT NULL,
    BetAmount DECIMAL(10, 2) NOT NULL CHECK (BetAmount >= 10.00 AND BetAmount <= 1000.00),
    PotentialWin DECIMAL(10, 2) NOT NULL,
    Status VARCHAR(20) DEFAULT 'pending' CHECK (Status IN ('pending', 'won', 'lost', 'cancelled')),
    ResultValue DECIMAL(10, 2) DEFAULT 0.00,
    SettledAt TIMESTAMP,
    CreatedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    UpdatedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

CREATE TABLE IF NOT EXISTS Transactions (
    Id SERIAL PRIMARY KEY,
    UserId INTEGER NOT NULL REFERENCES Users(Id) ON DELETE CASCADE,
    Type VARCHAR(20) NOT NULL CHECK (Type IN ('deposit', 'withdrawal', 'bet', 'win', 'bonus', 'refund')),
    Amount DECIMAL(10, 2) NOT NULL CHECK (Amount != 0),
    PreviousBalance DECIMAL(10, 2) NOT NULL,
    NewBalance DECIMAL(10, 2) NOT NULL,
    Status VARCHAR(20) DEFAULT 'completed' CHECK (Status IN ('pending', 'completed', 'failed', 'cancelled')),
    Description TEXT,
    ReferenceId INTEGER,
    PaymentMethod VARCHAR(50),
    PixKey VARCHAR(255),
    CreatedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    UpdatedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

CREATE TABLE IF NOT EXISTS Notifications (
    Id SERIAL PRIMARY KEY,
    UserId INTEGER NOT NULL REFERENCES Users(Id) ON DELETE CASCADE,
    Type VARCHAR(50) NOT NULL,
    Title VARCHAR(255) NOT NULL,
    Message TEXT NOT NULL,
    IsRead BOOLEAN DEFAULT FALSE,
    ReferenceId INTEGER,
    CreatedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

CREATE TABLE IF NOT EXISTS UserRoles (
    Id SERIAL PRIMARY KEY,
    UserId INTEGER NOT NULL REFERENCES Users(Id) ON DELETE CASCADE,
    RoleId INTEGER NOT NULL REFERENCES Roles(Id) ON DELETE CASCADE,
    AssignedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    AssignedBy INTEGER REFERENCES Users(Id) ON DELETE SET NULL,
    UNIQUE(UserId, RoleId)
);

-- Insert sample data
INSERT INTO Roles (Id, Name, Description) VALUES
(1, 'User', 'Regular user with basic permissions'),
(2, 'Admin', 'Administrator with elevated permissions'),
(3, 'Moderator', 'Moderator with content management permissions'),
(4, 'SuperAdmin', 'Super administrator with full system access')
ON CONFLICT (Name) DO NOTHING;

-- Set sequence to continue from 5
SELECT setval('roles_id_seq', 4, true);

INSERT INTO Championships (Name, Slug, Description, Country, Season) VALUES
('Copa do Brasil', 'copa-brasil', 'Torneio eliminatório nacional', 'Brasil', '2024'),
('Libertadores', 'libertadores', 'Principal competição sul-americana', 'América do Sul', '2024'),
('Campeonato Brasileiro', 'brasileiro', 'Liga nacional por pontos corridos', 'Brasil', '2024')
ON CONFLICT (Slug) DO NOTHING;

INSERT INTO Teams (Name, ShortName, Country) VALUES
('Flamengo', 'FLA', 'Brasil'),
('Palmeiras', 'PAL', 'Brasil'),
('Corinthians', 'COR', 'Brasil'),
('São Paulo', 'SAO', 'Brasil'),
('Santos', 'SAN', 'Brasil'),
('Grêmio', 'GRE', 'Brasil'),
('Internacional', 'INT', 'Brasil'),
('Atlético Mineiro', 'CAM', 'Brasil')
ON CONFLICT DO NOTHING;

-- Create default admin user
INSERT INTO Users (Email, PasswordHash, Nickname, FullName, Balance, IsVerified, IsActive, CreatedAt, UpdatedAt) VALUES
('admin@soccerbet.com', '$2a$11$8K1p/a0dL2LkqvQOuiOX2uy7YhC4Nd6FGCmzYinSJp.dNB4qiAYHm', 'Admin', 'System Administrator', 0.00, true, true, CURRENT_TIMESTAMP, CURRENT_TIMESTAMP)
ON CONFLICT (Email) DO NOTHING;

-- Assign SuperAdmin role to admin user
INSERT INTO UserRoles (UserId, RoleId, AssignedAt) 
SELECT u.Id, 4, CURRENT_TIMESTAMP 
FROM Users u 
WHERE u.Email = 'admin@soccerbet.com' 
AND NOT EXISTS (SELECT 1 FROM UserRoles ur WHERE ur.UserId = u.Id AND ur.RoleId = 4);

-- Create indexes
CREATE INDEX IF NOT EXISTS idx_users_email ON Users(Email);
CREATE INDEX IF NOT EXISTS idx_matches_championship ON Matches(ChampionshipId);
CREATE INDEX IF NOT EXISTS idx_matches_date ON Matches(MatchDate);
CREATE INDEX IF NOT EXISTS idx_odds_match ON Odds(MatchId);
CREATE INDEX IF NOT EXISTS idx_bets_user ON Bets(UserId);
CREATE INDEX IF NOT EXISTS idx_transactions_user ON Transactions(UserId);
CREATE INDEX IF NOT EXISTS idx_notifications_user ON Notifications(UserId);
CREATE INDEX IF NOT EXISTS idx_roles_name ON Roles(Name);
CREATE INDEX IF NOT EXISTS idx_userroles_user ON UserRoles(UserId);
CREATE INDEX IF NOT EXISTS idx_userroles_role ON UserRoles(RoleId);