using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace Lab1Take3
{
    public partial class Armor
    {
        public Armor()
        {
            Characters = new HashSet<Character>();
        }

        public int Id { get; set; }

        [Display(Name = "Название доспеха")]
        public string ArmorName { get; set; }
        [Display(Name = "Изменение здоровья")]
        public int? ArmorHealthChange { get; set; }

        public virtual ICollection<Character> Characters { get; set; }
    }
}
