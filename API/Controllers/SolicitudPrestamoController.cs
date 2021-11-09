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
    public class SolicitudPrestamoController : ApiController
    {
        private INTERNET_BANKING_DW1_3C2021Entities db = new INTERNET_BANKING_DW1_3C2021Entities();

        // GET: api/SolicitudPrestamo
        public IQueryable<SolicitudPrestamo> GetSolicitudPrestamo()
        {
            return db.SolicitudPrestamo;
        }

        // GET: api/SolicitudPrestamo/5
        [ResponseType(typeof(SolicitudPrestamo))]
        public IHttpActionResult GetSolicitudPrestamo(int id)
        {
            SolicitudPrestamo solicitudPrestamo = db.SolicitudPrestamo.Find(id);
            if (solicitudPrestamo == null)
            {
                return NotFound();
            }

            return Ok(solicitudPrestamo);
        }

        // PUT: api/SolicitudPrestamo/5
        [ResponseType(typeof(SolicitudPrestamo))]
        public IHttpActionResult PutSolicitudPrestamo(SolicitudPrestamo solicitudPrestamo)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Entry(solicitudPrestamo).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SolicitudPrestamoExists(solicitudPrestamo.Codigo))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(solicitudPrestamo);
        }

        // POST: api/SolicitudPrestamo
        [ResponseType(typeof(SolicitudPrestamo))]
        public IHttpActionResult PostSolicitudPrestamo(SolicitudPrestamo solicitudPrestamo)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.SolicitudPrestamo.Add(solicitudPrestamo);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = solicitudPrestamo.Codigo }, solicitudPrestamo);
        }

        // DELETE: api/SolicitudPrestamo/5
        [ResponseType(typeof(SolicitudPrestamo))]
        public IHttpActionResult DeleteSolicitudPrestamo(int id)
        {
            SolicitudPrestamo solicitudPrestamo = db.SolicitudPrestamo.Find(id);
            if (solicitudPrestamo == null)
            {
                return NotFound();
            }

            db.SolicitudPrestamo.Remove(solicitudPrestamo);
            db.SaveChanges();

            return Ok(solicitudPrestamo);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool SolicitudPrestamoExists(int id)
        {
            return db.SolicitudPrestamo.Count(e => e.Codigo == id) > 0;
        }
    }
}