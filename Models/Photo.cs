using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SurfergraphyApi.Models
{
    public class Photo
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string PhotographerId { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public string Place { get; set; }
        public int Wave { get; set; }
        public string MimeType { get; set; }
        public int DimensionWidth { get; set; }
        public int DimensionHeight { get; set; }
        public int Resolution { get; set; }
        public int TotalCount { get; set; }
        public DateTime Date { get; set; }
        public DateTime ExpirationDate { get; set; }
        public bool Valid { get; set; }
        public bool Expired { get; set; }

        public Photo()
        {
            Valid = true;
            Expired = false;
        }
    }
}