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

            string name = data.name;
            string email = data.email;
            string password = data.password;
            string address = data.address;
            string nif = data.nif;
            string type = data.type;

            Debug.Write(name);
            Debug.Write(email);
            Debug.Write(password);
            Debug.Write(address);
            Debug.Write(nif);
            Debug.Write(type);

            //Por agora até alguém alterar no pedido
            type = "cliente";

            if (email != null && password != null && address != null && nif != null && name != null) {
                //checks if login is correct

                int registerStatus = register(email, password, name, address, nif,type);
                Debug.Write(registerStatus);
                if (registerStatus == 1) {
                    

                    var response = Request.CreateResponse(
                       HttpStatusCode.OK, new { registered = "true" });

                    //string uri = Url.Link("DefaultApi", new { CodCliente = data.email });
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

        private int register(string email, string password, string name, string address, string nif, string type)
        {
            NpgsqlConnection conn = null;
            int registered = -1;

            try {
                // Creates and opens connection to the postgres DB
                conn = ConnectionFactory.MakePostgresConnection();
                conn.Open();

                // Rudimentar way to check if user exists and password coincides
                string sql = "INSERT INTO Utilizador (email, password, type) VALUES (:email,:password, :type) RETURNING code;";

              
                NpgsqlCommand command = new NpgsqlCommand();
                command.Connection = conn;
                command.CommandText = sql;
                command.Parameters.Add(new NpgsqlParameter("email", DbType.String));
                command.Parameters.Add(new NpgsqlParameter("password", DbType.String));
                command.Parameters.Add(new NpgsqlParameter("type", DbType.String));
                command.Parameters[0].Value = email;
                command.Parameters[1].Value = password;
                command.Parameters[2].Value = type;
                command.Prepare();
                NpgsqlDataReader dr = command.ExecuteReader();


                int codCliente = 0;
                if (dr.HasRows) {

                    while (dr.Read()) {

                        codCliente = (int) dr["code"];
                        Debug.Write("Codigo: " + codCliente.ToString());
                    }
                    registered = 1;
                } else {
                }

                // TEST PRIMAVERA TO CHECK IF CLIENT EXISTS WITH THE codCliente --  return the clienteName
                if (Lib_Primavera.PriIntegration.registerCliente(codCliente.ToString(), email, name, address, nif) != null)
                    registered = 1;

                return registered; // && primaveraUserExists;
            } catch (Exception msg) {
                // something went wrong, and you wanna know why
                Debug.Write("/n");
                Debug.Write(msg.StackTrace);
                //MessageBox.Show(msg.ToString());
                //throw;
                Debug.Write("Entered catch");

                //TODO: analyze error message to check if user already existed and give proper return
                return -1;
            } finally {
                if (conn != null)
                    conn.Close();

            }
        }

    }
}
