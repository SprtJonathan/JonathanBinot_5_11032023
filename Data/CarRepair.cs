using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ExpressVoitures.Data
{
    public class CarRepair
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("Vehicle")]
        public int VehicleId { get; set; }
        public string Description { get; set; }
        public decimal CoutsReparation { get; set; }
    }
}
