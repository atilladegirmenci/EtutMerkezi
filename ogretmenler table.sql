CREATE TABLE Ogretmenler(
ogretmenID INT IDENTITY(1,1) primary key ,
isim varchar(20),
soyisim varchar(20),
brans varchar(20),
tel_no char(15) UNIQUE,
e_posta varchar(30) UNIQUE,
maas INT,
mudurID INT constraint fk_mudurID foreign key references Mudur(mudurID)
);
