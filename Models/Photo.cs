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
        public int PhotograperId { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public string Place { get; set; }
        public int Wave { get; set; }
        public string MimeType { get; set; }
        public int DimentionWidth { get; set; }
        public int DimentionHeight { get; set; }
        public int Resolution { get; set; }
        public DateTime Date { get; set; }
        public DateTime ExpirationDate { get; set; }
    }
}