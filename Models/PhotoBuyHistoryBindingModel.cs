using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SurfergraphyApi.Models
{
    public class PhotoBuyHistoryBindingModel
    {
        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "사용자 아이디")]
        public string UserId { get; set; }

        [Required]
        [Display(Name = "사진 아이디")]
        public int PhotoId { get; set; }
    }
}