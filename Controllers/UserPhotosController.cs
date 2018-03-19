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

namespace SurfergraphyApi.Controllers
{
    //[Authorize]
    public class UserPhotosController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/UserPhotoes
        public IQueryable<UserPhoto> GetUserPhotoes()
        {
            return db.UserPhotoes;
        }

        // GET: api/UserPhotoes/5
        [ResponseType(typeof(UserPhoto))]
        public async Task<IHttpActionResult> GetUserPhoto(int id)
        {
            UserPhoto userPhoto = await db.UserPhotoes.FindAsync(id);
            if (userPhoto == null)
            {
                return NotFound();
            }

            return Ok(userPhoto);
        }

        // PUT: api/UserPhotoes/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutUserPhoto(int id, UserPhoto userPhoto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != userPhoto.Id)
            {
                return BadRequest();
            }

            db.Entry(userPhoto).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserPhotoExists(id))
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

        // POST: api/UserPhotoes
        [ResponseType(typeof(UserPhoto))]
        public async Task<IHttpActionResult> PostUserPhoto(UserPhotoBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var savedUserPhotos = db.UserPhotoes.Where(photo => photo.UserId == model.UserId).Where(photo => photo.PhotoId == model.PhotoId);
            if (savedUserPhotos.Count() > 0)
            {
                var savedPhoto = savedUserPhotos.ElementAt(0);

                if (model.PhotoSaveHistoryId != 0)
                    savedPhoto.PhotoSaveHistoryId = model.PhotoSaveHistoryId;
                if (model.PhotoBuyHistoryId != 0)
                    savedPhoto.PhotoBuyHistoryId = model.PhotoBuyHistoryId;


                await db.SaveChangesAsync();

                return CreatedAtRoute("DefaultApi", new { id = savedPhoto.Id }, savedPhoto);
            }
            else
            {

                var userPhoto = new UserPhoto()
                {
                    UserId = model.UserId,
                    PhotoId = model.PhotoId,
                    PhotoSaveHistoryId = model.PhotoSaveHistoryId,
                    PhotoBuyHistoryId = model.PhotoBuyHistoryId,
                    Date = DateTime.Now,
                    Deleted = false
                };
                db.UserPhotoes.Add(userPhoto);

                await db.SaveChangesAsync();

                return CreatedAtRoute("DefaultApi", new { id = userPhoto.Id }, userPhoto);
            }
        }

        // DELETE: api/UserPhotoes/5
        [ResponseType(typeof(UserPhoto))]
        public async Task<IHttpActionResult> DeleteUserPhoto(int id)
        {
            UserPhoto userPhoto = await db.UserPhotoes.FindAsync(id);
            if (userPhoto == null)
            {
                return NotFound();
            }

            db.UserPhotoes.Remove(userPhoto);
            await db.SaveChangesAsync();

            return Ok(userPhoto);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool UserPhotoExists(int id)
        {
            return db.UserPhotoes.Count(e => e.Id == id) > 0;
        }
    }
}