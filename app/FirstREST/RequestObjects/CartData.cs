using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FirstREST
{
    public class CartData
    {
        public String id { get; set; }
        public String name { get; set; }
        public String email{ get; set; }
        public String nif { get; set; }
        public String address { get; set; }
        public String postal { get; set; }
        public String payment { get; set; }
        public List<Purchase> products { get; set; }

    }
}