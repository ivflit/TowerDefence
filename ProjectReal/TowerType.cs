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
        public Texture2D _texture { get; }
        public string _textureFilename { get; }
        public int _health { get; }
        public int _cost { get; }
        public string _towerDescription { get; }
        public Projectile _projectile { get; } //it will be set in this class so no need for set;

        public TowerType(string TextureFilename, int health, int cost, string towerDescription, Projectile projectile)
        {
            _textureFilename = TextureFilename;
            _health = health;
            _cost = cost;
            _towerDescription = towerDescription;
            _projectile = projectile;
        }





    }
}
