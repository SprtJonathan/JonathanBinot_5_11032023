using ExpressVoitures.Models;
using System.ComponentModel.DataAnnotations;

namespace ExpressVoitures.Data
{
    public class Vehicle
    {
        [Key]
        public int Id { get; set; }

        public string? CodeVIN { get; set; }

        [Required(ErrorMessage = "L'année du véhicule est requise.")]
        [YearRange(1990, ErrorMessage = "L'année doit être comprise entre 1990 et l'année en cours.")]
        public int Annee { get; set; }

        [Required(ErrorMessage = "La date d'achat est requise.")]
        [Display(Name = "Date d'achat")]
        public DateTime DateAchat { get; set; }

        [Required(ErrorMessage = "Le prix d'achat est requis.")]
        [RegularExpression(@"^\d+([.,]\d{1,2})?$", ErrorMessage = "Le prix d'achat doit être au format numérique avec au maximum 2 chiffres après la virgule.")]
        public decimal PrixAchat { get; set; }

        public string? Reparations { get; set; }
        public decimal? CoutsReparation { get; set; }

        [Required(ErrorMessage = "La date de disponibilité de vente est requise.")]
        [Display(Name = "Date de disponibilité de vente")]
        public DateTime DateDisponibiliteVente { get; set; }

        [Required(ErrorMessage = "Le prix de vente est requis.")]
        [RegularExpression(@"^\d+([.,]\d{1,2})?$", ErrorMessage = "Le prix de vente doit être au format numérique avec au maximum 2 chiffres après la virgule.")]
        public decimal PrixVente { get; set; }

        [Display(Name = "Date de vente")]
        [DataType(DataType.Date)]
        public DateTime? DateVente { get; set; }

        public virtual ICollection<CarImage> Images { get; set; } = new HashSet<CarImage>();

        [Required(ErrorMessage = "La marque du véhicule est requise.")]
        public int MarqueId { get; set; }

        [Required(ErrorMessage = "Le modèle du véhicule est requis.")]
        public int ModeleId { get; set; }

        [Required(ErrorMessage = "La finition du véhicule est requise.")]
        public int FinitionId { get; set; }

        public virtual CarBrand Marque { get; set; }
        public virtual CarModel Modele { get; set; }
        public virtual CarFinish Finition { get; set; }

        public string? Description { get; set; }

        public bool IsPublished { get; set; } = false;
    }
}
