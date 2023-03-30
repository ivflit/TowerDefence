using System;
using System.Collections.Generic;
using System.Text;
using MonoGame;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System.IO;
using System.Linq.Expressions;

namespace ProjectReal
{
    class MapEnemy
    {
        public int _currentHealth { get; set; }
        public int _scorePerKill { get; set; }
        public  int _moneyPerKill { get; set; }
        public int _livesLostWhenEndReached { get; set; }
        public  Vector2 _position { get; set; }
        public Vector2 _positionRelativeToTextures { get; set; }
        public EnemyType _enemyType { get; }

       
        public List<Vector2> _path { get; set; }
        public int _indexInMapEnemy { get; set; }

        public Node _currentNode { get; set; }
       public Microsoft.Xna.Framework.Rectangle _enemyHitbox { get; set; }

        public MapEnemy(Vector2 position, EnemyType enemyType)// Microsoft.Xna.Framework.Rectangle rectangle)
        {
          
            _position = position;
          
            _enemyType = enemyType;
            _currentHealth = enemyType._health;
           // _enemyHitbox = rectangle;
        }
        public void FollowPath(GameTime gameTime)
        {
            float movementSpeed = 0;
            if (_path != null && _path.Count > 0)
            {
                // Move the sprite towards the next node in the path
                Vector2 target = new Vector2((_path[0].X*64), (_path[0].Y*64));
                Vector2 direction = new Vector2(0, 0);
                Vector2 aheadOfTarget = new Vector2(0, 0);
                if (_path.Count>1)
                {
                   aheadOfTarget = new Vector2(_path[1].X * 64, _path[1].Y * 64);
                }
                else
                {
                    aheadOfTarget = new Vector2((_path[0].X * 64), (_path[0].Y * 64));
                }
                
                Vector2 directionToNodeAhead = new Vector2(0, 0);
               Vector2 tempDirection = new Vector2(0, 0);
               
                Vector2 CurrentNodeVector = new Vector2(_currentNode._x * 64, _currentNode._y * 64);            
                
                Vector2 NodeToNodeDirection = new Vector2(0, 0);

                            

              

                if ( Math.Abs(Vector2.Distance(_position, target)) < 1  )//|| (Vector2.Distance(_position,target) < Vector2.Distance(new Vector2(_currentNode._x*64,_currentNode._y*64),target)))
                {
                  
                    
                    _path.RemoveAt(0);// if the enemy is close enough then go onto the next vector in the path by removing the current target ie If we're close enough to the target node, remove it from the path
                }
                else
                {


                    // _position = _position / 64;
                    //NodeToNodeDirection = Vector2.Normalize(aheadOfTarget - CurrentNodeVector);
                    tempDirection = Vector2.Normalize(target - _position);
                    if (Math.Abs(tempDirection.X) > Math.Abs(tempDirection.Y))
                    {
                        // Move horizontally
                        tempDirection = new Vector2(Math.Sign(tempDirection.X), 0);
                    }
                    else
                    {
                        // Move vertically
                        tempDirection = new Vector2(0, Math.Sign(tempDirection.Y));
                    }
                    tempDirection.Normalize();

                    directionToNodeAhead = Vector2.Normalize(aheadOfTarget - _position);
                    if (Math.Abs(directionToNodeAhead.X) > Math.Abs(directionToNodeAhead.Y))
                    {
                        // Move horizontally
                        directionToNodeAhead = new Vector2(Math.Sign(directionToNodeAhead.X), 0);
                    }
                    else
                    {
                        // Move vertically
                        directionToNodeAhead = new Vector2(0, Math.Sign(directionToNodeAhead.Y));
                    }
                    directionToNodeAhead.Normalize();

                    if ( Math.Sign(tempDirection.X) !=  Math.Sign(directionToNodeAhead.X) || Math.Sign(tempDirection.Y) != Math.Sign(directionToNodeAhead.Y))
                    {
                        if (Math.Abs(tempDirection.X) == Math.Abs(directionToNodeAhead.X) && Math.Abs(tempDirection.Y) == Math.Abs(directionToNodeAhead.Y)) 
                        {
                            _path.RemoveAt(0);
                            direction = directionToNodeAhead;
                        }
                        else
                        {
                            direction = tempDirection;
                        }
                                               
                       
                    }
                    else
                    {
                        direction = tempDirection;
                    }

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
        
        public void DrawHealthBar(SpriteBatch spriteBatch, Texture2D pixelTexture, int currentHealth, int maxHealth, Color bgColor, Color fillColor)
        {
            // Define the position and size of the health bar
            Rectangle healthBarRect = new Rectangle((int)_position.X+15, (int)_position.Y+10, 32, 5);

            // Calculate the fill percentage of the health bar
            float fillPercentage = (float)currentHealth / maxHealth;

            // Draw the background rectangle of the health bar
            spriteBatch.Draw(pixelTexture, healthBarRect, bgColor);

            // Draw the fill rectangle of the health bar
            int fillWidth = (int)(healthBarRect.Width * fillPercentage);
            Rectangle fillRect = new Rectangle(healthBarRect.X, healthBarRect.Y, fillWidth, healthBarRect.Height);
            spriteBatch.Draw(pixelTexture, fillRect, fillColor);
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            Texture2D rectTexture = new Texture2D(Game1._graphics.GraphicsDevice, 1, 1);
            rectTexture.SetData(new Color[] { Color.White });
           // spriteBatch.Draw(rectTexture,_enemyHitbox, Color.Red);
            spriteBatch.Draw(_enemyType._texture, _position, Color.White);
            DrawHealthBar(spriteBatch, rectTexture, _currentHealth , _enemyType._health, Microsoft.Xna.Framework.Color.Red, Microsoft.Xna.Framework.Color.LawnGreen);

        }
       

    }
}
