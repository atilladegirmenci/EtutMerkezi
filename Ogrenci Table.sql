create table Ogrenci(
ogrenciID int identity(1,1) primary key,
tc_no char(11) NOT NULL UNIQUE,
ad varchar(20) not null,
soyad varchar(20) not null,
sinif_seviye INT CHECK (sinif_seviye BETWEEN 9 AND 12),
dogum_tarihi date not null,
tel_no char(11) not null unique,
adres varchar(100) not null, 
kayit_tarihi date default getdate(),
kayit_bitis_tarihi date,
kayit_ucret money not null
);

