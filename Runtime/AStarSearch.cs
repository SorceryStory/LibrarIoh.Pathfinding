using SorceressSpell.LibrarIoh.Collections;
using System;
using System.Collections.Generic;

namespace SorceressSpell.LibrarIoh.Pathfinding
{
    public class AStarSearch<TLocation>
        where TLocation : IEquatable<TLocation>
    {
        #region Fields

        public readonly Dictionary<TLocation, TLocation> CameFrom;
        public readonly Dictionary<TLocation, float> CostSoFar;

        #endregion Fields

        #region Delegates

        public delegate float HeuristicDelegate(TLocation currentLocation, TLocation goal);

        #endregion Delegates

        #region Constructors

        public AStarSearch()
        {
            CameFrom = new Dictionary<TLocation, TLocation>();

            CostSoFar = new Dictionary<TLocation, float>();
        }

        public AStarSearch(IWeightedGraph<TLocation> graph, TLocation start, TLocation goal, HeuristicDelegate heuristic)
            : this()
        {
            Calculate(graph, start, goal, heuristic);
        }

        #endregion Constructors

        #region Methods

        public void Calculate(IWeightedGraph<TLocation> graph, TLocation start, TLocation goal, HeuristicDelegate heuristic)
        {
            CameFrom.Clear();
            CostSoFar.Clear();

            PriorityQueue<TLocation, float> frontier = new PriorityQueue<TLocation, float>();
            frontier.Enqueue(start, 0);

            CameFrom[start] = start;
            CostSoFar[start] = 0;

            while (frontier.Count > 0)
            {
                TLocation current = frontier.Dequeue();

                if (current.Equals(goal))
                {
                    break;
                }

                foreach (TLocation next in graph.GetNeighbours(current))
                {
                    float newCost = CostSoFar[current] + graph.Cost(current, next);

                    if (!CostSoFar.ContainsKey(next) || newCost < CostSoFar[next])
                    {
                        CostSoFar[next] = newCost;
                        float priority = newCost + heuristic(next, goal);
                        frontier.Enqueue(next, priority);
                        CameFrom[next] = current;
                    }
                }
            }
        }

        public List<TLocation> ReconstructPath(TLocation start, TLocation goal, bool beginOnStart, bool addStart)
        {
            List<TLocation> path = new List<TLocation>();

            if (!CameFrom.ContainsKey(goal))
            {
                return path;
            }

            TLocation current = goal;

            while (!current.Equals(start))
            {
                if (beginOnStart)
                {
                    path.Insert(0, current);
                }
                else
                {
                    path.Add(current);
                }

                current = CameFrom[current];
            }

            if (addStart)
            {
                if (beginOnStart)
                {
                    path.Insert(0, start);
                }
                else
                {
                    path.Add(start);
                }
            }

            return path;
        }

        #endregion Methods
    }
}
