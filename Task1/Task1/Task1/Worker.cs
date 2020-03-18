using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Task1
{
	public class Worker
	{
        // получение начального состояния
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

        // выполнение задания та вывод результата лучшей попытки поиска из attempts раз
		public void Do(int attempts)
        {
            var bestStateChain = new Stack<State>();
            var bestChainCount = int.MaxValue;

            for (int i = 0; i < attempts; ++i)
            {
                var attemptStateChain = Tick();

				if (attemptStateChain == null) continue;

                if (attemptStateChain.Count >= bestChainCount) continue;

                bestStateChain = attemptStateChain;
                bestChainCount = attemptStateChain.Count;
            }

			PrintStateChain(bestStateChain);
        }

        // выполнить поиск в глубину и вернуть цепочку состояний
		public Stack<State> Tick()
        {
            var stateStack = new Stack<State>();
			stateStack.Push(GenerateStartState());

			while (stateStack.Count > 0)
			{
				var currentState = stateStack.Pop();

				//Console.WriteLine(currentState);
				//Thread.Sleep(200);

				if (IsGoalState(currentState))
				{
					return SaveStateChain(currentState);
                }

                // перемешать списки для избежания зациклености
				Shuffle(currentState.LeftSideCreatures);
				Shuffle(currentState.RightSideCreatures);
				Shuffle(currentState.Boat.OnBoardCreatures);

                // проверка предыдущего состояния на целесообразность действия
				if (currentState.PreStateCode != StateCode.LeftSideToBoat)
				{
					// проверка и создания новых состояний когда лодка на левом берегу и происходит высадка
                    #region LeftSideFromBoat

					if (currentState.Boat.IsOnLeftSide && currentState.Boat.OnBoardCreatures.Count > 0)
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
							stateStack.Push(newState);
						}
					}

					#endregion
				}

                // проверка и создания новых состояний когда лодка на правом берегу и происходит посадка
				#region RightSideToBoat

				if (currentState.RightSideCreatures.Count > 0 && !currentState.Boat.IsOnLeftSide &&
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
							stateStack.Push(newState);
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
							stateStack.Push(newState);
						}
					}
				}

				#endregion

                // проверка предыдущего состояния на целесообразность действия
				if (currentState.PreStateCode != StateCode.BoatTransfer)
				{
                    // перемещение лодки на противоположный берег
					#region BoatTransfer

					{
						var newState = new State(currentState);
						newState.Boat.IsOnLeftSide = !newState.Boat.IsOnLeftSide;
						newState.PreStateCode = StateCode.BoatTransfer;
						newState.PreState = currentState;
						stateStack.Push(newState);
					}

					#endregion
				}

                // проверка предыдущего состояния на целесообразность действия
				if (currentState.PreStateCode != StateCode.RightSideToBoat)
				{
                    // проверка и создания новых состояний когда лодка на правом берегу и происходит высадка
					#region RightSideFromBoat

					if (!currentState.Boat.IsOnLeftSide && currentState.Boat.OnBoardCreatures.Count > 0)
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
							stateStack.Push(newState);
						}
					}

					#endregion
				}

                // проверка и создания новых состояний когда лодка на левом берегу и происходит посадка
				#region LeftSideToBoat

				if (currentState.LeftSideCreatures.Count > 0 && currentState.Boat.IsOnLeftSide &&
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
							stateStack.Push(newState);
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
							stateStack.Push(newState);
						}
					}
				}

				#endregion
			}

            return null;
        }

		// проверка на то, является ли состояние целевым
		public bool IsGoalState(State state)
		{
			return state.RightSideCreatures.Count == 6;
		}

        // сохранить цепочку состояний в правильном порядке (от начального к целевому)
		public Stack<State> SaveStateChain(State state)
		{
			var chain = new Stack<State>();
            var curState = state;
			
			do
			{
				chain.Push(curState);
                curState = curState.PreState;
			} 
			while (curState != null);

            return chain;
        }

		// вывести в консоль цепочку состояний
        public void PrintStateChain(Stack<State> states)
        {
            var no = 0;
            foreach (var state in states)
            {
                ++no;
				Console.Write($"#{no}");
				Console.WriteLine(state);
            }
        }

		// перемешать список случайным образом
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