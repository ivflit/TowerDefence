using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProjectReal
{
//    class Pathfinder
//    {
//        private Node[,] _nodes;
//        private int _width, _height;

//        public Pathfinder(int width, int height)
//        {
//            this._width = width;
//            this._height = height;
//            _nodes = new Node[width, height];

//            // Initialize the nodes with their positions and whether they are walkable
//            for (int x = 0; x < width; x++)
//            {
//                for (int y = 0; y < height; y++)
//                {
//                   // bool walkable = /* determine whether this node is walkable */
//                    //_nodes[x, y] = new Node(x, y, walkable);
//                }
//            }

//            // Set up the neighbors for each node
//            for (int x = 0; x < width; x++)
//            {
//                for (int y = 0; y < height; y++)
//                {
//                    Node node = _nodes[x, y];

//                    if (x > 0)
//                        node.neighbors.Add(_nodes[x - 1, y]);
//                    if (x < width - 1)
//                        node.neighbors.Add(_nodes[x + 1, y]);
//                    if (y > 0)
//                        node.neighbors.Add(_nodes[x, y - 1]);
//                    if (y < height - 1)
//                        node.neighbors.Add(_nodes[x, y + 1]);
//                }
//            }
//        }

//        public List<Node> FindPath(Node startNode, Node endNode)
//        {
//            // Initialize the open and closed sets
//            List<Node> openSet = new List<Node>();
//            HashSet<Node> closedSet = new HashSet<Node>();
//            openSet.Add(startNode);

//            // Initialize the scores for each node
//            startNode.gScore = 0;
//            startNode.hScore = startNode.DistanceTo(endNode);

//            while (openSet.Count > 0)
//            {
//                // Find the node with the lowest fScore in the open set
//                Node current = openSet.OrderBy(n => n.fScore).First();

//                // If we've reached the end node, return the path
//                if (current == endNode)
//                {
//                    List<Node> path = new List<Node>();
//                    while (current != null)
//                    {
//                        path.Add(current);
//                        current = current.parent;
//                    }
//                    path.Reverse();
//                    return path;
//                }
//            }
//        }
//    }
}
