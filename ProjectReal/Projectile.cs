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
        public Projectile(Texture2D texture, string name, int speed, int damage)
        {
            _texture = texture;
            _name = name;
            _speed = speed;
            _damage = damage;
        }

        //csv (name, texture, damage, speed)

       
        public Texture2D _texture {get;}
        public Vector2 _position { get; set; }
        public Rectangle _hitbox { get; } //rectangle created based on projectile texture width/height
        public string _name { get; }
        public int _speed { get; }
        public int _damage { get; }

       
        public void MoveToEnemy() // This will make the projectile go to the enemy
        {
            //access enemy position each frame and direct vector to that position
        }



    }

}

    

