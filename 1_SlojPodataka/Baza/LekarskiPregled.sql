USE [master]
GO

IF EXISTS (SELECT name FROM sys.databases WHERE name = N'LekarskiPregledi')
    DROP DATABASE [LekarskiPregledi]
GO

CREATE DATABASE [LekarskiPregledi]
GO

USE [LekarskiPregledi]
GO

-- =============================================
-- Tabela: ObrazovniProfil (sifrarnik)
-- Sifra se koristi za uparivanje sa eksternim XML/JSON parametrom
-- (WebServis/XML/Pravila.xml) koji propisuje koji profili zahtijevaju
-- dodatne specijalisticke preglede.
-- =============================================
CREATE TABLE [dbo].[ObrazovniProfil] (
    [ProfilID]          [int] IDENTITY(1,1) NOT NULL,
    [Sifra]             [nvarchar](20) NOT NULL,
    [Naziv]             [nvarchar](150) NOT NULL,
    [Opis]              [nvarchar](300) NULL,
    [NivoZahtjevnosti]  [int] NOT NULL DEFAULT(1),
    CONSTRAINT [PK_ObrazovniProfil] PRIMARY KEY CLUSTERED ([ProfilID] ASC),
    CONSTRAINT [UQ_ObrazovniProfil_Sifra] UNIQUE ([Sifra])
)
GO

-- =============================================
-- Tabela: SrednjaSkola (sifrarnik)
-- =============================================
CREATE TABLE [dbo].[SrednjaSkola] (
    [SkolaID]        [int] IDENTITY(1,1) NOT NULL,
    [Naziv]          [nvarchar](150) NOT NULL,
    [Adresa]         [nvarchar](150) NOT NULL,
    [Grad]           [nvarchar](100) NOT NULL,
    [Telefon]        [nvarchar](20) NULL,
    [DatumOsnivanja] [date] NULL,
    CONSTRAINT [PK_SrednjaSkola] PRIMARY KEY CLUSTERED ([SkolaID] ASC)
)
GO

-- =============================================
-- Tabela: Kandidat
-- =============================================
CREATE TABLE [dbo].[Kandidat] (
    [KandidatID]    [int] IDENTITY(1,1) NOT NULL,
    [Ime]           [nvarchar](50) NOT NULL,
    [Prezime]       [nvarchar](50) NOT NULL,
    [JMBG]          [nvarchar](13) NOT NULL,
    [DatumRodjenja] [date] NOT NULL,
    [SkolaID]       [int] NOT NULL,
    [ProfilID]      [int] NOT NULL,
    [VrstaUpisa]    [nvarchar](50) NOT NULL DEFAULT('Redovan upis'),
    [Aktivan]       [bit] NOT NULL DEFAULT(1),
    CONSTRAINT [PK_Kandidat] PRIMARY KEY CLUSTERED ([KandidatID] ASC),
    CONSTRAINT [UQ_Kandidat_JMBG] UNIQUE ([JMBG])
)
GO

-- =============================================
-- Tabela: LekarskiPregled
-- =============================================
CREATE TABLE [dbo].[LekarskiPregled] (
    [PregledID]              [int] IDENTITY(1,1) NOT NULL,
    [KandidatID]             [int] NOT NULL,
    [DatumPregleda]          [datetime] NOT NULL,
    [ZdravstvenaUstanova]    [nvarchar](200) NOT NULL,
    [AdresaUstanove]         [nvarchar](200) NOT NULL,
    [TrajanjePregledaMinuta] [int] NOT NULL DEFAULT(30),
    [BrojLekaraUKomisiji]    [int] NOT NULL DEFAULT(2),
    [Zavrsen]                [bit] NOT NULL DEFAULT(0),
    [Napomena]               [nvarchar](500) NULL,
    CONSTRAINT [PK_LekarskiPregled] PRIMARY KEY CLUSTERED ([PregledID] ASC)
)
GO

