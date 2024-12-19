CREATE TRIGGER trg_InsertOdemePlanlari
ON Odeme_Planlari
AFTER INSERT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @planID INT;
    DECLARE @ogrenciID INT;
    DECLARE @kayit_tarihi DATE;
    DECLARE @kayit_bitis_tarihi DATE;
    DECLARE @kayit_ucret INT;
    DECLARE @aylik_tutar INT;
    DECLARE @ay_sayisi INT;

    -- Yeni eklenen satırı almak için
    SELECT 
        @planID = planID,
        @ogrenciID = ogrenciID
    FROM INSERTED;

    -- Ogrenci tablosundan bilgileri al
    SELECT 
        @kayit_tarihi = kayit_tarihi,
        @kayit_bitis_tarihi = kayit_bitis_tarihi,
        @kayit_ucret = kayit_ücret
    FROM Ogrenci
    WHERE ogrenciID = @ogrenciID;

    -- Ay sayısını hesapla
    SET @ay_sayisi = DATEDIFF(MONTH, @kayit_tarihi, @kayit_bitis_tarihi);

    -- Aylık tutarı hesapla
    IF @ay_sayisi > 0
    BEGIN
        SET @aylik_tutar = @kayit_ucret / @ay_sayisi;
    END
    ELSE
    BEGIN
        SET @aylik_tutar = @kayit_ucret; -- Tek ödeme durumu
    END;

    -- Odeme_Planlari tablosunu güncelle
    UPDATE Odeme_Planlari
    SET toplam_tutar = @kayit_ucret,
        aylik_tutar = @aylik_tutar,
        plan_baslangiç = @kayit_tarihi,
        plan_bitis = @kayit_bitis_tarihi
    WHERE planID = @planID;

    -- Ödemeler tablosuna taksit bilgilerini ekle
    DECLARE @current_month DATE = @kayit_tarihi;
    WHILE @current_month <= @kayit_bitis_tarihi
    BEGIN
        INSERT INTO Odemeler (planID, ay, yil, tutar, odeme_durumu, odeme_tarihi)
        VALUES (@planID, DATENAME(MONTH, @current_month), YEAR(@current_month), @aylik_tutar, 0, NULL);

        -- Bir sonraki aya geç
        SET @current_month = DATEADD(MONTH, 1, @current_month);
    END;
END;