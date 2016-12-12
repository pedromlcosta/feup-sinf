using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FirstREST
{
    public class CartData
    {
        public String id { get; set; }
        public String date { get; set; }
        public String address { get; set; }
        public List<Purchase> products { get; set; }

    }
}