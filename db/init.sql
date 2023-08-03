\connect TodoDb

DROP TABLE IF EXISTS ListItemCheck;
DROP TABLE IF EXISTS ListItem;
DROP TABLE IF EXISTS ListItemType;
DROP TABLE IF EXISTS ListUser;
DROP TABLE IF EXISTS List;
DROP TABLE IF EXISTS "user";
DROP TABLE IF EXISTS "role";

CREATE TABLE List(
    id SERIAL PRIMARY KEY,
    title varchar(100) NOT NULL,
    archive boolean NOT NULL DEFAULT false,
    creationDate timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP
);

CREATE TABLE ListItemType(
    id SERIAL PRIMARY KEY,
    title varchar(100) NOT NULL,
    archive boolean NOT NULL DEFAULT false
);

CREATE TABLE ListItem(
    id SERIAL PRIMARY KEY,
    listId integer REFERENCES List(id),
    typeId integer REFERENCES ListItemType(id) DEFAULT 0,
    content varchar(1000) NOT NULL,
    archive boolean NOT NULL DEFAULT false,
    creationDate timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP
);

CREATE TABLE "user"(
    id SERIAL PRIMARY KEY,
    username varchar(50) NOT NULL UNIQUE,
    password varchar(100) NOT NULL,
    email varchar(100) NOT NULL UNIQUE,
    archive boolean NOT NULL DEFAULT false,
    creationDate timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP
);

CREATE TABLE ListItemCheck(
    listItemId integer REFERENCES ListItem(id),
    userId integer REFERENCES "user"(id),
    creationDate timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
    CONSTRAINT listItemCheck_PKEY PRIMARY KEY(listItemId, userId)
);

CREATE TABLE "role"(
    id SERIAL PRIMARY KEY,
    title varchar(100) NOT NULL,
    archive boolean NOT NULL DEFAULT false
);

CREATE TABLE ListUser (
    listId integer REFERENCES List(id),
    userId integer REFERENCES "user"(id),
    roleId integer NOT NULL REFERENCES "role"(id),
    archive boolean NOT NULL DEFAULT false,
    creationDate timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP
);

-- Add list item types
INSERT INTO ListItemType(title) VALUES ('MultiCheck');
INSERT INTO ListItemType(title) VALUES ('SingleCheck');
--

-- Add roles
INSERT INTO "role"(title) VALUES ('List Admin');
INSERT INTO "role"(title) VALUES ('List User');
--

INSERT INTO "user" (username, password, email)
    VALUES ('user1', '1234', 'user1@test.com');
INSERT INTO "user" (username, password, email)
    VALUES ('user2', '1234', 'user2@test.com');  
INSERT INTO "user" (username, password, email)
    VALUES ('user3', '1234', 'user3@test.com');  
INSERT INTO "user" (username, password, email)
    VALUES ('user4', '1234', 'user4@test.com');            

INSERT INTO List (title)
    VALUES ('TestList');

INSERT INTO ListItem (listId, typeId, content)
    VALUES (1, 1, 'Test content blub blub blub....');

INSERT INTO ListUser (listId, userId, roleId)
    VALUES (1, 1, 1);
INSERT INTO ListUser (listId, userId, roleId)
    VALUES (1, 2, 1);
INSERT INTO ListUser (listId, userId, roleId)
    VALUES (1, 3, 2);
INSERT INTO ListUser (listId, userId, roleId)
    VALUES (1, 4, 2);    

INSERT INTO ListItemCheck (listItemId, userId)
    VALUES (1, 3);    