-- =============================================
-- Tabela: PrilozeniNalaz (dodatni specijalisticki nalazi uz pregled)
-- =============================================
CREATE TABLE [dbo].[PrilozeniNalaz] (
    [NalazID]     [int] IDENTITY(1,1) NOT NULL,
    [PregledID]   [int] NOT NULL,
    [VrstaNalaza] [nvarchar](150) NOT NULL,
    [DatumNalaza] [date] NOT NULL,
    [Ustanova]    [nvarchar](200) NOT NULL,
    [Uredan]      [bit] NOT NULL DEFAULT(1),
    [Napomena]    [nvarchar](500) NULL,
    CONSTRAINT [PK_PrilozeniNalaz] PRIMARY KEY CLUSTERED ([NalazID] ASC)
)
GO

-- =============================================
-- Tabela: Korisnik
-- =============================================
CREATE TABLE [dbo].[Korisnik] (
    [KorisnikID] [int] IDENTITY(1,1) NOT NULL,
    [Ime]        [nvarchar](50) NOT NULL,
    [Prezime]    [nvarchar](50) NOT NULL,
    [Email]      [nvarchar](100) NOT NULL,
    [Lozinka]    [nvarchar](100) NOT NULL,
    [Uloga]      [nvarchar](30) NOT NULL,
    CONSTRAINT [PK_Korisnik] PRIMARY KEY CLUSTERED ([KorisnikID] ASC),
    CONSTRAINT [UQ_Korisnik_Email] UNIQUE ([Email])
)
GO

-- =============================================
-- Foreign Keys
-- =============================================
ALTER TABLE [dbo].[Kandidat]
    ADD CONSTRAINT [FK_Kandidat_Skola] FOREIGN KEY([SkolaID]) REFERENCES [dbo].[SrednjaSkola]([SkolaID])
GO
ALTER TABLE [dbo].[Kandidat]
    ADD CONSTRAINT [FK_Kandidat_Profil] FOREIGN KEY([ProfilID]) REFERENCES [dbo].[ObrazovniProfil]([ProfilID])
GO
ALTER TABLE [dbo].[LekarskiPregled]
    ADD CONSTRAINT [FK_LekarskiPregled_Kandidat] FOREIGN KEY([KandidatID]) REFERENCES [dbo].[Kandidat]([KandidatID])
GO
ALTER TABLE [dbo].[PrilozeniNalaz]
    ADD CONSTRAINT [FK_PrilozeniNalaz_Pregled] FOREIGN KEY([PregledID]) REFERENCES [dbo].[LekarskiPregled]([PregledID])
GO

-- =============================================
-- Check Constraints
-- =============================================
ALTER TABLE [dbo].[LekarskiPregled]
    ADD CONSTRAINT [CHK_TrajanjePregledaMinuta] CHECK ([TrajanjePregledaMinuta] >= 5 AND [TrajanjePregledaMinuta] <= 180)
GO
ALTER TABLE [dbo].[LekarskiPregled]
    ADD CONSTRAINT [CHK_BrojLekaraUKomisiji] CHECK ([BrojLekaraUKomisiji] >= 1 AND [BrojLekaraUKomisiji] <= 10)
GO
ALTER TABLE [dbo].[ObrazovniProfil]
    ADD CONSTRAINT [CHK_NivoZahtjevnosti] CHECK ([NivoZahtjevnosti] >= 1 AND [NivoZahtjevnosti] <= 5)
GO

-- =============================================
-- Podaci: ObrazovniProfil
-- Sifre 142, 233, 310, 405 se nalaze u WebServis/XML/Pravila.xml kao profili
-- koji zahtijevaju dodatne specijalisticke preglede. Sifre 018 i 025 ne
-- zahtijevaju dodatne preglede - obicno ljekarsko uvjerenje je dovoljno.
-- =============================================
SET IDENTITY_INSERT [dbo].[ObrazovniProfil] ON
INSERT [dbo].[ObrazovniProfil] ([ProfilID],[Sifra],[Naziv],[Opis],[NivoZahtjevnosti])
    VALUES (1, N'025', N'Gimnazija - opsti smjer',        N'Opsteobrazovni smjer bez dodatnih zdravstvenih uslova', 1)
