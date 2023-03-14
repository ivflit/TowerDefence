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
    class MapTower //Tower that is on the map
    {
       private bool _unlocked = false;

        public MapTower(bool unlocked, TowerType towerType)
        {
            _unlocked = unlocked;
            _towerType = towerType;

        }

        public Dictionary<string,Projectile> _nameOfProjectilesToProjectile { get; set; }
        public Dictionary<string, TowerType> _nameOfTowerToTower { get; set; }
        public TowerType _towerType { get; }
       public Vector2 _position { get; set; }
        
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
                        
                        projectile = new Projectile(ProjectileTexture,name,speed,damage);
                       
                        _nameOfProjectilesToProjectile.Add(name,projectile);    



                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("The file could not be read");
                Console.WriteLine(e.Message);
            }
        }
       //load tower types
       private void LoadTowerTypes()
        {
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
                using (System.IO.StreamReader ReaderForTileMap = new System.IO.StreamReader("Tower.txt"))
                {


                    while (ReaderForTileMap.EndOfStream == false)
                    {
                        TowerType towerType;
                        Projectile projectile;
                        string[] splitLine;
                        lineInput = ReaderForTileMap.ReadLine().ToLower();
                        splitLine = lineInput.Split(",");
                        name = splitLine[0];
                        fileName = splitLine[1];
                        projectileType = splitLine[2];
                        cost= Convert.ToInt32(splitLine[3]);
                        descriptionOfTower = splitLine[4];
                        upgradeOneCost = Convert.ToInt32(splitLine[5]);
                        upgradeTwoCost = Convert.ToInt32(splitLine[6]);
                        health = Convert.ToInt32(splitLine[7]);
                        Texture2D TowerTopTexture = Game1._graphicsloader.Load<Texture2D>(fileName);
                        _nameOfProjectilesToProjectile.TryGetValue(projectileType, out projectile);
                        towerType = new TowerType(name, upgradeOneCost, upgradeTwoCost, TowerTopTexture, health, cost, descriptionOfTower, projectile);
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
       private void RotateToFollowEnemy()
        {

        }

        private void Shoot()
        {

        }

    }
}
