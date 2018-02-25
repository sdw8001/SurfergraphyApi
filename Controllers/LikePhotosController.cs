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
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;

namespace SurfergraphyApi.Controllers
{
    [Authorize]
    public class LikePhotosController : ApiController
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

        // GET: api/LikePhotos
        public IQueryable<LikePhoto> GetLikePhotos()
        {
            return db.LikePhotos;
        }

        // GET: api/LikePhotos/UserPhotos
        [Route("api/LikePhotos/UserPhotos")]
        public IQueryable<LikePhoto> GetUserLikePhotos()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            return db.LikePhotos.Where(photo => photo.UserId == user.Id);
        }

        // GET: api/LikePhotos/UserPhotos/{photoId}
        [Route("api/LikePhotos/UserPhotos/{photoId}")]
        public IQueryable<LikePhoto> GetPhotoSaveHistoryPhoto(int photoId)
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            return db.LikePhotos.Where(photo => photo.UserId == user.Id).Where(photo => photo.PhotoId == photoId);
        }

        // GET: api/LikePhotos/5
        [ResponseType(typeof(LikePhoto))]
        public async Task<IHttpActionResult> GetPhotoSaveHistory(int id)
        {
            LikePhoto likePhoto = await db.LikePhotos.FindAsync(id);
            if (likePhoto == null)
            {
                return NotFound();
            }

            return Ok(likePhoto);
        }

        // PUT: api/LikePhotos/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutLikePhoto(int id, LikePhoto likePhoto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != likePhoto.Id)
            {
                return BadRequest();
            }

            db.Entry(likePhoto).State = EntityState.Modified;

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

        // POST: api/LikePhotos
        [ResponseType(typeof(LikePhoto))]
        public async Task<IHttpActionResult> PostPhotoSaveHistory(LikePhotoBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var likePhoto = new LikePhoto()
            {
                UserId = model.UserId,
                PhotoId = model.PhotoId,
                Date = DateTime.Now,
                Deleted = false
            };

            db.LikePhotos.Add(likePhoto);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = likePhoto.Id }, likePhoto);
        }

        // DELETE: api/LikePhotos/5
        [ResponseType(typeof(LikePhoto))]
        public async Task<IHttpActionResult> DeletePhotoSaveHistory(int id)
        {
            LikePhoto likePhoto = await db.LikePhotos.FindAsync(id);
            if (likePhoto == null)
            {
                return NotFound();
            }

            db.LikePhotos.Remove(likePhoto);
            await db.SaveChangesAsync();

            return Ok(likePhoto);
        }

        // DELETE: api/LikePhotos
        [ResponseType(typeof(LikePhoto))]
        public async Task<IHttpActionResult> DeletePhotoSaveHistory(LikePhotoBindingModel model)
        {
            LikePhoto likePhoto = await db.LikePhotos.Where(photo => photo.UserId == model.UserId).Where(photo => photo.PhotoId == model.PhotoId).FirstAsync();
            if (likePhoto == null)
            {
                return NotFound();
            }

            db.LikePhotos.Remove(likePhoto);
            await db.SaveChangesAsync();

            return Ok(likePhoto);
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
            return db.LikePhotos.Count(e => e.Id == id) > 0;
        }
    }
}