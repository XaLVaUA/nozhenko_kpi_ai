using System;
using System.Collections.Generic;
using System.Linq;

namespace Task1
{
	public class Boat
	{
		public bool IsOnLeftSide { get; set; }

		public List<Creature> OnBoardCreatures { get; set; }

		public Boat(bool isOnLeftSide, List<Creature> onBoardCreatures)
		{
			IsOnLeftSide = isOnLeftSide;
			OnBoardCreatures = onBoardCreatures;
		}

		public Boat() : this(true, new List<Creature>())
		{
		}

		public Boat(Boat boat) : this(boat.IsOnLeftSide, boat.OnBoardCreatures.ConvertAll(creature => new Creature(creature)))
		{
		}

        protected bool Equals(Boat other)
        {
            return IsOnLeftSide == other.IsOnLeftSide && OnBoardCreatures.SequenceEqual(other.OnBoardCreatures);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Boat) obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(IsOnLeftSide, OnBoardCreatures);
        }
    }
}