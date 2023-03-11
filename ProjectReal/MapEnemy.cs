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
    class MapEnemy
    {
        public Dictionary<string, EnemyType> _nameOfEnemyToType;
        public Rectangle _hitbox { get; }
        public Vector2 _position { get; }

        private void LoadEnemyTypes()
        {
            String lineInput;
            string name;
            string fileName;
            int health;
            int speed;
            int damage;
            bool isCamoflagued;
            try
            {
                using (System.IO.StreamReader ReaderForTileMap = new System.IO.StreamReader("Tower.txt"))
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
                        Texture2D enemyTexture = Game._graphicsloader.Load<Texture2D>(fileName);                      
                        enemyType = new EnemyType(enemyTexture,health,speed,damage,name,isCamoflagued);
                        _nameOfEnemyToType.Add(name,enemyType);


                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("The file could not be read");
                Console.WriteLine(e.Message);
            }
        }
        private void FollowPath()
        {

        }

       

    }
}
