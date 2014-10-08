using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Joust
{
    // Structure for containing sprite animations.
    public struct animate
    {
        public string animationName;        // Name of the animation
        public Point fStart;                // Point that contains where the frame starts on the sprite sheet
        public Point sSize;                 // Point that contains the size of the sprite sheet
        public Point fSize;                 // Point that contains the size of each frame
        public int millisecondsPerFrame;    // The speed at which the animation should be played.
    }

    abstract public class Sprite
    {
        Texture2D textureImage;                                 // Sprite or psrite sheet of image being drawn
        protected Point currentFrame;                           // Index of current frame in sprite sheet
        int collisionOffset;                                    // Offset used to modify frame-size rectangle for collision checks against this sprite
        int timeSinceLastFrame;                                 // Number of milliseconds since last frame was drawn
        int millisecondsPerFrame;                               // Number of milliseconds to wait between frame changes
        const int defaultMillisecondsPerFrame = 16;             // Default
        protected Vector2 speed;                                // Speed at which sprite will move in both X and Y directions
        protected Vector2 position;                             // Position at which to draw sprite
        protected Vector2 oldPosition;                          

        protected Dictionary<string, animate> animations;       // Dictionary that contains the animations associated with a specific sprite

        protected animate defaultAnimation;                     // The default animation, it's the currently used animation for a sprite

        protected SpriteEffects sEffect;                        // Holds the current sprite effect for a sprite. Used to keep track of which way the sprite is flipped

        // Constructor: If no milllisecondsperframe is specified, calls the main constructor with the default milliseconds being passed
        public Sprite(Texture2D textureImage, Vector2 position, int collisionOffset, Point currentFrame, Vector2 speed, Dictionary<string, animate> animations)
            : this(textureImage, position, collisionOffset, currentFrame, speed, defaultMillisecondsPerFrame, animations)
        {
        }

        // Constructor: sets the the class variable to the passed values
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

        // Updates the positioning of the sprite and the animation frame
        public virtual void Update(GameTime gameTime, Rectangle clientBounds)
        {
            // These two if statements are used to place the x position on the opposite of the screen when
            // the sprite goes out of bounds.

            // Shifts the x position over to the other side of the screen when the sprite goes off screen to the left
            if (position.X < -defaultAnimation.fSize.X * Globals.SCALE) position.X += clientBounds.Width;
            // Shifts the x position over to the left side of the screen when it goes outside the right side of hte client bounds
            if (position.X > clientBounds.Width) position.X -= clientBounds.Width;

            // This if statement keeps the sprite from falling outside the floor of the screen
            if (position.Y > clientBounds.Height - defaultAnimation.fSize.Y * Globals.SCALE)
            {
                position.Y = clientBounds.Height - defaultAnimation.fSize.Y * Globals.SCALE;
            }

            // Increments animation to the next frame in the animation cycle
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

        // Draws the sprite
        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch, Rectangle clientBounds)
        {
            // Draws the sprite based on it's location
            spriteBatch.Draw(
                textureImage, position,
                new Rectangle(  defaultAnimation.fStart.X + currentFrame.X * defaultAnimation.fSize.X,
                                defaultAnimation.fStart.Y + currentFrame.Y * defaultAnimation.fSize.Y,
                                defaultAnimation.fSize.X, 
                                defaultAnimation.fSize.Y),
                Color.White, 0, Vector2.Zero, Globals.SCALE, sEffect, 0);

            // If the sprite is out of bounds, another iamge will be drawn on the opposite of the screen
            // If the position is outside the left hand bounds, the rest of the sprite is drawn on the
            // right side of the screen.
            if (position.X < 0)
            {
                spriteBatch.Draw(
                    textureImage, position + new Vector2(clientBounds.Width, 0),
                    new Rectangle(  defaultAnimation.fStart.X + currentFrame.X * defaultAnimation.fSize.X,
                                    defaultAnimation.fStart.Y + currentFrame.Y * defaultAnimation.fSize.Y,
                                    defaultAnimation.fSize.X,
                                    defaultAnimation.fSize.Y),
                    Color.White, 0, Vector2.Zero, Globals.SCALE, sEffect, 0);
            }
                // If the position is outside the right hand bounds, the rest of hte sprite is drawn on the
                // left side of the screen.
            else if (position.X + defaultAnimation.fSize.X * Globals.SCALE > clientBounds.Width)
            {
                spriteBatch.Draw(
                    textureImage, position - new Vector2(clientBounds.Width, 0),
                    new Rectangle(  defaultAnimation.fStart.X + currentFrame.X * defaultAnimation.fSize.X,
                                    defaultAnimation.fStart.Y + currentFrame.Y * defaultAnimation.fSize.Y,
                                    defaultAnimation.fSize.X,
                                    defaultAnimation.fSize.Y),
                    Color.White, 0, Vector2.Zero, Globals.SCALE, sEffect, 0);
            }
        }

        // Direction method, it is overridden by the direction methods called in automated sprite and usercontrolled sprite.
        public abstract Vector2 direction
        {
            get;
        }

        // Takes a list of sprites, and calculates if there are any collisions from 
        // the sprite that this method references and a sent list of sprites.
        // The client bounds is sent so that it can be utilzed in the undoPosition method.
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

        // Checks to see if the sprite this method references collides with any of hte sprites in the 
        // sent list. If a collision is detected, a boolean is returned.
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

        // undoPosition: Is used to check in what direction the sprite encountered
        // a update it's movement accordingly
        public void undoPosition(List<Sprite> list, Rectangle clientBounds)
        {
            // Position is set to the previous position.
            position = oldPosition;

            // The sprites frame is moved one frame over in the x direction
            position.X += speed.X;

            // If a collision is detected, the frame is reverted back
            if (collisionCheck(list))
            {
                position.X -= speed.X;
            }

            // The sprites frame is moved one frame over in the y direction
            position.Y += speed.Y;

            // If a collision is detected, the frame is reverted back
            while (collisionCheck(list))
            {
                position.Y -= speed.Y;
            }

            // The sprites new move is recalculated to see if it went out of bounds and if the x position
            // should be assigned a new variable on the other side of the screen.
            if (position.X < -defaultAnimation.fSize.X * Globals.SCALE) position.X += clientBounds.Width;
            if (position.X > clientBounds.Width) position.X -= clientBounds.Width;

            // Makes sure that the sprites y position doesn't not fall out of bounds at the bottom of the screen.
            if (position.Y > clientBounds.Height - defaultAnimation.fSize.Y * Globals.SCALE)
            {
                position.Y = clientBounds.Height - defaultAnimation.fSize.Y * Globals.SCALE;
            }
        }

        // Obtains a collision rectange for the sprite.
        // Used when checking for collision with other sprites.
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