INSERT [dbo].[ObrazovniProfil] ([ProfilID],[Sifra],[Naziv],[Opis],[NivoZahtjevnosti])
    VALUES (2, N'018', N'Ekonomski tehnicar',              N'Ekonomsko-pravni smjer', 1)
INSERT [dbo].[ObrazovniProfil] ([ProfilID],[Sifra],[Naziv],[Opis],[NivoZahtjevnosti])
    VALUES (3, N'142', N'Elektrotehnicar racunara',        N'Zahtijeva provjeru vida i sluha', 3)
INSERT [dbo].[ObrazovniProfil] ([ProfilID],[Sifra],[Naziv],[Opis],[NivoZahtjevnosti])
    VALUES (4, N'233', N'Kozmeticki tehnicar',             N'Zahtijeva dermatoloski pregled', 2)
INSERT [dbo].[ObrazovniProfil] ([ProfilID],[Sifra],[Naziv],[Opis],[NivoZahtjevnosti])
    VALUES (5, N'310', N'Vozac motornih vozila',           N'Zahtijeva provjeru vida, sluha i neuropsihijatrijski pregled', 4)
INSERT [dbo].[ObrazovniProfil] ([ProfilID],[Sifra],[Naziv],[Opis],[NivoZahtjevnosti])
    VALUES (6, N'405', N'Sportski smjer - fudbal',         N'Zahtijeva kardioloski i ortopedski pregled', 5)
SET IDENTITY_INSERT [dbo].[ObrazovniProfil] OFF
GO

-- =============================================
-- Podaci: SrednjaSkola
-- =============================================
SET IDENTITY_INSERT [dbo].[SrednjaSkola] ON
INSERT [dbo].[SrednjaSkola] ([SkolaID],[Naziv],[Adresa],[Grad],[Telefon],[DatumOsnivanja])
    VALUES (1, N'Prva gimnazija',                    N'Obala Kulina bana 4',  N'Sarajevo', N'033-201-100', '1930-09-01')
INSERT [dbo].[SrednjaSkola] ([SkolaID],[Naziv],[Adresa],[Grad],[Telefon],[DatumOsnivanja])
    VALUES (2, N'Srednja elektrotehnicka skola',     N'Ferde Hauptmana 1',    N'Sarajevo', N'033-201-200', '1958-09-01')
INSERT [dbo].[SrednjaSkola] ([SkolaID],[Naziv],[Adresa],[Grad],[Telefon],[DatumOsnivanja])
    VALUES (3, N'Mjesovita srednja skola Ilidza',    N'Butmirska cesta 12',   N'Ilidza',   N'033-777-300', '1975-09-01')
SET IDENTITY_INSERT [dbo].[SrednjaSkola] OFF
GO

-- =============================================
-- Podaci: Kandidat
-- =============================================
SET IDENTITY_INSERT [dbo].[Kandidat] ON
INSERT [dbo].[Kandidat] ([KandidatID],[Ime],[Prezime],[JMBG],[DatumRodjenja],[SkolaID],[ProfilID],[VrstaUpisa],[Aktivan])
    VALUES (1, N'Amina',  N'Hodzic',    N'0105010175018', '2010-05-01', 1, 1, N'Redovan upis', 1)
INSERT [dbo].[Kandidat] ([KandidatID],[Ime],[Prezime],[JMBG],[DatumRodjenja],[SkolaID],[ProfilID],[VrstaUpisa],[Aktivan])
    VALUES (2, N'Emir',   N'Kovac',     N'1503010175033', '2010-03-15', 2, 3, N'Redovan upis', 1)
