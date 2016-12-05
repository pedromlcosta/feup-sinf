using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FirstREST{
    public class ClientRegisterData {
        public String email { get; set; }
        public String morada { get; set; }
        public long nif { get; set; }
        public String password { get; set; }
    }
}