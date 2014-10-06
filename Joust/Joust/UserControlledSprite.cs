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

        public UserControlledSprite(Texture2D textureImage, Vector2 position, int collisionOffset, int scale, Point currentFrame, Vector2 speed, Dictionary<string, animate> animations)
            : base(textureImage, position, collisionOffset, scale, currentFrame, speed, animations)
        {
        }

        public UserControlledSprite(Texture2D textureImage, Vector2 position, int collisionOffset, int scale, Point currentFrame, Vector2 speed, int millisecondsPerFrame, Dictionary<string, animate> animations)
            : base(textureImage, position, collisionOffset, scale, currentFrame, speed, millisecondsPerFrame, animations)
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
                        defaultAnimation = animations["default"];
                        speed.X = 0;
                    }
                    else if (oldKeyState.IsKeyUp(Keys.Left))
                    {
                        defaultAnimation = animations["walking"];

                        if (speed.X > -3)
                        {
                            speed.X -= 1;
                        }

                    }

                    defaultAnimation.sEffect = SpriteEffects.FlipHorizontally;

                } 
                else if (newKeyState.IsKeyDown(Keys.Right))
                {
                    if (speed.X < 0)
                    {
                        defaultAnimation = animations["default"];
                        speed.X = 0;
                    }
                    else if (oldKeyState.IsKeyUp(Keys.Right))
                    {
                        defaultAnimation = animations["walking"];           

                        if (speed.X < 3)
                        {                           
                            speed.X += 1;
                        }

                    }

                    defaultAnimation.sEffect = SpriteEffects.None;
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
                position.X = clientBounds.Width - defaultAnimation.fSize.X * scale;// +frameSize.X * scale;

            if (position.X > clientBounds.Width - defaultAnimation.fSize.X * scale)
                position.X = 0;

            base.Update(gameTime, clientBounds);
        }
    }
}
