using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;

namespace SurfergraphyApi.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public string NickName { get; set; }
        public int Wave { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager, string authenticationType)
        {
            // authenticationType은 CookieAuthenticationOptions.AuthenticationType에 정의된 항목과 일치해야 합니다.
            var userIdentity = await manager.CreateIdentityAsync(this, authenticationType);
            // 여기에 사용자 지정 사용자 클레임 추가
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("SurfergraphyContext", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public System.Data.Entity.DbSet<SurfergraphyApi.Models.Photo> Photos { get; set; }

        public System.Data.Entity.DbSet<SurfergraphyApi.Models.UserPhoto> UserPhotoes { get; set; }

        public System.Data.Entity.DbSet<SurfergraphyApi.Models.PhotoSaveHistory> PhotoSaveHistories { get; set; }

        public System.Data.Entity.DbSet<SurfergraphyApi.Models.PhotoBuyHistory> PhotoBuyHistories { get; set; }

        public System.Data.Entity.DbSet<SurfergraphyApi.Models.LikePhoto> LikePhotos { get; set; }

        public System.Data.Entity.DbSet<SurfergraphyApi.Models.Purchase> Purchases { get; set; }

        public System.Data.Entity.DbSet<SurfergraphyApi.Models.AdminUser> AdminUsers { get; set; }

        public System.Data.Entity.DbSet<SurfergraphyApi.Models.Member> Members { get; set; }
    }
}