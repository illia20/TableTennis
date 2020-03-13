using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace TableTennis
{
    public partial class Player
    {
        public Player()
        {
            GamePlayer1 = new HashSet<Game>();
            GamePlayer2 = new HashSet<Game>();
            PlayerRackets = new HashSet<PlayerRackets>();
        }

        public int Id { get; set; }
        [Required(ErrorMessage = "Поле не повинно бути порожнім")]
        [Display(Name = "Ім'я, прізвище")]
        
        public string Name { get; set; }

        [Display(Name = "Країна")]
        public int CountryId { get; set; }
        [Display(Name = "Правша/шульга")]
        public bool Arm { get; set; }
        [Display(Name = "Країна")]
        public virtual Country Country { get; set; }
        public virtual ICollection<Game> GamePlayer1 { get; set; }
        public virtual ICollection<Game> GamePlayer2 { get; set; }
        public virtual ICollection<PlayerRackets> PlayerRackets { get; set; }
    }
}
