USE [Etut]
GO
/****** Object:  StoredProcedure [dbo].[ogretmenListele]    Script Date: 17.12.2024 19:03:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

Create PROCEDURE ogretmenListele
    @isim NVARCHAR(50) = NULL
AS
BEGIN
    IF @isim IS NULL OR @isim = ''
    BEGIN
        -- Tüm öğretmenleri listele
        SELECT * 
        FROM Ogretmenler;
    END
    ELSE
    BEGIN
        -- Filtreleme yaparak listele
        SELECT * 
        FROM Ogretmenler
        WHERE isim LIKE '%' + @isim + '%';
    END
END;
