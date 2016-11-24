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
        public IEnumerable<Lib_Primavera.Model.Categoria> Get()
        {
            return Lib_Primavera.PriIntegration.ListaCategorias(); ;
        }

        // GET api/<controller>/5, ID da categoria/familia return artigos relacionados com ela
        public CategoriaArtigo Get(string id)
        {
            Lib_Primavera.Model.CategoriaArtigo categoriaArtigos = Lib_Primavera.PriIntegration.GetCategoriaArtigos(id);
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