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
    public class OrderStatusController : ApiController
    {
       
        public IEnumerable<Lib_Primavera.Model.OrderStatus> Get()
        {
            return Lib_Primavera.PriIntegration.ListaOrderStatus();
        }

        
        // GET api/artigo/5    
        public OrderStatus Get(string id)
        {
            Lib_Primavera.Model.OrderStatus artigo = Lib_Primavera.PriIntegration.GetOrderStatus(id);
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

       
        public HttpResponseMessage Post(Lib_Primavera.Model.OrderStatus ord)
        {
            Lib_Primavera.Model.RespostaErro erro = new Lib_Primavera.Model.RespostaErro();
            erro = Lib_Primavera.PriIntegration.InsereOrderStatusObj(ord);

            if (erro.Erro == 0)
            {
                var response = Request.CreateResponse(
                   HttpStatusCode.Created, ord);
                string uri = Url.Link("DefaultApi", new { idCabecDoc = ord.idCabecDoc });
                response.Headers.Location = new Uri(uri);
                return response;
            }

            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

        }
    
    }
}
