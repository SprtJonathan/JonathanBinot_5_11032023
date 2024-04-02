using System.ComponentModel.DataAnnotations;

namespace ExpressVoitures.Data
{
    public class CarBrand
    {
        [Key]
        public int Id { get; set; }
        public string Nom { get; set; }
        public virtual ICollection<CarModel> Modeles { get; set; }
    }
}
