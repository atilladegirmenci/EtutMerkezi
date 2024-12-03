CREATE TABLE ders_bilgileri(
ders_bilgiID INT IDENTITY(1,1) primary key,
dersID INT  constraint fk_Dersler foreign key references Dersler(dersID),
s�n�fID varchar(4) UNIQUE,
tarih date,
baslang�c_saat TIME,
bitis_saat TIME
);