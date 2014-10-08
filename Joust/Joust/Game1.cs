// test test from main!
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

namespace Joust
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    /// 

    public static class Globals
    {
        public const Int32 SCALE = 3;                   // Global scale variable. Can't be changed.
        public static SpriteManager spriteManager;      // Global sprite manager. Used for accessing the wallList on a global level
    }

    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Texture2D backgroundTexture;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            Globals.spriteManager = new SpriteManager(this);
            Components.Add(Globals.spriteManager);

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            backgroundTexture = Content.Load<Texture2D>("JoustSheet");

            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            // TODO: Add your update logic here

            // Screen size is updated that is set to the scale size
            graphics.PreferredBackBufferHeight = 194 * Globals.SCALE;
            graphics.PreferredBackBufferWidth = 235 * Globals.SCALE;
            graphics.ApplyChanges();

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // Sprite batch is setup so that the world is drawn with a pixelated look
            spriteBatch.Begin(SpriteSortMode.Immediate,
                    BlendState.AlphaBlend,
                    SamplerState.PointClamp,
                    null,
                    null
                    );

            // Draw the background image first
            spriteBatch.Draw(
                backgroundTexture,
                new Vector2(0, 0) * Globals.SCALE,
                new Rectangle(2, 44, 235, 237),
                Color.White,
                0,
                Vector2.Zero,
                Globals.SCALE,
                SpriteEffects.None,
                1);

            spriteBatch.End();

            base.Draw(gameTime); // All the sprites are drawn

            // Sprite batch is restarted and the lava is drawn so that it is on top of the sprites
            spriteBatch.Begin(SpriteSortMode.Immediate,
                    BlendState.AlphaBlend,
                    SamplerState.PointClamp,
                    null,
                    null
                    );

            // Draw the lava
            spriteBatch.Draw(
                backgroundTexture,
                new Vector2(0, 168) * Globals.SCALE,
                new Rectangle(2, 243, 236, 26),
                Color.White,
                0,
                Vector2.Zero,
                Globals.SCALE,
                SpriteEffects.None,
                1);

            spriteBatch.End();

            

        }
    }
}
