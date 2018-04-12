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
using SurfergraphyApi.Utils;

namespace SurfergraphyApi.Controllers
{
    //[Authorize]
    public class PhotoBuyHistoriesController : ApiController
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

        // GET: api/PhotoBuyHistories
        public IQueryable<PhotoBuyHistory> GetPhotoBuyHistories()
        {
            return db.PhotoBuyHistories;
        }

        // GET: api/PhotoBuyHistories/UserPhotos/{memberId}
        [Route("api/PhotoBuyHistories/UserPhotos/{memberId}")]
        public IQueryable<PhotoBuyHistory> GetPhotoBuyHistoryPhotos(string memberId)
        {
            return db.PhotoBuyHistories.Where(photo => photo.UserId == memberId);
        }

        // GET: api/PhotoBuyHistories/5
        [ResponseType(typeof(PhotoBuyHistory))]
        public async Task<IHttpActionResult> GetPhotoBuyHistory(int id)
        {
            PhotoBuyHistory photoBuyHistory = await db.PhotoBuyHistories.FindAsync(id);
            if (photoBuyHistory == null)
            {
                return NotFound();
            }

            return Ok(photoBuyHistory);
        }

        // PUT: api/PhotoBuyHistories/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutPhotoBuyHistory(int id, PhotoBuyHistory photoBuyHistory)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != photoBuyHistory.Id)
            {
                return BadRequest();
            }

            db.Entry(photoBuyHistory).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PhotoBuyHistoryExists(id))
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

        // POST: api/PhotoBuyHistories
        [ResponseType(typeof(PhotoBuyHistory))]
        public async Task<IHttpActionResult> PostPhotoBuyHistory(PhotoBuyHistoryBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var member = db.Members.Find(model.UserId);
            var photo = db.Photos.Find(model.PhotoId);
            if (photo == null)
            {
                // 구매사진 정보를 찾을 수 없습니다.                 
                var error = new HttpError();
                error["ErrorCode"] = ErrorCode.PhotoSaveHistory_NoPhoto;
                error.Message = "구매사진정보를 찾을 수 없습니다.";
                HttpResponseMessage response = Request.CreateErrorResponse(HttpStatusCode.BadRequest, error);

                return ResponseMessage(response);
            }
            if (member.Wave < photo.Wave)
            {
                // 구매에 필요한 Wave가 부족합니다.                    
                var error = new HttpError();
                error["ErrorCode"] = ErrorCode.PhotoSaveHistory_NoWave;
                error.Message = "구매에 필요한 Wave가 부족합니다.";
                HttpResponseMessage response = Request.CreateErrorResponse(HttpStatusCode.BadRequest, error);

                return ResponseMessage(response);
            }
            member.Wave = member.Wave - photo.Wave;
            var photoBuyHistory = new PhotoBuyHistory()
            {
                UserId = model.UserId,
                PhotoId = model.PhotoId,
                Wave = photo.Wave,
                Date = DateTime.Now,
                Paid = false,
                PaidDate = DateTime.Now
            };


            db.PhotoBuyHistories.Add(photoBuyHistory);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = photoBuyHistory.Id }, photoBuyHistory);
        }

        // DELETE: api/PhotoBuyHistories/5
        [ResponseType(typeof(PhotoBuyHistory))]
        public async Task<IHttpActionResult> DeletePhotoBuyHistory(int id)
        {
            PhotoBuyHistory photoBuyHistory = await db.PhotoBuyHistories.FindAsync(id);
            if (photoBuyHistory == null)
            {
                return NotFound();
            }

            db.PhotoBuyHistories.Remove(photoBuyHistory);
            await db.SaveChangesAsync();

            return Ok(photoBuyHistory);
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

        private bool PhotoBuyHistoryExists(int id)
        {
            return db.PhotoBuyHistories.Count(e => e.Id == id) > 0;
        }
    }
}