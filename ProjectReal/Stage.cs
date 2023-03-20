using System;
using System.Collections.Generic;
using System.Text;
using MonoGame;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace ProjectReal
{
    public class Stage //the place where our game takes place; map players enemies will go here
    {
        private Pathfinder _pathfinder;
        private int _tileSize;
        private Dictionary<string, EnemyType> _nameOfEnemyToType;
        public int _difficulty { get; set; }
        private Player _player;
        private int _frameCounter;
        private Map _map;
        private Texture2D _interfaceTexture;
        private List<TowerType> _towerTypes;
        private List<string> _enemyTypeNames;
        private List<MapEnemy> _mapEnemies;
        private int _selectedBuidlingIndex;
        private Point _mouseTilePosition;
        private bool _gameOver;
        private MapEnemy _mapEnemy;
       
        public Stage()
        {
            _tileSize = 64;
            _mapEnemies = new List<MapEnemy>(); 
            _map = new Map();
            _pathfinder = new Pathfinder(_map._listOfNodes);
            LoadEnemyTypes();
            InstantiateMapEnemy();
            
            
                
        }

        private void LoadEnemyTypes()
        {
            
            String lineInput;
            string name;
            string fileName;
            int health;
            int speed;
            int damage;
            bool isCamoflagued;
            _enemyTypeNames = new List<string>();
            _nameOfEnemyToType = new Dictionary<string, EnemyType>();
            try
            {
                using (System.IO.StreamReader ReaderForTileMap = new System.IO.StreamReader("Enemies.txt"))
                {


                    while (ReaderForTileMap.EndOfStream == false)
                    {
                        EnemyType enemyType;
                        string[] splitLine;
                        lineInput = ReaderForTileMap.ReadLine().ToLower();
                        splitLine = lineInput.Split(",");
                        name = splitLine[0];
                        fileName = splitLine[1];
                        health = Convert.ToInt32(splitLine[2]);
                        speed = Convert.ToInt32(splitLine[3]);
                        damage = Convert.ToInt32(splitLine[4]);
                        isCamoflagued = Convert.ToBoolean(splitLine[5]);
                        Texture2D enemyTexture = Game1._graphicsloader.Load<Texture2D>(fileName);
                        enemyType = new EnemyType(enemyTexture, health, speed, damage, name, isCamoflagued);
                        _nameOfEnemyToType.Add(name, enemyType);
                        _enemyTypeNames.Add(name);

                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("The file could not be read");
                Console.WriteLine(e.Message);
            }
        }

   
       
     private void InstantiateMapEnemy()
        {
            Random rnd = new Random();
             int randomEnemyIndex = rnd.Next(_enemyTypeNames.Count);
            string nameOfenemy = _enemyTypeNames[randomEnemyIndex];

            _nameOfEnemyToType.TryGetValue(nameOfenemy, out EnemyType enemyType);
            //_nameOfEnemyToType[nameOfenemy, out EnemyType enemyType];


            MapEnemy newEnemy = new MapEnemy(_map._spawnerPositions[0], enemyType);
            newEnemy._currentNode = new Node(_map._startNode._x, _map._startNode._y, _map._startNode._walkable, _map._startNode._movementModifier);
            
            _mapEnemies.Add(newEnemy);

            InstantiatePathForEnemy();
        }


        private void LoadBuildingTypes()
        { 
        
        }
    
        private void InstantiatePathForEnemy() //Gets node path from pathfinder, converts to vector path and inserts into the map enemy
        {
            
            List<Vector2> vectorPath;
            


            for (int i = 0; i < _mapEnemies.Count; i++) //loop through the list of enemies
            {
                List<Node> path = new List<Node>();
                path.AddRange( _pathfinder.FindPath(_mapEnemies[i]._currentNode, _map._endNode )); //finds a path based on an enemies current node and the map end node NEED TO CHANGE TO TOWER WITH LOWEST HEALTH LATER
                vectorPath = new List<Vector2>();
                for (int count = 0; count < path.Count; count++)
                {
                     vectorPath.Add(new Vector2(path[count]._x, path[count]._y));

                }
                _mapEnemies[i]._path = new List<Vector2>();
                _mapEnemies[i]._path.AddRange(vectorPath); //add the contents of vector path to the map enemy path.
            }
        }
        
        public void Update(GameTime gameTime) //stage will be updated every frame, well undate logic/movemt/collison here
        {
            //if (Buildingplaced)
            // {
            // InstantiatePathForEnemy();
            //}
            for (int i = 0; i < _mapEnemies.Count; i++) //loop through the list of enemies
            {
                _mapEnemies[i].FollowPath(gameTime);
            }
        }

    
        public void Draw(SpriteBatch spriteBatch)
        {
            _map.Draw(spriteBatch);
            for (int i = 0; i < _mapEnemies.Count; i++)
            {
                _mapEnemies[i].Draw(spriteBatch);
            }
            
        }

    }
}
