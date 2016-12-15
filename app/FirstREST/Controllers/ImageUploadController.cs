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

    }
}
