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
using SurfergraphyApi.Utils;
using Microsoft.WindowsAzure.Storage.Blob;
using System.Web.Hosting;
using System.IO;
using Microsoft.WindowsAzure.Storage;

namespace SurfergraphyApi.Controllers
{
    //[Authorize] // 권한은 배포시에만 적용
    public class PhotosController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/Photo/UserPhotos/{userId}
        [Route("api/Photo/UserPhotos/{userId}")]
        public IQueryable<Photo> GetPhotoFromUserPhotoByUser(string userId)
        {
            return from photo in db.Photos
                   join userPhoto in db.UserPhotoes on photo.Id equals userPhoto.PhotoId
                   where userPhoto.UserId == userId
                   select photo;
        }

        // GET: api/Photo/LikePhotos/{userId}
        [Route("api/Photo/LikePhotos/{userId}")]
        public IQueryable<Photo> GetPhotoFromLikePhotoByUser(string userId)
        {
            return from photo in db.Photos
                   join likePhoto in db.LikePhotos on photo.Id equals likePhoto.PhotoId
                   where likePhoto.UserId == userId
                   select photo;
        }

        // GET: api/Photos
        [Route("api/Dates")]
        public async Task<IHttpActionResult> GetDates()
        {
            var photos = from photo in db.Photos
                         join buyPhoto in db.PhotoBuyHistories on photo.Id equals buyPhoto.PhotoId into o
                         from buyPhoto in o.DefaultIfEmpty()
                         where photo.Valid == true && photo.Expired == false && buyPhoto.UserId == null
                         select photo;

            List<string> dateStrings = new List<string>();
            List<PhotoDate> dates = new List<PhotoDate>();
            foreach (var photo in photos)
            {
                dateStrings.Add(photo.Date.ToString("yyyyMMdd"));
            }

            foreach(string dateString in dateStrings.Distinct())
            {
                PhotoDate photoDate = new PhotoDate();
                photoDate.DateString = dateString;
                dates.Add(photoDate);
            }

            return Ok(dates.Distinct().OrderByDescending(photoDate => photoDate.DateString));
        }

        // GET: api/Photos
        [Route("api/Dates/Place/{place}")]
        public async Task<IHttpActionResult> GetPlaceDates(string place)
        {
            var photos = from photo in db.Photos
                         join buyPhoto in db.PhotoBuyHistories on photo.Id equals buyPhoto.PhotoId into o
                         from buyPhoto in o.DefaultIfEmpty()
                         where photo.Valid == true && photo.Expired == false && buyPhoto.UserId == null && photo.Place == place
                         select photo;

            List<string> dateStrings = new List<string>();
            List<PhotoDate> dates = new List<PhotoDate>();
            foreach (var photo in photos)
            {
                dateStrings.Add(photo.Date.ToString("yyyyMMdd"));
            }

            foreach (string dateString in dateStrings.Distinct())
            {
                PhotoDate photoDate = new PhotoDate();
                photoDate.DateString = dateString;
                dates.Add(photoDate);
            }

            return Ok(dates.Distinct().OrderByDescending(photoDate => photoDate.DateString));
        }

        // GET: api/Photos
        public IQueryable<Photo> GetPhotos()
        {
            return db.Photos.Where(photo => photo.Valid == true && photo.Expired == false).OrderByDescending(photo => photo.Id);
        }

        // GET: api/Photos
        [Route("api/Photos/Date/{date}")]
        public IQueryable<Photo> GetDatePhotos(string date)
        {
            string strStartDate = date + "000000";
            string strEndDate = date + "235959";
            string format = "yyyyMMddHHmmss";
            DateTime startDate = DateTime.ParseExact(strStartDate, format, null);
            DateTime endDate = DateTime.ParseExact(strEndDate, format, null);
            var photos = from photo in db.Photos
                         join buyPhoto in db.PhotoBuyHistories on photo.Id equals buyPhoto.PhotoId into o
                         from buyPhoto in o.DefaultIfEmpty()
                         where photo.Valid == true && photo.Expired == false && buyPhoto.UserId == null && photo.Date >= startDate && photo.Date <= endDate
                         orderby photo.Id descending
                         select photo;
            return photos;
            //return db.Photos.Where(photo => photo.Valid == true && photo.Expired == false && photo.Date >= startDate && photo.Date <= endDate).OrderByDescending(photo => photo.Id);
        }

        // GET: api/Photos
        [Route("api/Photos/Place/{place}")]
        public IQueryable<Photo> GetPlacePhotos(string place)
        {
            var photos = from photo in db.Photos
                         join buyPhoto in db.PhotoBuyHistories on photo.Id equals buyPhoto.PhotoId into o
                         from buyPhoto in o.DefaultIfEmpty()
                         where photo.Valid == true && photo.Expired == false && buyPhoto.UserId == null && photo.Place == place
                         orderby photo.Id descending
                         select photo;
            return photos;
            //return db.Photos.Where(photo => photo.Valid == true && photo.Expired == false && photo.Place == place).OrderByDescending(photo => photo.Id);
        }

        // GET: api/Photos
        [Route("api/Photos/Place/Date/{place}/{date}")]
        public IQueryable<Photo> GetPlaceDatePhotos(string place, string date)
        {
            string strStartDate = date + "000000";
            string strEndDate = date + "235959";
            string format = "yyyyMMddHHmmss";
            DateTime startDate = DateTime.ParseExact(strStartDate, format, null);
            DateTime endDate = DateTime.ParseExact(strEndDate, format, null);
            var photos = from photo in db.Photos
                         join buyPhoto in db.PhotoBuyHistories on photo.Id equals buyPhoto.PhotoId into o
                         from buyPhoto in o.DefaultIfEmpty()
                         where photo.Valid == true && photo.Expired == false && buyPhoto.UserId == null && photo.Place == place && photo.Date >= startDate && photo.Date <= endDate
                         orderby photo.Id descending
                         select photo;
            return photos;
            //return db.Photos.Where(photo => photo.Valid == true && photo.Expired == false && photo.Place == place && photo.Date >= startDate && photo.Date <= endDate).OrderByDescending(photo => photo.Id);
        }

