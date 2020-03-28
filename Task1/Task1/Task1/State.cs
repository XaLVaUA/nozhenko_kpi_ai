using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Task1
{
	public class State
	{
		public Boat Boat { get; set; }

		public List<Creature> LeftSideCreatures { get; set; }

		public List<Creature> RightSideCreatures { get; set; }

		public StateCode PreStateCode { get; set; }

		public State PreState { get; set; }

		public State(Boat boat, List<Creature> leftSideCreatures, List<Creature> rightSideCreatures, StateCode preStateCode)
		{
			Boat = boat;
			LeftSideCreatures = leftSideCreatures;
			RightSideCreatures = rightSideCreatures;
			PreStateCode = preStateCode;
		}

		public State(StateCode stateCode) : this(new Boat(), new List<Creature>(), new List<Creature>(), stateCode)
		{
		}

		public State(State state) : this(new Boat(state.Boat), state.LeftSideCreatures.ConvertAll(creature => new Creature(creature)), state.RightSideCreatures.ConvertAll(creature => new Creature(creature)), state.PreStateCode)
		{
		}

		public override string ToString()
		{
			var sb = new StringBuilder();

			sb.AppendLine();
			sb.AppendLine("---");

			sb.Append("L: ");
			foreach (var creature in LeftSideCreatures)
			{
				sb.Append($"{creature.Name} ");
			}

			sb.AppendLine();

			if (!Boat.IsOnLeftSide)
			{
				sb.AppendLine();
			}

			sb.Append($"B: ");
			foreach (var creature in Boat.OnBoardCreatures)
			{
				sb.Append($"{creature.Name} ");
			}

			if (Boat.IsOnLeftSide)
			{
				sb.AppendLine();
			}

			sb.AppendLine();

			sb.Append("R: ");
			foreach (var creature in RightSideCreatures)
			{
				sb.Append($"{creature.Name} ");
			}

			sb.AppendLine("\n---");
			sb.AppendLine();

			return sb.ToString();
		}

        protected bool Equals(State other)
        {
            return Equals(Boat, other.Boat) && LeftSideCreatures.SequenceEqual(other.LeftSideCreatures) && RightSideCreatures.SequenceEqual(other.RightSideCreatures);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((State) obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Boat, LeftSideCreatures, RightSideCreatures);
        }
    }
}