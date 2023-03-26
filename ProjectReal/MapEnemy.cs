using System;
using System.Collections.Generic;
using System.Text;
using MonoGame;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System.IO;

namespace ProjectReal
{
    class MapEnemy
    {
       
        public Rectangle _hitbox { get; }
        public Vector2 _position { get; set; }
        public Vector2 _positionRelativeToTextures { get; set; }
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
                Vector2 target = new Vector2((_path[0].X*64), (_path[0].Y*64));
              //  Vector2 aheadOfTarget = new Vector2(_path[0].X, _path[0].Y);
                Vector2 direction = new Vector2(0,0);

                direction = Vector2.Normalize(target - _position); //vector from our position to target
              //  Vector2 targetToAheadOfTarget = new Vector2();
             //   targetToAheadOfTarget = Vector2.Normalize(aheadOfTarget * 64 - target * 64); //vector from target to ahead of target
                // If we're close enough to the target node, remove it from the path
                //if distance from pos to target > position to target +1
               
                if ( Math.Abs(Vector2.Distance(_position, target)) < 1 )
                {
                  
                    
                    _path.RemoveAt(0);// if the enemy is close enough then go onto the next vector in the path by removing the current target
                }
                else
                {
                   

                    // _position = _position / 64;

                    direction = Vector2.Normalize(target - _position);

                    //System.Diagnostics.Debug.WriteLine("{0},{1},  {2},{3}   {4},{5}", target.X, target.Y, Math.Round(_position.X), Math.Round(_position.Y), _path[0].X, _path[0].Y);// direction.X, direction.Y, _path[0].X, _path[0].Y, _path[1].X, _path[1].Y);
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
                    _positionRelativeToTextures = _position + new Vector2(32, 32);
                    
                   // _currentNode._x = Convert.ToInt16((_position.X / 64));
                   // _currentNode._y = Convert.ToInt16((_position.Y / 64));
                   
                }
              
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_enemyType._texture, _position, Color.White);

        }
       

    }
}
