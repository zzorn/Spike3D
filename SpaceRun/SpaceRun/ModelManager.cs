using System;
using System.Collections.Generic;
using System.Collections;
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
    public enum ModelType
    {
        ASTEROID,
        ASTEROID2,
        PLAYER_SHIP,
        ENEMY_FIGHTER,
        PLANET,
    }

    public class ModelManager
    {
        protected Dictionary<ModelType, Model> models { get; set; }

        private static ModelManager instance;

        public static ModelManager get()
        {
            if (instance == null)
            {
                instance = new ModelManager();
            }
            return instance;
        }

        public Model getModel(ModelType modelType)
        {
            return models[modelType];
        }

        public void loadModels(ContentManager Content)
        {
            models[ModelType.ASTEROID] = Content.Load<Model>("media\\models\\asteroids\\asteroid1");
            models[ModelType.ASTEROID2] = Content.Load<Model>("media\\models\\asteroids\\asteroid2");
            models[ModelType.PLAYER_SHIP] = Content.Load<Model>("media\\models\\alpha_ship\\alpha_ship");
            models[ModelType.ENEMY_FIGHTER] = Content.Load<Model>("media\\models\\alpha_ship\\alpha_ship");
            models[ModelType.PLANET] = Content.Load<Model>("media\\models\\alpha_ship\\alpha_ship");
        }
        
        private ModelManager()
        {
            models = new Dictionary<ModelType, Model>();
        }

    }
}