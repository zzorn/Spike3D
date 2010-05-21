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
    public class ParticleSource
    {
        public List<Texture2D> sprites;
        public float particleSpread;
        public Boolean relativePosition;
        public float particleLifeTime;
        public Color startColor = Color.White;
        public Color endColor = Color.White;
        public float rot = 0;
        public float alpha = 1f;
        public float size = 1;


        public Particle CreateParticle(GameTime time, float t, ParticleSystem particleSystem)
        {
            Random r = new Random();

            Particle p = new Particle();
            p.sprite = sprites[r.Next(sprites.Count)];
            p.pos = new Vector3(randomFloat(r, particleSpread), randomFloat(r, particleSpread), randomFloat(r, particleSpread));
            p.maxAge = particleLifeTime;
            p.maxDistance = particleSpread;
            p.angle = randomFloat(r, 180);
            p.normalAlpha = alpha;
            p.rot = rot;
            p.relativePosition = relativePosition;

            if (!relativePosition)
            {
                p.pos += particleSystem.position;
            }

            return p;
        }

        private float randomFloat(Random r, float a)
        {
            return (float)(a * (r.NextDouble() * 2 - 1));
        }
    }

    public class Particle
    {
        public Texture2D sprite = null;
        public Vector3 pos = Vector3.Zero;
        public Vector3 vel = Vector3.Zero;
        public Vector3 acc = Vector3.Zero;
        public float angle = 0;
        public float rot = 0;
        public float age = 0;
        public float maxAge = 100;
        public Boolean relativePosition = true;
        public float maxDistance = 100;
        public Boolean dead = false;
        public Color color = Color.White;
        public Color startColor = Color.White;
        public Color endColor = Color.White;
        public float normalAlpha = 1f;
        public float alpha = 1f;
        public Boolean dying;
        public float dyingAge = 0;
        public float dyingTime = 5;
        public float size = 1;

        private Quad quad = null;
        private VertexDeclaration quadVertexDeclaration;

        public void Update(float t, ParticleSystem system)
        {
            vel += acc * t;
            pos += vel * t;
            angle += rot * t;
            age += t;

            float progress = age / maxAge;

            color = Color.Lerp(startColor, endColor, progress);

            // Kill if too old
            if (age >= maxAge) dying = true;

            // Kill if too far
            float distance2 = relativePosition ? pos.LengthSquared() : (system.position - pos).LengthSquared();
            if (distance2 > (maxDistance * maxDistance)) dying = true;

            alpha = normalAlpha;
            if (dying && dyingTime > 0)
            {
                dyingAge += t;
                alpha = MathHelper.Lerp(normalAlpha, 0, dyingAge/dyingTime);
            }

            if (dying && dyingAge > dyingTime) dead = true;
        }

        public Boolean IsDead() { return dead;  }

        public void Render(GraphicsDeviceManager graphics, Matrix cameraViewMatrix, Matrix cameraProjectionMatrix, ParticleSystem system, Entity camera)
        {
            // TODO: Fix.

            Vector3 ps = new Vector3(pos.X, pos.Y, pos.Z);
            if (relativePosition)
            {
                ps += system.position;
            }

            Matrix billboardMatrix = Matrix.CreateBillboard(pos, camera.position, camera.getUpVector(), camera.getForwardVector());

            if (quad == null)
            {
            }
            quad = new Quad(ps, Vector3.Backward, Vector3.Up, size, size);
            quadVertexDeclaration = new VertexDeclaration(graphics.GraphicsDevice, VertexPositionNormalTexture.VertexElements);

            foreach (VertexPositionNormalTexture v in quad.Vertices)
            {
                Vector3.Transform(v.Position, billboardMatrix);
            }

            graphics.GraphicsDevice.VertexDeclaration = quadVertexDeclaration;
            graphics.GraphicsDevice.DrawUserIndexedPrimitives
                <VertexPositionNormalTexture>(
                PrimitiveType.TriangleList,
                quad.Vertices, 0, 4,
                quad.Indexes, 0, 2);
        }
    }



    public class ParticleSystem : Entity
    {
        public ParticleSystem()
        {
        }

        public ParticleSystem(Entity trackedEntity, List<Texture2D> sprites, float spread, float size, int number, Boolean relativePosition, float lifetime)
        {
            this.trackedEntity = trackedEntity;
            this.maxNumberOfParticles = number;

            ParticleSource source = new ParticleSource();
            source.sprites = sprites;
            source.particleSpread = spread;
            source.size = size;
            source.relativePosition = relativePosition;
            source.particleLifeTime = lifetime;
            addParticleSource(source);
        }

        public Entity trackedEntity = null;

        public int maxNumberOfParticles = 100;

        private List<ParticleSource> particleSources = new List<ParticleSource>();
        private List<Particle> particles = new List<Particle>();


        public void addParticleSource(ParticleSource source)
        {
            particleSources.Add(source);
        }

        public override void LogicUpdate(GameTime time, float t)
        {
            // Track entity
            if (trackedEntity != null)
            {
                this.position = trackedEntity.position;
                this.heading = trackedEntity.heading;
            }

            // Add new
            if (particles.Count < maxNumberOfParticles)
            {
                foreach (ParticleSource source in particleSources)
                {
                    Particle p = source.CreateParticle(time, t, this);
                    if (p != null) particles.Add(p);
                }
            }

            // Update
            foreach (Particle p in particles)
            {
                p.Update(t, this);
            }

            // Cull dead
            particles.RemoveAll(p => p.IsDead());
        }

        public override void Render(GraphicsDeviceManager graphics, Matrix cameraViewMatrix, Matrix cameraProjectionMatrix)
        {
            Entity camera = EntityManager.get().cameraEntity;

            SpriteBatch spriteBatch = new SpriteBatch(graphics.GraphicsDevice);
            spriteBatch.Begin();

            foreach (Particle p in particles)
            {
                p.Render(graphics, cameraViewMatrix, cameraProjectionMatrix, this, camera);
            }

            spriteBatch.End();
        }

    }
}
