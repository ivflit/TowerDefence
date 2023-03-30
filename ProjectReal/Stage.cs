using System;
using System.Collections.Generic;
using System.Text;
using MonoGame;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System.ComponentModel.Design;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using System.Reflection.Metadata;

namespace ProjectReal
{
    public class Stage //the place where our game takes place; map players enemies will go here
    {
        private Input _mouse = new Input();
        private Pathfinder _pathfinder;
        private Player _currentPlayer;
        private int _tileSize;
        private Dictionary<string, EnemyType> _nameOfEnemyToType;
        private bool _pathChecked;
        private int _intersectionCount;
        private int _waveEnemyAmount;
        private int _waveAmount = 4;
        public float _damageTimer = 0;
        public float _damageRate = 5;
        public int _difficulty { get; set; }
        private Player _player;
        private int _frameCounter;
        private Map _map;
        private Texture2D _interfaceTexture;
        private MapEnemy _towerTargetedEnemy;
        private List<TowerType> _towerTypes;
        private List<string> _towerTypeNames;
        private List<string> _enemyTypeNames;
        private List<MapEnemy> _mapEnemies = new List<MapEnemy>();
        private List<MapTower> _mapTowers = new List<MapTower>();
        private List<Projectile> _projectilesOnMap = new List<Projectile>();
        private int _selectedTileX;
        private int _selectedTileY;
        private float _spawnTimer=0;
        private float _waveTimer = 0;
        private bool _waveEnded = false;
        private int _selectedBuidlingIndex;
        private Point _mouseTilePosition;
        private bool _gameOver;
        private MapEnemy _mapEnemy;
        private Shop _shop;
        private int _enemiesKilled;
        private bool _startWaveEarly;
        private bool _startWave;
        private int _enemiesSpawned;
        private int _enemyAmountBeforeBoss;
        private bool _bossWave;
        private List<Boss> _bossList = new List<Boss>();
        private bool _bossAttacked;
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
            //InstantiateMapEnemy();

            // player(health, score ,money, isAlive, stats offset)
            _waveEnemyAmount = 5;//could vary on difficulty
            _currentPlayer = new Player(100,0,800,true, _map._mapYAmount, _map._mapXAmount);
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
            int score;
            int money;
            int livesLost;
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
                        score = Convert.ToInt32(splitLine[6]);
                        money = Convert.ToInt32(splitLine[7]);
                        livesLost = Convert.ToInt32(splitLine[8]);
                        Texture2D enemyTexture = Game1._graphicsloader.Load<Texture2D>(fileName);

