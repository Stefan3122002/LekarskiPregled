using System.Collections.Generic;

namespace KlasePodataka.Repozitorijumi
{
    public interface IRepozitorijum<T> where T : class
    {
        IEnumerable<T> DajSve();
        T              DajPoId(int id);
        void           Dodaj(T entitet);
        void           Izmeni(T entitet);
        void           Obrisi(int id);
        void           Sacuvaj();
    }
}
