
create table Odeme_Planlari(
planID int identity(1,1) primary key,
ogrenciID int constraint fk_ogrenciID foreign key references Ogrenci(ogrenciID),
toplam_tutar int not null,
aylik_tutar int,
plan_baslangiç date ,
plan_bitis date
);



