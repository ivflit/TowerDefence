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
       
        public Rectangle _hitbox { get; }
        public Vector2 _position { get; set; }
        public EnemyType _enemyType { get; }

        public List<Vector2> _path { get; set; }

        public Node _currentNode { get; set; }

        public MapEnemy(Vector2 position, EnemyType enemyType)
        {
          
            _position = position;
            _enemyType = enemyType;
        }
        public void FollowPath(GameTime gameTime)
        {
            float movementSpeed = 0;
            if (_path != null && _path.Count > 0)
            {
                // Move the sprite towards the next node in the path
                Vector2 target = new Vector2(_path[0].X, _path[0].Y);
               
               
                
                // If we're close enough to the target node, remove it from the path
                if (Math.Abs(Vector2.Distance(_position, target*64)) < 1)
                {
                    _path.RemoveAt(0);// adjust this value to control how close the sprite needs to be to a node to move on to the next one
                }
                else
                {
                    Vector2 direction = new Vector2();

                    // _position = _position / 64;

                    direction = Vector2.Normalize(target * 64 - _position);

                    System.Diagnostics.Debug.WriteLine("{0},{1}", target.X, target.Y);
                    if (_currentNode._movementModifier == 0)
                    {
                        movementSpeed = 1;
                    }
                    else
                    {
                        movementSpeed = _currentNode._movementModifier;
                    }
                    float speed = _enemyType._movementSpeed * movementSpeed; // adjust this value to control the sprite's speed
                    _position += direction * speed * (float)gameTime.ElapsedGameTime.TotalSeconds;

                    _currentNode._x = Convert.ToInt16(Math.Round(_position.X / 64));
                    _currentNode._y = Convert.ToInt16(Math.Round(_position.Y / 64));
                }
                    
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_enemyType._texture, _position, Color.White);
        }
       

    }
}