INSERT [dbo].[Kandidat] ([KandidatID],[Ime],[Prezime],[JMBG],[DatumRodjenja],[SkolaID],[ProfilID],[VrstaUpisa],[Aktivan])
    VALUES (3, N'Lamija', N'Begic',     N'2207010175022', '2010-07-22', 3, 6, N'Prijem sportiste', 1)
SET IDENTITY_INSERT [dbo].[Kandidat] OFF
GO

-- =============================================
-- Podaci: LekarskiPregled
-- =============================================
SET IDENTITY_INSERT [dbo].[LekarskiPregled] ON
INSERT [dbo].[LekarskiPregled] ([PregledID],[KandidatID],[DatumPregleda],[ZdravstvenaUstanova],[AdresaUstanove],[TrajanjePregledaMinuta],[BrojLekaraUKomisiji],[Zavrsen],[Napomena])
    VALUES (1, 1, '2026-05-10 09:00:00', N'Dom zdravlja Centar',       N'Kranjceviceva 12, Sarajevo', 20, 1, 1, NULL)
INSERT [dbo].[LekarskiPregled] ([PregledID],[KandidatID],[DatumPregleda],[ZdravstvenaUstanova],[AdresaUstanove],[TrajanjePregledaMinuta],[BrojLekaraUKomisiji],[Zavrsen],[Napomena])
    VALUES (2, 2, '2026-05-12 10:30:00', N'Dom zdravlja Novo Sarajevo', N'Zmaja od Bosne 44, Sarajevo', 30, 2, 1, N'Kandidat konkurise za tehnicki smjer')
INSERT [dbo].[LekarskiPregled] ([PregledID],[KandidatID],[DatumPregleda],[ZdravstvenaUstanova],[AdresaUstanove],[TrajanjePregledaMinuta],[BrojLekaraUKomisiji],[Zavrsen],[Napomena])
    VALUES (3, 3, '2026-05-14 08:00:00', N'Dom zdravlja Ilidza',       N'Butmirska cesta 5, Ilidza',  45, 3, 0, N'Potrebna dopuna nalaza')
SET IDENTITY_INSERT [dbo].[LekarskiPregled] OFF
GO

-- =============================================
-- Podaci: PrilozeniNalaz
-- Kandidat 2 (Elektrotehnicar racunara - sifra 142) ima kompletne nalaze
-- (oftalmolog + ORL) => uvjerenje je validno.
-- Kandidat 3 (Sportski smjer - sifra 405) ima samo kardiologa, nedostaje
-- ortoped => uvjerenje NIJE validno dok se ne prilozi nedostajuci nalaz.
-- =============================================
SET IDENTITY_INSERT [dbo].[PrilozeniNalaz] ON
INSERT [dbo].[PrilozeniNalaz] ([NalazID],[PregledID],[VrstaNalaza],[DatumNalaza],[Ustanova],[Uredan],[Napomena])
    VALUES (1, 2, N'Pregled oftalmologa',              '2026-05-11', N'Klinicki centar Sarajevo', 1, NULL)
INSERT [dbo].[PrilozeniNalaz] ([NalazID],[PregledID],[VrstaNalaza],[DatumNalaza],[Ustanova],[Uredan],[Napomena])
    VALUES (2, 2, N'Pregled otorinolaringologa (ORL)', '2026-05-11', N'Klinicki centar Sarajevo', 1, NULL)
INSERT [dbo].[PrilozeniNalaz] ([NalazID],[PregledID],[VrstaNalaza],[DatumNalaza],[Ustanova],[Uredan],[Napomena])
    VALUES (3, 3, N'Pregled kardiologa',               '2026-05-13', N'Zavod za sportsku medicinu', 1, NULL)
SET IDENTITY_INSERT [dbo].[PrilozeniNalaz] OFF
GO

