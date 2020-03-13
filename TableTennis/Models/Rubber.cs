using System;
using System.Collections.Generic;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TableTennis
{
    public partial class Rubber
    {
        public Rubber()
        {
            RacketBhrubber = new HashSet<Racket>();
            RacketFhrubber = new HashSet<Racket>();
        }

        public int Id { get; set; }
        [Display(Name = "Factory")]
        public int FactoryId { get; set; }
        [Required]
        [StringLength(50)]
        [Display(Name = "Name")]
        public string RubberName { get; set; }
        [Required]
        [StringLength(50)]
        [Display(Name = "Type: inverted, long, short etc.")]
        public string Pimples { get; set; }
        [Display(Name = "Factory")]
        public virtual Factory Factory { get; set; }
        public virtual ICollection<Racket> RacketBhrubber { get; set; }
        public virtual ICollection<Racket> RacketFhrubber { get; set; }
    }
}
