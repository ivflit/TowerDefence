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
    class Tile
    {
        public Tile(Terrain terrain, bool isSpawner, bool isEndTile)
        {
            _terrain = terrain;
            _isSpawner = isSpawner;
            _isEndTile = isEndTile;
        }

        public Terrain _terrain { get; set; }
        public bool _isSpawner { get; set; }
        public bool _isEndTile { get; set; }


        public static void loadContent()
        {

        }
             

    }
}
