using System;
using System.Collections.Generic;
using System.Linq;

namespace Task3
{
    public class Worker
    {
        public State GenerateStartState()
        {
            var state = new State();
            state.Left.Add(new Creature("h1", true));
            state.Left.Add(new Creature("h2", true));
            state.Left.Add(new Creature("h3", true));

            state.Left.Add(new Creature("m1", false));
            state.Left.Add(new Creature("m2", false));
            state.Left.Add(new Creature("m3", false));

            state.BoatLeft = true;

            state.Code = StateCode.Start;

            state.OrderAll();

            return state;
        }

        public void Start()
        {
            var state = GenerateStartState();
            var visited = new List<State>();

            var i = 0;

            while (true)
            {
                visited.Add(state);
                var curVal = Val(state);

                Console.Write($"#{++i} ({curVal})");
                Console.WriteLine(state);

                if (IsGoal(state))
                {
                    return;
                }

                var pool = new List<State>();

                {
                    if (state.Code != StateCode.Start && state.Prev.Code != StateCode.Transfer)
                    {
                        var newState = new State(state);
                        newState.BoatLeft = !newState.BoatLeft;
                        newState.Code = StateCode.Transfer;
                        if (visited.Find(x => x.Equals(newState)) == null)
                        {
                            pool.Add(newState);
                        }
                    }
                }

                {
                    if (state.BoatLeft && state.Left.Count > 0 && state.Boat.Count < 2)
                    {
                        if (state.Left.Count < 3)
                        {
                            foreach (var creature in state.Left)
                            {
                                var newState = new State(state);
                                var cre = newState.Left.Find(x => x.Equals(creature));
                                newState.Left.Remove(cre);
                                newState.Boat.Add(cre);
                                newState.Code = StateCode.FromLeft;
                                newState.OrderAll();
                                if (visited.Find(x => x.Equals(newState)) == null)
                                {
                                    pool.Add(newState);
                                }
                            }
                        }
                        else if (state.Boat.Count == 0)
                        {
                            foreach (var c in state.Left.Where(x => x.IsHumanEating))
                            {
                                foreach (var m in state.Left.Where(x => !x.IsHumanEating))
                                {
                                    var newState = new State(state);
                                    var can = newState.Left.Find(x => x.Equals(c));
                                    var mis = newState.Left.Find(x => x.Equals(m));
                                    newState.Left.Remove(can);
                                    newState.Left.Remove(mis);
                                    newState.Boat.Add(can);
                                    newState.Boat.Add(mis);
                                    newState.Code = StateCode.FromLeft;
                                    newState.OrderAll();
                                    if (visited.Find(x => x.Equals(newState)) == null)
                                    {
                                        pool.Add(newState);
                                    }
                                }
                            }
                        }
                    }
                }

                {
                    if (!state.BoatLeft && state.Right.Count > 0 && state.Boat.Count < 2)
                    {
                        if (state.Right.Count < 3)
                        {
                            foreach (var creature in state.Right)
                            {
                                var newState = new State(state);
                                var cre = newState.Right.Find(x => x.Equals(creature));
                                newState.Right.Remove(cre);
                                newState.Boat.Add(cre);
                                newState.Code = StateCode.FromRight;
                                newState.OrderAll();
                                if (visited.Find(x => x.Equals(newState)) == null)
                                {
                                    pool.Add(newState);
                                }
                            }
                        }
                        else if (state.Boat.Count == 0)
                        {
                            foreach (var c in state.Right.Where(x => x.IsHumanEating))
                            {
                                foreach (var m in state.Right.Where(x => !x.IsHumanEating))
                                {
                                    var newState = new State(state);
                                    var can = newState.Right.Find(x => x.Equals(c));
                                    var mis = newState.Right.Find(x => x.Equals(m));
                                    newState.Right.Remove(can);
                                    newState.Right.Remove(mis);
                                    newState.Boat.Add(can);
                                    newState.Boat.Add(mis);
                                    newState.Code = StateCode.FromRight;
                                    newState.OrderAll();
                                    if (visited.Find(x => x.Equals(newState)) == null)
                                    {
                                        pool.Add(newState);
                                    }
                                }
                            }
                        }
                    }
                }

                {
                    if (state.BoatLeft && state.Boat.Count > 0)
                    {
                        
                        if (state.Boat.Count == 2 && state.Left.Count > 1)
                        {
                            var newState = new State(state);
                            newState.Left.AddRange(newState.Boat);
                            newState.Boat.Clear();
                            newState.Code = StateCode.ToLeft;
                            if (visited.Find(x => x.Equals(newState)) == null)
                            {
                                pool.Add(newState);
                            }
                        }
                        else if (state.Left.Count < 2)
                        {
                            foreach (var creature in state.Boat)
                            {
                                var newState = new State(state);
                                var cre = newState.Boat.Find(x => x.Equals(creature));
                                newState.Boat.Remove(cre);
                                newState.Left.Add(cre);
                                newState.Code = StateCode.ToLeft;
                                if (visited.Find(x => x.Equals(newState)) == null)
                                {
                                    pool.Add(newState);
                                }
                            }
                        }
                    }
                }

                {
                    if (!state.BoatLeft && state.Boat.Count > 0)
                    {

                        if (state.Boat.Count == 2 && state.Right.Count > 1)
                        {
                            var newState = new State(state);
                            newState.Right.AddRange(newState.Boat);
                            newState.Boat.Clear();
                            newState.Code = StateCode.ToRight;
                            if (visited.Find(x => x.Equals(newState)) == null)
                            {
                                pool.Add(newState);
                            }
                        }
                        else if (state.Right.Count < 2)
                        {
                            foreach (var creature in state.Boat)
                            {
                                var newState = new State(state);
                                var cre = newState.Boat.Find(x => x.Equals(creature));
                                newState.Boat.Remove(cre);
                                newState.Right.Add(cre);
                                newState.Code = StateCode.ToRight;
                                if (visited.Find(x => x.Equals(newState)) == null)
                                {
                                    pool.Add(newState);
                                }
                            }
                        }
                    }
                }

                {
                    var newState = false;
                    foreach (var st in pool)
                    {
                        if (Val(st) >= curVal)
                        {
                            state = st;
                            newState = true;
                            break;
                        }
                    }

                    if (!newState)
                    {
                        return;
                    }
                }
            }
        }

        public int Val(State state)
        {
            if (state.Code == StateCode.Start)
            {
                return 0;
            }

            if (!state.BoatLeft)
            {
                return state.Right.Count + state.Boat.Count;
            }

            if (state.BoatLeft)
            {
                return state.Right.Count + 2;
            }

            return state.Right.Count;
        }

        public bool IsGoal(State state)
        {
            return state.Right.Count == 6;
        }
    }
}