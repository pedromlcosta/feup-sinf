using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FirstREST.Lib_Primavera.Model
{
    public class OrderStatus
    {
        public string idCabecDoc
        { get; set; }

        public string Estado
        { get; set; }

        public Boolean Anulado
        { get; set; }
        
        public Boolean Fechado
        { get; set; }

    }
}