using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Joust
{
    class UserControlledSprite : Sprite
    {
        public UserControlledSprite(Texture2D textureImage, Vector2 position,
            Point frameSize, Point frameStart, int collisionOffset, int scale, Point currentFrame, Point sheetSize,
            Vector2 speed)
            : base(textureImage, position, frameSize, frameStart, collisionOffset, scale, currentFrame,
            sheetSize, speed)
        {
        }

        public UserControlledSprite(Texture2D textureImage, Vector2 position,
            Point frameSize, Point frameStart, int collisionOffset, int scale, Point currentFrame, Point sheetSize,
            Vector2 speed, int millisecondsPerFrame)
            : base(textureImage, position, frameSize, frameStart, collisionOffset, scale, currentFrame,
            sheetSize, speed, millisecondsPerFrame)
        {
        }

        public override Vector2 direction
        {
            get
            {
                Vector2 inputDirection = new Vector2(1, 1);

                if (Keyboard.GetState().IsKeyDown(Keys.Left))
                    speed.X -= 1;
                if (Keyboard.GetState().IsKeyDown(Keys.Right))
                    speed.X += 1;
                if (Keyboard.GetState().IsKeyDown(Keys.Up))
                    speed.Y -= 1;
                if (Keyboard.GetState().IsKeyDown(Keys.Down))
                    speed.Y += 1;

                return inputDirection * speed;
            }
        }

        public override void Update(GameTime gameTime, Rectangle clientBounds)
        {
            // Move the sprite based on direction
            previousPosition = position;
            position += direction;

            // If sprite is off the screen, move it back within the game window
            if (position.X < 0)
                //position.X = 0;
                //position = previousPosition;
                position.X = clientBounds.Width + frameSize.X * scale;
            if (position.Y < 0)
                //position.Y = 0;
                //position = previousPosition;
                position.Y = clientBounds.Height + frameSize.Y * scale;
            if (position.X > clientBounds.Width - frameSize.X * scale)
                //position.X = clientBounds.Width - frameSize.X;
                //position = previousPosition;
                position.X = 0;
            if (position.Y > clientBounds.Height - frameSize.Y * scale)
                //position.Y = clientBounds.Height - frameSize.Y;
                //position = previousPosition;
                position.Y = 0;
            


            base.Update(gameTime, clientBounds);
        }
    }
}
