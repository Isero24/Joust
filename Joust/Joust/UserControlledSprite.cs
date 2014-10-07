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

                if (defaultAnimation.Equals(animations["flying"]))
                {
                    position.Y += 6;
                    if (collisionCheck(Globals.spriteManager.wallList))
                    {
                        position.Y -= 6;
                        if (!collisionCheck(Globals.spriteManager.wallList))
                        {
                            
                            if (speed.X == 0)
                            {
                                defaultAnimation = animations["default"];
                            }
                            else
                            {
                                defaultAnimation = animations["walking"];
                            }
                        }
                        
                    }
                }

                if (newKeyState.IsKeyDown(Keys.Left))
                {                    
                    if (speed.X > 0)
                    {
                        if (!defaultAnimation.Equals(animations["flying"]))
                        {
                            defaultAnimation = animations["default"];
                        }
                        speed.X = 0;
                    }
                    else if (oldKeyState.IsKeyUp(Keys.Left))
                    {
                        if (!defaultAnimation.Equals(animations["flying"]))
                        {
                            defaultAnimation = animations["walking"];
                        }

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
                        if (!defaultAnimation.Equals(animations["flying"]))
                        {
                            defaultAnimation = animations["default"];
                        } 
                        speed.X = 0;
                    }
                    else if (oldKeyState.IsKeyUp(Keys.Right))
                    {
                        if (!defaultAnimation.Equals(animations["flying"]))
                        {
                            defaultAnimation = animations["walking"];
                        }

                        if (speed.X < 3)
                        {                           
                            speed.X += 1;
                        }

                    }

                    defaultAnimation.sEffect = SpriteEffects.None;
                }

                if (newKeyState.IsKeyDown(Keys.Space) && oldKeyState.IsKeyUp(Keys.Space))
                {
                    if (!defaultAnimation.Equals(animations["flying"]))
                    {
                        defaultAnimation = animations["flying"];
                    }
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
