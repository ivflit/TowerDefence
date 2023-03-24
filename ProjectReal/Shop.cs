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
        public TowerType _selectedTower;
        public List<Microsoft.Xna.Framework.Rectangle> _towerGridHitboxes;
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
           
            for (int i = 0; i < _selectedTower._statsOfTower.Count; i++)
            {
                 spriteBatch.DrawString(Game1._font, _selectedTower._statsOfTower[i], new Vector2(_StatSectionOffsetX * _tileSize, _StatSectionOffsetY * _tileSize*i + _towerGridHitboxes[_towerGridHitboxes.Count - 1].Bottom), Microsoft.Xna.Framework.Color.White);
            }
        }

        public void DisplayUpgrade()
        {

        }
        public void DisplaySell()
        {

        }
        public void Upgrade()
        {

        }
        public void Sell()
        {

        }
        public void Draw(SpriteBatch spriteBatch)
        {
           // string title = "SHOP";
            //spriteBatch.DrawString(Game1._font, title, new Vector2(_StatSectionOffsetX * _tileSize, (_StatSectionOffsetY) * _tileSize), Microsoft.Xna.Framework.Color.White);
            for (int y = 0; y< _towerGrid.GetLength(1); y++)
            {
                for (int x = 0; x < _towerGrid.GetLength(0); x++)
                {
                    if (_towerGrid[x,y] == _selectedTower)
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

            
            
           

        }
    }
}
