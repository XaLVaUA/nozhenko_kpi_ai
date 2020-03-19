using System;
using System.Linq;
using System.Text;

namespace Task2
{
    public class State
    {
        public const int XSize = 3;
        public const int YSize = 3;

        public int[] Board { get; set; }

        public int ZeroPosition { get; set; }

        public State PrevState { get; set; }

        public int WeightValue { get; set; }

        public int Steps { get; set; }

        public State()
        {
            Board = new int[XSize * YSize];
            ZeroPosition = 0;
            PrevState = null;
        }

        public State(State state) : this()
        {
            Array.Copy(state.Board, Board, Board.Length);
            ZeroPosition = state.ZeroPosition;
            Steps = state.Steps;
        }

        public override string ToString()
        {
            var sb = new StringBuilder();

            sb.AppendLine();
            sb.AppendLine("---");

            {
                var k = 0;
                for (int i = 0; i < XSize; i++)
                {
                    for (int j = 0; j < YSize; j++)
                    {
                        sb.Append($"{Board[k++]} ");
                    }

                    sb.AppendLine();
                }
            }

            sb.AppendLine("---");
            sb.AppendLine();

            return sb.ToString();
        }

        protected bool Equals(State other)
        {
            return Board.SequenceEqual(other.Board) && ZeroPosition == other.ZeroPosition;
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
            return HashCode.Combine(Board, ZeroPosition);
        }
    }
}