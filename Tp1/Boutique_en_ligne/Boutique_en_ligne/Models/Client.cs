namespace Boutique_en_ligne.Models
{
    public class Client : Utilisateur
    {
        public Client() {
            Factures = new List<Facture>();
        }


        public float? solde { get; set; }

        //one to on with carte de credit
        public int? CarteId { get; set; }
        public CarteCredit? carteCredit { get; set; }

        //one to many with facture
        public ICollection<Facture> Factures { get; set; }
    }
}
