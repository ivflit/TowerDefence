﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Net.Mime;
using System.Reflection.Metadata;

namespace ProjectReal
{
    class Map
    {
        
        public Node _startNode { get; set; }
        public Node _endNode;
        public Node[,] _nodeMap { get; set; }
    public List<Microsoft.Xna.Framework.Rectangle> _rectangleMap { get; set; }
        Node _tileNode;
       public Tile[,] _map;
        char[,] _symbolMap;
        Dictionary<char, Terrain> _symbolToTerrainDictionary;
        Dictionary<string,Obstacle> _nameToObstacleDictionary;
        List<string> _obstacleNames;
        List<Tile> _tiles = new List<Tile>();
        int _tileSize = 64;
       public int _mapXAmount { get; set; }   

       public  int _mapYAmount { get; set; }
        public List<Vector2> _spawnerPositions { get; set; }
        public Map()
        {
            LoadTileMap();
            LoadTerrain();//calls createTile
            LoadObstacles();
            CreateMap();
            spawnObstacles();
            GetNeighborNodes();
            Console.WriteLine();
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
                   //if ((yAxis==4 && x ==0) || (yAxis==4 && x ==9)) //GET RID TO MAKE IT RANDOM
                    //{
                        fileMap[x, yAxis] = 'p';
                    //}
                        
                    }
                    _symbolMap[x, yAxis] = fileMap[x, yAxis];
                }

            }
        }
        private void LoadObstacles()//loading tilemap.txt into _map
        {
          _obstacleNames = new List<string>();
            _nameToObstacleDictionary = new Dictionary<string, Obstacle>();
            String lineInput;
            string[] splitLine;
            string name;
            string fileName;
            int costToRemove;
            try
            {
                using (System.IO.StreamReader ReaderForTileMap = new System.IO.StreamReader("Obstacles.txt"))
                {


                    while (ReaderForTileMap.EndOfStream == false)
                    {

                        Obstacle obstacle;
                        lineInput = ReaderForTileMap.ReadLine();
                        splitLine = lineInput.Split(",");
                        name = splitLine[0];
                        fileName = splitLine[1];
                        costToRemove = Convert.ToInt16(splitLine[2]);
                        Texture2D obstacleTexture = Game1._graphicsloader.Load<Texture2D>(fileName);
                        _obstacleNames.Add(name); 
                        obstacle = new Obstacle(name, costToRemove, obstacleTexture, Microsoft.Xna.Framework.Color.White);
                        _nameToObstacleDictionary.Add(name, obstacle);

                    }
                }
            }
            catch (Exception e)
            {
                //Console.WriteLine("The file could not be read");
                //Console.WriteLine(e.Message);
            }
           
        }
        private void spawnObstacles()
        {
            Random rnd = new Random();
            Tile randomTile;
            int XvalueForSpawner;
            int YvalueForSpawner;
            int amountOfObstacles = 10;
            for (int i = 0; i < amountOfObstacles +1; i++)
            { 
                int ObstacleIndex= rnd.Next(_obstacleNames.Count);
                do
                {
                     XvalueForSpawner = rnd.Next(1,_mapXAmount-1); //avoids obstacles surrounding the spawners
                     YvalueForSpawner = rnd.Next(1, _mapYAmount - 1);
                     randomTile = _map[XvalueForSpawner, YvalueForSpawner];
                } while (_symbolMap[XvalueForSpawner,YvalueForSpawner] == 'p' || randomTile._obstacle !=null);
                
                string name = _obstacleNames[ObstacleIndex];
               
                _nameToObstacleDictionary.TryGetValue(name, out Obstacle obstacle);
                Obstacle mapObstacle = new Obstacle(name, obstacle._costToRemove,obstacle._texture,obstacle._color);
                _map[XvalueForSpawner, YvalueForSpawner]._obstacle = mapObstacle;
                _nodeMap[XvalueForSpawner, YvalueForSpawner]._walkable = false;
            }
        }// Generates and places obstacles
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



            _rectangleMap = new List<Rectangle>();
            _mapXAmount = _symbolMap.GetUpperBound(0) + 1;
            _mapYAmount = _symbolMap.GetUpperBound(1) + 1;
            _spawnerPositions = new List<Vector2>();
            
            _map = new Tile[_mapXAmount, _mapYAmount];
            _nodeMap = new Node[_mapXAmount, _mapYAmount];
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
                                    _spawnerPositions.Add(new Vector2(0, (y * _tileSize)));
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
                          
                            int rectangleX = (x * _tileSize); //-_listOfTowers[i]._textureBottom.Width/2;
                            int rectangleY;
                            if (y == 0)
                            {
                                rectangleY = 0;
                            }
                            else
                            {
                                rectangleY = (y * _tileSize);
                            }

                            int rectangleWidth = mapTile._terrain._texture.Width;
                            int rectangleHieght = mapTile._terrain._texture.Height;
                            _rectangleMap.Add(new Microsoft.Xna.Framework.Rectangle(x * _tileSize, y * _tileSize, rectangleWidth, rectangleHieght));
                            _tileNode = new Node(x, y, true, mapTile._terrain._movementModifier);//make node
                            
                            _nodeMap[x, y] = _tileNode; //make node map
                            
                        }
                    }

                }

            }
                         
           
        }
        public void GetNeighborNodes()
            {

            int noneigh = 0;
            for (int y = 0; y < _nodeMap.GetUpperBound(1)+1; y++)
            {
                for (int x = 0; x < _nodeMap.GetUpperBound(0) +1; x++) //loop through the array of nodes, add neighbours to each node given the x and y of each node
                {
                    List<Node> neighborNodes = new List<Node>();
                    _nodeMap[x, y]._neighbors = new List<Node>();
                    // Check the node above
                    if (_nodeMap[x, y]._walkable ==true) {
                        if (y > 0 && _nodeMap[x, y - 1]._walkable == true)
                        {
                            int newY = y - 1;
                            neighborNodes.Add(_nodeMap[x, newY]);
                        }

                        //int ylength = _listOfNodes.GetLength(1);
                        // Check the node below
                        if (y < _nodeMap.GetLength(1) - 1 && _nodeMap[x, y + 1]._walkable == true)
                        {
                            int newY = y + 1;
                            neighborNodes.Add(_nodeMap[x, newY]);
                        }

                        // Check the node to the left
                        if (x > 0 && _nodeMap[x - 1, y]._walkable == true)
                        {
                            int newX = x - 1;
                            neighborNodes.Add(_nodeMap[newX, y]);
                        }

                        // Check the node to the right
                        if (x < _nodeMap.GetLength(0) - 1 && _nodeMap[x + 1, y]._walkable == true)
                        {
                            int newx = x + 1;
                            neighborNodes.Add(_nodeMap[newx, y]);
                        }

                        neighborNodes.Remove(_nodeMap[x, y]);
                        if (neighborNodes.Count == 0)
                        {
                            noneigh++;
                        }
                        _nodeMap[x, y]._neighbors.AddRange(neighborNodes); //that nodes neighbours are the list we just created
                    }
                   
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
                        spriteBatch.Draw(_map[x, y]._terrain._texture, new Vector2(x * _tileSize, y * _tileSize), Color.Purple); //draw spawner + end tile
                    }
                    else
                    {
                        spriteBatch.Draw(_map[x, y]._terrain._texture, new Vector2(x * _tileSize, y * _tileSize), Color.White); //draw other tiles
                    }
                    if (_map[x, y]._obstacle != null)
                    {
                        spriteBatch.Draw(_map[x, y]._obstacle._texture, new Vector2(x * _tileSize, y * _tileSize), _map[x, y]._obstacle._color);//draw obstaclles
                    }
                    if (_map[x, y]._mapTower != null)
                    {
                        Vector2 origin = new Vector2(_map[x, y]._mapTower._towerType._textureTop.Width / 2, _map[x, y]._mapTower._towerType._textureTop.Height / 2);
                        Texture2D rectTexture = new Texture2D(Game1._graphics.GraphicsDevice, 1, 1);
                        rectTexture.SetData(new Color[] { Color.White });
                        spriteBatch.Draw(_map[x, y]._mapTower._towerType._textureBottom, new Vector2(x * _tileSize, y * _tileSize),_map[x, y]._mapTower._color);
                        spriteBatch.Draw(_map[x, y]._mapTower._towerType._textureTop, new Vector2(x * _tileSize +32, y * _tileSize + 32), null, _map[x, y]._mapTower._color, _map[x, y]._mapTower._angleOfTexture, origin, 1f, SpriteEffects.None, 0);
                        _map[x, y]._mapTower.DrawHealthBar(spriteBatch, rectTexture, _map[x, y]._mapTower._health, _map[x, y]._mapTower._maxHealth, Microsoft.Xna.Framework.Color.Red, Microsoft.Xna.Framework.Color.LawnGreen);
                        // spriteBatch.Draw(_map[x, y]._mapTower._towerType._textureTop, new Vector2(x * _tileSize, y * _tileSize), _map[x, y]._mapTower._color);




                    }

                }


            }
        }
      
        }

    }

