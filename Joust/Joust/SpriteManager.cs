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
    public class SpriteManager : Microsoft.Xna.Framework.DrawableGameComponent
    {
        SpriteBatch spriteBatch;

        UserControlledSprite player;

        List<Sprite> spriteList = new List<Sprite>();

        AutomatedSprite background;

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
                Game.Content.Load<Texture2D>("JoustSheet"),
                Vector2.Zero, new Point(15, 20), new Point(247, 62), 10, 3, new Point(0, 0),
                new Point(4, 1), new Vector2(0, 0), 50,
                new Dictionary<string, animate>()
                {
                    {"default", new animate {animationName = "walking", fStart = new Point(247, 62), sSize = new Point(4, 1)}},
                    {"walking", new animate {animationName = "walking", fStart = new Point(247, 62), sSize = new Point(4, 1)}}
                }

                );
        
            

            base.LoadContent();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            // TODO: Add your update code here
            
            // Update background
            background.Update(gameTime, Game.Window.ClientBounds);

            // Update player
            player.Update(gameTime, Game.Window.ClientBounds);

            // Update the automated sprites
            foreach (Sprite s in spriteList) 
            { 
                s.Update(gameTime, Game.Window.ClientBounds);
            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            //spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend);
            spriteBatch.Begin(SpriteSortMode.Immediate,
                    BlendState.AlphaBlend,
                    SamplerState.PointClamp,
                    null,
                    null,
                    null);

            // Draw the background
            background.Draw(gameTime, spriteBatch);

            // Draw the player
            player.Draw(gameTime, spriteBatch);            

            // Draw the automated sprites
            foreach (Sprite s in spriteList)
                s.Draw(gameTime, spriteBatch);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
