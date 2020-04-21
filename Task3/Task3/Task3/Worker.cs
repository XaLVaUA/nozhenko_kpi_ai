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
                    if (state.Code != StateCode.Start && state.Boat.Count > 0 && state.Code != StateCode.Transfer)
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
                    if (state.BoatLeft && state.Left.Count > 0)
                    {
                        if (state.Boat.Count == 0)
                        {
                            foreach (var creature in state.Left)
                            {
                                {
                                    var newState = new State(state);
                                    var cre = newState.Left.Find(x => x.Equals(creature));

                                    newState.Left.Remove(cre);
                                    newState.Boat.Add(cre);

                                    newState.Code = StateCode.FromLeft;
                                    newState.OrderAll();
                                    if (IsBeachValid(newState.Left) && visited.Find(x => x.Equals(newState)) == null)
                                    {
                                        pool.Add(newState);
                                    }
                                }

                                foreach (var creature2 in state.Left)
                                {
                                    if (creature.Equals(creature2))
                                    {
                                        continue;
                                    }

                                    var newState = new State(state);
                                    var cre = newState.Left.Find(x => x.Equals(creature));
                                    var cre2 = newState.Left.Find(x => x.Equals(creature2));

                                    newState.Left.Remove(cre);
                                    newState.Boat.Add(cre);

                                    newState.Left.Remove(cre2);
                                    newState.Boat.Add(cre2);

                                    newState.Code = StateCode.FromLeft;
                                    newState.OrderAll();
                                    if (IsBeachValid(newState.Left) && visited.Find(x => x.Equals(newState)) == null)
                                    {
                                        pool.Add(newState);
                                    }
                                }
                            }
                        }

                        if (state.Boat.Count == 1)
                        {
                            foreach (var creature in state.Left)
                            {
                                { 
                                    var newState = new State(state);
                                    var cre = newState.Left.Find(x => x.Equals(creature));

                                    newState.Left.Remove(cre);
                                    newState.Boat.Add(cre);

                                    newState.Code = StateCode.FromLeft;
                                    newState.OrderAll();
                                    if (IsBeachValid(newState.Left) && visited.Find(x => x.Equals(newState)) == null)
                                    {
                                        pool.Add(newState);
                                    }
                                }

                                {
                                    var newState = new State(state);
                                    var cre = newState.Left.Find(x => x.Equals(creature));
                                    var boatCre = newState.Boat.First();

                                    newState.Left.Remove(cre);
                                    newState.Boat.Add(cre);

                                    newState.Left.Add(boatCre);
                                    newState.Boat.Remove(boatCre);

                                    newState.Code = StateCode.FromLeft;
                                    newState.OrderAll();
                                    if (IsBeachValid(newState.Left) && visited.Find(x => x.Equals(newState)) == null)
                                    {
                                        pool.Add(newState);
                                    }
                                }

                                foreach (var creature2 in state.Left)
                                {
                                    if (creature.Equals(creature2))
                                    {
                                        continue;
                                    }

                                    var newState = new State(state);
                                    var cre = newState.Left.Find(x => x.Equals(creature));
                                    var cre2 = newState.Left.Find(x => x.Equals(creature2));
                                    var boatCre = newState.Boat.First();

                                    newState.Left.Remove(cre);
                                    newState.Boat.Add(cre);

                                    newState.Left.Remove(cre2);
                                    newState.Boat.Add(cre2);

                                    newState.Left.Add(boatCre);
                                    newState.Boat.Remove(boatCre);

                                    newState.Code = StateCode.FromLeft;
                                    newState.OrderAll();
                                    if (IsBeachValid(newState.Left) && visited.Find(x => x.Equals(newState)) == null)
                                    {
                                        pool.Add(newState);
                                    }
                                }
                            }
                        }

                        if (state.Boat.Count == 2)
                        {
                            {
                                var newState = new State(state);

                                newState.Left.AddRange(newState.Boat);
                                newState.Boat.Clear();

                                newState.Code = StateCode.FromLeft;
                                newState.OrderAll();
                                if (IsBeachValid(newState.Left) &&
                                    visited.Find(x => x.Equals(newState)) == null)
                                {
                                    pool.Add(newState);
                                }
                            }

                            foreach (var creature in state.Left)
                            {
                                var newState = new State(state);
                                var cre = newState.Left.Find(x => x.Equals(creature));

                                newState.Left.AddRange(newState.Boat);
                                newState.Boat.Clear();
                                newState.Left.Remove(cre);
                                newState.Boat.Add(cre);

                                newState.Code = StateCode.FromRight;
                                newState.OrderAll();
                                if (IsBeachValid(newState.Left) &&
                                    visited.Find(x => x.Equals(newState)) == null)
                                {
                                    pool.Add(newState);
                                }
                            }

                            foreach (var boatCreature in state.Boat)
                            {
                                {
                                    var newState = new State(state);
                                    var boatCre = newState.Boat.Find(x => x.Equals(boatCreature));

                                    newState.Left.Add(boatCre);
                                    newState.Boat.Remove(boatCre);

                                    newState.Code = StateCode.FromLeft;
                                    newState.OrderAll();
                                    if (IsBeachValid(newState.Left) &&
                                        visited.Find(x => x.Equals(newState)) == null)
                                    {
                                        pool.Add(newState);
                                    }
                                }

                                foreach (var creature in state.Left)
                                {
                                    {
                                        var newState = new State(state);
                                        var cre = newState.Left.Find(x => x.Equals(creature));
                                        var boatCre = newState.Boat.Find(x => x.Equals(boatCreature));

                                        newState.Left.Remove(cre);
                                        newState.Boat.Add(cre);

                                        newState.Left.Add(boatCre);
                                        newState.Boat.Remove(boatCre);

                                        newState.Code = StateCode.FromLeft;
                                        newState.OrderAll();
                                        if (IsBeachValid(newState.Left) &&
                                            visited.Find(x => x.Equals(newState)) == null)
                                        {
                                            pool.Add(newState);
                                        }
                                    }

                                    {
                                        foreach (var creature2 in state.Left)
                                        {
                                            if (creature.Equals(creature2))
                                            {
                                                continue;
                                            }

                                            var newState = new State(state);
                                            var cre = newState.Left.Find(x => x.Equals(creature));
                                            var cre2 = newState.Left.Find(x => x.Equals(creature2));
                                            var boatCre = newState.Boat.First();
                                            var boatCre2 = newState.Boat.Last();

                                            newState.Left.Remove(cre);
                                            newState.Boat.Add(cre);
                                            newState.Left.Remove(cre2);
                                            newState.Boat.Add(cre2);

                                            newState.Boat.Remove(boatCre);
                                            newState.Left.Add(boatCre);
                                            newState.Boat.Remove(boatCre2);
                                            newState.Left.Add(boatCre2);

                                            newState.Code = StateCode.FromLeft;
                                            newState.OrderAll();
                                            if (IsBeachValid(newState.Left) && visited.Find(x => x.Equals(newState)) == null)
                                            {
                                                pool.Add(newState);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                {
                    if (!state.BoatLeft && state.Right.Count > 0)
                    {
                        if (state.Boat.Count == 0)
                        {
                            foreach (var creature in state.Right)
                            {
                                {
                                    var newState = new State(state);
                                    var cre = newState.Right.Find(x => x.Equals(creature));

                                    newState.Right.Remove(cre);
                                    newState.Boat.Add(cre);

                                    newState.Code = StateCode.FromRight;
                                    newState.OrderAll();
                                    if (IsBeachValid(newState.Right) && visited.Find(x => x.Equals(newState)) == null)
                                    {
                                        pool.Add(newState);
                                    }
                                }

                                foreach (var creature2 in state.Right)
                                {
                                    if (creature.Equals(creature2))
                                    {
                                        continue;
                                    }

                                    var newState = new State(state);
                                    var cre = newState.Right.Find(x => x.Equals(creature));
                                    var cre2 = newState.Right.Find(x => x.Equals(creature2));

                                    newState.Right.Remove(cre);
                                    newState.Boat.Add(cre);

                                    newState.Right.Remove(cre2);
                                    newState.Boat.Add(cre2);

                                    newState.Code = StateCode.FromRight;
                                    newState.OrderAll();
                                    if (IsBeachValid(newState.Right) && visited.Find(x => x.Equals(newState)) == null)
                                    {
                                        pool.Add(newState);
                                    }
                                }
                            }
                        }

                        if (state.Boat.Count == 1)
                        {
                            foreach (var creature in state.Right)
                            {
                                {
                                    var newState = new State(state);
                                    var cre = newState.Right.Find(x => x.Equals(creature));

                                    newState.Right.Remove(cre);
                                    newState.Boat.Add(cre);

                                    newState.Code = StateCode.FromLeft;
                                    newState.OrderAll();
                                    if (IsBeachValid(newState.Right) && visited.Find(x => x.Equals(newState)) == null)
                                    {
                                        pool.Add(newState);
                                    }
                                }

                                {
                                    var newState = new State(state);
                                    var cre = newState.Right.Find(x => x.Equals(creature));
                                    var boatCre = newState.Boat.First();

                                    newState.Right.Remove(cre);
                                    newState.Boat.Add(cre);

                                    newState.Right.Add(boatCre);
                                    newState.Boat.Remove(boatCre);

                                    newState.Code = StateCode.FromRight;
                                    newState.OrderAll();
                                    if (IsBeachValid(newState.Right) && visited.Find(x => x.Equals(newState)) == null)
                                    {
                                        pool.Add(newState);
                                    }
                                }

                                foreach (var creature2 in state.Right)
                                {
                                    if (creature.Equals(creature2))
                                    {
                                        continue;
                                    }

                                    var newState = new State(state);
                                    var cre = newState.Right.Find(x => x.Equals(creature));
                                    var cre2 = newState.Right.Find(x => x.Equals(creature2));
                                    var boatCre = newState.Boat.First();

                                    newState.Right.Remove(cre);
                                    newState.Boat.Add(cre);

                                    newState.Right.Remove(cre2);
                                    newState.Boat.Add(cre2);

                                    newState.Right.Add(boatCre);
                                    newState.Boat.Remove(boatCre);

                                    newState.Code = StateCode.FromRight;
                                    newState.OrderAll();
                                    if (IsBeachValid(newState.Right) && visited.Find(x => x.Equals(newState)) == null)
                                    {
                                        pool.Add(newState);
                                    }
                                }
                            }
                        }

                        if (state.Boat.Count == 2)
                        {
                            {
                                var newState = new State(state);

                                newState.Right.AddRange(newState.Boat);
                                newState.Boat.Clear();

                                newState.Code = StateCode.FromRight;
                                newState.OrderAll();
                                if (IsBeachValid(newState.Right) &&
                                    visited.Find(x => x.Equals(newState)) == null)
                                {
                                    pool.Add(newState);
                                }
                            }

                            foreach (var creature in state.Right)
                            {
                                {
                                    var newState = new State(state);
                                    var cre = newState.Right.Find(x => x.Equals(creature));

                                    newState.Right.AddRange(newState.Boat);
                                    newState.Boat.Clear();
                                    newState.Right.Remove(cre);
                                    newState.Boat.Add(cre);

                                    newState.Code = StateCode.FromRight;
                                    newState.OrderAll();
                                    if (IsBeachValid(newState.Right) &&
                                        visited.Find(x => x.Equals(newState)) == null)
                                    {
                                        pool.Add(newState);
                                    }
                                }
                            }

                            foreach (var boatCreature in state.Boat)
                            {
                                {
                                    var newState = new State(state);
                                    var boatCre = newState.Boat.Find(x => x.Equals(boatCreature));

                                    newState.Right.Add(boatCre);
                                    newState.Boat.Remove(boatCre);

                                    newState.Code = StateCode.FromRight;
                                    newState.OrderAll();
                                    if (IsBeachValid(newState.Right) &&
                                        visited.Find(x => x.Equals(newState)) == null)
                                    {
                                        pool.Add(newState);
                                    }
                                }

                                foreach (var creature in state.Right)
                                {
                                    {
                                        var newState = new State(state);
                                        var cre = newState.Right.Find(x => x.Equals(creature));
                                        var boatCre = newState.Boat.Find(x => x.Equals(boatCreature));

                                        newState.Right.Remove(cre);
                                        newState.Boat.Add(cre);

                                        newState.Right.Add(boatCre);
                                        newState.Boat.Remove(boatCre);

                                        newState.Code = StateCode.FromRight;
                                        newState.OrderAll();
                                        if (IsBeachValid(newState.Right) &&
                                            visited.Find(x => x.Equals(newState)) == null)
                                        {
                                            pool.Add(newState);
                                        }
                                    }

                                    {
                                        var newState = new State(state);
                                        var cre = newState.Right.Find(x => x.Equals(creature));
                                        var boatCre = newState.Boat.Find(x => x.Equals(boatCreature));

                                        newState.Right.Remove(cre);
                                        newState.Boat.Add(cre);

                                        newState.Right.Add(boatCre);
                                        newState.Boat.Remove(boatCre);

                                        newState.Code = StateCode.FromRight;
                                        newState.OrderAll();
                                        if (IsBeachValid(newState.Right) &&
                                            visited.Find(x => x.Equals(newState)) == null)
                                        {
                                            pool.Add(newState);
                                        }
                                    }

                                    {
                                        foreach (var creature2 in state.Right)
                                        {
                                            if (creature.Equals(creature2))
                                            {
                                                continue;
                                            }

                                            var newState = new State(state);
                                            var cre = newState.Right.Find(x => x.Equals(creature));
                                            var cre2 = newState.Right.Find(x => x.Equals(creature2));
                                            var boatCre = newState.Boat.First();
                                            var boatCre2 = newState.Boat.Last();

                                            newState.Right.Remove(cre);
                                            newState.Boat.Add(cre);
                                            newState.Right.Remove(cre2);
                                            newState.Boat.Add(cre2);

                                            newState.Boat.Remove(boatCre);
                                            newState.Right.Add(boatCre);
                                            newState.Boat.Remove(boatCre2);
                                            newState.Right.Add(boatCre2);

                                            newState.Code = StateCode.FromLeft;
                                            newState.OrderAll();
                                            if (IsBeachValid(newState.Right) && visited.Find(x => x.Equals(newState)) == null)
                                            {
                                                pool.Add(newState);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                {
                    if (state.BoatLeft && state.Boat.Count > 0)
                    {
                        foreach (var boatCreature in state.Boat)
                        {
                            {
                                var newState = new State(state);
                                var boatCre = newState.Boat.Find(x => x.Equals(boatCreature));

                                newState.Boat.Remove(boatCre);
                                newState.Left.Add(boatCre);

                                newState.Code = StateCode.ToLeft;
                                newState.OrderAll();
                                if (IsBeachValid(newState.Left) && visited.Find(x => x.Equals(newState)) == null)
                                {
                                    pool.Add(newState);
                                }
                            }

                            foreach (var creature in state.Left)
                            {
                                var newState = new State(state);
                                var cre = newState.Left.Find(x => x.Equals(creature));
                                var boatCre = newState.Boat.Find(x => x.Equals(boatCreature));

                                newState.Left.Remove(cre);
                                newState.Boat.Add(cre);

                                newState.Boat.Remove(boatCre);
                                newState.Left.Add(boatCre);

                                newState.Code = StateCode.ToLeft;
                                newState.OrderAll();
                                if (IsBeachValid(newState.Left) && visited.Find(x => x.Equals(newState)) == null)
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
                        foreach (var boatCreature in state.Boat)
                        {
                            {
                                var newState = new State(state);
                                var boatCre = newState.Boat.Find(x => x.Equals(boatCreature));

                                newState.Boat.Remove(boatCre);
                                newState.Right.Add(boatCre);

                                newState.Code = StateCode.ToRight;
                                newState.OrderAll();
                                if (IsBeachValid(newState.Right) && visited.Find(x => x.Equals(newState)) == null)
                                {
                                    pool.Add(newState);
                                }
                            }

                            foreach (var creature in state.Right)
                            {
                                var newState = new State(state);
                                var cre = newState.Right.Find(x => x.Equals(creature));
                                var boatCre = newState.Boat.Find(x => x.Equals(boatCreature));

                                newState.Right.Remove(cre);
                                newState.Boat.Add(cre);

                                newState.Boat.Remove(boatCre);
                                newState.Right.Add(boatCre);

                                newState.Code = StateCode.ToRight;
                                newState.OrderAll();
                                if (IsBeachValid(newState.Right) && visited.Find(x => x.Equals(newState)) == null)
                                {
                                    pool.Add(newState);
                                }
                            }
                        }
                    }
                }

                {
                    var newState = false;
                    pool = pool.OrderBy(x => Val(x)).ToList();
                    var candidate = pool.Last();
                    if (Val(candidate) >= curVal)
                    {
                        state = candidate;
                        newState = true;
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

            if (state.Right.Count == 6)
            {
                return 7;
            }

            if (!state.BoatLeft)
            {
                return state.Right.Count + state.Boat.Count;
            }

            return state.Right.Count + 2;
        }

        public bool IsGoal(State state)
        {
            return state.Right.Count == 6;
        }

        public bool IsBeachValid(List<Creature> creatures)
        {
            var hCount = creatures.Count(x => x.IsHumanEating);
            var mCount = creatures.Count(x => !x.IsHumanEating);

            if (hCount == 0 || mCount == 0)
            {
                return true;
            }

            return hCount - mCount == 0;
        }
    }
}