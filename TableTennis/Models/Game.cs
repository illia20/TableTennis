using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TableTennis.Models;
namespace TableTennis
{
    public partial class Game
    {
        public int Id { get; set; }
        [Display(Name = "First player")]
        public int Player1Id { get; set; }
        [Display(Name = "Second player")]
        public int Player2Id { get; set; }
        [Display(Name = "Racket of first player")]
        public int Racket1Id { get; set; }
        [Display(Name = "Racket of second player")]
        public int Racket2Id { get; set; }
        [Display(Name = "Winner - first player")]
        public bool Result { get; set; }
        [StringLength(50)]
        [Display(Name = "Score")]
        [Required]
        public string Score { get; set; }
        [Display(Name = "Date")]
        [DataType(DataType.Date)]
        [ValidateDate]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime GameDate { get; set; }

        [Display(Name = "Перший гравець")]
        public virtual Player Player1 { get; set; }
        [Display(Name = "Другий гравець")]
        public virtual Player Player2 { get; set; }
        [Display(Name = "Ракетка першого")]
        public virtual PlayerRackets Racket1 { get; set; }
        [Display(Name = "Ракетка другого")]
        public virtual PlayerRackets Racket2 { get; set; }
    }
}