-- =============================================
-- Podaci: Korisnik
-- =============================================
INSERT INTO [dbo].[Korisnik] ([Ime],[Prezime],[Email],[Lozinka],[Uloga])
    VALUES (N'Admin', N'Administrator', N'admin@lekarskipregled.com', N'admin123', N'Admin')
GO

-- =============================================
-- Stored Procedures: Korisnik
-- =============================================
CREATE PROCEDURE spDajKorisnikaPoKredencijalima
    @Email   NVARCHAR(100),
    @Lozinka NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;
    SELECT KorisnikID, Ime, Prezime, Email, Lozinka, Uloga
    FROM Korisnik
    WHERE Email = @Email AND Lozinka = @Lozinka;
END
GO

-- =============================================
-- SP: ObrazovniProfil
-- =============================================
CREATE PROCEDURE spDajSveProfile
AS
BEGIN
    SET NOCOUNT ON;
    SELECT ProfilID, Sifra, Naziv, Opis, NivoZahtjevnosti FROM ObrazovniProfil ORDER BY Naziv;
END
GO

-- =============================================
-- SP: SrednjaSkola
-- =============================================
CREATE PROCEDURE spDajSveSkole
AS
BEGIN
    SET NOCOUNT ON;
    SELECT SkolaID, Naziv, Adresa, Grad, Telefon, DatumOsnivanja FROM SrednjaSkola ORDER BY Naziv;
END
GO

-- =============================================
-- SP: Kandidat
-- =============================================
CREATE PROCEDURE spDajSveKandidate
AS
BEGIN
    SET NOCOUNT ON;
    SELECT k.KandidatID, k.Ime, k.Prezime,
           k.Ime + ' ' + k.Prezime AS ImePrezime,
           k.JMBG, k.DatumRodjenja, k.VrstaUpisa, k.Aktivan,
           k.SkolaID, k.ProfilID,
           s.Naziv AS SkolaNaziv,
           p.Naziv AS ProfilNaziv, p.Sifra AS ProfilSifra,
           DATEDIFF(YEAR, k.DatumRodjenja, GETDATE()) AS Starost
    FROM Kandidat k
    INNER JOIN SrednjaSkola s     ON k.SkolaID  = s.SkolaID
    INNER JOIN ObrazovniProfil p  ON k.ProfilID = p.ProfilID
    ORDER BY k.Prezime, k.Ime;
END
GO

CREATE PROCEDURE spDajKandidataPoId
    @KandidatID INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT * FROM Kandidat WHERE KandidatID = @KandidatID;
END
GO

CREATE PROCEDURE spDodajKandidata
    @Ime           NVARCHAR(50),
    @Prezime       NVARCHAR(50),
    @JMBG          NVARCHAR(13),
    @DatumRodjenja DATE,
    @SkolaID       INT,
    @ProfilID      INT,
    @VrstaUpisa    NVARCHAR(50),
    @Aktivan       BIT
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO Kandidat (Ime, Prezime, JMBG, DatumRodjenja, SkolaID, ProfilID, VrstaUpisa, Aktivan)
    VALUES (@Ime, @Prezime, @JMBG, @DatumRodjenja, @SkolaID, @ProfilID, @VrstaUpisa, @Aktivan);
END
GO

CREATE PROCEDURE spIzmeniKandidata
    @KandidatID    INT,
    @Ime           NVARCHAR(50),
    @Prezime       NVARCHAR(50),
    @JMBG          NVARCHAR(13),
    @DatumRodjenja DATE,
    @SkolaID       INT,
    @ProfilID      INT,
    @VrstaUpisa    NVARCHAR(50),
    @Aktivan       BIT
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE Kandidat SET
        Ime=@Ime, Prezime=@Prezime, JMBG=@JMBG,
        DatumRodjenja=@DatumRodjenja, SkolaID=@SkolaID,
        ProfilID=@ProfilID, VrstaUpisa=@VrstaUpisa, Aktivan=@Aktivan
    WHERE KandidatID=@KandidatID;
