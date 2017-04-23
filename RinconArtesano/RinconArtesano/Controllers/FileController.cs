using RinconArtesano.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RinconArtesano.Controllers
{
    public class FileController : Controller
    {
        private RinconArtesanoEntities db = new RinconArtesanoEntities();
        //
        // GET: /File/
        public ActionResult Index(int id)
        {
             var fileToRetrieve = db.File.Find(id);
             return File(fileToRetrieve.Content, fileToRetrieve.ContentType);
        }
    }
}