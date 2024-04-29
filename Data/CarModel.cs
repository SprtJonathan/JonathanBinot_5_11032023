using static System.Runtime.InteropServices.JavaScript.JSType;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExpressVoitures.Data
{
    public class CarModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int MarqueId { get; set; }

        [Required(ErrorMessage = "Le nom du modèle est requis.")]
        [StringLength(50, ErrorMessage = "Le nom du modèle ne peut pas dépasser 50 caractères.")]
        public string Nom { get; set; }

        [Required(ErrorMessage = "La marque du modèle est requise.")]
        [DisplayName("Marque")]
        public virtual CarBrand Marque { get; set; }

        public virtual ICollection<CarFinish> Finitions { get; set; } = new List<CarFinish>();
    }
}
