using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Joust
{
    public struct animate
    {
        public string animationName;
        public Point fStart;
        public Point sSize;
        public Point fSize;
        public SpriteEffects sEffect;
        public int millisecondsPerFrame;
    }

    abstract public class Sprite
    {
        Texture2D textureImage;                         // Sprite or psrite sheet of image being drawn
        protected Point currentFrame;                             // Index of current frame in sprite sheet
        int collisionOffset;                            // Offset used to modify frame-size rectangle for collision checks against this sprite
        int timeSinceLastFrame;                         // Number of milliseconds since last frame was drawn
        int millisecondsPerFrame;                       // Number of milliseconds to wait between frame changes
        const int defaultMillisecondsPerFrame = 16;
        protected Vector2 speed;                        // Speed at which sprite will move in both X and Y directions
        protected Vector2 position;                     // Position at which to draw sprite
        protected Vector2 oldPosition;

        protected Dictionary<string, animate> animations;

        protected animate defaultAnimation;

        public Sprite(Texture2D textureImage, Vector2 position, int collisionOffset, Point currentFrame, Vector2 speed, Dictionary<string, animate> animations)
            : this(textureImage, position, collisionOffset, currentFrame, speed, defaultMillisecondsPerFrame, animations)
        {
        }

        public Sprite(Texture2D textureImage, Vector2 position, int collisionOffset, Point currentFrame, Vector2 speed, int millisecondsPerFrame, Dictionary<string, animate> animations)
        {
            this.textureImage = textureImage;
            this.position = position;
            this.collisionOffset = collisionOffset;
            this.currentFrame = currentFrame;
            this.speed = speed;
            this.millisecondsPerFrame = millisecondsPerFrame;
            this.animations = animations;
            defaultAnimation = animations["default"];
        }

        public virtual void Update(GameTime gameTime, Rectangle clientBounds)
        {
            if (position.X < -defaultAnimation.fSize.X * Globals.SCALE) position.X += clientBounds.Width;
            if (position.X > clientBounds.Width) position.X -= clientBounds.Width;

            if (position.Y > clientBounds.Height - defaultAnimation.fSize.Y * Globals.SCALE)
            {
                position.Y = clientBounds.Height - defaultAnimation.fSize.Y * Globals.SCALE;
            }

            timeSinceLastFrame += gameTime.ElapsedGameTime.Milliseconds;
            if (timeSinceLastFrame > defaultAnimation.millisecondsPerFrame)
            {
                timeSinceLastFrame = 0;
                ++currentFrame.X;
                if (currentFrame.X >= defaultAnimation.sSize.X)
                {
                    currentFrame.X = 0;
                    ++currentFrame.Y;
                    if (currentFrame.Y >= defaultAnimation.sSize.Y)
                        currentFrame.Y = 0;
                }
            }
        }

        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch, Rectangle clientBounds)
        {
            spriteBatch.Draw(
                textureImage, position,
                new Rectangle(  defaultAnimation.fStart.X + currentFrame.X * defaultAnimation.fSize.X,
                                defaultAnimation.fStart.Y + currentFrame.Y * defaultAnimation.fSize.Y,
                                defaultAnimation.fSize.X, 
                                defaultAnimation.fSize.Y),
                Color.White, 0, Vector2.Zero, Globals.SCALE, defaultAnimation.sEffect, 0);

            if (position.X < 0)
            {
                spriteBatch.Draw(
                    textureImage, position + new Vector2(clientBounds.Width, 0),
                    new Rectangle(  defaultAnimation.fStart.X + currentFrame.X * defaultAnimation.fSize.X,
                                    defaultAnimation.fStart.Y + currentFrame.Y * defaultAnimation.fSize.Y,
                                    defaultAnimation.fSize.X,
                                    defaultAnimation.fSize.Y),
                    Color.White, 0, Vector2.Zero, Globals.SCALE, defaultAnimation.sEffect, 0);
            }
            else if (position.X + defaultAnimation.fSize.X * Globals.SCALE > clientBounds.Width)
            {
                spriteBatch.Draw(
                    textureImage, position - new Vector2(clientBounds.Width, 0),
                    new Rectangle(  defaultAnimation.fStart.X + currentFrame.X * defaultAnimation.fSize.X,
                                    defaultAnimation.fStart.Y + currentFrame.Y * defaultAnimation.fSize.Y,
                                    defaultAnimation.fSize.X,
                                    defaultAnimation.fSize.Y),
                    Color.White, 0, Vector2.Zero, Globals.SCALE, defaultAnimation.sEffect, 0);
            }
        }

        public abstract Vector2 direction
        {
            get;
        }

        public void findCollisions(List<Sprite> list, Rectangle clientBounds)
        {
            foreach (Sprite s in list)
            {
                if (collisionRect.Intersects(s.collisionRect))
                {
                    undoPosition(list, clientBounds);
                }
            }
        }

        public bool collisionCheck(List<Sprite> list)
        {
            foreach (Sprite s in list)
            {
                if (collisionRect.Intersects(s.collisionRect))
                {
                    return true;
                }
            }

            return false;
        }

        public void undoPosition(List<Sprite> list, Rectangle clientBounds)
        {
            position = oldPosition;

            position.X += speed.X;

            if (collisionCheck(list))
            {
                position.X -= speed.X;
            }

            position.Y += speed.Y;

            while (collisionCheck(list))
            {
                position.Y -= speed.Y;
            }

            if (position.X < -defaultAnimation.fSize.X * Globals.SCALE) position.X += clientBounds.Width;
            if (position.X > clientBounds.Width) position.X -= clientBounds.Width;

            if (position.Y > clientBounds.Height - defaultAnimation.fSize.Y * Globals.SCALE)
            {
                position.Y = clientBounds.Height - defaultAnimation.fSize.Y * Globals.SCALE;
            }
        }

        public Rectangle collisionRect
        {
            get
            {
                return new Rectangle(
                    (int)position.X,// + collisionOffset,
                    (int)position.Y,// + collisionOffset,
                    defaultAnimation.fSize.X * Globals.SCALE,// - (collisionOffset * 2),
                    defaultAnimation.fSize.Y * Globals.SCALE);// - (collisionOffset * 2));
            }
        }
    }
}
