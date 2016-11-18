using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using FirstREST.Lib_Primavera.Model;


namespace FirstREST.Controllers
{
    public class ArtigoArmazemController : ApiController
    {
        //
        // GET: /Artigos/

        public IEnumerable<Lib_Primavera.Model.ArtigoArmazem> Get(string id)
        {
            return Lib_Primavera.PriIntegration.GetArtigoArmazens(id);
        }

        /*
        // GET api/artigo/5    
        public ArtigoArmazem Get(string id)
        {
            Lib_Primavera.Model.ArtigoArmazem artigoArm = Lib_Primavera.PriIntegration.GetArtigoArmazens(id);
            if (artigo == null)
            {
                throw new HttpResponseException(
                  Request.CreateResponse(HttpStatusCode.NotFound));
            }
            else
            {
                return artigoArm;
            }
        }*/
    }
}
