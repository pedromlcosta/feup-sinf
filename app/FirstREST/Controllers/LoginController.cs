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

        string codCliente;

        //[System.Web.Services.WebMethod(EnableSession = true)]

        [WebMethod(EnableSession = true)]
        public HttpResponseMessage Post(LoginData data) {


            string email = data.email;
            string password = data.password;
            
            if (email != null && password != null) {

                //checks if login is correct
                int codCliente = login(email, password);

                if (codCliente > -1) {

                    //Check if Primavera has the same person that is trying to login
                    //TODO: written above + get person's name to store in session
                    //string name = primaveraGetClientName(codCliente);

                    // Session variables set here
                    System.Web.HttpContext.Current.Session["username"] = email;
                    System.Web.HttpContext.Current.Session["codCliente"] = codCliente;
                    //System.Web.HttpContext.Current.Session["name"] = name;




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
         * Returns -1 if login failed or clientCode if successful
         */
        private int login(string email, string password) {
            NpgsqlConnection conn = null;
            int postgresLogin = -1;
            bool primaveraUserExists = false;

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

                        //TODO: read codCliente from the selected account here
                        //codCliente = dr["codCliente"].ToString();

                        /*
                        for(int i = 0; i< dr.VisibleFieldCount; i++){
                            Debug.Write(dr[i].ToString() + "\n");
                        }
                         */

                        Debug.Write(dr["email"].ToString());
                        Debug.Write(" - " + dr["password"].ToString() + "\n");
                    }
                    postgresLogin = 1;
                } else {
                    Debug.Write("No Rows on this table");
                    postgresLogin = -1;
                }

                // TEST PRIMAVERA TO CHECK IF CLIENT EXISTS WITH THE codCliente
                // primaveraUserExists = checkUserExists(codCliente);


                return postgresLogin; // && primaveraUserExists;
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
