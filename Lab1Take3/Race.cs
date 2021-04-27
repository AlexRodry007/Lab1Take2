using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


#nullable disable

namespace Lab1Take3
{
    public partial class Race
    {
        public Race()
        {
            Characters = new HashSet<Character>();
        }

        public int Id { get; set; }
        [Display(Name = "Название рассы")]
        public string RaceName { get; set; }
        [Display(Name = "Изменение урона")]
        public int? RaceDamageChange { get; set; }
        [Display(Name = "Изменение здоровья")]
        public int? RaceHealthChange { get; set; }

        public virtual ICollection<Character> Characters { get; set; }
    }
}
