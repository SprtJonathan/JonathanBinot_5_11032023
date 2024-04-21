using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExpressVoitures.Data
{
    public class CarBrand
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "Le nom de la marque est requis.")]
        [StringLength(50, ErrorMessage = "Le nom de la marque ne peut pas dépasser 50 caractères.")]
        public string Nom { get; set; }

        public virtual ICollection<CarModel> Modeles { get; set; } = new List<CarModel>();
    }
}
