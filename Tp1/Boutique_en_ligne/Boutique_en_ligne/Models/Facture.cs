namespace Boutique_en_ligne.Models
{
    public class Facture
    {
        public int? Id { get; set; }
        public int? nombre_article { get; set; }
        public float? montant_depense { get; set; }
        public int? nombre_facture { get; set; }

        //many to one client
        public int? ClientId { get; set; }
        public Client? Client { get; set; }
    }
}
