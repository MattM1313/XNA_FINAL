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

namespace Particles
{
   public class ParticleEngine
    {
        private Random random;
        public Vector2 EmitterLocation { get; set; }
        private List<Particle> particles;
        private List<Texture2D> textures;
 
        public ParticleEngine(List<Texture2D> textures, Vector2 location)
        {
            EmitterLocation = location;
            this.textures = textures;
            this.particles = new List<Particle>();
            random = new Random();
        }
 
        public void Update()
        {
            int total = 50;
 
            for (int i = 0; i < total; i++)
            {
                particles.Add(GenerateNewParticle());
            }
 
            for (int particle = 0; particle < particles.Count; particle++)
            {
                particles[particle].Update();
                if (particles[particle].LifeSpan <= 0)
                {
                    particles.RemoveAt(particle);
                    particle--;
                }
                else if (particles[particle].Position.X < 0 && particles[particle].Position.X > 800 &&
                    particles[particle].Position.Y < 0 && particles[particle].Position.X > 600)
                {
                    particles.RemoveAt(particle);
                    particle--;
                }
                
            }
        }
 
        private Particle GenerateNewParticle()
        {
            Texture2D texture = textures[random.Next(textures.Count)];
            Vector2 position = EmitterLocation;
            Vector2 velocity = new Vector2(
                                    8f * (float)(random.NextDouble() * 2 - 1),
                                    8f * (float)(random.NextDouble() * 2 - 1));
            float angle = 0;
            float angularVelocity = 0.1f * (float)(random.NextDouble() * 2 - 1);
            int c = random.Next(3);
            Color color = new Color();
            switch(c){

                case 0:
            color = new Color(255,255,255);
            break;

                case 1:
            color = new Color(233, 150, 122);
            break;

                case 2:
            color = new Color(135, 206, 250);
             break;
            }


            float size = (float)random.NextDouble();
            int ttl = 100 + random.Next(40);
 
            return new Particle(texture, position, velocity, angle, angularVelocity, color, size, ttl);
        }
 
        public void Draw(SpriteBatch spriteBatch)
        {
          
            for (int index = 0; index < particles.Count; index++)
            {
                if (particles[index].Position.X >= 0 || particles[index].Position.X <= 800)
                {
                    if (particles[index].Position.Y >= 0 || particles[index].Position.X <= 600)
                    {

                        particles[index].Draw(spriteBatch);
                    }
                }
            
            
            
            }
            
        }
    }
}

