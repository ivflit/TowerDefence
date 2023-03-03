using System;
using System.Collections.Generic;
using System.Text;
using MonoGame;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace ProjectReal
{
    public class Stage //the place where our game takes place; map players enemies will go here
    {
        private int _tileSize;
        public int _difficulty { get; set; }
        private Player _player;
        private int _frameCounter;
        private Map _map;
        private Texture2D _interfaceTexture;
        private List<TowerType> _towerTypes;
        private List<EnemyType> _enemyTypes;
        private int _selectedBuidlingIndex;
        private Point _mouseTilePosition;
        private bool _gameOver;
        private Dictionary<char, Tile> _symbolToTile;
        public Stage()
        {
        }

        private void DictionaryLoad()
        {

        }

     

        private void LoadEnemyTypes()
        {
            
        }
        private void LoadBuildingTypes()
        { 
        
        }
    
        
        
        public void Update() //stage will be updated every frame, well undate logic/movemt/collison here
        {

        }
        public void Draw()
        {

        }

    }
}