END
GO

CREATE PROCEDURE spObrisiKandidata
    @KandidatID INT
AS
BEGIN
    SET NOCOUNT ON;
    DELETE FROM PrilozeniNalaz WHERE PregledID IN (SELECT PregledID FROM LekarskiPregled WHERE KandidatID = @KandidatID);
    DELETE FROM LekarskiPregled WHERE KandidatID = @KandidatID;
    DELETE FROM Kandidat WHERE KandidatID = @KandidatID;
END
GO

-- =============================================
-- SP: LekarskiPregled
-- =============================================
CREATE PROCEDURE spDajSvePreglede
AS
BEGIN
    SET NOCOUNT ON;
    SELECT p.PregledID, p.DatumPregleda, p.ZdravstvenaUstanova, p.AdresaUstanove,
           p.TrajanjePregledaMinuta, p.BrojLekaraUKomisiji, p.Zavrsen, p.Napomena, p.KandidatID,
           k.Ime + ' ' + k.Prezime AS KandidatImePrezime,
           pr.Naziv AS ProfilNaziv
    FROM LekarskiPregled p
    INNER JOIN Kandidat k        ON p.KandidatID = k.KandidatID
    INNER JOIN ObrazovniProfil pr ON k.ProfilID   = pr.ProfilID
    ORDER BY p.DatumPregleda DESC;
END
GO

CREATE PROCEDURE spDajPreglededPoId
    @PregledID INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT * FROM LekarskiPregled WHERE PregledID = @PregledID;
END
GO

CREATE PROCEDURE spDodajPregled
    @KandidatID             INT,
    @DatumPregleda          DATETIME,
    @ZdravstvenaUstanova    NVARCHAR(200),
    @AdresaUstanove         NVARCHAR(200),
    @TrajanjePregledaMinuta INT,
    @BrojLekaraUKomisiji    INT,
    @Zavrsen                BIT,
    @Napomena               NVARCHAR(500) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO LekarskiPregled (KandidatID, DatumPregleda, ZdravstvenaUstanova, AdresaUstanove, TrajanjePregledaMinuta, BrojLekaraUKomisiji, Zavrsen, Napomena)
    VALUES (@KandidatID, @DatumPregleda, @ZdravstvenaUstanova, @AdresaUstanove, @TrajanjePregledaMinuta, @BrojLekaraUKomisiji, @Zavrsen, @Napomena);
END
GO

CREATE PROCEDURE spIzmeniPregled
    @PregledID              INT,
    @KandidatID             INT,
    @DatumPregleda          DATETIME,
    @ZdravstvenaUstanova    NVARCHAR(200),
    @AdresaUstanove         NVARCHAR(200),
    @TrajanjePregledaMinuta INT,
    @BrojLekaraUKomisiji    INT,
    @Zavrsen                BIT,
    @Napomena               NVARCHAR(500) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE LekarskiPregled SET
        KandidatID=@KandidatID, DatumPregleda=@DatumPregleda,
        ZdravstvenaUstanova=@ZdravstvenaUstanova, AdresaUstanove=@AdresaUstanove,
        TrajanjePregledaMinuta=@TrajanjePregledaMinuta, BrojLekaraUKomisiji=@BrojLekaraUKomisiji,
        Zavrsen=@Zavrsen, Napomena=@Napomena
    WHERE PregledID=@PregledID;
END
GO

CREATE PROCEDURE spObrisiPregled
    @PregledID INT
AS
BEGIN
    SET NOCOUNT ON;
    DELETE FROM PrilozeniNalaz WHERE PregledID = @PregledID;
    DELETE FROM LekarskiPregled WHERE PregledID = @PregledID;
END
GO

CREATE PROCEDURE spDajPreglededZaStampu
    @DatumOd  DATETIME = NULL,
    @DatumDo  DATETIME = NULL,
    @ProfilID INT      = NULL
