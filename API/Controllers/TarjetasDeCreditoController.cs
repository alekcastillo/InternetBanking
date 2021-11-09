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
    public class TarjetasDeCreditoController : ApiController
    {
        private INTERNET_BANKING_DW1_3C2021Entities db = new INTERNET_BANKING_DW1_3C2021Entities();

        // GET: api/TarjetasDeCredito
        public IQueryable<TarjetaDeCredito> GetTarjetaDeCredito()
        {
            return db.TarjetaDeCredito;
        }

        // GET: api/TarjetasDeCredito/5
        [ResponseType(typeof(TarjetaDeCredito))]
        public IHttpActionResult GetTarjetaDeCredito(int id)
        {
            TarjetaDeCredito TarjetaDeCredito = db.TarjetaDeCredito.Find(id);
            if (TarjetaDeCredito == null)
            {
                return NotFound();
            }

            return Ok(TarjetaDeCredito);
        }

        // PUT: api/TarjetasDeCredito/5
        [ResponseType(typeof(TarjetaDeCredito))]
        public IHttpActionResult PutTarjetaDeCredito(TarjetaDeCredito TarjetaDeCredito)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Entry(TarjetaDeCredito).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TarjetaDeCreditoExists(TarjetaDeCredito.Codigo))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(TarjetaDeCredito);
        }

        // POST: api/TarjetasDeCredito
        [ResponseType(typeof(TarjetaDeCredito))]
        public IHttpActionResult PostTarjetaDeCredito(TarjetaDeCredito TarjetaDeCredito)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.TarjetaDeCredito.Add(TarjetaDeCredito);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = TarjetaDeCredito.Codigo }, TarjetaDeCredito);
        }

        // DELETE: api/TarjetaDeCreditos/5
        [ResponseType(typeof(TarjetaDeCredito))]
        public IHttpActionResult DeleteTarjetaDeCredito(int id)
        {
            TarjetaDeCredito TarjetaDeCredito = db.TarjetaDeCredito.Find(id);
            if (TarjetaDeCredito == null)
            {
                return NotFound();
            }

            db.TarjetaDeCredito.Remove(TarjetaDeCredito);
            db.SaveChanges();

            return Ok(TarjetaDeCredito);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool TarjetaDeCreditoExists(int id)
        {
            return db.TarjetaDeCredito.Count(e => e.Codigo == id) > 0;
        }
    }
}