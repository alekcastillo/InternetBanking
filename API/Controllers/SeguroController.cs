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
    //[AllowAnonymous]
    public class SeguroController : ApiController
    {
        private INTERNET_BANKING_DW1_3C2021Entities db = new INTERNET_BANKING_DW1_3C2021Entities();

        // GET: api/
        public IQueryable<Seguro> GetSeguro()
        {
            return db.Seguro;
        }

        // GET: api/Seguro/5
        [ResponseType(typeof(Seguro))]
        public IHttpActionResult GetSeguro(int id)
        {
            Seguro seguro = db.Seguro.Find(id);
            if (seguro == null)
            {
                return NotFound();
            }

            return Ok(seguro);
        }

        // PUT: api/Seguro/5
        [ResponseType(typeof(Seguro))]
        public IHttpActionResult PutSeguro(Seguro seguro)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Entry(seguro).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SeguroExists(seguro.Codigo))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(seguro);
        }

        // POST: api/Seguro
        [ResponseType(typeof(Seguro))]
        public IHttpActionResult PostSeguro(Seguro seguro)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Seguro.Add(seguro);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = seguro.Codigo }, seguro);
        }

        // DELETE: api/Seguro/5
        [ResponseType(typeof(Seguro))]
        public IHttpActionResult DeleteSeguro(int id)
        {
            Seguro seguro = db.Seguro.Find(id);
            if (seguro == null)
            {
                return NotFound();
            }

            db.Seguro.Remove(seguro);
            db.SaveChanges();

            return Ok(seguro);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool SeguroExists(int id)
        {
            return db.Seguro.Count(e => e.Codigo == id) > 0;
        }
    }
}