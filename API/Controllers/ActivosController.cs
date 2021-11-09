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
    public class ActivosController : ApiController
    {
        private INTERNET_BANKING_DW1_3C2021Entities db = new INTERNET_BANKING_DW1_3C2021Entities();

        // GET: api/Activos
        public IQueryable<Activo> GetActivo()
        {
            return db.Activo;
        }

        // GET: api/Activos/5
        [ResponseType(typeof(Activo))]
        public IHttpActionResult GetActivo(int id)
        {
            Activo activo = db.Activo.Find(id);
            if (activo == null)
            {
                return NotFound();
            }

            return Ok(activo);
        }

        // PUT: api/Activos/5
        [ResponseType(typeof(Activo))]
        public IHttpActionResult PutActivo(Activo activo)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Entry(activo).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ActivoExists(activo.Codigo))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(activo);
        }

        // POST: api/Activos
        [ResponseType(typeof(Activo))]
        public IHttpActionResult PostActivo(Activo activo)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Activo.Add(activo);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = activo.Codigo }, activo);
        }

        // DELETE: api/Activos/5
        [ResponseType(typeof(Activo))]
        public IHttpActionResult DeleteActivo(int id)
        {
            Activo activo = db.Activo.Find(id);
            if (activo == null)
            {
                return NotFound();
            }

            db.Activo.Remove(activo);
            db.SaveChanges();

            return Ok(activo);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ActivoExists(int id)
        {
            return db.Activo.Count(e => e.Codigo == id) > 0;
        }
    }
}