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

            Debug.Write("\n"+cli + ";" + art + ";" + text + ";" + score.ToString()+"\n");

            if (art != null && cli != null)
            {

                if (insertOrUpdReview(cli,art,text,score) == 0)
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

        public ReviewReturn Get(string codArt)
        {
            if (codArt != null)
            {
                return getReviews(codArt);
            }
            else
            {
                return null;
            }
        }
       
        private int insertOrUpdReview(string cli, string art, string text, int score)
        {
            
            NpgsqlConnection conn = null;
            try
            {
                bool prevReviewed=false;
                // Creates and opens connection to the postgres DB
              
                conn = ConnectionFactory.MakePostgresConnection();
                conn.Open();
                NpgsqlTransaction transaction = conn.BeginTransaction();
               
                string secondSQL;
                int idCli = -1;
                int idArt = -1;
                string sql = "SELECT reviews.productcode,reviews.utilizador FROM reviews WHERE reviews.productcode=:art AND reviews.utilizador=:cli";
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
                    prevReviewed = true;
                }
                else
                {
                    prevReviewed = false;
                }
                dr.Close();
                
                if (prevReviewed==false)
                {
                    Debug.Write("\n" +"Not previously reviewed"+ "\n");
                    secondSQL = "INSERT INTO reviews (utilizador,productcode,review,score) VALUES (:cli,:art,:text,:score);";
                }
                else secondSQL = "UPDATE reviews SET review=:text,score=:score WHERE reviews.utilizador=:cli AND reviews.productcode=:art;";

                NpgsqlCommand command2 = new NpgsqlCommand();
                command2.Connection = conn;
                command2.CommandText = secondSQL;
                command2.Parameters.Add(new NpgsqlParameter("cli", DbType.String));
                command2.Parameters.Add(new NpgsqlParameter("art", DbType.String));
                command2.Parameters.Add(new NpgsqlParameter("text", DbType.String));
                command2.Parameters.Add(new NpgsqlParameter("score", DbType.Int16));
                command2.Parameters[0].Value = cli;
                command2.Parameters[1].Value = art;
                command2.Parameters[2].Value = text;
                command2.Parameters[3].Value = score;
                command2.Prepare();
                command2.ExecuteNonQuery();
                transaction.Commit();
                transaction.Dispose();
                if (prevReviewed)
                    Debug.Write("\nUPDATED REVIEW\n");
                else Debug.Write("\nADDED REVIEW\n");
                return 0;
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

        private ReviewReturn getReviews(string art){
            NpgsqlConnection conn = null;
            try
            {
                // Creates and opens connection to the postgres DB
                ReviewReturn revs = new ReviewReturn();
                revs.reviews = new List<string>();
                conn = ConnectionFactory.MakePostgresConnection();
                conn.Open();
                NpgsqlTransaction transaction = conn.BeginTransaction();
                string sql = "SELECT avg(score) as average,count(*) as count FROM reviews  WHERE productcode=:art;";
                NpgsqlCommand command = new NpgsqlCommand();
                command.Connection = conn;
                command.CommandText = sql;
                command.Parameters.Add(new NpgsqlParameter("art", DbType.String));
                command.Parameters[0].Value = art;
                command.Prepare();
                NpgsqlDataReader dr = command.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        Debug.Write("\nReading stuff\n");
                        revs.count=(Int64)dr["count"];
                        if (dr["average"] != System.DBNull.Value)
                            revs.average = (Decimal)dr["average"];
                        else revs.average = -1;
                        Debug.Write("\ncount/average " + revs.count.ToString()+"/"+revs.average.ToString());
                    }
                }
                dr.Close();
                
                string sql2 = "SELECT review FROM reviews WHERE productcode=:art;";
                NpgsqlCommand command2 = new NpgsqlCommand();
                command2.Connection = conn;
                command2.CommandText = sql2;
                command2.Parameters.Add(new NpgsqlParameter("art", DbType.String));
                command2.Parameters[0].Value = art;
                command2.Prepare();
                NpgsqlDataReader dr2 = command2.ExecuteReader();
                if (dr2.HasRows)
                {
                    while (dr2.Read())
                    {
                        revs.reviews.Add((string)dr2["review"]);
                        Debug.Write("\nreview: " + (string)dr2["review"]);
                    }
                }
                dr2.Close();
                
                return revs;
            }
            catch (Exception msg)
            {
                // something went wrong, and you wanna know why
                MessageBox.Show(msg.ToString());
                //throw;
                Debug.Write("Entered catch");
                return null;
            }
            finally
            {
                if (conn != null)
                    conn.Close();

            }
        }
    }
}
