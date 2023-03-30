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
            public int _health { get; }
        public int _movementSpeed { get; }
        public int _damage { get; }    
        public string _typeName { get; }
        public bool _isCamoflagued { get; }

        public int _scorePerKill { get; set; }
        public int _moneyPerKill { get; set; }
        public int _livesLostWhenEnd { get; set; }
        public EnemyType(Texture2D texture, int health, int movementSpeed, int damage, string typeName, bool iscamoflagued, int scorePerKill, int moneyPerKill, int liveslost)
        {
            _texture = texture;
            _health = health;
            _movementSpeed = movementSpeed;
            _damage = damage;
            _typeName = typeName;
            _isCamoflagued = iscamoflagued;
            _scorePerKill = scorePerKill;
            _moneyPerKill = moneyPerKill;
            _livesLostWhenEnd = liveslost;
        }      

        

    }
}
