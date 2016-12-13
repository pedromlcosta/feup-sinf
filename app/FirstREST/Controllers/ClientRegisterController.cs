using System;
using System.Transactions;
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
        private string patternToMatch = "^\\s*(.{5})\\z";

        public HttpResponseMessage Post(ClientRegisterData data)
        {

            string name = data.name;
            string email = data.email;
            string password = data.password;
            string address = data.address;
            string nif = data.nif;
            string adminCode = data.adminCode;
            string type = "cliente";
            Debug.Write(name + "\n");
            Debug.Write(email + "\n");
            Debug.Write(password + "\n");
            Debug.Write(address + "\n");
            Debug.Write(nif + "\n");
            Debug.Write(adminCode + "\n");

            //Por agora até alguém alterar no pedido
            if (adminCode != null && System.Text.RegularExpressions.Regex.Match(adminCode, patternToMatch, System.Text.RegularExpressions.RegexOptions.IgnoreCase).Success)
            {
                type = "admin";
                Debug.Write("\n it passed the regex\n");
            }

            if (email != null && password != null && address != null && nif != null && name != null)
            {
                //checks if login is correct

                int registerStatus = register(email, password, name, address, nif, type);
                Debug.Write(name + "\n");
                Debug.Write(registerStatus);
                if (registerStatus == 1)
                {


                    var response = Request.CreateResponse(
                       HttpStatusCode.OK, new { registered = "true" });

                    //string uri = Url.Link("DefaultApi", new { CodCliente = data.email });
                    //response.Headers.Location = new Uri(uri);
                    return response;
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest);
                }

            }
            else
            {
                // if email or password is null, immediately send bad request
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

        }

        private int register(string email, string password, string name, string address, string nif, string type)
        {
            NpgsqlConnection conn = null;
            Debug.Write(name + "\n");
            try
            {
                // Creates and opens connection to the postgres DB
                conn = ConnectionFactory.MakePostgresConnection();
                conn.Open();
                NpgsqlTransaction transaction = conn.BeginTransaction();
                // Rudimentar way to check if user exists and password coincides
                try
                {
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
                    if (dr.HasRows)
                    {

                        while (dr.Read())
                        {

                            codCliente = (int)dr["code"];
                            Debug.Write("\nCodigo: " + codCliente.ToString());
                        }
                    }
                    else
                    {
                        transaction.Rollback();
                        transaction.Dispose();
                        return -1;
                    }
                    dr.Close();
                    // TEST PRIMAVERA TO CHECK IF CLIENT EXISTS WITH THE codCliente --  return the clienteName
                    int returnValue = Lib_Primavera.PriIntegration.registerCliente(codCliente.ToString(), email, name, address, nif);

                    if (returnValue > 0)
                    {
                        transaction.Commit();
                        transaction.Dispose();
                        Debug.Write("\nSHOULD BE WORKING\n");
                        return 1;
                    }
                    else
                    {
                        transaction.Rollback();
                        transaction.Dispose();
                    }

                    return -1;

                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    transaction.Dispose();
                    Debug.Write(ex);
                    return -1;
                }

            }
            catch (Exception msg)
            {
                // something went wrong, and you wanna know why
                Debug.Write("/n");
                Debug.Write(msg.StackTrace);
                //MessageBox.Show(msg.ToString());
                //throw;
                Debug.Write("Entered catch");

                //TODO: analyze error message to check if user already existed and give proper return
                return -1;
            }
            finally
            {
                if (conn != null)
                    conn.Close();

            }
        }

    }
}
