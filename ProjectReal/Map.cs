using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Net.Mime;

namespace ProjectReal
{
    class Map
    {
        Tile[,] _map;
        char[,] _symbolMap;
        Dictionary<char, Terrain> _symbolToTerrainDictionary;
        List<Tile> _tiles = new List<Tile>();
        int _tileSize = 64;
        int _mapXAmount;
        int _mapYAmount;
        public Map()
        {
            LoadTileMap();
            LoadTerrain();//calls createTile
            CreateMap();
            //tileSize = 32;
            //Vector2 tilePosition;
            //Vector2 tileCentre = new Vector2(_tileSize / 2, _tileSize / 2);
            //tilePosition = new Vector2(x * _tileSize, y * _tileSize);

        }

        private void LoadTileMap()//loading tilemap.txt into _map
        {
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
            _symbolMap = new char[columnAmount, rowAmount];
            for (int yAxis = 0; yAxis < rowAmount; yAxis++)
            {
                for (int x = 0; x < columnAmount; x++)
                {
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
            
            Random rnd = new Random();
            
            
            
            _mapXAmount = _symbolMap.GetUpperBound(0) + 1;
            _mapYAmount = _symbolMap.GetUpperBound(1) + 1;
            int YvalueForSpawner = rnd.Next(_mapYAmount);
            int YvalueForEndTile = rnd.Next(_mapYAmount);
            _map = new Tile[_mapXAmount, _mapYAmount];
            //loop through symbol map, set each symbol to a tile on the map/
            for (int y = 0; y < _mapYAmount; y++)
            {
                for (int x = 0; x < _mapXAmount; x++)
                {
                    _symbolToTerrainDictionary.TryGetValue(_symbolMap[x, y], out Terrain terrainInDictionary);
                    for (int i = 0; i < _tiles.Count; i++)
                    {
                        if (_tiles[i]._terrain._name == terrainInDictionary._name)
                        {
                            Tile mapTile = new Tile(_tiles[i]._terrain, _tiles[i]._isSpawner, _tiles[i]._isEndTile);
                            _map[x, y] = mapTile;
                           
                        }
                    }

                }


            }
           
                _map[0, YvalueForSpawner]._isSpawner = true;
            _map[_mapXAmount - 1,YvalueForEndTile]._isEndTile = true;
           
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
