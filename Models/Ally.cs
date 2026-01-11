using System;

namespace KillerDex.Models
{
    public class Ally
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime DateAdded { get; set; }

        public Ally()
        {
            Id = Guid.NewGuid();
            DateAdded = DateTime.Now;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}