CREATE TABLE ders_bilgileri(
ders_bilgiID INT IDENTITY(1,1) primary key,
dersID INT  constraint fk_dersler_bilgi foreign key references dersler(dersID),
sinifID INT  constraint fk_sinifsal foreign key references Sinif(sinifID),
tarih date,
baslangıc_saat TIME,
bitis_saat TIME,
kontenjan INT 
);
