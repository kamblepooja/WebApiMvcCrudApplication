using MVC.Models;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Web;
using System.Web.Mvc;

namespace MVC.Controllers
{
    public class EmployeController : Controller
    {
        // GET: Employe
        public ActionResult Index()
        {
            ServicePointManager.ServerCertificateValidationCallback = delegate (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
            mvcEmploye employe = new mvcEmploye();
            List<mvcEmploye> empList = new List<mvcEmploye>();
            HttpResponseMessage response = GlobalVariables.webapiClient.GetAsync("Employe").Result;
            if (response.IsSuccessStatusCode)
            {
                var readTask = response.Content.ReadAsAsync<List<mvcEmploye>>();
                readTask.Result.ForEach(emp => emp.imageString = Convert.ToBase64String(emp.image));
                readTask.Wait();
                empList = readTask.Result;
            }
            return View(empList);
        }

        //Get:Employe:AddOrEdit
        public ActionResult AddOrEdit(int id = 0)
        {
            if (id == 0)
                return View(new mvcEmploye());
            else
            {
                ServicePointManager.ServerCertificateValidationCallback = delegate (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
                HttpResponseMessage response = GlobalVariables.webapiClient.GetAsync("Employe/" + id.ToString()).Result;
                var readTask = response.Content.ReadAsAsync<mvcEmploye>();
                readTask.Result.imageString = Convert.ToBase64String(readTask.Result.image);

                readTask.Wait();
                return View(readTask.Result);
            }
        }

        //post:Employe:AddOrEdit
        [HttpPost]
        public ActionResult AddOrEdit(mvcEmploye emp)
        {
            if (emp.EmployeId == 0)
            {
                ServicePointManager.ServerCertificateValidationCallback = delegate (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
                HttpPostedFileBase img = null;
                if (Request.Files.Count > 0)
                {
                    if (Request.Files[0].ContentLength > 0)
                    {
                        img = Request.Files[0];
                    }
                }
                byte[] fileData = new byte[img.ContentLength];
                img.InputStream.ReadAsync(fileData, 0, img.ContentLength);
                emp.image = fileData;
                HttpResponseMessage response = GlobalVariables.webapiClient.PostAsJsonAsync("Employe", emp).Result;
                TempData["SuccessMessage"] = "Data Saved Successfully";
                return RedirectToAction("Index");
            }
            else
            {
                HttpPostedFileBase img = null;
                if (Request.Files.Count > 0)
                {
                    if (Request.Files[0].ContentLength > 0)
                    {
                        img = Request.Files[0];
                        byte[] fileData = new byte[img.ContentLength];
                        img.InputStream.ReadAsync(fileData, 0, img.ContentLength);
                        emp.image = fileData;
                    }
                }
                if (emp.image == null)
                {
                    HttpResponseMessage response = GlobalVariables.webapiClient.GetAsync("Employe/" + emp.EmployeId.ToString()).Result;
                    var readTask = response.Content.ReadAsAsync<mvcEmploye>();
                    emp.image = readTask.Result.image;
                }
                HttpResponseMessage response1 = GlobalVariables.webapiClient.PutAsJsonAsync("Employe/" + emp.EmployeId, emp).Result;
                TempData["SuccessMessage"] = "Data Updated Successfully";
                return RedirectToAction("Index");
            }
        }

        public ActionResult Details(int id = 0)
        {
            ServicePointManager.ServerCertificateValidationCallback = delegate (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
            HttpResponseMessage response = GlobalVariables.webapiClient.GetAsync("Employe/" + id.ToString()).Result;
            var readTask = response.Content.ReadAsAsync<mvcEmploye>();
            readTask.Result.imageString = Convert.ToBase64String(readTask.Result.image);
            readTask.Wait();
            return View(readTask.Result);
        }

        public ActionResult Delete(int id = 0)
        {
            ServicePointManager.ServerCertificateValidationCallback = delegate (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
            HttpResponseMessage response = GlobalVariables.webapiClient.DeleteAsync("Employe/" + id.ToString()).Result;
            TempData["SuccessMessage"] = "Data Deleted Successfully";
            return RedirectToAction("Index");
        }
    }
}