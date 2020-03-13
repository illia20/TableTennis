using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace TableTennis
{
    public partial class Country
    {
        public Country()
        {
            Factory = new HashSet<Factory>();
            Player = new HashSet<Player>();
        }

        public int Id { get; set; }
        [Required(ErrorMessage = "Заповніть назву країни")]
        [Display(Name ="Name of the country")]
        [StringLength(50)]
        public string CountryName { get; set; }

        public virtual ICollection<Factory> Factory { get; set; }
        public virtual ICollection<Player> Player { get; set; }
    }
}
