using System.ComponentModel.DataAnnotations;

namespace ExpressVoitures.Data
{
    public class Vehicle
    {
        [Key]
        public int Id { get; set; }
        public string CodeVIN { get; set; }
        public DateTime Annee { get; set; }
        public DateTime DateAchat { get; set; }
        public decimal PrixAchat { get; set; }
        public virtual ICollection<CarRepair> Reparations { get; set; } = new HashSet<CarRepair>();
        public DateTime DateDisponibiliteVente { get; set; }
        public decimal PrixVente { get; set; }
        public DateTime? DateVente { get; set; }
        public virtual ICollection<CarImage> Images { get; set; } = new HashSet<CarImage>();
        public int MarqueId { get; set; }
        public int ModeleId { get; set; }
        public virtual CarBrand Marque { get; set; }
        public virtual CarModel Modele { get; set; }
        public string Description { get; set; }
    }
}
