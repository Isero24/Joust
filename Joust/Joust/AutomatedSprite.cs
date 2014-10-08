using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Joust
{
    class AutomatedSprite : Sprite
    {
        public AutomatedSprite(Texture2D textureImage, Vector2 position, int collisionOffset, Point currentFrame, Vector2 speed, Dictionary<string, animate> animations)
            : base(textureImage, position, collisionOffset, currentFrame, speed, animations)
        {
        }

        public AutomatedSprite(Texture2D textureImage, Vector2 position, int collisionOffset, Point currentFrame, Vector2 speed, int millisecondsPerFrame, Dictionary<string, animate> animations)
            : base(textureImage, position, collisionOffset, currentFrame, speed, millisecondsPerFrame, animations)
        {
        }

        public override Vector2 direction
        {
            get 
            {
                if (defaultAnimation.animationName != "wall")
                {
                    // Uses collision detections to decide what animation state to be in
                    // Checks the position one move ahead.
                    position.Y += speed.Y;
                    if (defaultAnimation.Equals(animations["flying"]))
                    {
                        // If it's in the flying animation, is slowly decreases the rate it goes up
                        if (speed.Y < 2)
                        {
                            speed.Y++;
                        }

                        // If it collides with a wall beneath the sprite, it switches the animation to walking
                        // and sets the fall speed to the default speed
                        if (collisionCheck(Globals.spriteManager.wallList))
                        {
                            defaultAnimation = animations["default"];
                            speed.Y = 2;
                        }
                    }
                        // If the sprite is in a state where there is nothing beneath it to collide with
                        // it is set to the flying animation.
                        // The speed is set to make the sprite go up and slowly come down.
                        // It also checks to make sure that that the position is beneath the screen top.
                        // This keeps the sprite from flying away.
                        // Sets the sprite animation to flying
                    else if (!collisionCheck(Globals.spriteManager.wallList) && position.Y >= 0)
                    {
                        speed.Y = -6;
                        defaultAnimation = animations["flying"];
                    }
                    // Move reverted back to current spot after checkings
                    position.Y -= speed.Y;

                }

                // Return the speed
                return speed; 
            }
        }

        public override void Update(GameTime gameTime, Rectangle clientBounds)
        {
            oldPosition = position;
            position += direction;

            base.Update(gameTime, clientBounds);
        }
    }
}
