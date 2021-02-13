﻿
namespace P03_FootballBetting.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Position
    {
        [Key]
        public int PositionId { get; set; }

        public string Name { get; set; }

        public ICollection<Player> Players => new HashSet<Player>();
    }
}
