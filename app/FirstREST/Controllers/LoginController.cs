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

namespace FirstREST.Controllers {

    public class LoginController : ApiController, IRequiresSessionState {
        private string codCliente = null;
        private string clientName = null;
        private string userType = "cliente";

        //[System.Web.Services.WebMethod(EnableSession = true)]

        [WebMethod(EnableSession = true)]
        public HttpResponseMessage Post(LoginData data) {
            string email = data.email;
            string password = data.password;

            if (email != null && password != null) {
                //checks if login is correct
                int loggedIn = login(email, password);

                if (loggedIn > 0) {
                    Debug.Write("Logged in ");
                    //Check if Primavera has the same person that is trying to login
                    //TODO: written above + get person's name to store in session
                    //string name = primaveraGetClientName(codCliente);

                    // Session variables set here
                    System.Web.HttpContext.Current.Session["username"] = email;
                    System.Web.HttpContext.Current.Session["codCliente"] = codCliente;
                    System.Web.HttpContext.Current.Session["name"] = clientName;
                    System.Web.HttpContext.Current.Session["type"] = userType;

                    var response = Request.CreateResponse(
                       HttpStatusCode.OK, new { loggedIn = "true" });
                    string uri = Url.Link("DefaultApi", new { CodCliente = data.email });
                    //response.Headers.Location = new Uri(uri);
                    return response;
                } else {
                    return Request.CreateResponse(HttpStatusCode.BadRequest);
                }
            } else {
                // if email or password is null, immediately send bad request
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        /*
         * Returns 
         */
        private int login(string email, string password) {
            NpgsqlConnection conn = null;
            int login = 1;
            // bool primaveraUserExists = false;

            try {
                // Creates and opens connection to the postgres DB
                conn = ConnectionFactory.MakePostgresConnection();
                conn.Open();

                // Rudimentar way to check if user exists and password coincides
                string sql = "SELECT * FROM utilizador WHERE email= (:email) AND password= (:password);";

                // data adapter making request from our connection
                /*
                NpgsqlDataAdapter da = new NpgsqlDataAdapter(sql, conn);             
                NpgsqlCommand command = da.SelectCommand;
                da.Fill(ds);
                Debug.Write(ds.);
                 */

                NpgsqlCommand command = new NpgsqlCommand();
                command.Connection = conn;
                command.CommandText = sql;
                command.Parameters.Add(new NpgsqlParameter("email", DbType.String));
                command.Parameters.Add(new NpgsqlParameter("password", DbType.String));
                command.Parameters[0].Value = email == null ? "" : email;
                command.Parameters[1].Value = password == null ? "" : password;
                command.Prepare();
                NpgsqlDataReader dr = command.ExecuteReader();

                if (dr.HasRows) {

                    while (dr.Read()) {
                        codCliente = dr["primaveracode"].ToString();
                        userType = dr["type"].ToString();

                        /*
                        for(int i = 0; i< dr.VisibleFieldCount; i++){
                            Debug.Write(dr[i].ToString() + "\n");
                        }
                         */

                        Debug.Write(dr["email"].ToString());
                        Debug.Write(" - " + dr["password"].ToString() + "\n");
                    }
                } else {
                    Debug.Write("No Rows on this table");
                    return -1;
                }

                // login = -1 -> postgres failed   login = -2 -> primavera failed finding name
                // TEST PRIMAVERA TO CHECK IF CLIENT EXISTS WITH THE codCliente
                if (codCliente != null)
                    clientName = Lib_Primavera.PriIntegration.getClienteName(codCliente);
                else
                    login = -1;

                if (clientName == null)
                    login = -2;

                // Login success!
                if (login > 0) {
                    Debug.Write("Gonna login... checking client other stuff.");
                    Lib_Primavera.Model.Cliente cliente = Lib_Primavera.PriIntegration.GetCliente(codCliente);
                    if (cliente != null) {
                        Debug.Write("Postal Code: " + cliente.CodPostal);
                        Debug.Write("Telemovel: " + cliente.NumTelemovel);
                        System.Web.HttpContext.Current.Session["codPostal"] = cliente.CodPostal;
                        System.Web.HttpContext.Current.Session["numTelemovel"] = cliente.NumTelemovel;
                    }

                }



                return login; // && primaveraUserExists;
            } catch (Exception msg) {
                // something went wrong, and you wanna know why
                MessageBox.Show(msg.ToString());
                //throw;
                Debug.Write("Entered catch");
                return -1;
            } finally {
                if (conn != null)
                    conn.Close();

            }
        }
    }
}
