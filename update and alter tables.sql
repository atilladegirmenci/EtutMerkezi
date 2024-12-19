--sınav notu sayısını üçe çıkart
ALTER TABLE Sinavlar
DROP COLUMN puan;

ALTER TABLE Sinavlar
ADD sinav1_puan DECIMAL(5,3),
    sinav2_puan DECIMAL(5,3),
    sinav3_puan DECIMAL(5,3);

--dersler tablosundaki tüm derslerin adını büyük harfle başlayacak şekilde güncelle
UPDATE Dersler
SET ders_ad = UPPER(LEFT(ders_ad, 1)) + SUBSTRING(ders_ad, 2, LEN(ders_ad))

--ders kayıt ve odeme planları tablosundaki fk ilişksisini cascade olarak değiştir
ALTER TABLE Ders_Kayit
ADD CONSTRAINT fk_derskayit_ogrenci
FOREIGN KEY (ogrenciID)
REFERENCES Ogrenci (ogrenciID)
ON DELETE CASCADE;

ALTER TABLE Odeme_Planlari
ADD CONSTRAINT fk_odeme_ogrenci
FOREIGN KEY (OgrenciID)
REFERENCES Ogrenci (OgrenciID)
ON DELETE CASCADE;

