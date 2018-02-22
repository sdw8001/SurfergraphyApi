using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using SurfergraphyApi.Models;
using Microsoft.AspNet.Identity;

namespace SurfergraphyApi.Controllers
{
    public class PurchasesController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/Purchases
        public IQueryable<Purchase> GetPurchases()
        {
            return db.Purchases;
        }

        // GET: api/Purchases/5
        [ResponseType(typeof(Purchase))]
        public async Task<IHttpActionResult> GetPurchase(string id)
        {
            Purchase purchase = await db.Purchases.FindAsync(id);
            if (purchase == null)
            {
                return NotFound();
            }

            return Ok(purchase);
        }

        // PUT: api/Purchases/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutPurchase(string id, Purchase purchase)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != purchase.OrderId)
            {
                return BadRequest();
            }

            db.Entry(purchase).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PurchaseExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Purchases
        [ResponseType(typeof(Purchase))]
        public async Task<IHttpActionResult> PostPurchase(Purchase purchase)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Purchases.Add(purchase);

            int purchasedWave = 0;
            purchasedWave = Convert.ToInt16(purchase.ProductId.Replace("wave", ""));
            var user = db.Users.Find(User.Identity.GetUserId());
            user.Wave = user.Wave + purchasedWave;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (PurchaseExists(purchase.OrderId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = purchase.OrderId }, purchase);
        }

        // DELETE: api/Purchases/5
        [ResponseType(typeof(Purchase))]
        public async Task<IHttpActionResult> DeletePurchase(string id)
        {
            Purchase purchase = await db.Purchases.FindAsync(id);
            if (purchase == null)
            {
                return NotFound();
            }

            db.Purchases.Remove(purchase);
            await db.SaveChangesAsync();

            return Ok(purchase);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool PurchaseExists(string id)
        {
            return db.Purchases.Count(e => e.OrderId == id) > 0;
        }
    }
}