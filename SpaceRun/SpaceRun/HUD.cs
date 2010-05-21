using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    class HUD
    {
        public PlayerShip playerShip { get; set; }

        private Texture2D gaugeBg;
        private Texture2D gaugeFg;
        private Texture2D gaugeIndicator1;
        private Texture2D gaugeIndicator2;
        private Texture2D screenBlack;
        private Texture2D screenFrame;
        private Texture2D screenTessa;

        private Random random = new Random();

        public void LoadContent(ContentManager content)
        {
            gaugeBg = content.Load<Texture2D>("media\\textures\\ui-gauge-background");
            gaugeFg = content.Load<Texture2D>("media\\textures\\ui-gauge-frame");
            gaugeIndicator1 = content.Load<Texture2D>("media\\textures\\ui-gauge-indicator-1");
            gaugeIndicator2 = content.Load<Texture2D>("media\\textures\\ui-gauge-indicator-2");
            screenBlack = content.Load<Texture2D>("media\\textures\\ui-screen-black");
            screenFrame = content.Load<Texture2D>("media\\textures\\ui-screen-frame");
            screenTessa = content.Load<Texture2D>("media\\textures\\video-tessa-preview");
            
        }

        public void Update(GameTime time) 
        {
        }

        public void Render(GraphicsDeviceManager graphics)
        {
            float hullPercent = playerShip.hull / playerShip.maxHull;
            float shieldPercent = playerShip.shield / playerShip.maxShield;
            float energyPercent = playerShip.energy / playerShip.maxEnergy;

            int w = graphics.GraphicsDevice.Viewport.Width;
            int h = graphics.GraphicsDevice.Viewport.Height;

            int screenH= h/4;
            int gaugeH = (h - screenH) / 2;
            int gaugeW = w / 10;
            int gx1 = 0;
            int gx2 = gaugeW;
            int gx3 = 2*gaugeW;
            int gy1 = screenH;
            int gy2 = screenH + gaugeH;

            SpriteBatch batch = new SpriteBatch(graphics.GraphicsDevice);
            batch.Begin();
            drawGauge(batch, gx1, gy1, gaugeW, gaugeH, hullPercent, Color.Red);
            drawGauge(batch, gx1, gy2, gaugeW, gaugeH, shieldPercent, Color.Blue);
            batch.End();
        }

        private void drawGauge(SpriteBatch batch, int x, int y, int w, int h, float percent, Color color)
        {
            batch.Draw(gaugeBg, new Rectangle(x, y, w, h), Color.Gray);

            Texture2D indicatorTxt = (random.NextDouble() < 0.5) ? gaugeIndicator1 : gaugeIndicator2;
            int indY = (int)(y+h - percent * h);
            int indH = (int)(h * percent);
            batch.Draw(indicatorTxt, new Rectangle(x, indY, w, indH), color);

            batch.Draw(gaugeFg, new Rectangle(x, y, w, h), Color.Gray);
        }
    }
}
