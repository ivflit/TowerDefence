using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Net.Mime;

namespace ProjectReal
{
    class Map
    {
        
        public Node _startNode { get; set; }
        public Node _endNode;
        public Node[,] _listOfNodes { get; set; }
        Node _tileNode;
        Tile[,] _map;
        char[,] _symbolMap;
        Dictionary<char, Terrain> _symbolToTerrainDictionary;
        List<Tile> _tiles = new List<Tile>();
        int _tileSize = 64;
       public int _mapXAmount { get; set; }   

       public  int _mapYAmount { get; set; }
        public List<Vector2> _spawnerPositions { get; set; }
        public Map()
        {
            LoadTileMap();
            LoadTerrain();//calls createTile
            CreateMap();
            GetNeighborNodes();
            //tileSize = 32;
            //Vector2 tilePosition;
            //Vector2 tileCentre = new Vector2(_tileSize / 2, _tileSize / 2);
            //tilePosition = new Vector2(x * _tileSize, y * _tileSize);

        }

        private void LoadTileMap()//loading tilemap.txt into _map
        {
            Random rnd = new Random();
          
            char[,] fileMap = new char[20, 20];
            String lineInput;
            string[] splitLine;
            int ColumnLength = 0;
            int columnAmount = 0;
            int rowCounter = 0;
            int rowAmount = 0;
            int y = 0;
            try
            {
                using (System.IO.StreamReader ReaderForTileMap = new System.IO.StreamReader("TileMap.txt"))
                {


                    while (ReaderForTileMap.EndOfStream == false)
                    {

                        lineInput = ReaderForTileMap.ReadLine();
                        rowCounter++;
                        splitLine = lineInput.Split(",");
                        ColumnLength = splitLine.Length;

                        for (int i = 0; i < ColumnLength; i++)
                        {
                            if (y == 0) //allows us to get the amount of coloumns in the file 
                            {
                                columnAmount++;
                            }

                            fileMap[i, y] = Convert.ToChar(splitLine[i]);
                        }
                        y++;
                    }
                }
            }
            catch (Exception e)
            {
                //Console.WriteLine("The file could not be read");
                //Console.WriteLine(e.Message);
            }
            rowAmount = rowCounter - 1; //this allows us to get the correct amount of rows
            int YvalueForSpawner = rnd.Next(rowAmount);
            int YvalueForEndTile = rnd.Next(rowAmount);
            _symbolMap = new char[columnAmount, rowAmount];
            for (int yAxis = 0; yAxis < rowAmount; yAxis++)
            {
                for (int x = 0; x < columnAmount; x++)
                {
                    if ((yAxis == YvalueForSpawner && x == 0) || (yAxis == YvalueForEndTile  && x == columnAmount-1) )
                    {
                        fileMap[x, yAxis] = 'p';
                    }
                    _symbolMap[x, yAxis] = fileMap[x, yAxis];
                }

            }
        }

