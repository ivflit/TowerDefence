using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using System.Text;

namespace ProjectReal
{
    class Obstacle
    {
        public Obstacle(string name, int costToRemove, Texture2D texture, Microsoft.Xna.Framework.Color color)
        {
            _name = name;
            _costToRemove = costToRemove;
            _texture = texture;
            _color = color;
        }
        public Microsoft.Xna.Framework.Color _color { get; set; }
        public string _name { get; set; }
        public int _costToRemove { get;set; }
        public Texture2D _texture { get; set; }


    }
}
