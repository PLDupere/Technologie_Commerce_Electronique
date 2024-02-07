using Microsoft.EntityFrameworkCore.Query;
using System.ComponentModel.DataAnnotations.Schema;

namespace Boutique_en_ligne.Models
{
    public enum Profil
    {
        Client,
        Vendeur,
    }
    public class Utilisateur
    {
        public Utilisateur() {
            JeuxVideos = new List<JeuVideo>();
        }

        public int? Id { get; set; }
        public string? nom { get; set; }
        public string? prenom { get; set; }
        public string? adresse_courriel { get; set; }
        public DateTime? date_naissance { get; set; }
        public string? ville { get; set; }
        public string? mot_de_passe { get; set; }
        public Profil? profil { get; set; }

        //many to many with JeuVideo
        public ICollection<JeuVideo> JeuxVideos { get; set; }
    }
}