        private void LoadTerrain() //loading from terrain.txt and making new terrain based on the content of the file
        {
            String lineInput;
            string name;
            string fileName;
            char symbol;
            float movementModifier;
            Boolean isObjectPlaceable;
            int y = 0;
            _symbolToTerrainDictionary = new Dictionary<char, Terrain>();
            try
            {
                using (System.IO.StreamReader ReaderForTileMap = new System.IO.StreamReader("Terrain.txt"))
                {


                    while (ReaderForTileMap.EndOfStream == false)
                    {
                        Terrain terrain;
                        string[] splitLine;
                        lineInput = ReaderForTileMap.ReadLine();
                        splitLine = lineInput.Split(",");
                        name = splitLine[0];
                        fileName = splitLine[1];
                        movementModifier = float.Parse(splitLine[2]);
                        isObjectPlaceable = Convert.ToBoolean(splitLine[3]);
                        symbol = Convert.ToChar(splitLine[4]);
                        Texture2D terrainTexture = Game1._graphicsloader.Load<Texture2D>(fileName);
                        
                        terrain = new Terrain(name, terrainTexture, movementModifier, isObjectPlaceable);
                        CreateTile(terrain);

                        _symbolToTerrainDictionary.Add(symbol, terrain);






                    }
                }
            }
            catch (Exception e)
            {
                //Console.WriteLine("The file could not be read");
                //Console.WriteLine(e.Message);
            }
        }
        private void CreateTile(Terrain terrain) //called during LoadTerrain()
        {
            Tile newTile;
            newTile = new Tile(terrain, false, false); // Creates a new tile as a new terrain is created

            _tiles.Add(newTile);
        }
        private void CreateMap()
        {
            
           


            _mapXAmount = _symbolMap.GetUpperBound(0) + 1;
            _mapYAmount = _symbolMap.GetUpperBound(1) + 1;
            _spawnerPositions = new List<Vector2>();
            int portalCount = 0;
            _map = new Tile[_mapXAmount, _mapYAmount];
            _listOfNodes = new Node[_mapXAmount, _mapYAmount];
            //loop through symbol map, set each symbol to a tile on the map/
            for (int y = 0; y < _mapYAmount; y++)
            {
                for (int x = 0; x < _mapXAmount; x++) //loop through each tile on the map
                {
                    _symbolToTerrainDictionary.TryGetValue(_symbolMap[x, y], out Terrain terrainInDictionary); //get the terrain of the symbol
                    for (int i = 0; i < _tiles.Count; i++)
                    {
                        if (_tiles[i]._terrain._name == terrainInDictionary._name)
                        {
                            Tile mapTile = new Tile(_tiles[i]._terrain, _tiles[i]._isSpawner, _tiles[i]._isEndTile); //create a new tile based on the tile properties
                            if (terrainInDictionary._name == "portal")
                            {
                                //portalCount++;
                                if (x == 0)
                                {
                                    mapTile._isSpawner = true;
                                    _spawnerPositions.Add(new Vector2(x * _tileSize, y * _tileSize));
                                   //_spawnerPositions.Add(new Vector2(x, y));
                                     _startNode = new Node(x, y,true, mapTile._terrain._movementModifier);
                                    
                                }

                                else
                                {
                                    mapTile._isEndTile = true;
                                    
                                    _endNode = new Node(x, y, true,mapTile._terrain._movementModifier);
                                }

                            }
                            _map[x, y] = mapTile;                            
                            _tileNode = new Node(x, y, true, mapTile._terrain._movementModifier);//make node
                            
                            _listOfNodes[x, y] = _tileNode; //make node map
                            
                        }
                    }

                }

            }
                         
           
        }
        private void GetNeighborNodes()
            {

            int noneigh = 0;
            for (int y = 0; y < _listOfNodes.GetUpperBound(1)+1; y++)
            {
                for (int x = 0; x < _listOfNodes.GetUpperBound(0) +1; x++) //loop through the array of nodes, add neighbours to each node given the x and y of each node
                {
                    List<Node> neighborNodes = new List<Node>();
                    
                    // Check the node above
                    if (y > 0)
                    {
                        neighborNodes.Add(_listOfNodes[x, y - 1]);
                    }

                    //int ylength = _listOfNodes.GetLength(1);
                    // Check the node below
                    if (y < _listOfNodes.GetLength(1)-1)
                    {
                        neighborNodes.Add(_listOfNodes[x, y + 1]);
                    }

                    // Check the node to the left
                    if (x > 0)
                    {
                        neighborNodes.Add(_listOfNodes[x - 1, y]);
                    }

                    // Check the node to the right
                    if (x < _listOfNodes.GetLength(0)-1 )
                    {
                        neighborNodes.Add(_listOfNodes[x + 1, y]);
                    }
                    
                    if (neighborNodes.Count ==0)
                    {
                        noneigh++;
                    }
                    _listOfNodes[x, y]._neighbors.AddRange(neighborNodes); //that nodes neighbours are the list we just created
                }
            }
            
        }
       
        public void Draw(SpriteBatch spriteBatch) //draw the map
        {

            for (int y = 0; y < _mapYAmount; y++)
            {
                for (int x = 0; x < _mapXAmount; x++)
                {
                    if (_map[x, y]._isSpawner == true || _map[x,y]._isEndTile == true) 
                    {
                        spriteBatch.Draw(_map[x, y]._terrain._texture, new Vector2(x * _tileSize, y * _tileSize), Color.Purple);
                    }
                    else
                    {
                        spriteBatch.Draw(_map[x, y]._terrain._texture, new Vector2(x * _tileSize, y * _tileSize), Color.White);
                    }
                    

                }


            }
        }
        private void CreateSpawner()
        {

        }
        private void Pathfind()
        {

        }

        private void CheckIfPathBlocked()
        {

        }


        private void SpawnEnemy()
        {

        }

    }
}
