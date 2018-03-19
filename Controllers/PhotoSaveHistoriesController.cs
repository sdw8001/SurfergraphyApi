using System;
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
using Microsoft.AspNet.Identity.Owin;

namespace SurfergraphyApi.Controllers
{
    //[Authorize]
    public class PhotoSaveHistoriesController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private ApplicationUserManager _userManager;

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? Request.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        // GET: api/PhotoSaveHistories
        public IQueryable<PhotoSaveHistory> GetPhotoSaveHistories()
        {
            return db.PhotoSaveHistories;
        }

        // GET: api/PhotoSaveHistories/UserPhotos/{memberId}
        [Route("api/PhotoSaveHistories/UserPhotos/{memberId}")]
        public IQueryable<PhotoSaveHistory> GetPhotoSaveHistoryPhotos(string memberId)
        {
            return db.PhotoSaveHistories.Where(photo => photo.UserId == memberId);
        }

        // GET: api/PhotoSaveHistories/5
        [ResponseType(typeof(PhotoSaveHistory))]
        public async Task<IHttpActionResult> GetPhotoSaveHistory(int id)
        {
            PhotoSaveHistory photoSaveHistory = await db.PhotoSaveHistories.FindAsync(id);
            if (photoSaveHistory == null)
            {
                return NotFound();
            }

            return Ok(photoSaveHistory);
        }

        // PUT: api/PhotoSaveHistories/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutPhotoSaveHistory(int id, PhotoSaveHistory photoSaveHistory)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != photoSaveHistory.Id)
            {
                return BadRequest();
            }

            db.Entry(photoSaveHistory).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PhotoSaveHistoryExists(id))
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

        // POST: api/PhotoSaveHistories
        [ResponseType(typeof(PhotoSaveHistory))]
        public async Task<IHttpActionResult> PostPhotoSaveHistory(PhotoSaveHistoryBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var photoSaveHistory = new PhotoSaveHistory()
            {
                UserId = model.UserId,
                PhotoId = model.PhotoId,
                Date = DateTime.Now,
                Deleted = false
            };

            db.PhotoSaveHistories.Add(photoSaveHistory);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = photoSaveHistory.Id }, photoSaveHistory);
        }

        // DELETE: api/PhotoSaveHistories/5
        [ResponseType(typeof(PhotoSaveHistory))]
        public async Task<IHttpActionResult> DeletePhotoSaveHistory(int id)
        {
            PhotoSaveHistory photoSaveHistory = await db.PhotoSaveHistories.FindAsync(id);
            if (photoSaveHistory == null)
            {
                return NotFound();
            }

            db.PhotoSaveHistories.Remove(photoSaveHistory);
            await db.SaveChangesAsync();

            return Ok(photoSaveHistory);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }

            if (disposing && _userManager != null)
            {
                _userManager.Dispose();
                _userManager = null;
            }
            base.Dispose(disposing);
        }

        private bool PhotoSaveHistoryExists(int id)
        {
            return db.PhotoSaveHistories.Count(e => e.Id == id) > 0;
        }
    }
}