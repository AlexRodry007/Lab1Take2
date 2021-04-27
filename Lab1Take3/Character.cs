using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


#nullable disable

namespace Lab1Take3
{
    public partial class Character
    {
        public int Id { get; set; }
        [Display(Name = "Имя")]
        public string CharacterName { get; set; }
        [Display(Name = "Уровень")]
        public int? CharacterLevel { get; set; }
        [Display(Name = "Урон")]
        public int? CharacterDamage { get; set; }
        [Display(Name = "Здоровье")]
        public int? CharacterHealth { get; set; }
        [Display(Name = "Расса")]
        public int? RaceId { get; set; }
        [Display(Name = "Оружие")]
        public int? WeaponId { get; set; }
        [Display(Name = "Броня")]
        public int? ArmorId { get; set; }
        [Display(Name = "Бой")]
        public int? BattleId { get; set; }
        [Display(Name = "Сторона")]
        public int? SideId { get; set; }

        public virtual Armor Armor { get; set; }
        public virtual Battle Battle { get; set; }
        public virtual Race Race { get; set; }
        public virtual Weapon Weapon { get; set; }
        public virtual Side Side { get; set; }

    }
}
