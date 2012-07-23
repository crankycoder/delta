using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Delta.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Delta.Collision;

namespace Delta.Examples.Entities
{
    public class Barrel : Entity
    {
        SpriteEntity _sprite;

        public Barrel()
        {
            _sprite = SpriteEntity.Create(@"Graphics\SpriteSheets\16x16");
            _sprite.Play("barrel");
        }

        protected override void LateInitialize()
        {
            WrappedBody = Collider.CreateBody(new Box(16, 16));
            WrappedBody.OnCollisionEvent += OnCollision;
            base.LateInitialize();
        }

        protected override void LightUpdate(DeltaTime time)
        {
            _sprite.InternalUpdate(time);
            _sprite.Position = Position - (_sprite.Size * new Vector2(0.5f, 0.5f));
            base.LightUpdate(time);
        }

        protected override void Draw(DeltaTime time, SpriteBatch spriteBatch)
        {
            _sprite.InternalDraw(time, spriteBatch);
            base.Draw(time, spriteBatch);
        }

        protected bool OnCollision(IWrappedBody them, Vector2 normal)
        {
            Lily link = them.Owner as Lily;
            if (link != null)
            {
                Explode();
                RemoveNextUpdate = true;
            }
            return true;
        }

        public void Explode()
        {
            G.Audio.PlaySound("SFX_LargeExplosion", true);
            Visuals.Create(@"Graphics\SpriteSheets\16x16", "explode", Position);
            Visuals.CreateShatter(@"Graphics\SpriteSheets\16x16", "barrelDebris", Position, 13);
            G.World.Camera.Shake(10, 0.5f, ShakeAxis.X | ShakeAxis.Y);
        }
    }
}
