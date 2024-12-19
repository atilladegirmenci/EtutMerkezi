CREATE TABLE Ders_Kayit(
kayitID INT IDENTITY(1,1) primary key,
ogrenciID INT constraint fk_Ders_kayit_OgrenciID foreign key references Ogrenci(Ders_kayit_OgrenciID),
dersID INT constraint fk_Dersler foreign key references Dersler(dersID)
);