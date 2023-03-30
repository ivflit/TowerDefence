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
        public bool _alive { get; set; }
        private List<string> _stats;
        public int _totalSectionX { get; set; }
        public int _sectionOffSet { get; set; }
        public Player(int health, int score, int money, bool alive, int sectionOffSet, int totalSectionX)
        {
            _health = health;
            _score = score;
            _money = money;
            _alive = alive;
            _sectionOffSet = sectionOffSet;
            _totalSectionX = totalSectionX;
            
        }


        public void Draw(SpriteBatch spriteBatch)
        {
            _stats = new List<string>();
            _stats.Add("HEALTH: " + Convert.ToString(_health));
            _stats.Add("SCORE: " +Convert.ToString(_score));
            _stats.Add("MONEY: " +Convert.ToString(_money));

            int eachSectionXDivision = Convert.ToInt16(_totalSectionX / _stats.Count);
           
                for (int i = 0;  i < _stats.Count; i++)
            {
                spriteBatch.DrawString(Game1._font, _stats[i], new Vector2((eachSectionXDivision * i) * 64, (_sectionOffSet) * 64), Microsoft.Xna.Framework.Color.White);
            }
           
        }
       

    }
}
