using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Windows.Forms;
using System.Data;
using Npgsql;

namespace FirstREST.Controllers {
    public class LoginController : ApiController {

        DataSet ds = new DataSet();

        private bool login(string email, string password) {
            NpgsqlConnection conn = null;

            try {
                // Creates and opens connection to the postgres DB
                conn = ConnectionFactory.MakePostgresConnection();
                conn.Open();

                // quite complex sql statement
                string sql = "SELECT * FROM utilizador";
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
                command.Prepare();

                NpgsqlDataReader dr = command.ExecuteReader();

                if (dr.HasRows) {


                    while (dr.Read()) {
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
                }

                return true;

            } catch (Exception msg) {
                // something went wrong, and you wanna know why
                MessageBox.Show(msg.ToString());
                //throw;
                return false;
            } finally {
                if (conn != null)
                    conn.Close();

            }
        }


        public HttpResponseMessage Post(LoginData data) {

            
            string email = data.email;
            string password = data.password;

            if (email != null && password != null) {

                //checks if login is correct
                bool postgresLoggedIn = login(email, password);

                if (postgresLoggedIn) {
                    //Check if Primavera has the same person that is trying to login
                    //TODO: written above

                    var response = Request.CreateResponse(
                       HttpStatusCode.OK, new { loggedIn = "true" });
                    string uri = Url.Link("DefaultApi", new { CodCliente = data.email });
                    response.Headers.Location = new Uri(uri);
                    return response;
                } else {
                    return Request.CreateResponse(HttpStatusCode.BadRequest);
                }
            } else {
                if (data.email == "" || data.password == "")
                    return Request.CreateResponse(HttpStatusCode.BadRequest);
                else if (data.email == null)
                    return Request.CreateResponse(HttpStatusCode.Forbidden);
                else
                    return Request.CreateResponse(HttpStatusCode.InternalServerError);

            }

        }
    }
}
