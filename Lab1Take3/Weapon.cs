using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace Lab1Take3
{
    public partial class Weapon
    {
        public Weapon()
        {
            Characters = new HashSet<Character>();
        }

        public int Id { get; set; }
        [Display(Name = "Название оружия")]
        public string WeaponName { get; set; }
        [Display(Name = "Урон")]
        public int? WeaponDamageChange { get; set; }

        public virtual ICollection<Character> Characters { get; set; }
    }
}
