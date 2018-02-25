using System.ComponentModel.DataAnnotations;

namespace SurfergraphyApi.Models
{
    public class LikePhotoBindingModel
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