using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FirstREST.Lib_Primavera.Model
{
    public class SubCategoria
    {
        public SubCategoria(string cod, string desc)
        {
            subFamiliaCod = cod;
            subFamiliaDesc = desc;
        }
          public string subFamiliaCod
        {
            get;
            set;
        }

          public string subFamiliaDesc
          {
              get;
              set;
          }
          public int ordem
          {
              get;
              set;
          }
    }
}