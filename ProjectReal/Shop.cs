using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Security.Cryptography.X509Certificates;


namespace ProjectReal
{
    class Shop
    {
        int _tileSize = 64;
        public List<TowerType> _listOfTowers;
        public List<string> _listOfTowerNames;
        public Dictionary<string, TowerType> _nameToTower;
        public String[,] _NameOfTowerGrid;
        public TowerType[,] _towerGrid;
        public TowerType _selectedTowerFromTileGrid;
        public MapTower _towerOnTile;
        public Obstacle _mapObstacle;
       
        public bool _tileSelected = false;
        public bool _isObstacle = false;
        public bool _isTower = false;
        public List<Microsoft.Xna.Framework.Rectangle> _towerGridHitboxes;
        public Microsoft.Xna.Framework.Rectangle _sellHitbox;
        public Microsoft.Xna.Framework.Rectangle _upgradeHitbox;
        public Microsoft.Xna.Framework.Rectangle _healHitbox;
        public int _selectedTowerX;
        public int _selectedTowerY;
        public int _selectedObstacleX;
        public int _selectedObstacleY;
        public int _StatSectionOffsetX;
        public int _StatSectionOffsetY;
        public int _towerSectionOffsetX;
        public int _towerSectionOffsetY;
         public List<Vector2> _towerGridVectors { get; set; }
        public Shop(List<TowerType> listOfTowers, List<string> listOfTowerNames, Dictionary<string, TowerType> nameToTower, int Xoffset)
        {
            _listOfTowers = listOfTowers;
            _listOfTowerNames = listOfTowerNames;
            _nameToTower = nameToTower;
            _StatSectionOffsetX = Xoffset;
            _towerSectionOffsetX = Xoffset;
            MakeTowerGrid();
        }
        private void MakeTowerGrid()
        {
            Microsoft.Xna.Framework.Rectangle towerShopRectangle;
            int i = 0;
            _towerGridVectors = new List<Vector2>();
            _towerGrid = new TowerType[_listOfTowers.Count / 2, _listOfTowers.Count / 2];
            _towerGridHitboxes = new List<Microsoft.Xna.Framework.Rectangle>();
            for (int y = 0; y < _towerGrid.GetLength(1); y++)
            {
                for (int x = 0; x < _towerGrid.GetLength(0); x++)
                {
                    _towerGrid[x, y] = _listOfTowers[i];
                    _towerGridVectors.Add(new Vector2((x+_towerSectionOffsetX)*_tileSize, (y + _towerSectionOffsetY)* _tileSize ));
                    int rectangleX = ((x + _towerSectionOffsetX) * _tileSize); //-_listOfTowers[i]._textureBottom.Width/2;
                    int rectangleY;
                    if (y ==  0)
                    {
                         rectangleY = 0;
                    }
                    else
                    {
                         rectangleY = ((y + _towerSectionOffsetY) * _tileSize);
                    }
                   
                    int rectangleWidth = _listOfTowers[i]._textureBottom.Width;
                    int rectangleHieght = _listOfTowers[i]._textureBottom.Height;
                    towerShopRectangle = new Microsoft.Xna.Framework.Rectangle(rectangleX,rectangleY,rectangleWidth,rectangleHieght);
                    _towerGridHitboxes.Add(towerShopRectangle);
                    i++;
                }
            }
           
        }
       
       private void DrawTowerStatGrid(SpriteBatch spriteBatch)
        {
           if (_selectedTowerFromTileGrid != null)
            {
                
                for (int i = 0; i < _selectedTowerFromTileGrid._statsOfTower.Count; i++)
                {
                    spriteBatch.DrawString(Game1._font, _selectedTowerFromTileGrid._statsOfTower[i], new Vector2((_StatSectionOffsetX) * _tileSize, (_StatSectionOffsetY + i) * _tileSize + _towerGridHitboxes[_towerGridHitboxes.Count - 1].Bottom), Microsoft.Xna.Framework.Color.White);
                }
            }
            
        }
     
