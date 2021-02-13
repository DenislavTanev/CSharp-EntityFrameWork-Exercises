


namespace P03_FootballBetting.Data.Models
{
    using P03_FootballBetting.Data.Models.Enumerations;
    using System;
    using System.ComponentModel.DataAnnotations;

    public class Bet
    {
        [Key]
        public int BetId { get; set; }

        public decimal Amount { get; set; }

        public Prediction Prediction { get; set; }

        public DateTime DateTime { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

        public int GameId { get; set; }
        public Game Game { get; set; }
    }
}
