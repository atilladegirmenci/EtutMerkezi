-- Silinen öğrencileri kaydetmek için trigger
CREATE TRIGGER trg_SilinenOgrencilerPlanlar
ON Ogrenci
FOR DELETE
AS
BEGIN
    SET NOCOUNT ON;

    -- Silinen öğrencinin bilgilerini SilinenOgrenciler tablosuna ekleyelim
    INSERT INTO SilinenOgrenciler (OgrenciID, TcNo, Ad, Soyad, SinifSeviye, DogumTarihi, TelNo, Adres, KayitTarihi, KayitBitisTarihi, KayitUcret)
    SELECT 
        OgrenciID, tc_no, ad, soyad, sinif_seviye, dogum_tarihi, tel_no, Adres, kayit_tarihi, kayit_bitis_tarihi, kayit_ücret
    FROM deleted;

    -- Silinen öğrencinin ödeme planını kaydedelim
    INSERT INTO SilinenOdemePlanlari (PlanID, OgrenciID, ToplamTutar, AylikTutar, PlanBaslangic, PlanBitis)
    SELECT 
        PlanID, OgrenciID, toplam_tutar, aylik_tutar, plan_baslangiç, plan_bitis
    FROM Odeme_Planlari
    WHERE OgrenciID IN (SELECT OgrenciID FROM deleted);

    -- Silinen öğrencinin ödeme planlarını silelim
    DELETE FROM Odeme_Planlari 
    WHERE OgrenciID IN (SELECT OgrenciID FROM deleted);

    -- Silinen öğrencinin ders kayıtlarını silelim
    DELETE FROM Ders_Kayit
    WHERE OgrenciID IN (SELECT OgrenciID FROM deleted);
END;
