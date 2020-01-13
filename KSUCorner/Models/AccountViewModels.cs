using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Globalization;
using System.Web.Mvc;


namespace KSUCorner.Models
{
    public class ExternalLoginConfirmationViewModel
    {
        [Required]
        [Display(Name = "Username")]
        public string UserName { get; set; }

        [Required]
        [Display(Name = "Email Address")]
        public string Email { get; set; }
    }

    public class ExternalLoginListViewModel
    {
        public string ReturnUrl { get; set; }
    }

    public class SendCodeViewModel
    {
        public string SelectedProvider { get; set; }
        public ICollection<System.Web.Mvc.SelectListItem> Providers { get; set; }
        public string ReturnUrl { get; set; }
        public bool RememberMe { get; set; }
    }

    public class VerifyCodeViewModel
    {
        [Required]
        public string Provider { get; set; }

        [Required]
        [Display(Name = "Code")]
        public string Code { get; set; }
        public string ReturnUrl { get; set; }

        [Display(Name = "Remember this browser?")]
        public bool RememberBrowser { get; set; }

        public bool RememberMe { get; set; }
    }

    public class ForgotViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class LoginViewModel
    {
        [Required(ErrorMessage = "Your Username is required.")]
        [Display(Name = "Username")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }

    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Your Username is required.")]
        [StringLength(30, ErrorMessage = "Usernames can be no more than 30 characters.")]
        [Display(Name = "Username")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Your First Name is required.")]
        [StringLength(30, ErrorMessage = "Names can be no more than 30 characters.")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Your Last Name is required.")]
        [StringLength(30, ErrorMessage = "Names can be no more than 30 characters.")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Your Email address is required.")]
        [EmailAddress(ErrorMessage = "This is not a valid Email Address.")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Your Password is required.")]
        [StringLength(50, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [System.ComponentModel.DataAnnotations.CompareAttribute("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "You must identify your gender.")]
        [Display(Name = "Gender")]
        public string Gender { get; set; }

        [Required(ErrorMessage = "Your birth month is required.")]
        [Display(Name = "Month")]
        public int Month { get; set; }

        [Required(ErrorMessage = "Your birth day is required.")]
        [Display(Name = "Day")]
        public int Day { get; set; }

        [Required(ErrorMessage = "Your birth year is required.")]
        [Display(Name = "Year")]
        public int Year { get; set; }

        [Display(Name = "Security Question", Description = "An optional question to answer when you forget your password.")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string SecurityQuestion { get; set; }

        [Display(Name = "Security Answer", Description = "The answer to your optional question when you forget your password.")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string SecurityAnswer { get; set; }

        [Required(ErrorMessage = "Your required to check off the Terms of Use Agreement.")]
        [Display(Name = "Terms Of Use")]
        [MustBeTrue(ErrorMessage = "You must Agree to the Terms of Use.")]
        public bool TermsOfUse { get; set; }
    }

    public class ResetPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Username")]
        public string UserName { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [System.ComponentModel.DataAnnotations.CompareAttribute("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }
    }

    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
    public sealed class QuestionNeedsAnswerAttribute : ValidationAttribute
    {
        private const string _defaultErrorMessage = "You need an answer for a Security Question.";
        private readonly object _typeId = new object();

        public QuestionNeedsAnswerAttribute(string questionProperty, string answerProperty)
            : base(_defaultErrorMessage)
        {
            QuestionProperty = questionProperty;
            AnswerProperty = answerProperty;
        }

        public string QuestionProperty { get; private set; }
        public string AnswerProperty { get; private set; }

        public override object TypeId
        {
            get
            {
                return _typeId;
            }
        }

        public override string FormatErrorMessage(string name)
        {
            return String.Format(CultureInfo.CurrentUICulture, ErrorMessageString);
        }

        public override bool IsValid(object value)
        {
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(value);
            object questionValue = properties.Find(QuestionProperty, true).GetValue(value);
            object answerValue = properties.Find(AnswerProperty, true).GetValue(value);
            if (String.IsNullOrEmpty((string)questionValue) && String.IsNullOrEmpty((string)answerValue))
                return true;
            return (questionValue != null && answerValue != null &&
                      ((questionValue.ToString().Length == 0 && answerValue.ToString().Length == 0) ||
                      (questionValue.ToString().Length > 0 && answerValue.ToString().Length > 0)));
        }
    }

    public class MustBeTrueAttribute : ValidationAttribute, IClientValidatable
    {
        public override bool IsValid(object value)
        {
            return value is bool && (bool)value;
        }

        // Implement IClientValidatable for client side Validation
        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            return new ModelClientValidationRule[] {
                new ModelClientValidationRule { ValidationType = "checkboxtrue",
                                                   ErrorMessage = this.ErrorMessage } };
        }
    }
}
