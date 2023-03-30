using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectReal
{
    class Boss: MapEnemy
    {
       
        public int _damage;
        public bool _hasAttacked;
        public List<MapTower> _towerList { get; set; }
        public Boss(Vector2 position, EnemyType enemyType) : base(position, enemyType)
        {
            
            _damage = enemyType._damage;
        }

      
        public void DrawAttack(SpriteBatch spriteBatch)
        {
            var transparency_amount = 120;
            Texture2D rectTexture = new Texture2D(Game1._graphics.GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
            Microsoft.Xna.Framework.Color[] c = new Microsoft.Xna.Framework.Color[1];
            c[0] = Microsoft.Xna.Framework.Color.FromNonPremultiplied(255, 255, 255, transparency_amount);
            // rectTexture.SetData(new Microsoft.Xna.Framework.Color[] { Microsoft.Xna.Framework.Color.White });
            rectTexture.SetData<Microsoft.Xna.Framework.Color>(c);
            Microsoft.Xna.Framework.Rectangle hitbox = new Rectangle((_currentNode._x * 64) - 64, (_currentNode._y * 64) - 64, _enemyType._texture.Width * 3, _enemyType._texture.Height * 3);
            spriteBatch.Draw(rectTexture, hitbox, Microsoft.Xna.Framework.Color.Red);
        }
    }
}
