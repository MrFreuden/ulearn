using Digger.Architecture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Digger
{
    public static class GameFieldHelper
    {
        public static bool IsWithinMapBounds(int x, int y) =>
      x >= 0 && x < Game.MapWidth && y >= 0 && y < Game.MapHeight;

        public static bool IsCreatureOrEmptyBelow(int x, int y, Type? creatureType) =>
            IsWithinMapBounds(x, y + 1) && Game.Map[x, y + 1]?.GetType() == creatureType;

        public static bool IsMovementBlockerPlayerOnWay(int x, int y)
        {
            return Game.Map[x, y] is IMovementBlockerPlayer;
        }
    }
}
