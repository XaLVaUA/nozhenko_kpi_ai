using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Task1
{
	public class Worker
	{
		public Random Random { get; set; }

		public Stack<State> StateStack { get; set; }

		public Worker()
		{
			Random = new Random();
			StateStack = new Stack<State>();
			StateStack.Push(GenerateStartState());
		}

		public State GenerateStartState()
		{
			var state = new State(StateCode.BoatTransfer);

			state.LeftSideCreatures.Add(new Creature("m1", "p1", true));
			state.LeftSideCreatures.Add(new Creature("m2", "p2", true));
			state.LeftSideCreatures.Add(new Creature("m3", "p3", true));

			state.LeftSideCreatures.Add(new Creature("p1", "m1", false));
			state.LeftSideCreatures.Add(new Creature("p2", "m2", false));
			state.LeftSideCreatures.Add(new Creature("p3", "m3", false));

			state.PreState = null;

			return state;
		}

		public void Do()
		{
			while (StateStack.Count > 0)
			{
				var currentState = StateStack.Pop();

				//Console.WriteLine(currentState);
				//Thread.Sleep(200);

				if (IsGoalState(currentState))
				{
					Console.WriteLine("Goal reached!");
					PrintStateChain(currentState);
					break;
				}

				Shuffle(currentState.LeftSideCreatures);
				Shuffle(currentState.RightSideCreatures);
				Shuffle(currentState.Boat.OnBoardCreatures);

				if (currentState.PreStateCode != StateCode.LeftSideToBoat)
				{
					#region LeftSideFromBoat

					if (currentState.Boat.OnLeftSide && currentState.Boat.OnBoardCreatures.Count > 0)
					{
						foreach (var creature in currentState.Boat.OnBoardCreatures)
						{
							if (!creature.Independent)
							{
								if (currentState.LeftSideCreatures.Count == 1 && creature.RelatedName !=
									currentState.LeftSideCreatures.Last().Name)
								{
									continue;
								}
							}
							else
							{
								if (currentState.LeftSideCreatures.Count == 1)
								{
									var leftCreature = currentState.LeftSideCreatures.Last();

									if (!leftCreature.Independent && leftCreature.RelatedName != creature.Name)
									{
										continue;
									}
								}
							}

							var newState = new State(currentState);
							newState.Boat.OnBoardCreatures.Remove(creature);
							newState.LeftSideCreatures.Add(new Creature(creature));
							newState.PreStateCode = StateCode.LeftSideFromBoat;
							newState.PreState = currentState;
							StateStack.Push(newState);
						}
					}

					#endregion
				}

				#region RightSideToBoat

				if (currentState.RightSideCreatures.Count > 0 && !currentState.Boat.OnLeftSide &&
				    currentState.Boat.OnBoardCreatures.Count < 2)
				{
					if (currentState.Boat.OnBoardCreatures.Count == 0)
					{
						foreach (var creature in currentState.RightSideCreatures)
						{
							var newState = new State(currentState);
							newState.RightSideCreatures.Remove(creature);
							newState.Boat.OnBoardCreatures.Add(new Creature(creature));
							newState.PreStateCode = StateCode.RightSideToBoat;
							newState.PreState = currentState;
							StateStack.Push(newState);
						}
					}
					else
					{
						var onBoardCreature = currentState.Boat.OnBoardCreatures.Last();

						foreach (var creature in currentState.RightSideCreatures)
						{
							if (!((creature.Independent) || (!creature.Independent && !onBoardCreature.Independent) || (!creature.Independent && creature.RelatedName != onBoardCreature.Name))) continue;

							var newState = new State(currentState);
							newState.RightSideCreatures.Remove(creature);
							newState.Boat.OnBoardCreatures.Add(new Creature(creature));
							newState.PreStateCode = StateCode.RightSideToBoat;
							newState.PreState = currentState;
							StateStack.Push(newState);
						}
					}
				}

				#endregion

				if (currentState.PreStateCode != StateCode.BoatTransfer)
				{
					#region BoatTransfer

					{
						var newState = new State(currentState);
						newState.Boat.OnLeftSide = !newState.Boat.OnLeftSide;
						newState.PreStateCode = StateCode.BoatTransfer;
						newState.PreState = currentState;
						StateStack.Push(newState);
					}

					#endregion
				}

				if (currentState.PreStateCode != StateCode.RightSideToBoat)
				{
					#region RightSideFromBoat

					if (!currentState.Boat.OnLeftSide && currentState.Boat.OnBoardCreatures.Count > 0)
					{
						foreach (var creature in currentState.Boat.OnBoardCreatures)
						{
							if (!creature.Independent)
							{
								if (currentState.RightSideCreatures.Count == 1 && creature.RelatedName !=
									currentState.RightSideCreatures.Last().Name)
								{
									continue;
								}
							}
							else
							{
								if (currentState.RightSideCreatures.Count == 1)
								{
									var rightCreature = currentState.RightSideCreatures.Last();

									if (!rightCreature.Independent && rightCreature.RelatedName != creature.Name)
									{
										continue;
									}
								}
							}

							var newState = new State(currentState);
							newState.Boat.OnBoardCreatures.Remove(creature);
							newState.RightSideCreatures.Add(new Creature(creature));
							newState.PreStateCode = StateCode.RightSideFromBoat;
							newState.PreState = currentState;
							StateStack.Push(newState);
						}
					}

					#endregion
				}

				#region LeftSideToBoat

				if (currentState.LeftSideCreatures.Count > 0 && currentState.Boat.OnLeftSide &&
				    currentState.Boat.OnBoardCreatures.Count < 2)
				{
					if (currentState.Boat.OnBoardCreatures.Count == 0)
					{
						foreach (var creature in currentState.LeftSideCreatures)
						{
							var newState = new State(currentState);
							newState.LeftSideCreatures.Remove(creature);
							newState.Boat.OnBoardCreatures.Add(new Creature(creature));
							newState.PreStateCode = StateCode.LeftSideToBoat;
							newState.PreState = currentState;
							StateStack.Push(newState);
						}
					}
					else
					{
						var onBoardCreature = currentState.Boat.OnBoardCreatures.Last();

						foreach (var creature in currentState.LeftSideCreatures)
						{
							if (creature.Independent && !onBoardCreature.Independent && onBoardCreature.RelatedName != creature.Name) continue;
							if (!creature.Independent && onBoardCreature.Independent && creature.RelatedName != onBoardCreature.Name) continue;

							var newState = new State(currentState);
							newState.LeftSideCreatures.Remove(creature);
							newState.Boat.OnBoardCreatures.Add(new Creature(creature));
							newState.PreStateCode = StateCode.LeftSideToBoat;
							newState.PreState = currentState;
							StateStack.Push(newState);
						}
					}
				}

				#endregion
			}
		}

		public bool IsGoalState(State state)
		{
			return state.RightSideCreatures.Count == 6;
		}

		public void PrintStateChain(State state)
		{
			var count = 0;
			var curState = state;
			
			do
			{
				++count;
				Console.WriteLine($"#{count}");
				Console.WriteLine(curState);
				curState = curState.PreState;
			} 
			while (curState != null);

			Console.WriteLine($"\nStart here /|\\ \nNode count: {count}");
		}

		public static List<T> Shuffle<T>(List<T> list)
		{
			var rnd = new Random();
			for (int i = 0, size= list.Count; i < size; ++i)
			{
				var k = rnd.Next(0, i);
				var value = list[k];
				list[k] = list[i];
				list[i] = value;
			}
			return list;
		}
	}
}