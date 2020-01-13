using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace KSUCorner.Models
{
    [MetadataType(typeof(ProfileMetadata))]
    public partial class Profile
    {
    }

    public class ProfileMetadata
    {
        public int AccountID { get; set; }

        public string UserName { get; set; }

        [Required]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        [StringLength(30, MinimumLength = 1,
                      ErrorMessage = "Must be between 1 and 30 characters")]
        public string FirstName { get; set; }

        [Required]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        [StringLength(30, MinimumLength = 1,
                      ErrorMessage = "Must be between 1 and 30 characters")]
        public string LastName { get; set; }

        [Required]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        [StringLength(50, MinimumLength = 1,
                      ErrorMessage = "Must be between 1 and 50 characters")]
        [EmailAddress(ErrorMessage = "Invalid Email address.")]
        public string Email { get; set; }

        public bool EmailConfirmed { get; set; }

        public bool EmailIsPublic { get; set; }

        public string Gender { get; set; }

        public bool GenderIsPublic { get; set; }

        public DateTime BirthDate { get; set; }

        public bool BirthDateIsPublic { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string AvatarPath { get; set; }

        public int AvatarWidth { get; set; }

        public int AvatarHeight { get; set; }

        public bool AvatarIsPublic { get; set; }

        [StringLength(300, ErrorMessage = "This entry must be no more than 300 characters")]
        public string Interests { get; set; }

        public bool InterestsIsPublic { get; set; }

        [StringLength(300, ErrorMessage = "This entry must be no more than 300 characters")]
        public string AboutMe { get; set; }

        public bool AboutMeIsPublic { get; set; }

        public DateTime LastUpdateDate { get; set; }
    }
}