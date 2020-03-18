using System;
using System.Collections.Generic;

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
	}
}