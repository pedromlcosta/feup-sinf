using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using FirstREST.Lib_Primavera.Model;

namespace FirstREST.Controllers
{
    public class CategoriaArtigoController : ApiController
    {
        // GET api/<controller>
        public IEnumerable<Lib_Primavera.Model.CategoriaArtigo>  Get()
        {
            return Lib_Primavera.PriIntegration.ListaCategoriaArtigos(); ;
        }

        // GET api/<controller>/5, ID da categoria/familia return artigos relacionados com ela
        public IEnumerable<Lib_Primavera.Model.CategoriaArtigo> Get(string familia)
        {
            System.IO.File.WriteAllText(@"C:\Users\Public\TestFolder\WriteText1.txt", "why you no call");
            IEnumerable<Lib_Primavera.Model.CategoriaArtigo> categoriaArtigos = Lib_Primavera.PriIntegration.GetCategoriaArtigos(familia);
            if (categoriaArtigos == null)
            {
                throw new HttpResponseException(
                  Request.CreateResponse(HttpStatusCode.NotFound));
            }
            else
            {
                return categoriaArtigos;
            }
        }

      
    }
}