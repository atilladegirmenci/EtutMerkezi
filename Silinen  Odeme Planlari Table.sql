CREATE TABLE SilinenOdemePlanlari (
    PlanID INT PRIMARY KEY,
    OgrenciID INT,
    ToplamTutar INT,
    AylikTutar INT,
    PlanBaslangic DATE,
    PlanBitis DATE,
    SilinmeTarihi DATETIME DEFAULT GETDATE()
);
