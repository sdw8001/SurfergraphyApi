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

        // GET: api/Photos
        public IQueryable<Photo> GetPhotos()
        {
            return db.Photos.OrderByDescending(photo => photo.Id);
        }

        // GET: api/Photos
        [Route("api/Photos/Place/{place}")]
        public IQueryable<Photo> GetPlacePhotos(string place)
        {
            return db.Photos.Where(photo => photo.Place == place).OrderByDescending(photo => photo.Id);
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
        [Route("api/Photos/DeleteExpired")]
        public IHttpActionResult DeleteExpired()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var expiredPhotos = db.Photos.Where(x => x.ExpirationDate <= DateTime.Now).Where(x => x.Expired == false);

            // Retrieve storage account from connection string.
            CloudStorageAccount storageAccount = ConnectionManager.GetCloudStorageAcount();

            // Create the blob client.
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

            // Retrieve reference to a previously created container.
            CloudBlobContainer photosContainer = blobClient.GetContainerReference("photos");
            foreach (Photo expiredPhoto in expiredPhotos) { 
                // Retrieve reference to a blob named "myblob".
                CloudBlockBlob profileBlockBlob = photosContainer.GetBlockBlobReference(expiredPhoto.Name);
                if (profileBlockBlob.DeleteIfExists())
                {
                    expiredPhoto.Expired = true;

                    var expiredLikePhotos = db.LikePhotos.Where(x => x.PhotoId == expiredPhoto.Id);
                    foreach (LikePhoto expiredLikePhoto in expiredLikePhotos)
                    {
                        expiredLikePhoto.Deleted = true;
                    }

                    var expiredUserPhotos = db.UserPhotoes.Where(x => x.PhotoId == expiredPhoto.Id);
                    foreach (UserPhoto expiredUserPhoto in expiredUserPhotos)
                    {
                        expiredUserPhoto.Deleted = true;
                    }

                    var expiredPhotoSaveHistories = db.PhotoSaveHistories.Where(x => x.PhotoId == expiredPhoto.Id);
                    foreach (PhotoSaveHistory expiredPhotoSaveHistory in expiredPhotoSaveHistories)
                    {
                        expiredPhotoSaveHistory.Deleted = true;
                    }
                }
            }

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
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