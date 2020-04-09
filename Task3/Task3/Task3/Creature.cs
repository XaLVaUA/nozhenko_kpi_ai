using System;

namespace Task3
{
    public class Creature
    {
        public string Name { get; set; }

        public bool IsHumanEating { get; set; }

        public Creature(string name, bool isHumanEating)
        {
            Name = name;
            IsHumanEating = isHumanEating;
        }

        public Creature(Creature creature) : this(creature.Name, creature.IsHumanEating) { }

        protected bool Equals(Creature other)
        {
            return Name == other.Name && IsHumanEating == other.IsHumanEating;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Creature) obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Name, IsHumanEating);
        }
    }
}