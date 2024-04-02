using static System.Runtime.InteropServices.JavaScript.JSType;
using System.ComponentModel.DataAnnotations;

namespace ExpressVoitures.Data
{
    public class CarModel
    {
        [Key]
        public int Id { get; set; }
        public int MarqueId { get; set; }
        public string Nom { get; set; }
        public virtual CarBrand Marque { get; set; }
        public virtual ICollection<CarFinish> Finitions { get; set; }
    }
}
