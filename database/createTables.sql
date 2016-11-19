SET SCHEMA 'sinf';

DROP TABLE IF EXISTS Utilizador CASCADE; 
DROP TABLE IF EXISTS Reviews CASCADE; 
DROP TABLE IF EXISTS Product CASCADE;

CREATE TABLE IF NOT EXISTS Utilizador(
code SERIAL PRIMARY KEY,
password VARCHAR(256) NOT NULL,
email TEXT NOT NULL UNIQUE
);

CREATE TABLE IF NOT EXISTS Product(
code SERIAL PRIMARY KEY,
primaveraCode INTEGER NOT NULL UNIQUE,
img TEXT NOT NULL -- link to img of a product 
);

CREATE TABLE IF NOT EXISTS Reviews(
code SERIAL PRIMARY KEY,
utilizador INTEGER REFERENCES Utilizador(code),
productCode INTEGER REFERENCES Product(code),  --subject to change and verify with primavera
review TEXT NOT NULL,
score SMALLINT NOT NULL,
tsv tsvector
);

CREATE OR REPLACE FUNCTION lowerCaseEmailFunction() RETURNS trigger AS $$
BEGIN
 NEW.email = lower(NEW.email);
 RETURN NEW;
END 
$$ LANGUAGE 'plpgsql'; 

CREATE TRIGGER lowerCaseEmail
BEFORE INSERT OR UPDATE ON Utilizador 
FOR EACH ROW
EXECUTE PROCEDURE lowerCaseEmailFunction();


-- http://emailregex.com/
CREATE OR REPLACE FUNCTION validateEmailFunction() RETURNS trigger AS $$
BEGIN
 IF NEW.email ~* '^[A-Z0-9._%-]+@[A-Z0-9.-]+\.[A-Z]{2,4}$'
 THEN
 RETURN NEW;
 ELSE
 RETURN NULL;
 END IF;
END 
$$ LANGUAGE 'plpgsql'; 

CREATE TRIGGER validateEmail
BEFORE INSERT OR UPDATE ON Utilizador 
FOR EACH ROW
EXECUTE PROCEDURE validateEmailFunction();

INSERT INTO Utilizador (password,email) VALUES ('Admin','hi@gmail.com');
INSERT INTO Utilizador (password,email) VALUES ('Admin','TEST@gmail.com');
INSERT INTO Utilizador (password,email) VALUES ('Admin','TESTsdasdasd.com');

