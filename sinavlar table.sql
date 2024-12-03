CREATE TABLE Sinavlar(
sinavID INT IDENTITY (1,1) primary key,
kayitID INT  constraint fk_kayitID foreign key references Ders_Kayit(kayitID),
puan DECIMAL(3,3)
);