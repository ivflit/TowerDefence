using System;
using System.Collections.Generic;
using System.Text;
using MonoGame;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System.ComponentModel.Design;
using System.ComponentModel.DataAnnotations;

namespace ProjectReal
{
    public class Stage //the place where our game takes place; map players enemies will go here
    {
        private Input _mouse = new Input();
        private Pathfinder _pathfinder;
        private int _tileSize;
        private Dictionary<string, EnemyType> _nameOfEnemyToType;
        private bool _pathChecked;
        public int _difficulty { get; set; }
        private Player _player;
        private int _frameCounter;
        private Map _map;
        private Texture2D _interfaceTexture;
      
        private List<TowerType> _towerTypes;
        private List<string> _towerTypeNames;
        private List<string> _enemyTypeNames;
        private List<MapEnemy> _mapEnemies;
     
        
        private int _selectedBuidlingIndex;
        private Point _mouseTilePosition;
        private bool _gameOver;
        private MapEnemy _mapEnemy;
        private Shop _shop;
        private Dictionary<string, Projectile> _nameOfProjectilesToProjectile { get; set; }
        private Dictionary<string, TowerType> _nameOfTowerToTower { get; set; }
        public Stage()
        {
            _tileSize = 64;
            _mapEnemies = new List<MapEnemy>(); 
            _map = new Map();
            _pathfinder = new Pathfinder(_map._nodeMap);
            LoadEnemyTypes();
            LoadProjectiles();
            LoadTowerTypes();
            InstantiateMapEnemy();
            _shop = new Shop(_towerTypes, _towerTypeNames, _nameOfTowerToTower, _map._mapXAmount);
            
                
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


        private void LoadProjectiles()
        {
            String lineInput;
            string name;
            string fileName;
            int speed;
            int damage;
            try
            {
                using (System.IO.StreamReader ReaderForTileMap = new System.IO.StreamReader("Tower.txt"))
                {


                    while (ReaderForTileMap.EndOfStream == false)
                    {
                        Projectile projectile;
                        string[] splitLine;
                        lineInput = ReaderForTileMap.ReadLine().ToLower();
                        splitLine = lineInput.Split(",");
                        name = splitLine[0];
                        fileName = splitLine[1];
                        damage = Convert.ToInt32(splitLine[2]);
                        speed = Convert.ToInt32(splitLine[3]);
                        Texture2D ProjectileTexture = Game1._graphicsloader.Load<Texture2D>(fileName);
                        //MAKE HITBOX FOR PROJECTILE WHEN IT GETS A POSITION

                        projectile = new Projectile(ProjectileTexture, name, speed, damage);

                        _nameOfProjectilesToProjectile.Add(name, projectile);



                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("The file could not be read");
                Console.WriteLine(e.Message);
            }
        }
        
        private void LoadTowerTypes()
        {
            _nameOfTowerToTower = new Dictionary<string, TowerType>();
            _towerTypes = new List<TowerType>();
            _towerTypeNames = new List<string>();
            _nameOfProjectilesToProjectile = new Dictionary<string, Projectile>();
            String lineInput;
            string name;
            string fileName;
            String projectileType;
            int cost;
            string descriptionOfTower;
            int upgradeOneCost;
            int upgradeTwoCost;
            int health;
            try
            {
                using (System.IO.StreamReader ReaderForTileMap = new System.IO.StreamReader("Towers.txt"))
                {


                    while (ReaderForTileMap.EndOfStream == false)
                    {
                        TowerType towerType;
                        
                        string[] splitLine;
                        lineInput = ReaderForTileMap.ReadLine().ToLower();
                        splitLine = lineInput.Split(",");
                        name = splitLine[0];
                        fileName = splitLine[1];
                        projectileType = splitLine[2];
                        cost = Convert.ToInt32(splitLine[3]);
                        descriptionOfTower = splitLine[4];
                        upgradeOneCost = Convert.ToInt32(splitLine[5]);
                        upgradeTwoCost = Convert.ToInt32(splitLine[6]);
                        health = Convert.ToInt32(splitLine[7]);
                        Texture2D TowerTopTexture = Game1._graphicsloader.Load<Texture2D>(fileName);
                        Texture2D TowerBottomTexture = Game1._graphicsloader.Load<Texture2D>("towerDefense_tile181");
                       
                        _nameOfProjectilesToProjectile.TryGetValue(projectileType, out Projectile projectile);
                        towerType = new TowerType(name, upgradeOneCost, upgradeTwoCost, TowerTopTexture, TowerBottomTexture, health, cost, descriptionOfTower, projectile);
                        _towerTypes.Add(towerType);
                        _towerTypeNames.Add(name);
                        _nameOfTowerToTower.Add(name, towerType);



                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("The file could not be read");
                Console.WriteLine(e.Message);
            }
        }

        private void InstantiatePathForEnemy() //Gets node path from pathfinder, converts to vector path and inserts into the map enemy
        {
            
            List<Vector2> vectorPath;
            


            for (int i = 0; i < _mapEnemies.Count; i++) //loop through the list of enemies
            {
                List<Node> path = new List<Node>();
                _mapEnemies[i]._path = new List<Vector2>();
                Node EnemyStartNode = _mapEnemies[i]._currentNode;
                
                    path.AddRange(_pathfinder.FindPath(EnemyStartNode, _map._endNode)); //finds a path based on an enemies current node and the map end node
                
                
                vectorPath = new List<Vector2>();
                for (int count = 0; count < path.Count; count++)
                {
                     vectorPath.Add(new Vector2(path[count]._x, path[count]._y));

                }
                
                _mapEnemies[i]._path.AddRange(vectorPath); //add the contents of vector path to the map enemy path.
            }
        }

        private void ShopTowerSelection(Vector2 MousePosition)
        {
          
            //System.Diagnostics.Debug.WriteLine("{0},{1}", MousePosition.X, MousePosition.Y);
            if (_mouse.WasLeftButtonClicked())
            {

                if (_shop._selectedTower == null) //if there is no selected tower
                {
                    for (int i = 0; i < _shop._towerGridHitboxes.Count; i++)
                    {
                        if (_shop._towerGridHitboxes[i].Contains(MousePosition.X, MousePosition.Y))
                        {
                            int x = ((_shop._towerGridHitboxes[i].X + _shop._towerGrid[0, 0]._textureBottom.Width / 2) / _tileSize) - _shop._towerSectionOffsetX;
                            int y = (_shop._towerGridHitboxes[i].Y + _shop._towerGrid[0, 0]._textureBottom.Height / 2) / _tileSize;
                            _shop._selectedTower = _shop._towerGrid[x, y];
                        }
                    }
                }
                else
                {
                    bool towerSelect = false;
                    for (int i = 0; i < _shop._towerGridHitboxes.Count; i++)
                    {
                        if (_shop._towerGridHitboxes[i].Contains(MousePosition.X, MousePosition.Y))
                        {
                            int x = ((_shop._towerGridHitboxes[i].X + _shop._towerGrid[0, 0]._textureBottom.Width / 2) / _tileSize) - _shop._towerSectionOffsetX;
                            int y = (_shop._towerGridHitboxes[i].Y + _shop._towerGrid[0, 0]._textureBottom.Height / 2) / _tileSize;
                            _shop._selectedTower = _shop._towerGrid[x, y];
                            towerSelect = true;
                        }
                    }
                    if (!towerSelect)
                    {
                        _shop._selectedTower = null;
                    }


                }


            }
        }
        private bool ValidateTowerPlacement(int tileX, int tileY)
        {
            if (_map._map[tileX, tileY]._isSpawner == false && _map._map[tileX, tileY]._isEndTile == false && _map._map[tileX, tileY]._obstacle == null && _map._map[tileX, tileY]._mapTower == null && PathBlocked(tileX,tileY) == false )
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        private bool PathBlocked(int tileX, int tileY)
        {
            _map._nodeMap[tileX, tileY]._walkable = false;
            


            if (_pathfinder.FindPath(_map._startNode, _map._endNode) != null)
            {
                return false;
            }
            else
            {
                _map._nodeMap[tileX, tileY]._walkable = true;
                return true;
                
            }
            
           
        }
        private void TowerPlacement(Vector2 MousePosition)
        {

            int tileX;
            int tileY;
                    for (int i = 0; i < _map._rectangleMap.Count; i++)
                    {
               
                        if (_map._rectangleMap[i].Contains(MousePosition.X, MousePosition.Y))
                        {
                           tileX = _map._rectangleMap[i].X / _tileSize;
                           tileY = _map._rectangleMap[i].Y / _tileSize;
                            if (ValidateTowerPlacement(tileX, tileY))
                            {
                               _map._map[tileX, tileY]._mapTower = new MapTower(true, _shop._selectedTower);
                               InstantiatePathForEnemy();
                       // _map._nodeMap[tileX, tileY]._walkable = false; - done in pathBlocked()
                            }

                    
                        }
                    }
                
               
            
        }
        public void Update(GameTime gameTime) //stage will be updated every frame, well undate logic/movemt/collison here
        {
            //if (Buildingplaced)
            // {
            // InstantiatePathForEnemy();
            //}
            _mouse.Update();
            Vector2 MousePosition = _mouse.GetMousePosition();
            //System.Diagnostics.Debug.WriteLine("{0},{1}", _mapEnemies[0]._currentNode._x, _mapEnemies[0]._currentNode._y);
            if (_mouse.WasLeftButtonClicked()) //if left button was clicked
            {
                if (MousePosition.X <= _map._mapXAmount * _tileSize && MousePosition.Y <= _map._mapYAmount * _tileSize) //if mouse was in the map
                {
                    if (_shop._selectedTower != null) // if there is a selected tower
                    {
                        TowerPlacement(MousePosition); //place tower
                    }
                    else
                    {
                        //SELECT BUILDING OR OBSTACLE
                    }
                }
                else
                {
                    ShopTowerSelection(MousePosition); // if mouse is not in map bounds but left button clicked then select tower
                }
            }
                   

            for (int i = 0; i < _mapEnemies.Count; i++) //loop through the list of enemies
            {
                _mapEnemies[i].FollowPath(gameTime);
                _mapEnemies[i]._currentNode = _map._nodeMap[_mapEnemies[i]._currentNode._x, _mapEnemies[i]._currentNode._y];
            }
        }

    
        public void Draw(SpriteBatch spriteBatch)
        {
            _map.Draw(spriteBatch);
            for (int i = 0; i < _mapEnemies.Count; i++)
            {
                _mapEnemies[i].Draw(spriteBatch);
            }
            _shop.Draw(spriteBatch);
            
        }

    }
}
