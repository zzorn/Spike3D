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
        SpriteBatch spriteBatch;


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
            // TODO: A list of levels or something.
            new Level1().populateLevel();

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

            PlayerShip ship = new PlayerShip();
            ship.model = Content.Load<Model>("media\\models\\alpha_ship\\alpha_ship");
            EntityManager.get().playerShips.Add(ship);

            Random rand = new Random();
            for (int i = 0; i < 1000; i++)
            {
                PlayerShip ship2 = new PlayerShip();
                ship2.model = ship.model;
                ship2.position = new Vector3(rand.Next(5000) - 2500, rand.Next(5000) - 2500, rand.Next(5000) - 2500);
                EntityManager.get().playerShips.Add(ship2);
            }

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

            EntityManager.get().updateEntities(gameTime);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Magenta);

            GraphicsDevice.RenderState.DepthBufferEnable = true;
            GraphicsDevice.RenderState.DepthBufferWriteEnable = true;

            EntityManager.get().renderEntities(graphics);

            Matrix cameraViewMatrix = Matrix.CreateLookAt(Vector3.Zero, new Vector3(-1.0f, -1.0f, 1.0f), Vector3.Up);
            Matrix cameraProjectionMatrix = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45.0f), graphics.GraphicsDevice.Viewport.AspectRatio, 1.0f, 10000.0f);
            
            foreach (ModelMesh mesh in EntityManager.get().playerShips[0].model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();
                    effect.PreferPerPixelLighting = true;
                    effect.World = Matrix.CreateFromYawPitchRoll(0, 0, 0) * Matrix.CreateScale(1.0f) * Matrix.CreateTranslation(new Vector3(-200, -200, 200));
                    effect.Projection = cameraProjectionMatrix;
                    effect.View = cameraViewMatrix;
                }

                mesh.Draw();
            }

            base.Draw(gameTime);
        }
    }
}
