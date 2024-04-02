using ExpressVoitures.Models;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace ExpressVoitures.Data
{
    public class AccountRight
    {
        public int Id { get; set; }
        public string AccountId { get; set; }
        public FeatureRights FeatureRight { get; set; }

        public virtual Account Account { get; set; }
    }
}
