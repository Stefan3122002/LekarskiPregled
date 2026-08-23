using System.Data.Entity;
using KlasePodataka.Entiteti;

namespace KlasePodataka
{
    public class LekarskiPreglediKontekst : DbContext
    {
        public LekarskiPreglediKontekst()
            : base("name=LekarskiPreglediConn")
        {
            Database.SetInitializer<LekarskiPreglediKontekst>(null);
        }

        public DbSet<SrednjaSkolaKlasa>     Skole              { get; set; }
        public DbSet<ObrazovniProfilKlasa>  Profili            { get; set; }
        public DbSet<KandidatKlasa>         Kandidati          { get; set; }
        public DbSet<LekarskiPregledKlasa>  Pregledi           { get; set; }
        public DbSet<PrilozeniNalazKlasa>   PrilozeniNalazi    { get; set; }
        public DbSet<KorisnikKlasa>         Korisnici          { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SrednjaSkolaKlasa>().ToTable("SrednjaSkola").HasKey(s => s.SkolaID);
            modelBuilder.Entity<ObrazovniProfilKlasa>().ToTable("ObrazovniProfil").HasKey(p => p.ProfilID);
            modelBuilder.Entity<KorisnikKlasa>().ToTable("Korisnik").HasKey(k => k.KorisnikID);

            modelBuilder.Entity<KandidatKlasa>().ToTable("Kandidat").HasKey(k => k.KandidatID);
            modelBuilder.Entity<KandidatKlasa>()
                .HasRequired(k => k.Skola)
                .WithMany(s => s.Kandidati)
                .HasForeignKey(k => k.SkolaID);
            modelBuilder.Entity<KandidatKlasa>()
                .HasRequired(k => k.Profil)
                .WithMany(p => p.Kandidati)
                .HasForeignKey(k => k.ProfilID);

            modelBuilder.Entity<LekarskiPregledKlasa>().ToTable("LekarskiPregled").HasKey(p => p.PregledID);
            modelBuilder.Entity<LekarskiPregledKlasa>()
                .HasRequired(p => p.Kandidat)
                .WithMany(k => k.Pregledi)
                .HasForeignKey(p => p.KandidatID);

            modelBuilder.Entity<PrilozeniNalazKlasa>().ToTable("PrilozeniNalaz").HasKey(n => n.NalazID);
            modelBuilder.Entity<PrilozeniNalazKlasa>()
                .HasRequired(n => n.Pregled)
                .WithMany(p => p.PrilozeniNalazi)
                .HasForeignKey(n => n.PregledID)
                .WillCascadeOnDelete(true);

            base.OnModelCreating(modelBuilder);
        }
    }
}
