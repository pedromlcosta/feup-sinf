using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FirstREST
{
    public class ReviewReturn
    {

        public double average { get; set; }
        public int count { get; set; }
        public List<string>reviews  { get; set; }
    }
}
