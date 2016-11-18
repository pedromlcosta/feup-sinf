using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FirstREST.Lib_Primavera.Model
{
    public class ArtigoArmazem
    {
            public string Armazem
            {
                get;
                set;
            }

            public string CodArtigo
            {
                get;
                set;
            }

            public double StockActual
            {
                get;
                set;
            }

            public double PCMedio
            {
                get;
                set;
            }
    }
}