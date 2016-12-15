SET SCHEMA 'sinf';

DROP TABLE IF EXISTS Utilizador CASCADE; 
DROP TABLE IF EXISTS Reviews CASCADE; 
DROP TABLE IF EXISTS Product CASCADE;
DROP TABLE IF EXISTS Wishlist CASCADE;
DROP TRIGGER  IF EXISTS lowerCaseEmail ON Utilizador CASCADE;
DROP TRIGGER  IF EXISTS validateEmail ON Utilizador CASCADE;
DROP TRIGGER  IF EXISTS primaveraCodeInsert ON Utilizador CASCADE;

CREATE TABLE IF NOT EXISTS Utilizador(
code SERIAL PRIMARY KEY,
email TEXT NOT NULL UNIQUE,
primaveraCode TEXT NOT NULL UNIQUE,
type VARCHAR(256) NOT NULL,
password VARCHAR(256) NOT NULL
);

CREATE TABLE IF NOT EXISTS Product(
code SERIAL PRIMARY KEY,
primaveraCode TEXT NOT NULL UNIQUE, -- primavera code is a mix of letters and numbers
img TEXT NOT NULL -- link to img of a product 
);

CREATE TABLE IF NOT EXISTS Reviews(
code SERIAL PRIMARY KEY,
utilizador TEXT NOT NULL,
productCode TEXT NOT NULL,
review TEXT,
score SMALLINT
);
 
CREATE TABLE IF NOT EXISTS Wishlist(
utilizador TEXT NOT NULL,
productCode TEXT NOT NULL,
quantity INTEGER NOT NULL DEFAULT 1,
 PRIMARY KEY(utilizador, productCode)
);

--http://stackoverflow.com/questions/9807909/are-email-addresses-case-sensitive    to justify the trigger
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

CREATE OR REPLACE FUNCTION insertPrimaveraCode() RETURNS trigger AS $$
BEGIN
  NEW.primaveraCode = NEW.code;
  RETURN NEW;
END 
$$ LANGUAGE 'plpgsql'; 

CREATE TRIGGER validateEmail
BEFORE INSERT OR UPDATE ON Utilizador 
FOR EACH ROW
EXECUTE PROCEDURE validateEmailFunction();

CREATE TRIGGER primaveraCodeInsert
BEFORE INSERT ON Utilizador 
FOR EACH ROW
EXECUTE PROCEDURE insertPrimaveraCode();

--Test cases the first one should pass wihtout any problems, o 2º deve aparecer todo em lower case e o 3º não deve ser adicionado s
INSERT INTO Utilizador (email, type, password) VALUES ('hi@gmail.com', 'admin', '123');
INSERT INTO Utilizador (email, type, password) VALUES ('TEST@gmail.com', 'client', '123');
INSERT INTO Utilizador (email, type, password) VALUES ('TESTsdasdasd.com', 'client', '123');
INSERT INTO Utilizador (email,type,password) VALUES ('sofrio@gmail.com','client','123');
INSERT INTO Utilizador (email,type,password) VALUES ('alcad@gmail.com','client','123');
INSERT INTO Utilizador (email,type,password) VALUES ('lima@gmail.com','client','123');
INSERT INTO Utilizador (email,type,password) VALUES ('silva@gmail.com','client','123');
INSERT INTO Utilizador (email,type,password) VALUES ('jmf@gmail.com','client','123');
UPDATE utilizador SET primaveracode='SOFRIO' WHERE email='sofrio@gmail.com';
UPDATE utilizador SET primaveracode='ALCAD' WHERE email='alcad@gmail.com';
UPDATE utilizador SET primaveracode='LIMA' WHERE email='lima@gmail.com';
UPDATE utilizador SET primaveracode='SILVA' WHERE email='silva@gmail.com';
UPDATE utilizador SET primaveracode='J.M.F.' WHERE email='jmf@gmail.com';


