using System.ComponentModel.DataAnnotations.Schema;

namespace Boutique_en_ligne.Models
{
    public class Facture
    {
        public int? Id { get; set; }
        public int? nombre_article { get; set; }
        public float? montant_total { get; set; }
        public DateTime? date_achat { get; set; }


        public int UtilisateurId { get; set; }

        [ForeignKey("UtilisateurId")]
        public Utilisateur Utilisateur { get; set; }


        public ICollection<JeuVideo>? JeuxVideos { get; set; }
    }
}
