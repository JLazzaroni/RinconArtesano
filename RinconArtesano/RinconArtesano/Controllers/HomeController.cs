﻿using RinconArtesano.Models;
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
            var productos = (from p in db.Products
                             where p.DateNull == null && p.IsBlocked == false
                             orderby p.DateAdd descending
                             select p);

            List<Products> prod = productos.Take(3).ToList();
            ViewBag.Productss = prod;

            var experiencias = (from e in db.Experiences
                                 where e.DateNull == null && e.IsBlocked == false
                                 orderby e.DateAdd descending
                                 select e);

            List<Experiences> exper = experiencias.Take(3).ToList();
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