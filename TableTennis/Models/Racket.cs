using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace TableTennis
{
    public partial class Racket
    {
        public int Id { get; set; }
        [Display(Name = "Blade")]
        public int BladeId { get; set; }
        [Display(Name = "FH Rubber")]
        public int FhrubberId { get; set; }
        [Display(Name = "BH Rubber")]
        public int BhrubberId { get; set; }

        
        [Display(Name = "Blade")]
        public virtual Blade Blade { get; set; }
        [Display(Name = "FH Rubber")]
        public virtual Rubber Fhrubber { get; set; }
        [Display(Name = "BH Rubber")]
        public virtual Rubber Bhrubber { get; set; }
        public virtual PlayerRackets PlayerRackets { get; set; }
    }
}
