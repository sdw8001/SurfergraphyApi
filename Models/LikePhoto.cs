using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SurfergraphyApi.Models
{
    public class LikePhoto
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string UserId { get; set; }
        public int PhotoId { get; set; }
        public int PhotoBuyHistoryId { get; set; }
        public bool Deleted { get; set; }
        public DateTime Date { get; set; }
    }
}