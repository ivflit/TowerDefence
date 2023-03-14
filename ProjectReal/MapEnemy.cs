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
        public Vector2 _position { get; set; }
        public EnemyType _enemyType { get; }

        public List<Vector2> path;

        public MapEnemy(Vector2 position, EnemyType enemyType)
        {
            _position = position;
            _enemyType = enemyType;
        }
        public void FollowPath(GameTime gameTime)
        {
            if (path != null && path.Count > 0)
            {
                // Move the sprite towards the next node in the path
                Vector2 target = path[0];
                Vector2 direction = Vector2.Normalize(target - _position);
                float speed = 100; // adjust this value to control the sprite's speed
                _position += direction * speed * (float)gameTime.ElapsedGameTime.TotalSeconds;

                // If we're close enough to the target node, remove it from the path
                if (Vector2.Distance(_position, target) < 10) // adjust this value to control how close the sprite needs to be to a node to move on to the next one
                    path.RemoveAt(0);
            }
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
                        Texture2D enemyTexture = Game1._graphicsloader.Load<Texture2D>(fileName);                      
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
        
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_enemyType._texture, _position, Color.White);
        }
       

    }
}
