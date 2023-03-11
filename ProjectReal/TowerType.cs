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
        public Texture2D _texture { get; }
        public int _health { get; }
        public int _cost { get; }
        public string _description { get; }
        public Projectile _projectile { get; } //it will be set in this class so no need for set;

        public TowerType(string name, int upgradeOneCost, int upgradeTwoCost, Texture2D texture, int health, int cost, string description, Projectile projectile)
        {
            _name = name;
            _upgradeOneCost = upgradeOneCost;
            _upgradeTwoCost = upgradeTwoCost;
            _texture = texture;
            _health = health;
            _cost = cost;
            _description = description;
            _projectile = projectile;
        }
    }
}
