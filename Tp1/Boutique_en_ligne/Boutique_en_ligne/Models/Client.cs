using System.Diagnostics.CodeAnalysis;

namespace Boutique_en_ligne.Models
{
    public class Client : Utilisateur
    {
        public Client() {
            Factures = new List<Facture>();
            solde = 0;
            CartesCredit= new List<CarteCredit>();
        }

        public float? solde { get; set; }

        // One to many with CarteCredit
        public ICollection<CarteCredit> CartesCredit { get; set; } 

        //one to many with facture
        public ICollection<Facture> Factures { get; set; }
    }
}
