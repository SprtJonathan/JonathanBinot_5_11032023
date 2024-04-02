using System.ComponentModel.DataAnnotations;

namespace ExpressVoitures.Data
{
    public class CarFinish
    {
        [Key]
        public int Id { get; set; }
        public int ModeleId { get; set; }
        public string Nom { get; set; }
        public virtual CarModel Modele { get; set; }
    }
}
