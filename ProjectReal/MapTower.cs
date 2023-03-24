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
    class MapTower //Tower that is on the map
    {
       private bool _unlocked = false;

        public MapTower(bool unlocked, TowerType towerType)
        {
            _unlocked = unlocked;
            _towerType = towerType;

        }

        public TowerType _towerType { get; }
       public Vector2 _position { get; set; }
        
       

        //VALIDATE TOWER PLACEMENT -CANNOT PLACE ON PORTAL AND CANNOT BLOCK PORTAL
       private void RotateToFollowEnemy()
        {

        }

        private void Shoot()
        {

        }

    }
}
