namespace Boutique_en_ligne.Models
{
    public class Panier
    {
        public int Id { get; set; }
        public string UserId { get; set; } 
        public List<JeuVideo> Jeux { get; set; } 

    }
}
