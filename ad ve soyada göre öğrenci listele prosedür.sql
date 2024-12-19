CREATE PROCEDURE OgrenciListele
    @Ad NVARCHAR(50) = '', -- Varsayılan değer boş string
    @Soyad NVARCHAR(50) = ''
AS
BEGIN
    SELECT ogrenciID, tc_no, ad, soyad, sinif_seviye, dogum_tarihi, tel_no, adres, kayit_tarihi, kayit_bitis_tarihi, kayit_ücret
    FROM Ogrenci
    WHERE ad LIKE '%' + @Ad + '%' AND soyad LIKE '%' + @Soyad + '%';
END;
