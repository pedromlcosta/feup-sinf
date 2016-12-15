using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Helpers;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Data;
using Npgsql;


namespace FirstREST.Controllers {
    public class ImageUploadController : ApiController {


        //TIME TO ANALYZE IMAGE STUFF, UPLOAD IT AND SAVE URL ON PGSQL, YEAH!
        [HttpPost]
        public async Task<HttpResponseMessage> PostFormData() {
            
            {
                Dictionary<string, object> dict = new Dictionary<string, object>();
                try {

                    var httpRequest = HttpContext.Current.Request;
                    String filePath = HttpContext.Current.Server.MapPath("~/App_Data/" + "default.jpg"); //TODO: default image
                    String fileName = "";

                    String prodID = httpRequest.Form.Get("productID");
                    Trace.WriteLine("Prod ID: " + prodID);

                    foreach (string file in httpRequest.Files) {
                        HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created);

                        var postedFile = httpRequest.Files[file];
                        if (postedFile != null && postedFile.ContentLength > 0) {

                            int MaxContentLength = 1024 * 1024 * 1; //Size = 1 MB  

                            IList<string> AllowedFileExtensions = new List<string> { ".jpg", ".gif", ".png", ".jpeg" };
                            var ext = postedFile.FileName.Substring(postedFile.FileName.LastIndexOf('.'));
                            var extension = ext.ToLower();
                            if (!AllowedFileExtensions.Contains(extension)) {

                                var message = string.Format("Please Upload image of type .jpg,.gif,.png.");

                                dict.Add("error", message);
                                return Request.CreateResponse(HttpStatusCode.BadRequest, dict);
                            } else if (postedFile.ContentLength > MaxContentLength) {

                                var message = string.Format("Please Upload a file upto 1 mb.");

                                dict.Add("error", message);
                                return Request.CreateResponse(HttpStatusCode.BadRequest, dict);
                            } else {



                                filePath = HttpContext.Current.Server.MapPath("~/Images/" + postedFile.FileName);
                                fileName = postedFile.FileName;
                                postedFile.SaveAs(filePath);

                                var message1 = string.Format("Image Updated Successfully.");

                                uploadProductImg(prodID, fileName);

                                return Request.CreateResponse(HttpStatusCode.Created, new { imageURL = fileName });

                            }
                        }


                    }
                    var res = string.Format("Please Upload a image.");
                    dict.Add("error", res);
                    return Request.CreateResponse(HttpStatusCode.BadRequest, dict);
                } catch (Exception ex) {
                    var res = string.Format("some Message");
                    dict.Add("error", res);
                    return Request.CreateResponse(HttpStatusCode.NotFound, dict);
                }
            } 


            

            /*

            try {
                string root = HttpContext.Current.Server.MapPath("~/App_Data");
                var img = WebImage.GetImageFromRequest();
                if (img != null) {
                    Trace.WriteLine("Here");
                    root = HttpContext.Current.Server.MapPath("~/App_Data/image");
                    img.Save(root);
                    return Request.CreateErrorResponse(HttpStatusCode.OK, "");
                } else {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "");
                }
            } catch (Exception e){
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "");
            } catch (Throwable e){
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "");
            }
            
            // Check if the request contains multipart/form-data.
            if (!Request.Content.IsMimeMultipartContent()) {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }

           
            
            var provider = new MultipartFormDataStreamProvider(root);

            try {
                // Read the form data.
                await Request.Content.ReadAsMultipartAsync(provider);

                // This illustrates how to get the file names.
                foreach (MultipartFileData file in provider.FileData) {
                    Trace.WriteLine(file.Headers.ContentDisposition.FileName);
                    Trace.WriteLine("Server file path: " + file.LocalFileName);
                }

                

                return Request.CreateResponse(HttpStatusCode.OK);
            } catch (System.Exception e) {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e);
            }
             */
        }

        public static string getArtigoImg(string codArtigo) {
            string imgPath = "default.jpg";
            string sql = "SELECT img FROM  Product WHERE primaveraCode = :code;";
            NpgsqlConnection conn = null;
            // Creates and opens connection to the postgres DB
            conn = ConnectionFactory.MakePostgresConnection();
            conn.Open();
            // Rudimentar way to check if user exists and password coincides
            NpgsqlCommand command;
            try {
                command = new NpgsqlCommand();
                command.Connection = conn;
                command.CommandText = sql;
                command.Parameters.Add(new NpgsqlParameter("code", DbType.String));
                command.Parameters[0].Value = codArtigo;
                command.Prepare();
                NpgsqlDataReader dr = command.ExecuteReader();
                if (dr.HasRows) {
                    while (dr.Read()) {

                        imgPath = (string) dr["img"];
                        Debug.Write("\nImgPAath: " + imgPath);
                    }
                }
                dr.Close();
            } catch (Exception msg) {
                Debug.Write(msg);
                // something went wrong, and you wanna know why
            } finally {
                if (conn != null)
                    conn.Close();

            }
            return imgPath;
        }

        public static bool uploadProductImg(string codArtigo, string imgPath) {
            NpgsqlConnection conn = null;
            try {
                // Creates and opens connection to the postgres DB
                conn = ConnectionFactory.MakePostgresConnection();
                conn.Open();
                // Rudimentar way to check if user exists and password coincides
                NpgsqlCommand command;
                try {
                    string sql = "SELECT * FROM  Product WHERE primaveraCode = :code;";


                    command = new NpgsqlCommand();
                    command.Connection = conn;
                    command.CommandText = sql;
                    command.Parameters.Add(new NpgsqlParameter("code", DbType.String));
                    command.Parameters[0].Value = codArtigo;
                    command.Prepare();
                    NpgsqlDataReader dr = command.ExecuteReader();


                    if (dr.HasRows) {
                        dr.Close();
                        command = new NpgsqlCommand();
                        command.Connection = conn;
                        sql = "UPDATE Product SET img = :imgLink WHERE primaveraCode = :code;";
                        command.CommandText = sql;
                        command.Parameters.Add(new NpgsqlParameter("imgLink", DbType.String));
                        command.Parameters.Add(new NpgsqlParameter("code", DbType.String));
                        command.Parameters[0].Value = imgPath;
                        command.Parameters[1].Value = codArtigo;
                        command.Prepare();
                        if (command.ExecuteNonQuery() == 1)
                            return true;
                    } else {
                        //Insert
                        dr.Close();
                        sql = " INSERT INTO Product(primaveraCode,img) VALUES(:code,:img);";
                        command = new NpgsqlCommand();
                        command.Connection = conn;
                        command.CommandText = sql;
                        command.Parameters.Add(new NpgsqlParameter("code", DbType.String));
                        command.Parameters.Add(new NpgsqlParameter("img", DbType.String));
                        command.Parameters[0].Value = codArtigo;
                        command.Parameters[1].Value = imgPath;
                        command.Prepare();
                        if (command.ExecuteNonQuery() == 1)
                            return true;

                    }
                    return false;

                } catch (Exception ex) {
                    Debug.Write(ex);
                    return false;
                }

            } catch (Exception msg) {
                Debug.Write(msg);
                // something went wrong, and you wanna know why
            } finally {
                if (conn != null)
                    conn.Close();

            }
            return false;
        }
        
    }

    
}
