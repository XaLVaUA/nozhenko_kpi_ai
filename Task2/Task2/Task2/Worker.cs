using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;

namespace Task2
{
    public class Worker
    {
        public State GoalState { get; set; } = GenerateGoalState();

        public void Do()
        {
            Console.WriteLine("Wait please\n");
            var (result, _) = Search(GenerateStartState(), int.MaxValue);
            PrintResult(result);
        }

        public void PrintResult(State state)
        {
            var stack = new Stack<State>();

            var curState = state;

            do
            {
                stack.Push(curState);
                curState = curState.PrevState;
            } 
            while (curState != null);

            var no = 0;
            foreach (var st in stack)
            {
                Console.Write($"#{++no}");
                Console.WriteLine(st);
            }
        }

        public (State, int) Search(State state, int weightLimit)
        {
            if (state.Equals(GoalState)) return (state, weightLimit);
            var list = FindSuccessors(state);

            if (list.Count == 0)
            {
                return (null, int.MaxValue);
            }

            while (true)
            {
                // сортировка по возрастанию ф значения
                list = list.OrderBy(x => x.WeightValue).ToList();

                var best = list[0];

                if (best.WeightValue > weightLimit)
                {
                    return (null, best.WeightValue);
                }

                var alternative = int.MaxValue;

                if (list.Count > 1)
                {
                    alternative = list[1].WeightValue;
                }
                
                var (result, bestWeight) = Search(best, Math.Min(weightLimit, alternative));
                
                best.WeightValue = bestWeight;

                if (result != null)
                {
                    return (result, result.WeightValue);
                }
            }
        }

        // нахождение проверка и добавление новых состояний
        public List<State> FindSuccessors(State state)
        {
            var list = new List<State>();

            // проверка клетки справа
            var candidateRight = state.ZeroPosition + 1;
            CheckAddRowCandidate(state, candidateRight, list);

            // проверка клетки слева
            var candidateLeft = state.ZeroPosition - 1;
            CheckAddRowCandidate(state, candidateLeft, list);

            // проверка клетки сверху
            var candidateUp = state.ZeroPosition - State.YSize;
            CheckAddColCandidate(state, candidateUp, list);

            // проверка клетки снизу
            var candidateDown = state.ZeroPosition + State.YSize;
            CheckAddColCandidate(state, candidateDown, list);

            return list;
        }

        public void CheckAddRowCandidate(State state, int candidate, List<State> list)
        {
            var rowNo = state.ZeroPosition / State.XSize;
            if (candidate >= 0 && state.Board.Length > candidate && candidate / State.XSize == rowNo)
            {
                AddNewCandidate(state, candidate, list);
            }
        }

        public void CheckAddColCandidate(State state, int candidate, List<State> list)
        {
            if (candidate >= 0 && state.Board.Length > candidate)
            {
                AddNewCandidate(state, candidate, list);
            }
        }

        public void AddNewCandidate(State state, int candidate, List<State> list)
        {
            var newState = new State(state);
            var oldZero = newState.Board[state.ZeroPosition];
            newState.Board[state.ZeroPosition] = state.Board[candidate];
            newState.Board[candidate] = oldZero;
            newState.ZeroPosition = candidate;
            newState.PrevState = state;
            ++newState.Steps;
            //расчет ф значения для узла
            newState.WeightValue = WeightValue(newState) + newState.Steps;
            if (!(state.PrevState != null && state.PrevState.Equals(newState)))
            {
                list.Add(newState);
            }
        }

        public static State GenerateStartState()
        {
            var state = new State();
            
            var b = state.Board;
            b[0] = 2;
            b[1] = 8;
            b[2] = 1;
            b[3] = 3;
            b[4] = 6;
            b[5] = 4;
            b[6] = 7;
            b[7] = 0;
            b[8] = 5;

            state.ZeroPosition = 7;
            state.Steps = 0;
            state.WeightValue = int.MaxValue;

            return state;
        }

        public static State GenerateGoalState()
        {
            var state = new State();

            var b = state.Board;
            b[0] = 1;
            b[1] = 2;
            b[2] = 3;
            b[3] = 4;
            b[4] = 0;
            b[5] = 5;
            b[6] = 6;
            b[7] = 7;
            b[8] = 8;

            state.ZeroPosition = 4;

            return state;
        }

        public int WeightValue(State state)
        {
            var value = 9;

            for (int i = 0, size = GoalState.Board.Length; i < size; ++i)
            {
                if (GoalState.Board[i] == state.Board[i])
                {
                    --value;
                }
            }

            return value;
        }
    }
}