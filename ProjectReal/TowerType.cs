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
    class TowerType
    {
       

        //csv = (TextureBase, TextureTop, ProjectileName, cost, description, upgradeOneBonusCost, UpgradeTwoBonusCost)
        public string _name { get; set; }
        public int _upgradeOneCost { get; set; }
        public int _upgradeTwoCost { get; set; }   
        public Texture2D _textureTop { get; }
        public Texture2D _textureBottom { get; }
        public int _health { get; }
        public int _cost { get; }
        public string _description { get; }
        public List<string> _statsOfTower { get; set; }

        public Projectile _projectile { get; } //it will be set in this class so no need for set;

        public TowerType(string name, int upgradeOneCost, int upgradeTwoCost, Texture2D texture, Texture2D bottomOfTower, int health, int cost, string description, Projectile projectile)
        {
            _textureBottom = bottomOfTower;
            _name = name;
            _upgradeOneCost = upgradeOneCost;
            _upgradeTwoCost = upgradeTwoCost;
            _textureTop = texture;
            _health = health;
            _cost = cost;
            _description = description;
            _projectile = projectile;
            MakeTowerStats();
        }
        private void MakeTowerStats()
        {
            _statsOfTower = new List<string>();
            _statsOfTower.Add("NAME: " + _name);
            _statsOfTower.Add("HEALTH: " + _health);
            _statsOfTower.Add("COST: " + _cost);
            _statsOfTower.Add(_description);
            
        }
    }
}
