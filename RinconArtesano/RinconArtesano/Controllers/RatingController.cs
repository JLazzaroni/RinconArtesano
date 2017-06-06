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

            if (!existsProduct && !existsExperience)
            {
                Ratings _rating = new Ratings();
                _rating.UsersId = userId;
                _rating.Rating = model.Rating;
                if (model.ProductId != 0)
                    _rating.ProductId = model.ProductId;
                else if (model.ExperienceId != 0)
                    _rating.ExperienceId = model.ExperienceId;

                _rating.DateAdd = DateTime.Now;

                db.Ratings.Add(_rating);
                db.SaveChanges();
            }
            else
            {
                if (existsProduct)
                {
                    Ratings _rating = db.Ratings.Where(x => x.UsersId == userId && x.ProductId == model.ProductId).Single();
                    _rating.Rating = model.Rating;
                    if (model.ProductId != 0)
                        _rating.ProductId = model.ProductId;
                    else if (model.ExperienceId != 0)
                        _rating.ExperienceId = model.ExperienceId;

                    _rating.DateAdd = DateTime.Now;
                    db.SaveChanges();
                }
                else if (existsExperience)
                {
                    Ratings _rating = db.Ratings.Where(x => x.UsersId == userId && x.ExperienceId == model.ExperienceId).Single();
                    _rating.Rating = model.Rating;
                    if (model.ProductId != 0)
                        _rating.ProductId = model.ProductId;
                    else if (model.ExperienceId != 0)
                        _rating.ExperienceId = model.ExperienceId;

                    _rating.DateAdd = DateTime.Now;
                    db.SaveChanges();
                }
            }

            RatingViewModel _ratingModel = new RatingViewModel();
            if (model.ProductId != 0)
            {
                var datos = db.Ratings.Where(x => x.ProductId == model.ProductId).Select(x => x.Rating);
                Decimal puntos = datos.Any() ? datos.Sum() : 0;
                int cantidad = db.Ratings.Where(x => x.ProductId == model.ProductId).Select(x => x.Rating).Count();
                Decimal rating = cantidad > 0 ? (puntos / cantidad) : 0;

                _ratingModel.RatingPromedio = rating;
                _ratingModel.RatingSelect = db.Ratings.Where(x => x.ProductId == model.ProductId && x.UsersId == userId).Select(x => x.Rating).SingleOrDefault();
            }
            else if (model.ExperienceId != 0)
            {
                var datos = db.Ratings.Where(x => x.ExperienceId == model.ExperienceId).Select(x => x.Rating);
                Decimal puntos = datos.Any() ? datos.Sum() : 0;
                int cantidad = db.Ratings.Where(x => x.ExperienceId == model.ExperienceId).Select(x => x.Rating).Count();
                Decimal rating = cantidad > 0 ? (puntos / cantidad) : 0;

                _ratingModel.RatingPromedio = rating;
                _ratingModel.RatingSelect = db.Ratings.Where(x => x.ExperienceId == model.ExperienceId && x.UsersId == userId).Select(x => x.Rating).SingleOrDefault();
            }

            return PartialView("_Rating", _ratingModel);
        }
    }
}