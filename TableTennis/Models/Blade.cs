using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace TableTennis
{
    public partial class Blade
    {
        public Blade()
        {
            Racket = new HashSet<Racket>();
        }

        public int Id { get; set; }
        [Display(Name = "Factory")]
        public int FactoryId { get; set; }
        [Display(Name = "Name")]
        [Required(ErrorMessage = "Поле не повинно бути порожнім")]
        [StringLength(50)]
        public string BladeName { get; set; }
        [Display(Name = "Carbon/All wood")]
        public bool Composite { get; set; }

        public virtual Factory Factory { get; set; }
        public virtual ICollection<Racket> Racket { get; set; }
    }
}
