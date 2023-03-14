using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectReal
{
    class Node
    {
        public int x, y;
        public bool walkable;
        public List<Node> neighbors;
        public Node parent;
        public float gScore;
        public float hScore;
        public float fScore { get { return gScore + hScore; } }

        public Node(int x, int y, bool walkable)
        {
            this.x = x;
            this.y = y;
            this.walkable = walkable;
            neighbors = new List<Node>();
        }

        public float DistanceTo(Node other)
        {
            return (float)Math.Sqrt(Math.Pow(other.x - x, 2) + Math.Pow(other.y - y, 2));
        }
    }
}
