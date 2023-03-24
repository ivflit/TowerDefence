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
    public class Game1 : Microsoft.Xna.Framework.Game //make everything private and then do the thing. Make a tile arc class which acts as the edge between two tiles
    {
        public static Settings.GameState _gameState ; //what's the game doing?
        public static ContentManager _graphicsloader;
        public static SpriteFont _font;
        public static Input _mouse;
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Stage _currentStage;
        

        
        public Game1() //constructor
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            
        }

        protected override void Initialize() 
        {
            IsMouseVisible = true;
            _graphicsloader = Content;
            _currentStage = new Stage();            		//creating a new stage
            
            Window.Title = "Tower defence";
            _graphics.PreferredBackBufferWidth = 1280;		//set game screen resolution
            _graphics.PreferredBackBufferHeight = 720;
            _graphics.ApplyChanges();
            

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _font = Content.Load<SpriteFont>("font");
           // var game = new Microsoft.Xna.Framework.Game();

        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

           

            _currentStage.Update(gameTime);
            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            
            _spriteBatch.Begin();
            _currentStage.Draw( _spriteBatch);
            _spriteBatch.End();
            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
