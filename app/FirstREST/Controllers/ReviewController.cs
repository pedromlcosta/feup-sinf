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
    public class ReviewController : ApiController, IRequiresSessionState
    {
        [WebMethod(EnableSession = true)]
        public HttpResponseMessage Post(ReviewData data)
        {
            string cli = data.CodCliente;
            string art = data.CodArtigo;
            string text = data.text;
            int score = data.score;

            if (art != null && cli != null)
            {

                if (insertOrUpdReview == 0) 
                {
                    var response = Request.CreateResponse(
                       HttpStatusCode.OK);

                    return response;
                }
                else
                {
                    // if email or password is null, immediately send bad request
                    return Request.CreateResponse(HttpStatusCode.BadRequest);
                }

                    
            }
            else
            {
                // if email or password is null, immediately send bad request
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        /*
         * Returns 
         */
        private int insertOrUpdReview(string cli, string art,string text, int score)
        {
            bool prevReviewed;
            NpgsqlConnection conn = null;
            try
            {
                // Creates and opens connection to the postgres DB
                conn = ConnectionFactory.MakePostgresConnection();
                conn.Open();
                string secondSQL;
                int idCli=-1;
                int idArt=-1;
                string sql = "SELECT product.code,utilizador.code FROM product,utilizador WHERE product.primaveracode=:art AND utilizador.primaveracode=:cli";
                NpgsqlCommand command = new NpgsqlCommand();
                command.Connection = conn;
                command.CommandText = sql;
                command.Parameters.Add(new NpgsqlParameter("art", DbType.String));
                command.Parameters.Add(new NpgsqlParameter("cli", DbType.String));
                command.Parameters[0].Value = art;
                command.Parameters[1].Value = cli;
                command.Prepare();
                NpgsqlDataReader dr = command.ExecuteReader();
                if (dr.HasRows)
                {

                    while (dr.Read())
                    {
                        idCli = (int)dr["utilizador.code"];
                        idArt = (int)dr["product.code"];
                        Debug.Write("\nUser,product: " + idCli.ToString() + " " + idArt.ToString());
                    }
                    if (idCli != -1 && idArt != -1)
                        prevReviewed = true;
                }
                else
                {
                    prevReviewed = false;
                }
                dr.Close();

                if (prevReviewed) {
                    secondSQL = "INSERT INTO reviews (utilizador,productcode,text,review,score) VALUES (:idCli,:idArt,:text,:score) RETURNING code;";
                }
                else secondSQL = "UPDATE reviews SET review=:text AND score=:score WHERE reviews.utilizador=:idCli AND reviews.productcode=:idArt RETURNING code;";

                NpgsqlCommand command = new NpgsqlCommand();
                command.Connection = conn;
                command.CommandText = sql;
                command.Parameters.Add(new NpgsqlParameter("idCli", DbType.Int32));
                command.Parameters.Add(new NpgsqlParameter("idArt", DbType.Int32));
                command.Parameters.Add(new NpgsqlParameter("text", DbType.String));
                command.Parameters.Add(new NpgsqlParameter("score", DbType.Int16));
                command.Parameters[0].Value = idCli;
                command.Parameters[1].Value = idArt;
                command.Parameters[2].Value = text;
                command.Parameters[3].Value = score;
                command.Prepare();
                NpgsqlDataReader dr = command.ExecuteReader();
                int idRev = -1;
                if (dr.HasRows)
                {

                    while (dr.Read())
                    {

                        idRev = (int)dr["code"];
                        Debug.Write("\nCodigo: " + idRev.ToString());
                    }
                }
                else
                {
                    transaction.Rollback();
                    transaction.Dispose();
                    return -1;
                }
               
                dr.Close();


     
               transaction.Commit();
               transaction.Dispose();
               if (prevReviewed)
                   Debug.Write("\nUPDATED REVIEW\n");
               else Debug.Write("\nADDED REVIEW\n");
               return 1;
            }
            catch (Exception msg)
            {
                // something went wrong, and you wanna know why
                MessageBox.Show(msg.ToString());
                //throw;
                Debug.Write("Entered catch");
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
