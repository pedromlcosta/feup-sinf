using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FirstREST
{
    public class ReviewReturn
    {
        public long count { get; set; }
        public Decimal average { get; set; }
        public List<string>reviews  { get; set; }
    }
}
