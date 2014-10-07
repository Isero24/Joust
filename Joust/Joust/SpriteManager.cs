using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;


// YOU'RE CURRENT IMPLEMENTING THE STAND STILL FRAME WHEN NOT MOVING


namespace Joust
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    /// 

    public class SpriteManager : Microsoft.Xna.Framework.DrawableGameComponent
    {
        SpriteBatch spriteBatch;

        UserControlledSprite player;

        List<Sprite> spriteList = new List<Sprite>();
        public List<Sprite> wallList = new List<Sprite>();

        public SpriteManager(Game game)
            : base(game)
        {
            // TODO: Construct any child components here
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            // TODO: Add your initialization code here

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(Game.GraphicsDevice);

            player = new UserControlledSprite(
                Game.Content.Load<Texture2D>("JoustSheet"),                     // Loads the texture for the sprite
                Vector2.Zero,                                                   // Sets the sprite position on screen
                10,                                                             // Collision Offset
                new Point(0, 0),                                                // Current frame in the animation
                new Vector2(0, 2),                                              // Sprite movement speed
                50,                                                             // sprite animation speed
                new Dictionary<string, animate>()                               // Dictionary of animations for specific sprite
                {
                    {"default", new animate {animationName = "stopped", fStart = new Point(247, 42), sSize = new Point(1, 1), fSize = new Point(15, 20), millisecondsPerFrame = 50}},
                    {"walking", new animate {animationName = "walking", fStart = new Point(247, 62), sSize = new Point(4, 1), fSize = new Point(15, 20), millisecondsPerFrame = 50}},
                    {"flying", new animate {animationName = "flying", fStart = new Point(247, 101), sSize = new Point(1, 1), fSize = new Point(15, 14), millisecondsPerFrame = 100}}
                }
            );

            wallList.Add(new AutomatedSprite(
                Game.Content.Load<Texture2D>("Image1"), new Vector2(198, 29) * Globals.SCALE, 10, new Point(0, 0), new Vector2(0, 0),
                new Dictionary<string, animate>()
                {
                    {"default", new animate {animationName = "wall", fStart = new Point(0, 0), sSize = new Point(1, 1), fSize = new Point(37, 6)}}
                }));

            wallList.Add(new AutomatedSprite(
                Game.Content.Load<Texture2D>("Image1"), new Vector2(158, 83) * Globals.SCALE, 10, new Point(0, 0), new Vector2(0, 0),
                new Dictionary<string, animate>()
                {
                    {"default", new animate {animationName = "wall", fStart = new Point(0, 0), sSize = new Point(1, 1), fSize = new Point(46, 6)}}
                }));

            wallList.Add(new AutomatedSprite(
                Game.Content.Load<Texture2D>("Image1"), new Vector2(173, 89) * Globals.SCALE, 10, new Point(0, 0), new Vector2(0, 0),
                new Dictionary<string, animate>()
                {
                    {"default", new animate {animationName = "wall", fStart = new Point(0, 0), sSize = new Point(1, 1), fSize = new Point(28, 4)}}
                }));

            wallList.Add(new AutomatedSprite(
                Game.Content.Load<Texture2D>("Image1"), new Vector2(200, 91) * Globals.SCALE, 10, new Point(0, 0), new Vector2(0, 0),
                new Dictionary<string, animate>()
                {
                    {"default", new animate {animationName = "wall", fStart = new Point(0, 0), sSize = new Point(1, 1), fSize = new Point(35, 6)}}
                }));

            wallList.Add(new AutomatedSprite(
                Game.Content.Load<Texture2D>("Image1"), new Vector2(65, 40) * Globals.SCALE, 10, new Point(0, 0), new Vector2(0, 0),
                new Dictionary<string, animate>()
                {
                    {"default", new animate {animationName = "wall", fStart = new Point(0, 0), sSize = new Point(1, 1), fSize = new Point(70, 9)}}
                }));

            wallList.Add(new AutomatedSprite(
                Game.Content.Load<Texture2D>("Image1"), new Vector2(0, 29) * Globals.SCALE, 10, new Point(0, 0), new Vector2(0, 0),
                new Dictionary<string, animate>()
                {
                    {"default", new animate {animationName = "wall", fStart = new Point(0, 0), sSize = new Point(1, 1), fSize = new Point(24, 6)}}
                }));

            wallList.Add(new AutomatedSprite(
                Game.Content.Load<Texture2D>("Image1"), new Vector2(0, 91) * Globals.SCALE, 10, new Point(0, 0), new Vector2(0, 0),
                new Dictionary<string, animate>()
                {
                    {"default", new animate {animationName = "wall", fStart = new Point(0, 0), sSize = new Point(1, 1), fSize = new Point(49, 7)}}
                }));

            wallList.Add(new AutomatedSprite(
                Game.Content.Load<Texture2D>("Image1"), new Vector2(81, 114) * Globals.SCALE, 10, new Point(0, 0), new Vector2(0, 0),
                new Dictionary<string, animate>()
                {
                    {"default", new animate {animationName = "wall", fStart = new Point(0, 0), sSize = new Point(1, 1), fSize = new Point(51, 7)}}
                }));

            wallList.Add(new AutomatedSprite(
                Game.Content.Load<Texture2D>("Image1"), new Vector2(39, 157) * Globals.SCALE, 10, new Point(0, 0), new Vector2(0, 0),
                new Dictionary<string, animate>()
                {
                    {"default", new animate {animationName = "wall", fStart = new Point(2, 272), sSize = new Point(1, 1), fSize = new Point(149, 37)}}
                }));

            wallList.Add(new AutomatedSprite(
                Game.Content.Load<Texture2D>("JoustSheet"), new Vector2(-9, 157) * Globals.SCALE, 10, new Point(0, 0), new Vector2(0, 0),
                new Dictionary<string, animate>()
                {
                    {"default", new animate {animationName = "wall", fStart = new Point(2, 272), sSize = new Point(1, 1), fSize = new Point(48, 3)}}
                }));

            wallList.Add(new AutomatedSprite(
                Game.Content.Load<Texture2D>("JoustSheet"), new Vector2(187, 157) * Globals.SCALE, 10, new Point(0, 0), new Vector2(0, 0),
                new Dictionary<string, animate>()
                {
                    {"default", new animate {animationName = "wall", fStart = new Point(2, 272), sSize = new Point(1, 1), fSize = new Point(48, 3)}}
                }));

            base.LoadContent();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            // TODO: Add your update code here

            // Update player
            player.Update(gameTime, Game.Window.ClientBounds);

            foreach (Sprite s in wallList)
            {
                s.Update(gameTime, Game.Window.ClientBounds);

                //player.checkCollision(s, Game.Window.ClientBounds);
            }

            player.findCollisions(wallList, Game.Window.ClientBounds);

            // Update the automated sprites
            //foreach (Sprite s in spriteList) 
            //{ 
            //    s.Update(gameTime, Game.Window.ClientBounds);
            //}

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            //spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend);
            spriteBatch.Begin(SpriteSortMode.Immediate,
                    BlendState.AlphaBlend,
                    SamplerState.PointClamp,
                    null,
                    null
                    );

            // Draw the player
            player.Draw(gameTime, spriteBatch, Game.Window.ClientBounds);
            foreach (Sprite s in wallList)
                s.Draw(gameTime, spriteBatch, Game.Window.ClientBounds);

            // Draw the automated sprites
            foreach (Sprite s in spriteList)
                s.Draw(gameTime, spriteBatch, Game.Window.ClientBounds);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
