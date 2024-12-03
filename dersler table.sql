CREATE TABLE Dersler(
dersID INT  IDENTITY(1,1) primary key,
ders_ad varchar(20) ,
ogretmenID INT   constraint fk_Ogretmenler foreign key references Ogretmenler(ogretmenID),
kontenjan INT
);