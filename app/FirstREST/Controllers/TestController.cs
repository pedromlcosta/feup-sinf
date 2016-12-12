using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Mvc.HTML;

namespace FirstREST.Controllers
{
    public class TestController : ApiController
    {

        // GET: /Employee/
          public ActionResult Index()
         {
                     ViewBag.EmployeeName = “Muhammad Hamza”;
                     ViewBag.Company = “Web Development Company”;
                     ViewBag.Address = “Dubai, United Arab Emirates”;                     
                     return View();
         }

        public HttpResponseMessage Post(ClientRegisterData data) {

            String testVar = "le success est tré bon";
            ViewBag.testVar = testVar;


            var response = Request.CreateResponse(
                       HttpStatusCode.OK, new {  });
            return response;
            return View("~/Views/Wherever/SomeDir/MyView.aspx")
        }
    }
}
