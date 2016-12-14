using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Diagnostics;
using FirstREST.RequestObjects;
using FirstREST.Lib_Primavera.Model;


namespace FirstREST.Lib_Primavera
{
    public class ArtigosController : ApiController
    {
        //
        // GET: /Artigos/

        public IEnumerable<Lib_Primavera.Model.Artigo> Get()
        {
            return Lib_Primavera.PriIntegration.ListaArtigos();
        }


        // GET api/artigo/5    
        public Artigo Get(string id)
        {
            Lib_Primavera.Model.Artigo artigo = Lib_Primavera.PriIntegration.GetArtigo(id);
            if (artigo == null)
            {
                throw new HttpResponseException(
                  Request.CreateResponse(HttpStatusCode.NotFound));
            }
            else
            {
                return artigo;
            }
        }
        public HttpResponseMessage POST(EditArtigoData data)
        {
            Debug.Write(data);
            if (data == null || data.isFieldNull() || !Lib_Primavera.PriIntegration.editArtigo(data))
                return Request.CreateResponse(HttpStatusCode.BadRequest);


            return Request.CreateResponse(HttpStatusCode.Accepted);

        }
        //Send from Body ou a partir do URL?
        /*   public string[] Get([FromUri] string[] ids) {
               List<Artigo> wishList = new List<Artigo>();
              /* foreach (string id in ids)
               {
                   wishList.Add(Get(id));
               }
               return wishList;
               return ids;
           }*/
    }
}

