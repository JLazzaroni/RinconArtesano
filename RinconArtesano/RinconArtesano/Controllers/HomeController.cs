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
            //var productos = db.Products.Include("Files").OrderBy(x => x.DateAdd).Take(3).ToList();
            List<Products> prod = db.Products.Include("Files").Where(x => x.DateNull == null).OrderBy(x => x.DateAdd).Take(3).ToList();//productos.Where(x => x.IsBlocked == false).ToList();//(x => x.DateNull == null).ToList();// & x.IsBlocked == false).ToList();//db.Products.Include("Files").Where(x => x.DateNull == null & x.IsBlocked == false).OrderBy(x => x.DateAdd).Take(3).ToList();
            ViewBag.Productss = prod;
            //var experiencias = db.Experiences.OrderBy(x => x.DateAdd).Take(3).ToList();
            List<Experiences> exper = db.Experiences.AsEnumerable().Where(x => x.IsBlocked.Equals("0")).OrderBy(x => x.DateAdd).Take(3).ToList();//experiencias.Where(x => x.IsBlocked == false).ToList();//(x => x.DateNull == null).ToList();// & x.IsBlocked == false).ToList();//db.Experiences.Where(x => x.DateNull == null & x.IsBlocked == false).OrderBy(x => x.DateAdd).Take(3).ToList();
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