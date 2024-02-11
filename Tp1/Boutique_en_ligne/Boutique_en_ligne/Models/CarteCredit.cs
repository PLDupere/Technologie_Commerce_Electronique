namespace Boutique_en_ligne.Models
{
    public class CarteCredit
    {
        public int? Id { get; set; }
        public string? detenteur { get; set; }
        public string? numero { get; set; }
        public DateTime? date { get; set; }
        public string? numero_secret { get; set; }

        public int ClientId { get; set; }
        public Client? Client { get; set; }

    }
}
