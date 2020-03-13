using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace TableTennis
{
    public partial class Factory
    {
        public Factory()
        {
            Blade = new HashSet<Blade>();
            Rubber = new HashSet<Rubber>();
        }

        public int Id { get; set; }
        [Display( Name = "Name")]
        [Required(ErrorMessage = "Поле не повинно бути порожнім")]
        [StringLength(50)]
        public string FactoryName { get; set; }
        [Display(Name = "Country")]
        public int CountryId { get; set; }

        public virtual Country Country { get; set; }
        public virtual ICollection<Blade> Blade { get; set; }
        public virtual ICollection<Rubber> Rubber { get; set; }
    }
}
