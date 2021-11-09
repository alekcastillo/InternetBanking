using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using API.Models;

namespace API.Controllers
{
    [Authorize]
    public class PagosFavoritosController : ApiController
    {
        private INTERNET_BANKING_DW1_3C2021Entities db = new INTERNET_BANKING_DW1_3C2021Entities();

        // GET: api/PagosFavoritos
        public IQueryable<PagoFavorito> GetPagoFavorito()
        {
            return db.PagoFavorito;
        }

        // GET: api/PagosFavoritos/5
        [ResponseType(typeof(PagoFavorito))]
        public IHttpActionResult GetPagoFavorito(int id)
        {
            PagoFavorito PagoFavorito = db.PagoFavorito.Find(id);
            if (PagoFavorito == null)
            {
                return NotFound();
            }

            return Ok(PagoFavorito);
        }

        // PUT: api/PagosFavoritos/5
        [ResponseType(typeof(PagoFavorito))]
        public IHttpActionResult PutPagoFavorito(PagoFavorito PagoFavorito)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Entry(PagoFavorito).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PagoFavoritoExists(PagoFavorito.Codigo))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(PagoFavorito);
        }

        // POST: api/PagosFavoritos
        [ResponseType(typeof(PagoFavorito))]
        public IHttpActionResult PostPagoFavorito(PagoFavorito PagoFavorito)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.PagoFavorito.Add(PagoFavorito);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = PagoFavorito.Codigo }, PagoFavorito);
        }

        // DELETE: api/PagoFavoritos/5
        [ResponseType(typeof(PagoFavorito))]
        public IHttpActionResult DeletePagoFavorito(int id)
        {
            PagoFavorito PagoFavorito = db.PagoFavorito.Find(id);
            if (PagoFavorito == null)
            {
                return NotFound();
            }

            db.PagoFavorito.Remove(PagoFavorito);
            db.SaveChanges();

            return Ok(PagoFavorito);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool PagoFavoritoExists(int id)
        {
            return db.PagoFavorito.Count(e => e.Codigo == id) > 0;
        }
    }
}