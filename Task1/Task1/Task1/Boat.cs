using System;
using System.Collections.Generic;

namespace Task1
{
	public class Boat
	{
		public bool OnLeftSide { get; set; }

		public List<Creature> OnBoardCreatures { get; set; }

		public Boat(bool onLeftSide, List<Creature> onBoardCreatures)
		{
			OnLeftSide = onLeftSide;
			OnBoardCreatures = onBoardCreatures;
		}

		public Boat() : this(true, new List<Creature>())
		{
		}

		public Boat(Boat boat) : this(boat.OnLeftSide, boat.OnBoardCreatures.ConvertAll(creature => new Creature(creature)))
		{
		}
	}
}