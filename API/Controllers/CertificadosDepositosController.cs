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
    public class CertificadosDepositosController : ApiController
    {
        private INTERNET_BANKING_DW1_3C2021Entities db = new INTERNET_BANKING_DW1_3C2021Entities();

        // GET: api/CertificadosDepositos
        public IQueryable<CertificadoDeposito> GetCertificadoDeposito()
        {
            return db.CertificadoDeposito;
        }

        // GET: api/CertificadosDepositos/5
        [ResponseType(typeof(CertificadoDeposito))]
        public IHttpActionResult GetCertificadoDeposito(int id)
        {
            CertificadoDeposito certificadoDeposito = db.CertificadoDeposito.Find(id);
            if (certificadoDeposito == null)
            {
                return NotFound();
            }

            return Ok(certificadoDeposito);
        }

        // PUT: api/CertificadosDepositos/5
        [ResponseType(typeof(CertificadoDeposito))]
        public IHttpActionResult PutCertificadoDeposito(CertificadoDeposito certificadoDeposito)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Entry(certificadoDeposito).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CertificadoDepositoExists(certificadoDeposito.Codigo))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(certificadoDeposito);
        }

        // POST: api/CertificadosDepositos
        [ResponseType(typeof(CertificadoDeposito))]
        public IHttpActionResult PostCertificadoDeposito(CertificadoDeposito certificadoDeposito)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.CertificadoDeposito.Add(certificadoDeposito);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = certificadoDeposito.Codigo }, certificadoDeposito);
        }

        // DELETE: api/CertificadosDepositos/5
        [ResponseType(typeof(CertificadoDeposito))]
        public IHttpActionResult DeleteCertificadoDeposito(int id)
        {
            CertificadoDeposito certificadoDeposito = db.CertificadoDeposito.Find(id);
            if (certificadoDeposito == null)
            {
                return NotFound();
            }

            db.CertificadoDeposito.Remove(certificadoDeposito);
            db.SaveChanges();

            return Ok(certificadoDeposito);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CertificadoDepositoExists(int id)
        {
            return db.CertificadoDeposito.Count(e => e.Codigo == id) > 0;
        }
    }
}