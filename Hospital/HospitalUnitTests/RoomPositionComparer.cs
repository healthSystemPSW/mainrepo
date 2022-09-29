using Hospital.GraphicalEditor.Model;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace HospitalUnitTests
{
    internal class RoomPositionComparer : IEqualityComparer<RoomPosition>
    {
      
        public bool Equals(RoomPosition x, RoomPosition y)
        {
            if (x is null && y is null)
                return true;

            if (x is null || y is null)
                return false;
           
            return x.DimensionX == y.DimensionX
                && x.DimensionY == y.DimensionY
                && x.Height == y.Height
                && x.Width == y.Width;
        }

        public int GetHashCode([DisallowNull] RoomPosition obj)
        {
           
                if (obj == null) return 0;

                return obj.DimensionX.GetHashCode() ^ obj.DimensionY.GetHashCode() ^ obj.Height.GetHashCode() ^ obj.Width.GetHashCode();
            
        }
    }
}