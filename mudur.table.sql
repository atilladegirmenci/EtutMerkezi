CREATE TABLE Mudur(
mudurID INT  IDENTITY(1,1) primary key,
isim varchar(20),
soyisim varchar(20),
tel_no char(11) unique,
maas INT
);
