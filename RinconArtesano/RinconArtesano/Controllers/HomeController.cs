using RinconArtesano.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RinconArtesano.Controllers
{
    public class HomeController : Controller
    {
        private RinconArtesanoEntities db = new RinconArtesanoEntities();
        public ActionResult Index()
        {
            List<Products> prod = db.Products.Include("Files").OrderBy(x => x.DateAdd).ToList().Where(x => x.DateNull == null & x.IsBlocked == false).Take(3).ToList();
            ViewBag.Productss = prod;
            List<Experiences> exper = db.Experiences.OrderBy(x => x.DateAdd).ToList().Where(x => x.DateNull == null & x.IsBlocked == false).Take(3).ToList();
            ViewBag.Experiences = exper;

            return View();
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