                        enemyType = new EnemyType(enemyTexture, health, speed, damage, name, isCamoflagued,score,money, livesLost);
                        if (enemyType._typeName != "boss")
                        {
                           _enemyTypeNames.Add(name);
                        }
                        _nameOfEnemyToType.Add(name, enemyType);





                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("The file could not be read");
                Console.WriteLine(e.Message);
            }
        }

        private void InstantiateMapBoss()
        {
            _nameOfEnemyToType.TryGetValue("boss", out EnemyType enemyType);
            Boss mapBoss = new Boss(_map._spawnerPositions[0], enemyType);
            mapBoss._currentNode = new Node(_map._startNode._x, _map._startNode._y, _map._startNode._walkable, _map._startNode._movementModifier);

            mapBoss._moneyPerKill = mapBoss._enemyType._moneyPerKill;
            mapBoss._scorePerKill = mapBoss._enemyType._scorePerKill;
            mapBoss._livesLostWhenEndReached = mapBoss._enemyType._livesLostWhenEnd;
            mapBoss._towerList = new List<MapTower>();
            _mapEnemies.Add(mapBoss);
            _bossList.Add(mapBoss);
            InstantiatePathForEnemy();

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
            if (_mapEnemies.Count >0)
            {
                newEnemy._indexInMapEnemy = _mapEnemies.Count - 1;
            }
            newEnemy._moneyPerKill = newEnemy._enemyType._moneyPerKill;
            newEnemy._scorePerKill = newEnemy._enemyType._scorePerKill;
            newEnemy._livesLostWhenEndReached = newEnemy._enemyType._livesLostWhenEnd;
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

            _nameOfProjectilesToProjectile = new Dictionary<string, Projectile>();
            try
            {
                using (System.IO.StreamReader ReaderForTileMap = new System.IO.StreamReader("Projectile.txt"))
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

                        projectile = new Projectile(ProjectileTexture, name, speed);

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
           
            String lineInput;
            string name;
            string fileName;
            String projectileType;
            int cost;
            string descriptionOfTower;
            int upgradeOneCost;
            int upgradeTwoCost;
            int health;
            int damage;
            float fireRate;
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
                        fireRate = Convert.ToInt32(splitLine[8]);
                        damage = Convert.ToInt32(splitLine[9]);
                        Texture2D TowerTopTexture = Game1._graphicsloader.Load<Texture2D>(fileName);
                        Texture2D TowerBottomTexture = Game1._graphicsloader.Load<Texture2D>("towerDefense_tile181");

                        _nameOfProjectilesToProjectile.TryGetValue(projectileType, out Projectile projectile);
                        Projectile projectile1 = new Projectile(projectile._texture, projectile._name, projectile._speed);
                        towerType = new TowerType(name, upgradeOneCost, upgradeTwoCost, TowerTopTexture, TowerBottomTexture, health, cost, descriptionOfTower, projectile1, fireRate,damage);
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
                List<Node> _test = new List<Node>();
                List<Node> path = new List<Node>();
                _mapEnemies[i]._path = new List<Vector2>();
                //_mapEnemies[i]._path = null;
                // Node EnemyStartNode = _mapEnemies[i]._currentNode;
                _map.GetNeighborNodes();
               // _test = _pathfinder.FindPath(_mapEnemies[i]._currentNode, _map._endNode);
               // if (_test != null)
               // {
                    path.AddRange(_pathfinder.FindPath(_mapEnemies[i]._currentNode, _map._endNode)); //finds a path based on an enemies current node and the map end node
              //  }
                


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

            //System.Diagnotics.Debug.WriteLine("{0},{1}", MousePosition.X, MousePosition.Y);
           
                
            bool towerSelect = false;
           

            for (int i = 0; i < _shop._towerGridHitboxes.Count; i++)
                    {
                        if (_shop._towerGridHitboxes[i].Contains(MousePosition.X, MousePosition.Y))
                        {
                    //  DeSelect();
                           if (_shop._isTower == true)
                           {
                            _map._map[_shop._selectedTowerX, _shop._selectedTowerY]._mapTower._color = Color.White;
                            _shop._towerOnTile = null;
                      }
                      if (_shop._isObstacle)
                    {

                        _map._map[_shop._selectedObstacleX, _shop._selectedObstacleY]._obstacle._color = Color.White;
                        _shop._mapObstacle = null;
                    }
                            _shop._isObstacle = false;
                            _shop._tileSelected = false;
                            _shop._isTower = false;
                    _shop._isObstacle = false;
                            int x = ((_shop._towerGridHitboxes[i].X + _shop._towerGrid[0, 0]._textureBottom.Width / 2) / _tileSize) - _shop._towerSectionOffsetX;
                            int y = (_shop._towerGridHitboxes[i].Y + _shop._towerGrid[0, 0]._textureBottom.Height / 2) / _tileSize;
                            _shop._selectedTowerFromTileGrid = _shop._towerGrid[x, y];
                            towerSelect = true;
                        }
                    }
            
              
                    if (!towerSelect)
                    {
                        _shop._selectedTowerFromTileGrid = null;
                    }


                


            
        }
        private void TowerPlacement(Vector2 MousePosition)
        {
            int rectangleIndex;
            int tileX;
            int tileY;
           

                for (int i = 0; i < _map._rectangleMap.Count; i++)
                {

                if (_map._rectangleMap[i].Contains(MousePosition.X, MousePosition.Y))
                {
                    rectangleIndex = i;
                    tileX = _map._rectangleMap[i].X / _tileSize;
                    tileY = _map._rectangleMap[i].Y / _tileSize;
                    if (_map._map[tileX,tileY]._mapTower == null && _map._map[tileX, tileY]._obstacle ==null)
                    {
                        if (ValidateTowerPlacement(tileX, tileY, rectangleIndex) && _currentPlayer._money >= _shop._selectedTowerFromTileGrid._cost)
                        {
                            _currentPlayer._money -= _shop._selectedTowerFromTileGrid._cost;
                            Microsoft.Xna.Framework.Rectangle hitbox = new Rectangle((tileX * 64) - 64, (tileY * 64) - 64, _shop._selectedTowerFromTileGrid._textureBottom.Width * 3, _shop._selectedTowerFromTileGrid._textureBottom.Height * 3);


                            _map._map[tileX, tileY]._mapTower = new MapTower(true, _shop._selectedTowerFromTileGrid, Color.White, hitbox, _shop._selectedTowerFromTileGrid._damage, _shop._selectedTowerFromTileGrid._health);
                            _map._map[tileX, tileY]._mapTower._health = _map._map[tileX, tileY]._mapTower._maxHealth;
                            _map._map[tileX, tileY]._mapTower._position = new Vector2(tileX*64, tileY*64);
                            _mapTowers.Add(_map._map[tileX, tileY]._mapTower);
                            InstantiatePathForEnemy();

                        }
                    }
                    else
                    {
                        _shop._selectedTowerFromTileGrid = null;
                        SelectTileObject(MousePosition);
                    }
                   


                }
            }



        }
        private bool ValidateTowerPlacement(int tileX, int tileY, int rectangleIndex)
        {

            if (_map._map[tileX, tileY]._isSpawner == false && _map._map[tileX, tileY]._isEndTile == false && _map._map[tileX, tileY]._obstacle == null && _map._map[tileX, tileY]._mapTower == null && PathBlocked(tileX, tileY) == false && notOnTopOfEnemy(rectangleIndex))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        private bool notOnTopOfEnemy(int rectangleIndex)
        {

            for (int i = 0; i < _mapEnemies.Count; i++) //loop through all the enemies on the map
            {
                if (_map._rectangleMap[rectangleIndex].Contains(Convert.ToInt16((_mapEnemies[i]._positionRelativeToTextures.X)), Convert.ToInt16((_mapEnemies[i]._positionRelativeToTextures.Y)))) //if the tile rectangle you selected contains an enemy
                {

                    return false;
                }

            }
            return true;

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
        private void SelectTileObject(Vector2 MousePosition)
        {

          
            for (int i = 0; i < _map._rectangleMap.Count; i++)
            {

                if (_map._rectangleMap[i].Contains(MousePosition.X, MousePosition.Y))
                {
                   

                    _selectedTileX = _map._rectangleMap[i].X / _tileSize;
                    _selectedTileY = _map._rectangleMap[i].Y / _tileSize;
                    
                    if (_map._map[_selectedTileX, _selectedTileY]._obstacle != null)
                    {
                        if (_shop._isTower==true) 
                        {
                            _map._map[_shop._selectedTowerX, _shop._selectedTowerY]._mapTower._color = Color.White;
                            _shop._towerOnTile = null;
                         }
                        if (_shop._isObstacle)
                        {

                            _map._map[_shop._selectedObstacleX, _shop._selectedObstacleY]._obstacle._color = Color.White;
                            _shop._mapObstacle = null;
                        }
                        _shop._isObstacle = true;
                        _shop._isTower = false;
                        _map._map[_selectedTileX, _selectedTileY]._obstacle._color = Color.Red;
                        _shop._mapObstacle = _map._map[_selectedTileX, _selectedTileY]._obstacle;
                        _shop._tileSelected = true;
                        _shop._selectedObstacleX = _selectedTileX;
                        _shop._selectedObstacleY = _selectedTileY;
                    }
                        
                    if (_map._map[_selectedTileX, _selectedTileY]._mapTower != null)
                    {
                        if (_shop._isObstacle)
                        {

                            _map._map[_shop._selectedObstacleX, _shop._selectedObstacleY]._obstacle._color = Color.White;
                            _shop._mapObstacle = null;
                        }
                        if (_shop._isTower == true)
                        {
                            _map._map[_shop._selectedTowerX, _shop._selectedTowerY]._mapTower._color = Color.White;
                            _shop._towerOnTile = null;
                        }
                        _shop._selectedTowerX = _selectedTileX;
                        _shop._selectedTowerY = _selectedTileY;
                        _shop._towerOnTile = _map._map[_selectedTileX, _selectedTileY]._mapTower;
                        _map._map[_selectedTileX, _selectedTileY]._mapTower._color = Color.Red;
                        _shop._tileSelected = true;
                        _shop._isObstacle = false;
                        _shop._isTower = true;
                    }
                    


                }
            }
        }
        public void Sell(Vector2 MousePosition)
        {
            if (_shop._tileSelected == true)
            {
                if (_shop._sellHitbox.Contains(MousePosition.X, MousePosition.Y))
                {
                    if (_shop._isTower)
                    {
                        if (_map._map[_selectedTileX, _selectedTileY]._mapTower != null)
                        {
                            _currentPlayer._money += _map._map[_selectedTileX, _selectedTileY]._mapTower._towerType._cost / 4;
                            _mapTowers.Remove(_map._map[_selectedTileX, _selectedTileY]._mapTower);
                            _map._map[_selectedTileX, _selectedTileY]._mapTower = null;
                            _shop._isTower = false;
                        }
                             
                    }
                    else
                    {
                        _map._map[_selectedTileX, _selectedTileY]._obstacle = null;
                        _shop._isObstacle = false;          
                    }

                    _map._nodeMap[_selectedTileX, _selectedTileY]._walkable = true;
                   
                   
                    InstantiatePathForEnemy();
                }
            }
          
           
        }
        public void Upgrade(Vector2 MousePosition) 
        {
            if (_shop._tileSelected == true)
            {
                if (_shop._upgradeHitbox.Contains(MousePosition.X, MousePosition.Y))
                {
                    if (_shop._isTower)
                    {
                        if (_map._map[_selectedTileX, _selectedTileY]._mapTower != null)
                        {
                            int count;
                            count = 0;
                            if (_map._map[_selectedTileX, _selectedTileY]._mapTower._upgradeOne == false && _map._map[_selectedTileX, _selectedTileY]._mapTower._upgradeTwo == false  )
                            {
                                _currentPlayer._money -= _map._map[_selectedTileX, _selectedTileY]._mapTower._towerType._upgradeOneCost;
                                _map._map[_selectedTileX, _selectedTileY]._mapTower._upgradeOne = true;
                                _map._map[_selectedTileX, _selectedTileY]._mapTower._maxHealth += 50;
                                _map._map[_selectedTileX, _selectedTileY]._mapTower._health = _map._map[_selectedTileX, _selectedTileY]._mapTower._maxHealth;
                                _map._map[_selectedTileX, _selectedTileY]._mapTower._damage += 10;
                                count++;
                            }
                            if (_map._map[_selectedTileX, _selectedTileY]._mapTower._upgradeOne == true && _map._map[_selectedTileX, _selectedTileY]._mapTower._upgradeTwo == false && count !=1)
                            {
                                _currentPlayer._money -= _map._map[_selectedTileX, _selectedTileY]._mapTower._towerType._upgradeTwoCost;
                                _map._map[_selectedTileX, _selectedTileY]._mapTower._maxHealth += 50;
                                _map._map[_selectedTileX, _selectedTileY]._mapTower._health = _map._map[_selectedTileX, _selectedTileY]._mapTower._maxHealth;
                                _map._map[_selectedTileX, _selectedTileY]._mapTower._damage += 10;
                                _map._map[_selectedTileX, _selectedTileY]._mapTower._upgradeOne = true;
                                _map._map[_selectedTileX, _selectedTileY]._mapTower._upgradeTwo = true;
                            }
                                                        
                        }

                    }
                   
                }
            }
        }

        public void Heal(Vector2 MousePosition)
        {
            if (_shop._tileSelected == true)
            {
                if (_shop._healHitbox.Contains(MousePosition.X, MousePosition.Y))
                {
                    if (_shop._isTower)
                    {
                        if (_map._map[_selectedTileX, _selectedTileY]._mapTower != null)
                        {
                            if (_currentPlayer._money >= _map._map[_selectedTileX, _selectedTileY]._mapTower._maxHealth - _map._map[_selectedTileX, _selectedTileY]._mapTower._health)
                            {
                                _currentPlayer._money -= (_map._map[_selectedTileX, _selectedTileY]._mapTower._maxHealth - _map._map[_selectedTileX, _selectedTileY]._mapTower._health);
                                _map._map[_selectedTileX, _selectedTileY]._mapTower._health = _map._map[_selectedTileX, _selectedTileY]._mapTower._maxHealth;
                            }
                            
                        }

                    }
                    
                }
            }


        }
        public void Update(GameTime gameTime) //stage will be updated every frame, well undate logic/movemt/collison here
        {
           
            _mouse.Update();
            Vector2 MousePosition = _mouse.GetMousePosition();

            _waveTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            KeyboardState keyboardState = Keyboard.GetState();
            
            //wave start early
            if (_waveEnded && keyboardState.IsKeyDown(Keys.Space))
            {
                _currentPlayer._money += Convert.ToInt16(_waveTimer);
                _startWaveEarly = true;
            }

            //wave start normal
            if (_waveTimer <= 0)
            {
                _startWave = true;
                
            }

            //actual wave
            if ( _startWave || _startWaveEarly) // if wave timer is LEQ to 0 or wave ended and
            {
               
                _waveEnded = false;
                _spawnTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;


                if ( _spawnTimer <= 0 && (_enemiesSpawned <_waveEnemyAmount) && _bossWave == false )
                {
                    InstantiateMapEnemy();
                    _enemiesSpawned++;
                    _spawnTimer = 3;
                }
                
                if (_bossWave ==true && (_enemiesSpawned < _waveEnemyAmount))
                {
                    InstantiateMapBoss();
                    _enemiesSpawned++;
                }
            }
           
            //end of wave
            if (_enemiesKilled == _waveEnemyAmount) //end of wave
                {
                _waveAmount++; //increase wave amount
                _enemiesSpawned = 0;
                if (_waveAmount % 5 ==0) //every 5 waves
                {
                    _enemyAmountBeforeBoss = _waveEnemyAmount;
                    _waveEnemyAmount = 1;
                        _bossWave = true;
                }
                else
                {
                    _bossWave = false;
                    if (_waveAmount % 5 == 1) //wave just after boss wave
                    {
                        _waveEnemyAmount = Convert.ToInt16(_enemyAmountBeforeBoss * 1.3);
                    }
                    _waveEnemyAmount = Convert.ToInt16(_waveEnemyAmount * 1.3);
                }

                 
                _enemiesKilled = 0;
                _waveEnded = true;
                _startWave = false;
                _startWaveEarly = false;
                _waveTimer = 20;
                }

           
            if (_mouse.WasLeftButtonClicked()) //if left button was clicked
            {
                if (MousePosition.X <= _map._mapXAmount * _tileSize && MousePosition.Y <= _map._mapYAmount * _tileSize) //if mouse was in the map
                {
                    if (_shop._selectedTowerFromTileGrid != null) // if there is a selected tower
                    {

                        TowerPlacement(MousePosition); //place tower
                    }
                    else
                    {
                        //SELECT BUILDING OR OBSTACLE

                        SelectTileObject(MousePosition);


                    }
                }
                else
                {

                    ShopTowerSelection(MousePosition); // if mouse is not in map bounds but left button clicked then select tower or sell
                    Sell(MousePosition);
                    Upgrade(MousePosition);
                    Heal(MousePosition);
                    // DeSelect();    

                }
            }

            //ENEMY FOLLOW PATH
            for (int i = 0; i < _mapEnemies.Count; i++) //loop through the list of enemies
            {
                _mapEnemies[i].FollowPath(gameTime);
                _mapEnemies[i]._currentNode = _map._nodeMap[Convert.ToInt16(Convert.ToInt16(_mapEnemies[i]._position.X / 64)), Convert.ToInt16(_mapEnemies[i]._position.Y / 64)];
                if (_mapEnemies[i]._currentNode._x == _map._endNode._x && _mapEnemies[i]._currentNode._y == _map._endNode._y) // if enemy is on the end node (last tile)
                {
                    _enemiesKilled++;
                    float newHealth;
                    newHealth = _currentPlayer._health - _mapEnemies[i]._livesLostWhenEndReached;
                    if (newHealth <= 0 )
                    {
                        _currentPlayer._health = 0;
                    }
                    else
                    {
                        _currentPlayer._health -= _mapEnemies[i]._livesLostWhenEndReached;
                    }
                    _mapEnemies[i]._currentHealth = 0;
                    _mapEnemies.RemoveAt(i);
                    
                }
                else
                {
                    Microsoft.Xna.Framework.Rectangle hitbox = new Rectangle(Convert.ToInt16(_mapEnemies[i]._position.X) + 16, Convert.ToInt16(_mapEnemies[i]._position.Y) + 16, _mapEnemies[i]._enemyType._texture.Width / 2, _mapEnemies[i]._enemyType._texture.Height / 2);
                    _mapEnemies[i]._enemyHitbox = hitbox;

                }



            }

            
            //ADD OR REMOVE ENEMY TO TOWER Q
            for (int j = 0; j < _mapTowers.Count; j++) //loop through all towers on map...
            {
                for (int enemyCount = 0; enemyCount < _mapEnemies.Count; enemyCount++) //loop through enemies on the map
                {
                    if (_mapEnemies[enemyCount]._enemyHitbox.Intersects(_mapTowers[j]._hitbox))// if the enemy hitbox intersects the tower hitbox
                    {
                       if (_bossWave)
                       {
                            if (!_bossList[0]._towerList.Contains(_mapTowers[j]))
                            {
                                _bossList[0]._towerList.Add(_mapTowers[j]);
                            }
                          
                       }

                        if (!_mapTowers[j]._enemyQueue.Contains(_mapEnemies[enemyCount])) //if q does not contain the enemy
                        {
                            _mapTowers[j]._enemyQueue.Enqueue(_mapEnemies[enemyCount]); //add enemy to the queue
                        }
                        
                       
                        if (!_mapEnemies.Contains(_mapTowers[j]._enemyQueue.Peek())) //if enemy at top of q is not in map enemy list then dq
                        {
                                _mapTowers[j]._enemyQueue.Dequeue();
                            
                           
                        }
                        
                       
                                                 
                    }
                    else //does not intersect
                    {
                        if (_mapTowers[j]._enemyQueue.Contains(_mapEnemies[enemyCount])) //enemy is in the queue but not intersecting (implies it left)
                        {
                            _mapTowers[j]._enemyQueue.Dequeue();
                           
                        }

                        if (_bossWave)
                        {
                            if (_bossList[0]._towerList.Contains(_mapTowers[j]))
                            {
                                _bossList[0]._towerList.Remove(_mapTowers[j]);
                            }

                        }

                    }

                }

                if (_mapEnemies.Count == 0 && _mapTowers[j]._enemyQueue.Count !=0)
                {
                   _mapTowers[j]._enemyQueue.Dequeue();
                }

            }

            //LOOP THROUGH BOSSES
            if (_bossWave && _bossList.Count >0)
            {
                for (int i = 0; i < _bossList.Count; i++) //used if more bosses in a round ##EXTENSION   
                 {

                    // Decrement the damage timer
                    _damageTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                   

                    // Check if it's time to damage towers again
                    if (_damageTimer <= 0)
                    {
                        _bossList[i]._hasAttacked = true;
                      
                        // Reset the timer for the next interval
                        _damageTimer = _damageRate;


                        // Damage each intersecting tower
                        if (_bossList[i]._towerList != null)
                        {
                            foreach (var tower in _bossList[i]._towerList)
                            {

                                float newHealth;
                                newHealth = tower._health - _bossList[0]._damage;
                                if (newHealth <= 0)
                                {
                                    tower._health = 0;
                                    _bossList[i]._hasAttacked = false;
                                }
                                else
                                {
                                    tower._health = Convert.ToInt16(newHealth);
                                    _bossList[i]._hasAttacked = false;
                                }

                            }
                        }
                        
                    }
                    else
                    {
                        _bossList[i]._hasAttacked = false;
                    }

                }
                                           
            }

            if (_mapTowers.Count >0)
            {
                System.Diagnostics.Debug.WriteLine("{0}", _mapTowers[0]._health);
            }
            //spawns projectile
            for (int i = 0; i < _mapTowers.Count; i++) 
            {
                if (_mapTowers[i]._enemyQueue.Count > 0)
                {
                    Vector2 squareCenter = new Vector2(_mapTowers[i]._position.X + _mapTowers[i]._towerType._textureBottom.Width / 2, _mapTowers[i]._position.Y + _mapTowers[i]._towerType._textureBottom.Height / 2); // get the position of the middle of the square
                    Vector2 objectPosition = new Vector2(_mapTowers[i]._enemyQueue.Peek()._position.X + 25, _mapTowers[i]._enemyQueue.Peek()._position.Y + 32); // get the position of the object in the square
                    Vector2 vectorToObj = objectPosition - squareCenter; // calculate the vector between the middle of the square and the object
                    float angle = (float)Math.Atan2(vectorToObj.Y, vectorToObj.X);
                    angle = angle - Convert.ToInt16((Math.PI / 2) + Math.PI);// calculate the angle between the vector and the X-axis
                    _mapTowers[i]._angleOfTexture = angle;
                    //SETS ANGLE OF TOP TEXTURE

                    _mapTowers[i]._fireTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;

                    if (_mapTowers[i]._fireTimer < 0) 
                    {
                        //SPAWN PROJECTILE
                        Projectile projectile;                      
                       
                        projectile = new Projectile(_mapTowers[i]._towerType._projectile._texture, _mapTowers[i]._towerType._projectile._name, _mapTowers[i]._towerType._projectile._speed);
                        projectile._damage = _mapTowers[i]._damage;
                        projectile._position = _mapTowers[i]._position;
                        projectile._target = _mapTowers[i]._enemyQueue.Peek();
                        _projectilesOnMap.Add(projectile);
                        _mapTowers[i]._fireTimer = _mapTowers[i]._towerType._fireRate;
                    }

                }

            }

            //check projectile - remove from Q if necessary
           
            for (int i=0; i< _projectilesOnMap.Count; i++)
            {
                if (_projectilesOnMap[i]._target._currentHealth != 0)
                {

                    _projectilesOnMap[i].Update(gameTime); //updates projectiles position
                    _projectilesOnMap[i]._hitbox =  new Rectangle(Convert.ToInt16(_projectilesOnMap[i]._position.X) + 16, Convert.ToInt16(_projectilesOnMap[i]._position.Y) + 16, _projectilesOnMap[i]._texture.Width / 2, _projectilesOnMap[i]._texture.Height / 2);

                    if (_projectilesOnMap[i]._hitbox.Intersects(_projectilesOnMap[i]._target._enemyHitbox))
                    {

                        float newEnemyHealth;
                        newEnemyHealth = (_projectilesOnMap[i]._target._currentHealth - _projectilesOnMap[i]._damage);

                        if (newEnemyHealth <= 0)
                        {
                            _projectilesOnMap[i]._target._currentHealth = 0;
                            _currentPlayer._money += _projectilesOnMap[i]._target._moneyPerKill;
                            _mapEnemies.Remove(_projectilesOnMap[i]._target);
                            _enemiesKilled++;
                        }
                        else
                        {
                            _projectilesOnMap[i]._target._currentHealth = Convert.ToInt16(newEnemyHealth);
                        }

                        _projectilesOnMap.RemoveAt(i);


                    }
                }
                else
                {
                    _projectilesOnMap.RemoveAt(i);
                }
                
            }


           
        }


        public void Draw(SpriteBatch spriteBatch)
        {
            _currentPlayer.Draw(spriteBatch);
            _map.Draw(spriteBatch);
            for (int i = 0; i < _mapEnemies.Count; i++)
            {
                _mapEnemies[i].Draw(spriteBatch);
            }


            for (int projCount = 0; projCount<_projectilesOnMap.Count; projCount++)
            {
                _projectilesOnMap[projCount].Draw(spriteBatch);
            }

            _shop.Draw(spriteBatch);

          

            if (_waveEnded)
            {
                string time = "TIME TILL NEXT WAVE:  " + Convert.ToString(Math.Round(_waveTimer,2));
                spriteBatch.DrawString(Game1._font, time, new Vector2((_shop._towerSectionOffsetX*64) + 128, 64), Microsoft.Xna.Framework.Color.White);
                spriteBatch.DrawString(Game1._font, " PRESS 'SPACEBAR' TO SKIP ", new Vector2(_shop._towerSectionOffsetX*64 + 128, 128), Microsoft.Xna.Framework.Color.White);
                //draw box with wave timer
            }
        }

    }
}
