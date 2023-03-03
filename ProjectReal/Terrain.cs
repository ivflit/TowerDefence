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
    class Terrain
    {
        public Terrain(string name,Texture2D texture, float movementModifier, bool isTowerPlaceable)
        {
            _name = name;
            _texture = texture;          
            _movementModifier = movementModifier;
            _isTowerPlaceable = isTowerPlaceable;
        }

        public string _name { get; set; }
        public Texture2D _texture { get; set; }
        public string _textureFilename { get;} 
        public float _movementModifier { get; }
        public bool _isTowerPlaceable { get; }


    }
}
