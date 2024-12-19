CREATE TRIGGER tr_DersKayit_SinavEkle
ON Ders_Kayit
AFTER INSERT
AS
BEGIN
    SET NOCOUNT ON;

    -- Yeni eklenen kayıtlara bağlı olarak sınavlar tablosuna kayıt ekle
    INSERT INTO Sinavlar (kayitID, sinav1_puan, sinav2_puan, sinav3_puan)
    SELECT i.kayitID, 0, 0, 0
    FROM INSERTED i;
END;
