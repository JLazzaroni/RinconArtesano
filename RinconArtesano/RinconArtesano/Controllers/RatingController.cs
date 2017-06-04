using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RinconArtesano.Models;
using Microsoft.AspNet.Identity;
using Datos;

namespace RinconArtesano.Controllers
{
    public class RatingController : Controller
    {
        private RinconArtesanoEntities db = new RinconArtesanoEntities();

        [Authorize]
        [HttpPost]
        public ActionResult SendRating(RatingViewModel model)
        {
            string userId = User.Identity.GetUserId();

            bool existsProduct = db.Ratings.Where(x => x.UsersId == userId && x.ProductId == model.ProductId).Any();
            bool existsExperience = db.Ratings.Where(x => x.UsersId == userId && x.ExperienceId == model.ExperienceId).Any();
            bool existsUsers = db.Ratings.Where(x => x.UsersId == userId && x.UsId == model.UsersId).Any();

            if (!existsProduct && !existsExperience && !existsUsers)
            {
                Ratings _rating = new Ratings();
                _rating.UsersId = userId;
                _rating.Rating = model.Rating;
                if (model.ProductId != 0)
                    _rating.ProductId = model.ProductId;
                else if (model.ExperienceId != 0)
                    _rating.ExperienceId = model.ExperienceId;
                else if (model.ExperienceId != 0)
                    _rating.UsId = model.UsersId;

                _rating.DateAdd = DateTime.Now;

                db.Ratings.Add(_rating);
                db.SaveChanges();
            }
            else
            {
                if (existsProduct)
                {
                    Ratings _rating = db.Ratings.Where(x => x.UsersId == userId &&x.ProductId == model.ProductId).Single();
                    _rating.Rating = model.Rating;
                    if (model.ProductId != 0)
                        _rating.ProductId = model.ProductId;
                    else if (model.ExperienceId != 0)
                        _rating.ExperienceId = model.ExperienceId;
                    else if (model.ExperienceId != 0)
                        _rating.UsId = model.UsersId;

                    _rating.DateAdd = DateTime.Now;
                    db.SaveChanges();
                }
                else if(existsExperience)
                {
                    Ratings _rating = db.Ratings.Where(x => x.UsersId == userId && x.ExperienceId == model.ExperienceId).Single();
                    _rating.Rating = model.Rating;
                    if (model.ProductId != 0)
                        _rating.ProductId = model.ProductId;
                    else if (model.ExperienceId != 0)
                        _rating.ExperienceId = model.ExperienceId;
                    else if (model.ExperienceId != 0)
                        _rating.UsId = model.UsersId;

                    _rating.DateAdd = DateTime.Now;
                    db.SaveChanges();
                }
                else if (existsUsers)
                {
                    Ratings _rating = db.Ratings.Where(x => x.UsersId == userId && x.UsId == model.UsersId).Single();
                    _rating.Rating = model.Rating;
                    if (model.ProductId != 0)
                        _rating.ProductId = model.ProductId;
                    else if (model.ExperienceId != 0)
                        _rating.ExperienceId = model.ExperienceId;
                    else if (model.ExperienceId != 0)
                        _rating.UsId = model.UsersId;

                    _rating.DateAdd = DateTime.Now;
                    db.SaveChanges();
                }
            }
            return Json(new { status = "OK" });
        }
    }
}