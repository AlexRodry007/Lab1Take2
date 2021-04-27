using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


#nullable disable

namespace Lab1Take3
{
    public partial class Battle
    {
        public Battle()
        {
            Characters = new HashSet<Character>();
        }

        public int Id { get; set; }

        [Display(Name = "Название боя")]
        public string BattleName { get; set; }
        [Display(Name = "Тип боя")]
        public int? BattleTypeId { get; set; }
        [Display(Name = "Описание боя")]
        public string BattleDescription { get; set; }

       

        public virtual BattleType BattleType { get; set; }

        public virtual ICollection<Character> Characters { get; set; }
    }
}
