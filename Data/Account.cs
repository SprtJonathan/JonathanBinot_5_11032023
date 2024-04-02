using ExpressVoitures.Models;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace ExpressVoitures.Data
{
    public class Account: IdentityUser
    {
        public override string Id { get => base.Id; set => base.Id = value; }
        [Required]
        [EmailAddress]
        public override string Email { get; set; }

        [Required]
        public override string PasswordHash { get; set; }

        public FeatureRights FeatureRights { get; set; }
    }
}
