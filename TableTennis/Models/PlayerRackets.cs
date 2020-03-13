using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
namespace TableTennis
{
    public partial class PlayerRackets
    {
        public PlayerRackets()
        {
            GameRacket1 = new HashSet<Game>();
            GameRacket2 = new HashSet<Game>();
        }

        public int Id { get; set; }
        public int PlayerId { get; set; }
        public int RacketId { get; set; }
        [ForeignKey("RacketId")]
        public virtual Racket IdNavigation { get; set; }
        public virtual Player Player { get; set; }
        public virtual ICollection<Game> GameRacket1 { get; set; }
        public virtual ICollection<Game> GameRacket2 { get; set; }
    }
}