AS
BEGIN
    SET NOCOUNT ON;
    SELECT p.PregledID          AS [ID],
           k.Ime + ' ' + k.Prezime AS [Kandidat],
           pr.Naziv             AS [Obrazovni profil],
           p.DatumPregleda      AS [Datum pregleda],
           p.ZdravstvenaUstanova AS [Ustanova],
           p.TrajanjePregledaMinuta AS [Trajanje (min)],
           p.BrojLekaraUKomisiji AS [Ljekara u komisiji],
           CASE WHEN p.Zavrsen = 1 THEN 'Da' ELSE 'Ne' END AS [Zavrsen]
    FROM LekarskiPregled p
    INNER JOIN Kandidat k         ON p.KandidatID = k.KandidatID
    INNER JOIN ObrazovniProfil pr ON k.ProfilID    = pr.ProfilID
    WHERE (@DatumOd IS NULL OR p.DatumPregleda >= @DatumOd)
      AND (@DatumDo IS NULL OR p.DatumPregleda <= @DatumDo)
      AND (@ProfilID IS NULL OR k.ProfilID = @ProfilID)
    ORDER BY p.DatumPregleda DESC;
END
GO

-- =============================================
-- SP: PrilozeniNalaz
-- =============================================
CREATE PROCEDURE spDajSveNalaze
AS
BEGIN
    SET NOCOUNT ON;
    SELECT n.NalazID, n.VrstaNalaza, n.DatumNalaza, n.Ustanova, n.Uredan, n.Napomena,
           n.PregledID,
           k.Ime + ' ' + k.Prezime AS KandidatImePrezime
    FROM PrilozeniNalaz n
    INNER JOIN LekarskiPregled p ON n.PregledID = p.PregledID
    INNER JOIN Kandidat k        ON p.KandidatID = k.KandidatID
    ORDER BY n.DatumNalaza DESC;
END
GO

CREATE PROCEDURE spDajNalazeZaPregled
    @PregledID INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT NalazID, VrstaNalaza, DatumNalaza, Ustanova, Uredan, Napomena
    FROM PrilozeniNalaz
    WHERE PregledID = @PregledID
    ORDER BY VrstaNalaza;
END
GO

CREATE PROCEDURE spDajNalazPoId
    @NalazID INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT * FROM PrilozeniNalaz WHERE NalazID = @NalazID;
END
GO

CREATE PROCEDURE spDodajNalaz
    @PregledID   INT,
    @VrstaNalaza NVARCHAR(150),
    @DatumNalaza DATE,
    @Ustanova    NVARCHAR(200),
    @Uredan      BIT,
    @Napomena    NVARCHAR(500) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO PrilozeniNalaz (PregledID, VrstaNalaza, DatumNalaza, Ustanova, Uredan, Napomena)
    VALUES (@PregledID, @VrstaNalaza, @DatumNalaza, @Ustanova, @Uredan, @Napomena);
END
GO

CREATE PROCEDURE spIzmeniNalaz
    @NalazID     INT,
    @PregledID   INT,
    @VrstaNalaza NVARCHAR(150),
    @DatumNalaza DATE,
    @Ustanova    NVARCHAR(200),
    @Uredan      BIT,
    @Napomena    NVARCHAR(500) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE PrilozeniNalaz SET
        PregledID=@PregledID, VrstaNalaza=@VrstaNalaza, DatumNalaza=@DatumNalaza,
        Ustanova=@Ustanova, Uredan=@Uredan, Napomena=@Napomena
    WHERE NalazID=@NalazID;
END
GO

CREATE PROCEDURE spObrisiNalaz
    @NalazID INT
AS
BEGIN
    SET NOCOUNT ON;
    DELETE FROM PrilozeniNalaz WHERE NalazID = @NalazID;
END
GO
