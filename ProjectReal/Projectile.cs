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
    class Projectile
    {
        public Projectile(Texture2D texture, string name, int speed)
        {
            _texture = texture;
            _name = name;
            _speed = speed;
           
        }

        //csv (name, texture, damage, speed)

       
        public Texture2D _texture {get;}
        public MapEnemy _target { get; set;}
        public Vector2 _position { get; set; }
        public Rectangle _hitbox { get; set; } //rectangle created based on projectile texture width/height
        public string _name { get; }
        public int _speed { get; }
        public int _damage { get; set; }

       
        public void MoveToEnemy() // This will make the projectile go to the enemy
        {
            //access enemy position each frame and direct vector to that position
        }

        public void Update(GameTime gameTime)
        {
            // Update the position of the projectile
            Vector2 directionToEnemy;
            
            
            directionToEnemy = Vector2.Normalize(_target._position - _position);
            
            _position += directionToEnemy * _speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
           

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Texture2D rectTexture = new Texture2D(Game1._graphics.GraphicsDevice, 1, 1);
            rectTexture.SetData(new Color[] { Color.White });
            //spriteBatch.Draw(rectTexture, _hitbox, Color.Red);
            spriteBatch.Draw(_texture, _position, Color.White);
        }

    }

}

    

