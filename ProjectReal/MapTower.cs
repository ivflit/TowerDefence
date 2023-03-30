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
    class MapTower //Tower that is on the map
    {
       private bool _unlocked = false;
        public float _fireTimer = 0;
        public int _damage;
        public int _health;
        public int _maxHealth;
        public MapTower(bool unlocked, TowerType towerType, Color color, Rectangle hitbox, int damage, int health)
        {
            _unlocked = unlocked;
            _towerType = towerType;
            _color = color;
            _hitbox = hitbox;
            _damage = damage;
            _maxHealth = health;
        }
        public bool _upgradeOne;
        public bool _upgradeTwo;
        public float _angleOfTexture = 0f;
        public Queue<MapEnemy> _enemyQueue = new Queue<MapEnemy>();
        public MapEnemy _towerTargetedEnemy;
        public TowerType _towerType { get; }
        public Microsoft.Xna.Framework.Rectangle _hitbox { get; set; }
        public Microsoft.Xna.Framework.Color _color { get; set; }
       public Vector2 _position { get; set; }

        public void DrawHealthBar(SpriteBatch spriteBatch, Texture2D pixelTexture, int currentHealth, int maxHealth, Color bgColor, Color fillColor)
        {
            // Define the position and size of the health bar
            Rectangle healthBarRect = new Rectangle((int)_position.X + 15, (int)_position.Y + 10, 32, 5);

            // Calculate the fill percentage of the health bar
            float fillPercentage = (float)currentHealth / maxHealth;

            // Draw the background rectangle of the health bar
            spriteBatch.Draw(pixelTexture, healthBarRect, bgColor);

            // Draw the fill rectangle of the health bar
            int fillWidth = (int)(healthBarRect.Width * fillPercentage);
            Rectangle fillRect = new Rectangle(healthBarRect.X, healthBarRect.Y, fillWidth, healthBarRect.Height);
            spriteBatch.Draw(pixelTexture, fillRect, fillColor);
        }

       
        private void RotateToFollowEnemy()
        {

        }

        

    }
}
