USE [Etut]
GO
/****** Object:  Trigger [dbo].[BitisTarihiHesapla]    Script Date: 12/8/2024 3:09:38 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER TRIGGER [dbo].[BitisTarihiHesapla] 
ON [dbo].[Ogrenci] 
AFTER INSERT 
AS
BEGIN
    UPDATE Ogrenci
    SET kayit_bitis_tarihi = DATEADD(YEAR, 1, kayit_tarihi)
    WHERE ogrenciID IN (SELECT ogrenciID FROM inserted);
END;
