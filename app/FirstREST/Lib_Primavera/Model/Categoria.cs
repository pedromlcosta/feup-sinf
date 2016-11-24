using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FirstREST.Lib_Primavera.Model
{
    public class Categoria
    {
        public string familiaCod
        {
            get;
            set;
        }

        public string familiaDesc
        {
            get;
            set;
        }
        public List<SubCategoria> subFamilias
        {
            get;
            set;
        }
        public void addSubFamilia(SubCategoria sub)
        {
            if (subFamilias == null)
                subFamilias = new List<SubCategoria>();

            subFamilias.Add(sub);
        }

    }
}