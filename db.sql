CREATE TABLE Commands (
    Command CHAR(255) PRIMARY KEY,
    Message TEXT (510) NOT NULL,
    OriginalCommand CHAR(255) NOT NULL,
    Permission TINYINT DEFAULT 3 NOT NULL,
    Timer INT DEFAULT 60 NOT NULL
);

CREATE TABLE Users (
	Username CHAR(255) PRIMARY KEY,
    Password CHAR(60) NOT NULL,
    AuthorizationLevel TINYINT DEFAULT 0 NOT NULL
);

CREATE TABLE TimedMessages (
	Guid BINARY(16) PRIMARY KEY,
    Command CHAR(255) NOT NULL,
    IntervalInMinutes INT NOT NULL,
    OffsetInMinutes INT NOT NULL,
    
    CONSTRAINT FK_TimedMessages_CommandDetails FOREIGN KEY (Command) REFERENCES Commands(Command)
);

CREATE TABLE WebhookSubscribers (
	Guid BINARY(16) PRIMARY KEY,
    Uri TEXT(255) NOT NULL
);


CREATE TABLE StreamPlannings (
	Id BIGINT PRIMARY KEY,
    Date DATETIME NOT NULL,
    TimeSlot TEXT(255) NOT NULL,
    Game TEXT(255) NOT NULL,
    StreamType TEXT(255) NOT NULL,
    GameType TEXT(255) NOT NULL,
    TrailUri TEXT(255) NOT NULL
);

