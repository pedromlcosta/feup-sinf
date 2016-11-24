using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace FirstREST.Controllers
{
    public class CategoriaArtigo : ApiController
    {
        // GET api/<controller>
        public IEnumerable<Lib_Primavera.Model.CategoriaArtigo>  Get()
        {
            return null;
        }

        // GET api/<controller>/5, ID da categoria/familia
        public CategoriaArtigo Get(int familia)
        {
            return null;
        }

        // POST api/<controller>
        public HttpResponseMessage Post(Lib_Primavera.Model.CategoriaArtigo value)
        {
            return null;
        }

        // PUT api/<controller>/5
        public HttpResponseMessage Put(Lib_Primavera.Model.CategoriaArtigo value)
        {
            return null;
        }

        // DELETE api/<controller>/5 ID da categoria/familia
        public HttpResponseMessage Delete(int familia)
        {
            return null;
        }
    }
}