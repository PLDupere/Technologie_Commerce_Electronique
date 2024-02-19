﻿using System.ComponentModel.DataAnnotations.Schema;

namespace Boutique_en_ligne.Models
{
    public class JeuVideo
    {
        public JeuVideo()
        {
            Utilisateurs = new List<Utilisateur>();
        }

        public int? Id { get; set; }
        public string? titre { get; set; }
        public string? annee_sortie { get; set; }
        public string[] console { get; set; }
        public string[] genre { get; set; }
        public string[] editeur { get; set; }
        public string? pochette_jeu { get; set; }
        public string[] capture_ecran { get; set; }
        public float? prix_vente { get; set; }

        //many to many with Utilisateur
        public ICollection<Utilisateur> Utilisateurs { get; set; }
    }
}
