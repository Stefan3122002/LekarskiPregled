using System.Collections.Generic;
using KlasePodataka.Entiteti;

namespace KlasePodataka.Repozitorijumi
{
    public interface ILekarskiPregledRepozitorijum : IRepozitorijum<LekarskiPregledKlasa>
    {
        IEnumerable<LekarskiPregledKlasa> DajSaFilterom(string filter);
        LekarskiPregledKlasa              DajSaNalazima(int id);
    }
}
