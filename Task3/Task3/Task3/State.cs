using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Task3
{
    public class State
    {
        public State Prev { get; set; }

        public StateCode Code { get; set; }

        public List<Creature> Left { get; set; } = new List<Creature>();

        public List<Creature> Right { get; set; } = new List<Creature>();

        public List<Creature> Boat { get; set; } = new List<Creature>();

        public bool BoatLeft { get; set; }

        public State() { }

        public State(State state)
        {
            Boat = state.Boat.ConvertAll(x => new Creature(x));
            Left = state.Left.ConvertAll(x => new Creature(x));
            Right = state.Right.ConvertAll(x => new Creature(x));

            BoatLeft = state.BoatLeft;

            Prev = state;
        }

        public void OrderAll()
        {
            Left = Left.OrderBy(x => x.Name).ToList();
            Right = Right.OrderBy(x => x.Name).ToList();
            Boat = Boat.OrderBy(x => x.Name).ToList();
        }

        protected bool Equals(State other)
        {
            return Left.SequenceEqual(other.Left) && Right.SequenceEqual(other.Right) && Boat.SequenceEqual(other.Boat) && BoatLeft == other.BoatLeft;
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
            return HashCode.Combine(Left, Right, Boat, BoatLeft);
        }

        public override string ToString()
        {
            var sb = new StringBuilder();

            sb.AppendLine();
            sb.AppendLine("---");

            sb.Append("L: ");
            foreach (var creature in Left)
            {
                sb.Append($"{creature.Name} ");
            }

            sb.AppendLine();

            if (!BoatLeft)
            {
                sb.AppendLine();
            }

            sb.Append($"B: ");
            foreach (var creature in Boat)
            {
                sb.Append($"{creature.Name} ");
            }

            if (BoatLeft)
            {
                sb.AppendLine();
            }

            sb.AppendLine();

            sb.Append("R: ");
            foreach (var creature in Right)
            {
                sb.Append($"{creature.Name} ");
            }

            sb.AppendLine("\n---");
            sb.AppendLine();

            return sb.ToString();
        }
    }
}