        // GET: api/Photos/5
        [ResponseType(typeof(Photo))]
        public async Task<IHttpActionResult> GetPhoto(int id)
        {
            Photo photo = await db.Photos.FindAsync(id);
            if (photo == null)
            {
                return NotFound();
            }

            return Ok(photo);
        }

        // PUT: api/Photos/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutPhoto(int id, Photo photo)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != photo.Id)
            {
                return BadRequest();
            }

            db.Entry(photo).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PhotoExists(id))
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

        // POST: api/Photos
        [ResponseType(typeof(Photo))]
        public async Task<IHttpActionResult> PostPhoto()
        {
            // api 로 파일등록부분인데 초기 photo 클래스로 구현테스트 한적이 있다. 해당부분 활용하려면 활용시점 photo 클래스 생성부분에 추가된 parameter 확인후 수정 필요함.
            // Generate a new filename for every new blob
            var fileName = Guid.NewGuid().ToString();
            var storageAccount = ConnectionManager.GetCloudStorageAcount();
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
            CloudBlobContainer photosContainer = blobClient.GetContainerReference("photos");

            string root = HostingEnvironment.MapPath("~");
            var streamProvider = new MultipartFormDataStreamProvider(root);

            try
            {
                await Request.Content.ReadAsMultipartAsync(streamProvider);

                foreach (var fileData in streamProvider.FileData)
                {
                    var blob = photosContainer.GetBlockBlobReference(fileName);

                    blob.Properties.ContentType = fileData.Headers.ContentType.MediaType;
                    using (var filestream = File.OpenRead(fileData.LocalFileName))
                    {
                        blob.UploadFromStream(filestream);
                    }
                    File.Delete(fileData.LocalFileName);

                    Photo photo = new Photo();

                    photo.Url = blob.Uri.AbsoluteUri;
                    photo.MimeType = blob.Properties.ContentType;
                    photo.Name = streamProvider.FormData["Name"];
                    photo.Place = streamProvider.FormData["Place"];
                    photo.Wave = Convert.ToInt32(streamProvider.FormData["Wave"]);
                    photo.Date = DateTime.Now;

                    db.Photos.Add(photo);
                    await db.SaveChangesAsync();

                    return CreatedAtRoute("DefaultApi", new { id = photo.Id }, photo);
                }
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
            return BadRequest();
        }

        // DELETE: api/Photos/5
        [ResponseType(typeof(Photo))]
        public async Task<IHttpActionResult> DeletePhoto(int id)
        {
            Photo photo = await db.Photos.FindAsync(id);
            if (photo == null)
            {
                return NotFound();
            }

            db.Photos.Remove(photo);
            await db.SaveChangesAsync();

            return Ok(photo);
        }

        // POST: api/Photos/DeleteExpired
        [HttpPost]
        [Route("api/Photos/DeleteExpired")]
        public IHttpActionResult DeleteExpired()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var expiredPhotos = from photo in db.Photos
                                join buyPhoto in db.PhotoBuyHistories on photo.Id equals buyPhoto.PhotoId into o
                                from buyPhoto in o.DefaultIfEmpty()
                                where buyPhoto.UserId == null && photo.ExpirationDate <= DateTime.Now && photo.Expired == false
                                select photo;

            // Retrieve storage account from connection string.
            CloudStorageAccount storageAccount = ConnectionManager.GetCloudStorageAcount();

            // Create the blob client.
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

            // Retrieve reference to a previously created container.
            CloudBlobContainer photosContainer = blobClient.GetContainerReference("photos");
            foreach (Photo expiredPhoto in expiredPhotos.ToList())
            {
                // Retrieve reference to a blob named "myblob".
                CloudBlockBlob profileBlockBlob = photosContainer.GetBlockBlobReference(expiredPhoto.Name);
                profileBlockBlob.DeleteIfExists();

                expiredPhoto.Expired = true;

                // 좋아요 포토 Deleted 처리
                var expiredLikePhotos = db.LikePhotos.Where(x => x.PhotoId == expiredPhoto.Id);
                foreach (LikePhoto expiredLikePhoto in expiredLikePhotos)
                {
                    expiredLikePhoto.Deleted = true;
                }

                // 유저 다운로드 사진 Deleted 처리
                var expiredUserPhotos = db.UserPhotoes.Where(x => x.PhotoId == expiredPhoto.Id);
                foreach (UserPhoto expiredUserPhoto in expiredUserPhotos)
                {
                    expiredUserPhoto.Deleted = true;
                }

                // 유저 저장 사진 Deleted 처리
                var expiredPhotoSaveHistories = db.PhotoSaveHistories.Where(x => x.PhotoId == expiredPhoto.Id);
                foreach (PhotoSaveHistory expiredPhotoSaveHistory in expiredPhotoSaveHistories)
                {
                    expiredPhotoSaveHistory.Deleted = true;
                }

                try
                {
                    db.SaveChanges();
                }

                catch (DbUpdateConcurrencyException)
                {
                }
            }
            return Ok();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool PhotoExists(int id)
        {
            return db.Photos.Count(e => e.Id == id) > 0;
        }
    }
}