using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FirstREST{
    public class ClientRegisterData {
        public String email { get; set; }
        public String name { get; set; }
        public String nif { get; set; }
        public String address { get; set; }
        public String password { get; set; }
        public String adminCode { get; set; }
    }
}