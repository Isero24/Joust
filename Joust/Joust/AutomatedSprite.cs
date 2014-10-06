using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Joust
{
    class AutomatedSprite: Sprite
    {
        public AutomatedSprite(Texture2D textureImage, Vector2 position, int collisionOffset, int scale, Point currentFrame, Vector2 speed, Dictionary<string, animate> animations)
            : base(textureImage, position, collisionOffset, scale, currentFrame, speed, animations)
        {
        }

        public AutomatedSprite(Texture2D textureImage, Vector2 position, int collisionOffset, int scale, Point currentFrame, Vector2 speed, int millisecondsPerFrame, Dictionary<string, animate> animations)
            : base(textureImage, position, collisionOffset, scale, currentFrame, speed, millisecondsPerFrame, animations)
        {
        }

        public override Vector2 direction
        {
            get { return speed; }
        }

        public override void Update(GameTime gameTime, Rectangle clientBounds)
        {
            position += direction;

            base.Update(gameTime, clientBounds);
        }
    }
}
