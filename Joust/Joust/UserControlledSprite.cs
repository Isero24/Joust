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
        KeyboardState oldKeyState;

        public UserControlledSprite(Texture2D textureImage, Vector2 position,
            Point frameSize, Point frameStart, int collisionOffset, int scale, Point currentFrame, Point sheetSize,
            Vector2 speed, Dictionary<string, animate> animations)
            : base(textureImage, position, frameSize, frameStart, collisionOffset, scale, currentFrame,
            sheetSize, speed, animations)
        {
        }

        public UserControlledSprite(Texture2D textureImage, Vector2 position,
            Point frameSize, Point frameStart, int collisionOffset, int scale, Point currentFrame, Point sheetSize,
            Vector2 speed, int millisecondsPerFrame, Dictionary<string, animate> animations)
            : base(textureImage, position, frameSize, frameStart, collisionOffset, scale, currentFrame,
            sheetSize, speed, millisecondsPerFrame, animations)
        {
        }

        public override Vector2 direction
        {
            get
            {
                KeyboardState newKeyState = Keyboard.GetState();

                Vector2 inputDirection = new Vector2(1, 1);

                if (newKeyState.IsKeyDown(Keys.Left))
                {
                    
                    if (speed.X > 0)
                    {
                        state = State.Stop;
                        speed.X = 0;
                    }
                    else if (oldKeyState.IsKeyUp(Keys.Left))
                    {
                        if (speed.X > -3)
                        {
                            speed.X -= 1;
                        }

                        state = State.Moving;
                    }
                }

                if (newKeyState.IsKeyDown(Keys.Right))
                {

                    if (speed.X < 0)
                    {
                        state = State.Stop;
                        speed.X = 0;
                    }
                    else if (oldKeyState.IsKeyUp(Keys.Right))
                    {
                        if (speed.X < 3)
                        {
                            speed.X += 1;
                        }

                        state = State.Moving;
                    }
                }

                oldKeyState = newKeyState;

                return inputDirection * speed;
            }
        }

        public override void Update(GameTime gameTime, Rectangle clientBounds)
        {
            // Move the sprite based on direction
            position += direction;

            // If sprite is off the screen, move it back within the game window
            if (position.X <= 0)
                position.X = clientBounds.Width;// +frameSize.X * scale;

            if (position.X > clientBounds.Width - frameSize.X * scale)
                position.X = 0;

            base.Update(gameTime, clientBounds);
        }
    }
}
