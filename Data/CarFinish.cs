using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ExpressVoitures.Data
{
    public class CarFinish
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [DisplayName("Modèle")]
        public int ModeleId { get; set; }

        [Required(ErrorMessage = "Le nom de la finition est requis.")]
        [StringLength(50, ErrorMessage = "Le nom de la finition ne peut pas dépasser 50 caractères.")]
        public string Nom { get; set; }

        public virtual CarModel Modele { get; set; }
    }
}
