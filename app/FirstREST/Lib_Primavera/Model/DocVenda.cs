using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FirstREST.Lib_Primavera.Model
{
    public class DocVenda
    {

        public string id
        {
            get;
            set;
        }

        public string Entidade
        {
            get;
            set;
        }
        public string numContribuinte
        {
            get;
            set;
        }
        public string Morada
        {
            get;
            set;
        }
       
        public string CodPostal
        {
            get;
            set;
        }
        
        public int NumDoc
        {
            get;
            set;
        }

        public DateTime Data
        {
            get;
            set;
        }

        public double TotalMerc
        {
            get;
            set;
        }

        public double TotalDesc
        {
            get;
            set;
        }

        public double TotalIva
        {
            get;
            set;
        }

        public List<Model.LinhaDocVenda> LinhasDoc

        {
            get;
            set;
        }
 

    }
}