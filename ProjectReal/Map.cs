using System;
using System.Collections.Generic;
using System.Text;
using MonoGame;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System.ComponentModel.Design;
using System.Reflection.Metadata;
using System.Security.Cryptography.X509Certificates;

namespace ProjectReal
{
    class Map
    {
        Tile[,] _map;
        char[,] _symbolMap;
        Dictionary<char, Terrain> _symbolToTerrainDictionary;
       List<Tile> _tiles = new List<Tile>();
        int _tileSize = 64;
        public Map()
        {
            LoadTileMap();
            LoadTerrain();
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
            int columnCounter = 0;
            int rowCounter =0;
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
                        columnCounter = splitLine.Length;
                        for (int i = 0; i < columnCounter; i++)
                        {
                            fileMap[i, y] = Convert.ToChar(splitLine[i]);
                        }
                        y++;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("The file could not be read");
                Console.WriteLine(e.Message);
            }
            _symbolMap = new char[columnCounter, rowCounter];
            for (int yAxis = 0; yAxis < rowCounter; yAxis++)
            {
                for (int x = 0; x < columnCounter; x++)
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
                        Texture2D terrainTexture = Game._graphicsloader.Load<Texture2D>(fileName);
                        terrain = new Terrain(name,terrainTexture,movementModifier,isObjectPlaceable);
                        CreateTile(terrain);
                        
                         _symbolToTerrainDictionary.Add(symbol, terrain);
                       
                            
                        
                      
                                                              
                                                  
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("The file could not be read");
                Console.WriteLine(e.Message);
            }
        }
        private void CreateTile(Terrain terrain) //called during LoadTerrain()
        {
            Tile newTile;
            newTile = new Tile(terrain,false,false); // Creates a new tile as a new terrain is created
            
            _tiles.Add(newTile);           
        }

        private void CreateMap()
        {
            _map = new Tile[_symbolMap.GetUpperBound(0), _symbolMap.GetUpperBound(1)];
            //loop through symbol map, set each symbol to a tile on the map/
            for (int y = 0; y < _symbolMap.GetUpperBound(1); y++)
            {
                for (int x = 0; x < _symbolMap.GetUpperBound(0); x++)
                {
                    _symbolToTerrainDictionary.TryGetValue(_symbolMap[x, y], out Terrain terrainInDictionary);
                    for (int i = 0; i <_tiles.Count; i++)
                    {
                        if (_tiles[i]._terrain._name == terrainInDictionary._name)
                        {
                            _map[x,y] = _tiles[i];
                        }
                    }
                    
                }


            }
        }
        public void Draw(ref SpriteBatch spriteBatch) //draw the map
        {
            Vector2 tilePosition;
            
            for (int y = 0; y < _map.GetUpperBound(1); y++)
            {
                for (int x = 0; x < _map.GetUpperBound(0); x++)
                {
                    Texture2D tileTexture = _map[x, y]._terrain._texture;
                    spriteBatch.Draw(tileTexture,new Vector2(x*_tileSize,y*_tileSize), Color.White);
                   //DRAW terrain._texture
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
