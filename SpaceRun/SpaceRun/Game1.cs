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
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;

namespace SpaceRun
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        public SpriteBatch spriteBatch;

        public SpriteFont debugFont;

        private HUD hud = new HUD();

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

            debugFont = Content.Load<SpriteFont>("media\\fonts\\DebugFont");

            ModelManager.get().loadModels(Content);

            // TODO: A list of levels or something.
            new Level1().populateLevel();

            PlayerShip ship = new PlayerShip();
            ship.model = ModelManager.get().getModel(ModelType.PLAYER_SHIP);
            ship.mass_kg = 10000;
            EntityManager.get().playerShips.Add(ship);

            hud.playerShip = ship;
            hud.LoadContent(Content);

            // Space dust
            /* // TODO: Use some ready made particle engine.
            List<Texture2D> spaceDust = new List<Texture2D>();
            spaceDust.Add(Content.Load<Texture2D>("media\\textures\\dust-1"));
            spaceDust.Add(Content.Load<Texture2D>("media\\textures\\dust-2"));
            spaceDust.Add(Content.Load<Texture2D>("media\\textures\\dust-3"));
            spaceDust.Add(Content.Load<Texture2D>("media\\textures\\dust-4"));
            ParticleSystem dust = new ParticleSystem(ship, spaceDust, 1, 1, 1000, false, float.PositiveInfinity);
            EntityManager.get().entities.Add(dust);
            */
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

            EntityManager.get().updateEntities(gameTime);
            hud.Update(gameTime);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(new Color(0, 0, 0.02f));

            GraphicsDevice.RenderState.DepthBufferEnable = true;
            GraphicsDevice.RenderState.DepthBufferWriteEnable = true;

            EntityManager.get().renderEntities(graphics);

            spriteBatch.Begin(SpriteBlendMode.AlphaBlend);

            //HACK: player debug
            if (EntityManager.get().playerShips.Count > 0)
            {
                ((PlayerShip)EntityManager.get().playerShips[0]).DrawDebug(spriteBatch, debugFont);
            }
            spriteBatch.End();

            hud.Render(graphics);

            base.Draw(gameTime);
        }
    }
}
