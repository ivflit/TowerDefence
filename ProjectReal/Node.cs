using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectReal
{
    class Node
    {

        public int _x, _y;
        public bool _walkable;
        public bool _wall;
        public List<Node> _neighbors { get; set; }
        public Node _cameFrom;
        public float _movementModifier;
        public float _gCost;
        public float _hCost;
        public float _fCost;

        public Node(int x, int y, bool walkable,float movementModifier)
        {
            this._x = x;
            this._y = y;
            this._walkable = walkable;
            _neighbors = new List<Node>();
            _movementModifier = movementModifier;
        }

        public float DistanceTo(Node other)
        {
            return (float)Math.Sqrt(Math.Pow(other._x - _x, 2) + Math.Pow(other._y - _y, 2));
        }
    }
}
