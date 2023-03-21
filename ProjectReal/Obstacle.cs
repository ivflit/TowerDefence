using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using System.Text;

namespace ProjectReal
{
    class Obstacle
    {
        public Obstacle(string name, int costToRemove, Texture2D texture)
        {
            _name = name;
            _costToRemove = costToRemove;
            _texture = texture;
        }

        public string _name { get; set; }
        public int _costToRemove { get;set; }
        public Texture2D _texture { get; set; }


    }
}
