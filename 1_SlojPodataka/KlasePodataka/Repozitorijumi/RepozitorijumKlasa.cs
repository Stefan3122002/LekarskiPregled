using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace KlasePodataka.Repozitorijumi
{
    public abstract class RepozitorijumKlasa<T> : IRepozitorijum<T> where T : class
    {
        protected LekarskiPreglediKontekst _ctx;
        protected DbSet<T> _skup;

        protected RepozitorijumKlasa(LekarskiPreglediKontekst kontekst)
        {
            _ctx  = kontekst;
            _skup = kontekst.Set<T>();
        }

        public virtual IEnumerable<T> DajSve()  => _skup.ToList();
        public virtual T              DajPoId(int id) => _skup.Find(id);
        public virtual void           Dodaj(T e)     => _skup.Add(e);
        public virtual void           Obrisi(int id) { var e = _skup.Find(id); if (e != null) _skup.Remove(e); }
        public virtual void           Sacuvaj()      => _ctx.SaveChanges();
        public abstract void          Izmeni(T entitet);
    }
}
