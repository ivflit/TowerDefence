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
    public class Settings //stores various settings for the game
    {
        public static Point Resolution; //static variables do not need a class instance to be accessible
        public enum Difficulty
        {
            easy,
            hard,
            extreme
        }
        public enum GameState //used to select which bit of code we run
        {
            menu,
            leaderboardView,
            loading,
            playing,
            error
        }
    }
}
