using System;
using System.Collections.Generic;

namespace SorceressSpell.LibrarIoh.Pathfinding
{
    public interface IWeightedGraph<TLocation>
        where TLocation : IEquatable<TLocation>
    {
        #region Methods

        float Cost(TLocation from, TLocation to);

        List<TLocation> GetNeighbours(TLocation location);

        #endregion Methods
    }
}
