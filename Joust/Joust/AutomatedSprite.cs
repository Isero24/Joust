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
        public AutomatedSprite(Texture2D textureImage, Vector2 position, Point frameSize, Point frameStart,
            int collisionOffset, int scale, Point currentFrame, Point sheetSize, Vector2 speed)
            : base(textureImage, position, frameSize, frameStart, collisionOffset, scale, currentFrame,
            sheetSize, speed)
        {
        }

        public AutomatedSprite(Texture2D textureImage, Vector2 position, Point frameSize, Point frameStart,
            int collisionOffset, int scale, Point currentFrame, Point sheetSize, Vector2 speed,
            int millisecondsPerFrame)
            : base(textureImage, position, frameSize, frameStart, collisionOffset, scale, currentFrame,
            sheetSize, speed, millisecondsPerFrame)
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
