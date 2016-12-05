using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.SessionState;
using System.Web.Http;
using System.Web.Services;
using System.Web.UI;
using System.Windows.Forms;
using System.Data;
using Npgsql;

namespace FirstREST.Controllers
{
    public class ClientRegisterController : ApiController
    {
        public HttpResponseMessage Post(ClientRegisterData data) {
            
            string email = data.email;
            string password = data.password;
            string morada = data.morada;
            long nif = data.nif;

            if (email != null && password != null) {
                                
                    Debug.Write("Email is: " + email);

                    var response = Request.CreateResponse(
                       HttpStatusCode.OK, new { registered = "true" });
                    string uri = Url.Link("DefaultApi", new { CodCliente = data.email });
                    return response;
                
            } else {
                // if email or password is null, immediately send bad request
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

        }

    }
}
