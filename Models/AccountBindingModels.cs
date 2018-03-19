using System;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace SurfergraphyApi.Models
{
    // AccountController 작업에 대한 매개 변수로 사용된 모델입니다.

    public class AddExternalLoginBindingModel
    {
        [Required]
        [Display(Name = "외부 액세스 토큰")]
        public string ExternalAccessToken { get; set; }
    }

    public class ChangePasswordBindingModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "현재 암호")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "{0}은(는) {2}자 이상이어야 합니다.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "새 암호")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "새 암호 확인")]
        [Compare("NewPassword", ErrorMessage = "새 암호와 확인 암호가 일치하지 않습니다.")]
        public string ConfirmPassword { get; set; }
    }

    public class RegisterBindingModel
    {
        [Required]
        [Display(Name = "아이디")]
        public string Id { get; set; }

        [Required]
        [Display(Name = "전자 메일")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "가입 SNS 타입")]
        public string JoinType { get; set; }

        [Required]
        [Display(Name = "이름")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "프로필 ImageUrl")]
        public string ImageUrl { get; set; }
    }

    public class LoginBindingModel
    {
        [Required]
        [Display(Name = "아이디")]
        public string Id { get; set; }

        [Required]
        [Display(Name = "외부 액세스 토큰")]
        public string LoginToken { get; set; }
    }

    public class RegisterExternalBindingModel
    {
        [Required]
        [Display(Name = "전자 메일")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "닉네임")]
        public string NickName { get; set; }
    }

    public class RemoveLoginBindingModel
    {
        [Required]
        [Display(Name = "로그인 공급자")]
        public string LoginProvider { get; set; }

        [Required]
        [Display(Name = "공급자 키")]
        public string ProviderKey { get; set; }
    }

    public class SetPasswordBindingModel
    {
        [Required]
        [StringLength(100, ErrorMessage = "{0}은(는) {2}자 이상이어야 합니다.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "새 암호")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "새 암호 확인")]
        [Compare("NewPassword", ErrorMessage = "새 암호와 확인 암호가 일치하지 않습니다.")]
        public string ConfirmPassword { get; set; }
    }
}
