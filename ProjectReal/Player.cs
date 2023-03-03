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
    class Player
    {

        public int _health { get; set; }
        public int _score { get; set; }
        public int _money { get; set; }
        public int _alive { get; set; }

        public Player(int health, int score, int money, int alive)
        {
            _health = health;
            _score = score;
            _money = money;
            _alive = alive;
        }


       

    }
}
