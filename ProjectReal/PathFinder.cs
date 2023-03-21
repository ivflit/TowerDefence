using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Numerics;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace ProjectReal
{
    class Pathfinder
    {
        private Node[,] _nodeMap;
        private List<Node> _openSet;
        private List<Node> _closedSet;
        private List<Node> _path;
        private Node _startNode;
        private Node _endNode;

        int _winner = 0;
        public Pathfinder(Node[,] grid) //grid is the maps array of nodes
        {
            this._nodeMap = grid;

        }

        public List<Node> FindPath(Node startNode, Node endNode)
        {
            _openSet = new List<Node>();             //Openset is a list of all the cells that we have checked so we add startNode since that is the first point we check
            Node node = _nodeMap[startNode._x, startNode._y];
            _openSet.Add(node);
           
            _closedSet = new List<Node>();
            Node Current;
            

            while (_openSet.Count > 0)
            {
                for (var i = 0; i <= _openSet.Count - 1; i++)
                {
                    try
                    {
                        if (_openSet[i]._fCost < _openSet[_winner]._fCost)
                            _winner = i; //If the cost of going to The point openset[i] is less than the cost of the current winner, then set that point to be the winner
                    }
                    catch
                    {
                        _winner -= 1; //If there is no current best then we go back to the previous point that we checked and say that is the winner
                    }
                }

               // Node Current;
                Current = _openSet[_winner]; //Openset[0] is the startpoint as that is our current cell
                _openSet.Remove(Current);
                
                _closedSet.Add(Current); //We have visited the current cell so we add it to the closedset
                if (Current == _nodeMap[endNode._x,endNode._y]) // if we have reached the end
                {
                    Node temp = Current;
                    _path = new List<Node>();
                    _path.Add(Current); //adding endpoint to the path
                    while (temp._cameFrom != null)
                    {
                        _path.Add(temp._cameFrom); // add the point where it camefrom
                        temp = temp._cameFrom; //each cell has a camefrom cell
                    } //Backtracking all the points, we know where each one came from so go through it and add it to the path

                    _path.Reverse();
                    _path.RemoveAt(0); //removes the start node (where the enemy is initially) 
                    return _path;
                }

                List<Node> ListOfNeighbors = new List<Node> ();
              
                    ListOfNeighbors.AddRange(Current._neighbors);
                for (var i = 0; i <= ListOfNeighbors.Count - 1; i++)
                {
                    Node neighbor = ListOfNeighbors[i];

                    if (_closedSet.Contains(neighbor) == false & neighbor._wall == false)
                    {

                        // float TempG = Current._gCost + 1;
                        float TempG = Current._gCost + GetDistance(Current, endNode);
                        bool newpath = false;
                        if (_openSet.Contains(neighbor) == true)
                        {

                            if (TempG < neighbor._gCost)
                            {
                                neighbor._gCost = TempG;
                                neighbor._hCost = GetDistance(neighbor, endNode);//
                                neighbor._fCost = neighbor._gCost + neighbor._hCost;         
                                newpath = true;
                            }
                        }
                        else
                        {

                            neighbor._gCost = TempG;
                            newpath = true;
                            _openSet.Add(neighbor);
                            

                        }

                        if (newpath == true) //if we can go here ie a newpath can be made
                        {

                            neighbor._cameFrom = Current; //set the next cells came from as the current cell
                        }
                    }
                }
            }

            return null;
        }

        private static int GetDistance(Node nodeA, Node nodeB)
        {
            int distX = Math.Abs(nodeA._x - nodeB._x);
            int distY = Math.Abs(nodeA._y - nodeB._y);

            if (distX > distY)
            {
                return 14 * distY + 10 * (distX - distY);
            }
            else
            {
                return 14 * distX + 10 * (distY - distX);
            }
        }

        private Node GetLowestFCostNode(List<Node> nodeList)
        {
            Node lowestFCostNode = nodeList[0];

            for (int i = 1; i < nodeList.Count; i++)
            {
                if (nodeList[i]._fCost < lowestFCostNode._fCost)
                {
                    lowestFCostNode = nodeList[i];
                }
            }

            return lowestFCostNode;
        }
    }
}
    

