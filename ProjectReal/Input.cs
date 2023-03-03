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
    public class Input
    {
        //checking input
        public MouseState _currentState { get; set; }
        public MouseState _previousState { get; set; }

        private static Input instance;

        public static Input Instance
        {
            get
            {
                if (instance == null)
                    instance = new Input();
                return instance;
            }
        }
        public void update()
        {
            _previousState = _currentState;
            _currentState = Mouse.GetState();
        }
    }
}
