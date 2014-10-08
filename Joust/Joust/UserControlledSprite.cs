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

        public UserControlledSprite(Texture2D textureImage, Vector2 position, int collisionOffset, Point currentFrame, Vector2 speed, Dictionary<string, animate> animations)
            : base(textureImage, position, collisionOffset, currentFrame, speed, animations)
        {
        }

        public UserControlledSprite(Texture2D textureImage, Vector2 position, int collisionOffset, Point currentFrame, Vector2 speed, int millisecondsPerFrame, Dictionary<string, animate> animations)
            : base(textureImage, position, collisionOffset, currentFrame, speed, millisecondsPerFrame, animations)
        {
        }

        public override Vector2 direction
        {
            get
            {
                
                KeyboardState newKeyState = Keyboard.GetState();

                Vector2 inputDirection = new Vector2(1, 1);

                // Checking the sprite one frame ahead of where it is at
                position.Y += speed.Y;
                // Checks to see if there is a collision one frame beneath the sprite
                // If successful, it sets the default animation to the stand animation.
                if (collisionCheck(Globals.spriteManager.wallList))
                {
                    // default is the stand animation
                    defaultAnimation = animations["default"];

                    // if the speed is moving left or right, the default animation is set to walking
                    if (speed.X < 0 || speed.X > 0)
                    {
                        defaultAnimation = animations["walking"];
                    }
                }
                    // If there is no downward collision with anything one frame beneath the sprite
                    // the sprite is set to a flying animation.
                else if (!collisionCheck(Globals.spriteManager.wallList))
                {
                    defaultAnimation = animations["flying"];
                }
                // The sprites downward frame is set one frame back to
                position.Y -= speed.Y;
                
                // Sprite speed is calculated based off fresh presses to the right and the left.
                if (newKeyState.IsKeyDown(Keys.Left) && oldKeyState.IsKeyUp(Keys.Left))
                {
                    // If the sprite is moving in the left direction, and the sprite is not in flying animation, the speed is set to 0
                    // the sprite is now standing still
                    if (speed.X > 0 && !defaultAnimation.Equals(animations["flying"]))
                    {
                        speed.X = 0;
                    }
                        // If the sprite is already moving in the left direction, the speed is incremented in that direction by 1.
                        // The max speed of the sprite in the left direction is 3 units
                    else if (speed.X > -3)
                    {
                        speed.X -= 1;
                    }

                    // If the sprite is in the flying animation and the direction it is moving is right,
                    // the sprites speed is kept the same, but it is multiplied by one so it is now flying
                    // in the left direction.
                    if (defaultAnimation.Equals(animations["flying"]))
                    {
                        if (speed.X > 0)
                        {
                            speed.X *= -1;
                        }
                    }

                    // The sprite effect is set so that it is facing the left direction
                    sEffect = SpriteEffects.FlipHorizontally;

                } 
                    // This is the same as the fresh press for the left key except everything is
                    // calculated for the right direction instead.
                else if (newKeyState.IsKeyDown(Keys.Right) && oldKeyState.IsKeyUp(Keys.Right))
                {
                    if (speed.X < 0 && !defaultAnimation.Equals(animations["flying"]))
                    {
                        speed.X = 0;
                    }
                    else if (speed.X < 3)
                    {                           
                        speed.X += 1;
                    }

                    if (defaultAnimation.Equals(animations["flying"]))
                    {
                        if (speed.X < 0)
                        {
                            speed.X *= -1;
                        }
                    }

                    // sprite effect is set to none since the default frames are already facing
                    // the right direction.
                    sEffect = SpriteEffects.None;
                }

                // If a fresh press of the space bar is detected, the sprite fall speed is set in the negatives and slowly increases
                // back up to positive 2.
                // This is to simulate a jump with gravity that slowly pulls the sprite back down.
                if (newKeyState.IsKeyDown(Keys.Space) && oldKeyState.IsKeyUp(Keys.Space))
                {
                    speed.Y = -6;
                }
                else
                {
                    if (speed.Y < 2)
                    {
                        speed.Y++;
                    }
                }

                oldKeyState = newKeyState;

                return inputDirection * speed;
            }
        }

        public override void Update(GameTime gameTime, Rectangle clientBounds)
        {
            // Move the sprite based on direction
            oldPosition = position;
            position += direction;

            base.Update(gameTime, clientBounds);
        }
    }
}
