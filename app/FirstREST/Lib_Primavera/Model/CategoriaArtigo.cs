using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FirstREST.Lib_Primavera.Model
{
    public class CategoriaArtigo
    {
         public string familia
        {
            get;
            set;
        }
     
        public string familiaDesc
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
     
        public List<String> artigo
        {
            get;
            set;
        }
      
        public void addArtigo(string artigoToAdd)
        {

            if(artigoToAdd!=null)
             artigo.Add(artigoToAdd);
        }
    }
    
}