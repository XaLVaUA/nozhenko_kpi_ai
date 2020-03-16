using System;

namespace Task1
{
	public class Creature
	{
		public string Name { get; set; }

		public string RelatedName { get; set; }

		public bool Independent { get; set; }

		public Creature(string name, string relatedName, bool independent)
		{
			Name = name;
			RelatedName = relatedName;
			Independent = independent;
		}

		public Creature(Creature creature) : this(new string(creature.Name), new string(creature.RelatedName), creature.Independent)
		{
		}

		protected bool Equals(Creature other)
		{
			return Name == other.Name && RelatedName == other.RelatedName;
		}

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			if (ReferenceEquals(this, obj)) return true;
			if (obj.GetType() != this.GetType()) return false;
			return Equals((Creature) obj);
		}
	}
}