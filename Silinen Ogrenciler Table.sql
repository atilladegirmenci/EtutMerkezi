CREATE TABLE SilinenOgrenciler (
    OgrenciID INT PRIMARY KEY,
    TcNo CHAR(11),
    Ad VARCHAR(20),
    Soyad VARCHAR(20),
    SinifSeviye INT,
    DogumTarihi DATE,
    TelNo CHAR(11),
    Adres VARCHAR(100),
    KayitTarihi DATE,
    KayitBitisTarihi DATE,
    KayitUcret MONEY,
    SilinmeTarihi DATETIME DEFAULT GETDATE()
);
