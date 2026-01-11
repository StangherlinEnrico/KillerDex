using System;

namespace KillerDex.Models
{
    public class Killer
    {
        public Guid Id { get; set; }
        public string Alias { get; set; }
        public DateTime DateAdded { get; set; }

        public Killer()
        {
            Id = Guid.NewGuid();
            DateAdded = DateTime.Now;
        }

        public override string ToString()
        {
            return $"{Alias}";
        }
    }
}