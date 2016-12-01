using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FirstREST.Lib_Primavera.Model
{
    public class Artigo
    {
        public string CodArtigo
        {
            get;
            set;
        }

        public string DescArtigo
        {
            get;
            set;
        }

        public double StockActual
        {
            get;
            set;
        }

        public string Marca
        {
            get;
            set;
        }


        public double PCPadrao
        {
            get;
            set;
        }
        public string familia
        {
            get;
            set;
        }
        public string subFamilia
        {
            get;
            set;
        }
         
        public string subFamiliaDesc
        {
            get;
            set;
        }
     
        public string familiaDesc
        {
            get;
            set;
        }

        public string IVA
        {
            get;
            set;
        }
        public string moeadaSymbol
        {
            get;
            set;
        }
    }
}