        public void DisplayUpgrade(SpriteBatch spriteBatch)
        {
            int rectangleX = ((_towerSectionOffsetX+4) * _tileSize); //-_listOfTowers[i]._textureBottom.Width/2;
            int rectangleY;

            rectangleY = 640;


            Texture2D button = Game1._graphicsloader.Load<Texture2D>("tile");
            _upgradeHitbox = new Microsoft.Xna.Framework.Rectangle(rectangleX, rectangleY, button.Width, button.Height);
            spriteBatch.Draw(button, _upgradeHitbox, Microsoft.Xna.Framework.Color.White);
            spriteBatch.DrawString(Game1._font, "UPGRADE", new Vector2(rectangleX, rectangleY + (button.Height / 2)), Microsoft.Xna.Framework.Color.Black);
        }
        public void DisplaySell(SpriteBatch spriteBatch)
        {
            int rectangleX = ((_towerSectionOffsetX) * _tileSize); //-_listOfTowers[i]._textureBottom.Width/2;
            int rectangleY;
           
                rectangleY = 640;
            
            
           Texture2D button = Game1._graphicsloader.Load<Texture2D>("tile");
            _sellHitbox = new Microsoft.Xna.Framework.Rectangle(rectangleX, rectangleY, button.Width, button.Height);
            spriteBatch.Draw(button, _sellHitbox, Microsoft.Xna.Framework.Color.White);
            spriteBatch.DrawString(Game1._font, "SELL", new Vector2(rectangleX, rectangleY+(button.Height/2)), Microsoft.Xna.Framework.Color.Black);
        }
        public void DisplayHeal(SpriteBatch spriteBatch)
        {
            int rectangleX = ((_towerSectionOffsetX+2) * _tileSize); //-_listOfTowers[i]._textureBottom.Width/2;
            int rectangleY;

            rectangleY = 640;


            Texture2D button = Game1._graphicsloader.Load<Texture2D>("tile");
            _healHitbox = new Microsoft.Xna.Framework.Rectangle(rectangleX, rectangleY, button.Width, button.Height);
            spriteBatch.Draw(button, _healHitbox, Microsoft.Xna.Framework.Color.White);
            spriteBatch.DrawString(Game1._font, "HEAL", new Vector2(rectangleX, rectangleY + (button.Height / 2)), Microsoft.Xna.Framework.Color.Black);
        }

        public void DisplayTowerHitbox(SpriteBatch spriteBatch,Microsoft.Xna.Framework.Rectangle towerHitbox)
        {
            byte transparency_amount = 120;
            Texture2D rectTexture = new Texture2D(Game1._graphics.GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
            Microsoft.Xna.Framework.Color[] c = new Microsoft.Xna.Framework.Color[1];
            c[0] = Microsoft.Xna.Framework.Color.FromNonPremultiplied(255, 255, 255, transparency_amount);
            // rectTexture.SetData(new Microsoft.Xna.Framework.Color[] { Microsoft.Xna.Framework.Color.White });
            rectTexture.SetData<Microsoft.Xna.Framework.Color>(c);
            spriteBatch.Draw(rectTexture, towerHitbox, Microsoft.Xna.Framework.Color.Red);
        }
               
        public void Draw(SpriteBatch spriteBatch)
        {
           // string title = "SHOP";
            //spriteBatch.DrawString(Game1._font, title, new Vector2(_StatSectionOffsetX * _tileSize, (_StatSectionOffsetY) * _tileSize), Microsoft.Xna.Framework.Color.White);
            for (int y = 0; y< _towerGrid.GetLength(1); y++)
            {
                for (int x = 0; x < _towerGrid.GetLength(0); x++)
                {
                    if (_towerGrid[x,y] == _selectedTowerFromTileGrid)
                    {

                        spriteBatch.Draw(_towerGrid[x, y]._textureBottom, new Vector2((x + _towerSectionOffsetX) * _tileSize, (y + _towerSectionOffsetY) * _tileSize), Microsoft.Xna.Framework.Color.Red);
                        spriteBatch.Draw(_towerGrid[x, y]._textureTop, new Vector2((x + _towerSectionOffsetX) * _tileSize, (y + _towerSectionOffsetY) * _tileSize), Microsoft.Xna.Framework.Color.Red);
                        DrawTowerStatGrid(spriteBatch);

                    }
                    else
                    {
                        spriteBatch.Draw(_towerGrid[x, y]._textureBottom, new Vector2((x + _towerSectionOffsetX) * _tileSize, (y + _towerSectionOffsetY) * _tileSize), Microsoft.Xna.Framework.Color.White);
                        spriteBatch.Draw(_towerGrid[x, y]._textureTop, new Vector2((x + _towerSectionOffsetX) * _tileSize, (y + _towerSectionOffsetY) * _tileSize), Microsoft.Xna.Framework.Color.White);

                    }

                }
                
            } //draws the tower section

            if (_tileSelected)
            {
                if (_isTower == true)
                {
                    DrawTowerStatGrid(spriteBatch);
                    DisplayTowerHitbox(spriteBatch,_towerOnTile._hitbox);
                    DisplaySell(spriteBatch);
                    if (_towerOnTile._upgradeTwo==false)
                    {
                        DisplayUpgrade(spriteBatch);
                    }
                    if (_towerOnTile._health < _towerOnTile._maxHealth)
                    {
                        DisplayHeal(spriteBatch);
                    }

                }
                if (_isObstacle ==true)
                {
                    DisplaySell(spriteBatch);
                }
            }


            
            
           

        }
    }
}
