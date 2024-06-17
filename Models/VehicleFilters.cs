namespace ExpressVoitures.Models
{
    public class VehicleFilters
    {
        public int? MarqueId { get; set; }
        public int? ModeleId { get; set; }
        public int? FinitionId { get; set; }
        public int? Annee { get; set; }
        public bool? Disponible { get; set; }
    }
}
