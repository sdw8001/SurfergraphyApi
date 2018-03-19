using System;
using System.ComponentModel.DataAnnotations;

namespace SurfergraphyApi.Models
{
    public class Member
    {
        [Key]
        public string Id { get; set; }
        public string Email { get; set; }
        public string JoinType { get; set; }
        public string LoginToken { get; set; }
        public string PushToken { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public int Grade { get; set; }
        public int Wave { get; set; }
        public bool Deleted { get; set; }
        public DateTime JoinDate { get; set; }
        public DateTime LoginDate { get; set; }
        public DateTime DeletedDate { get; set; }
    }
}