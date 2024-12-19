create table Odemeler
(
odemeID int identity(1,1) primary key,
planID int constraint fk_planid foreign key references Odeme_Planlari(planID), 
ay varchar(20) not null,
yil char(4) not null,
tutar int not null,
odeme_durumu bit default 0,
odeme_tarihi date,
kayit_ücret int 
);
