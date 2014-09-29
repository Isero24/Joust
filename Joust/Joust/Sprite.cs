using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Joust
{
    abstract class Sprite
    {
        Texture2D textureImage;                         // Sprite or psrite sheet of image being drawn
        protected Point frameSize;                      // Size of each individual frame in sprite sheet
        protected Point frameStart;                     // Start location of the sprite on the sheet
        Point currentFrame;                             // Index of current frame in sprite sheet
        Point sheetSize;                                // Number of columns/rows in sprite sheet
        int collisionOffset;                            // Offset used to modify frame-size rectangle for collision checks against this sprite
        int timeSinceLastFrame;                         // Number of milliseconds since last frame was drawn
        int millisecondsPerFrame;                       // Number of milliseconds to wait between frame changes
        protected int scale;                            // Scale size of sprite
        const int defaultMillisecondsPerFrame = 16;
        protected Vector2 speed;                        // Speed at which sprite will move in both X and Y directions
        protected Vector2 position;                     // Position at which to draw sprite
        protected Vector2 previousPosition;             // Previous position of the sprite

        public Sprite(Texture2D textureImage, Vector2 position, Point frameSize, Point frameStart,
            int collisionOffset, int scale, Point currentFrame, Point sheetSize, Vector2 speed)
            : this(textureImage, position, frameSize, frameStart, collisionOffset, scale, currentFrame,
            sheetSize, speed, defaultMillisecondsPerFrame)
        {
        }

        public Sprite(Texture2D textureImage, Vector2 position, Point frameSize, Point frameStart,
            int collisionOffset, int scale, Point currentFrame, Point sheetSize, Vector2 speed,
            int millisecondsPerFrame)
        {
            this.textureImage = textureImage;
            this.position = position;
            this.previousPosition = position;
            this.frameSize = frameSize;
            this.frameStart = frameStart;
            this.collisionOffset = collisionOffset;
            this.scale = scale;
            this.currentFrame = currentFrame;
            this.sheetSize = sheetSize;
            this.speed = speed;
            this.millisecondsPerFrame = millisecondsPerFrame;
        }

        public virtual void Update(GameTime gameTime, Rectangle clientBounds)
        {
            if (previousPosition != position)
            {
                timeSinceLastFrame += gameTime.ElapsedGameTime.Milliseconds;
                if (timeSinceLastFrame > millisecondsPerFrame)
                {
                    timeSinceLastFrame = 0;
                    ++currentFrame.X;
                    if (currentFrame.X >= sheetSize.X)
                    {
                        currentFrame.X = 0;
                        ++currentFrame.Y;
                        if (currentFrame.Y >= sheetSize.Y)
                            currentFrame.Y = 0;
                    }
                }
            }
            else
            {
                
            }
        }

        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(textureImage,
                position,
                new Rectangle(frameStart.X + currentFrame.X * frameSize.X,
                    frameStart.Y + currentFrame.Y * frameSize.Y,
                    frameSize.X, frameSize.Y),
                Color.White, 0, Vector2.Zero,
                scale, SpriteEffects.None, 0);
        }

        public abstract Vector2 direction
        {
            get;
        }

        public Rectangle collisionRect
        {
            get
            {
                return new Rectangle(
                    (int)position.X + collisionOffset,
                    (int)position.Y + collisionOffset,
                    frameSize.X - (collisionOffset * 2),
                    frameSize.Y - (collisionOffset * 2));
            }
        }
    }
}
