using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using SurfergraphyApi.Models;

namespace SurfergraphyApi.Controllers
{
    //[Authorize] // 권한은 배포시에만 적용
    public class PhotographerController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/Photographers
        public IQueryable<AdminUser> GetPhotographers()
        {
            return db.AdminUsers;
        }

        // GET: api/Photographer/admin
        [ResponseType(typeof(AdminUser))]
        public async Task<IHttpActionResult> GetPhotographer(string id)
        {
            AdminUser photo = await db.AdminUsers.FindAsync(id);
            if (photo == null)
            {
                return NotFound();
            }

            return Ok(photo);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool PhotographerExists(string id)
        {
            return db.AdminUsers.Count(e => e.Id == id) > 0;
        }
    }
}