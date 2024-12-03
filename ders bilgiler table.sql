CREATE TABLE ders_bilgileri(
ders_bilgiID INT IDENTITY(1,1) primary key,
dersID INT  constraint fk_Dersler foreign key references Dersler(dersID),
sýnýfID varchar(4) UNIQUE,
tarih date,
baslangýc_saat TIME,
bitis_saat TIME
);