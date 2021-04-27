using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


#nullable disable

namespace Lab1Take3
{
    public partial class Side
    {
        public int Id { get; set; }
        [Display(Name = "Название стороны")]
        public string SideName { get; set; }
        public virtual ICollection<Character> Characters { get; set; }

    }
}
