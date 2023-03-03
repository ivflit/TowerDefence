using System;
using System.Collections.Generic;
using System.Text;
using MonoGame;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
namespace ProjectReal
{
    class Map
    {
        char[,] _map;
        Dictionary<char, Terrain> _symbolToTerrainDictionary;
       
         
        public Map()
        {
            LoadTileMap();
            LoadTerrain();
            //tileSize = 32;
            //Vector2 tilePosition;
            //Vector2 tileCentre = new Vector2(_tileSize / 2, _tileSize / 2);
            //tilePosition = new Vector2(x * _tileSize, y * _tileSize);

        }

        private void LoadTileMap()//loading tilemap.txt into _map
        {
            String lineInput;
            string[] splitLine;
            int y = 0;
            try
            {
                using (System.IO.StreamReader ReaderForTileMap = new System.IO.StreamReader("TileMap.txt"))
                {


                    while (ReaderForTileMap.EndOfStream == false)
                    {

                        lineInput = ReaderForTileMap.ReadLine();
                        splitLine = lineInput.Split(",");
                        
                        for (int i = 0; i < lineInput.Length; i++)
                        {
                            _map[i, y] = Convert.ToChar(splitLine[i]);
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
        }

        private void LoadTerrain() //loading from terrain.txt and making new terrain based on the content of the file
        {
            int terrainCount;
            String lineInput;
            string name;
            string fileName;
            char symbol;
            float movementModifier;
            Boolean isObjectPlaceable;
            
            int y = 0;
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
        private void CreateTiles()
        {
            
           
                
        }

        private void DisplayMap()
        {
            Terrain terrain;
            for (int y = 0; y < _map.GetUpperBound(1); y++)
            {
                for (int x = 0; x < _map.GetUpperBound(0); x++)
                {

                    _symbolToTerrainDictionary.TryGetValue(_map[x, y], out terrain);
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
