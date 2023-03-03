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
    class EnemyType
    {
       
        public Texture2D _texture { get; }
        public string _filename { get; }
        public int _health { get; }
        public int _movementSpeed { get; }
        public int _damage { get; }    
        public string _typeName { get; }
        public bool _isCamoflagued { get; }

        public EnemyType(string filename, int health, int movementSpeed, int damage, string typeName, bool iscamoflagued)
        {
            _filename = filename;
            _health = health;
            _movementSpeed = movementSpeed;
            _damage = damage;
            _typeName = typeName;
            _isCamoflagued = iscamoflagued;

        }      

    }
}
