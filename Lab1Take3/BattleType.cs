using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


#nullable disable

namespace Lab1Take3
{
    public partial class BattleType
    {
        public int Id { get; set; }
        [Display(Name = "Название типа")]
        public string BattleTypeName { get; set; }
        [Display(Name = "Полученные уровни")]
        public int? LevelGain { get; set; }
        public virtual ICollection<Battle> Battles { get; set; }


    }
}
