using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SurfergraphyApi.Models
{
    public class AdminUser
    {        
        [Key]
        public string Id { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string NickName { get; set; }
        public string Tel { get; set; }
        public string Image { get; set; }
        public string Grade { get; set; }
        public bool Deleted { get; set; }
    }
}