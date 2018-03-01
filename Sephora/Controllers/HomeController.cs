using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Sephora.Controllers
{
    public class HomeController : Controller
    {
        public async Task<ActionResult> Index()
        {
            return View();
        }


        public async Task<JsonResult> GetProducts()
        {
            try
            {
                var EmpInfo = new Products();
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));                      
                    HttpResponseMessage Res = await client.GetAsync("https://sephora-api-frontend-test.herokuapp.com/products"); 
                    if (Res.IsSuccessStatusCode)
                    {  
                        var EmpResponse = await Res.Content.ReadAsStringAsync();  
                        EmpInfo = JsonConvert.DeserializeObject<Products>(EmpResponse);
                    }
                }
                return Json(EmpInfo, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<ActionResult>Product(int? Id)
        {
            ViewBag.Id = Id;
            return View();
        }


        public async Task<JsonResult> GetProduct(int? Id)
        {
            try
            {
                var EmpInfo = new product();
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    HttpResponseMessage Res = await client.GetAsync("https://sephora-api-frontend-test.herokuapp.com/products/"+Id); 
                    if (Res.IsSuccessStatusCode)
                    {  
                        var EmpResponse = await Res.Content.ReadAsStringAsync(); 
                        EmpInfo = JsonConvert.DeserializeObject<product>(EmpResponse);
                    }
                }
                return Json(EmpInfo, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        
        

        public async Task<JsonResult> FilterByPrice(int? Lower, int? Upper)
        {
            try
            {
                var EmpInfo = new Products();
                var FilterdInfo = new List<data>();
               
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    HttpResponseMessage Res = await client.GetAsync("https://sephora-api-frontend-test.herokuapp.com/products?filter[price_lt]=" + Upper);
                    if (Res.IsSuccessStatusCode)
                    {
                        var EmpResponse = await Res.Content.ReadAsStringAsync();

                        EmpInfo = JsonConvert.DeserializeObject<Products>(EmpResponse);
                        FilterdInfo =  EmpInfo.data.Where(x => x.attributes.price > Lower).ToList();                   
                    }
                }
                return Json(FilterdInfo, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public async Task<JsonResult> GetByCatagory(string Catagory)
        {
            try
            {
                var EmpInfo = new Products();
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    HttpResponseMessage Res = await client.GetAsync("https://sephora-api-frontend-test.herokuapp.com/products?filter[category_eq]=" + Catagory);
                    if (Res.IsSuccessStatusCode)
                    {
                        var EmpResponse = await Res.Content.ReadAsStringAsync();
                        EmpInfo = JsonConvert.DeserializeObject<Products>(EmpResponse);
                    }
                }
                return Json(EmpInfo, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public async Task<JsonResult> GetSortBy(string Sortby)
        {
            try
            {
                var EmpInfo = new Products();
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    HttpResponseMessage Res = await client.GetAsync("https://sephora-api-frontend-test.herokuapp.com/products?sort=price(" + Sortby+")");
                    if (Res.IsSuccessStatusCode)
                    {
                        var EmpResponse = await Res.Content.ReadAsStringAsync();
                        EmpInfo = JsonConvert.DeserializeObject<Products>(EmpResponse);
                    }
                }
                return Json(EmpInfo, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public class Products
        {
            public List<data> data { get; set; }
            public links links { get; set; }
        }

        public class product
        {
            public data data { get; set; }           
        }

        public class data
        {
            public int id { get; set; }
            public string type { get; set; }
            public attributes attributes { get; set; }
        }

        public class links
        {
            public string self { get; set; }
            public string next { get; set; }
            public string last { get; set; }
        }

        public class attributes
        {
            public string name { get; set; }
            public bool? sold_out { get; set; }
            public string category { get; set; }
            public int price { get; set; }
            public bool? under_sale { get; set; }
            public int sale_price { get; set; }
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}