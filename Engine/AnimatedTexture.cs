#region File Description

//-----------------------------------------------------------------------------
// AnimatedTexture.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------

#endregion

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Engine
{
    internal class AnimatedTexture : Engine.Object
    {
        private const int Width = 64;
        private const int Height = 64;

        private int framecount;
        private Texture2D myTexture;
        private float TimePerFrame;
        private int Frame;
        private float TotalElapsed;
        private bool Paused;

        public float Rotation, Scale, Depth;
        public Vector2 Origin;
        private Vector2 position;

        public event Action Finish = delegate { };

        //------------------------------------------------------------------
        public AnimatedTexture (Object root, Vector2 position, float scale, float depth) : base (root)
        {
            this.position = position;
            this.Scale = scale;
            this.Depth = depth;
        }

        //------------------------------------------------------------------
        public void Load (Texture2D texture,
            int frameCount, int framesPerSec)
        {
            framecount = frameCount;
            myTexture = texture;
            TimePerFrame = (float) 1 / framesPerSec;
            Frame = 0;
            TotalElapsed = 0;
            Paused = false;

            Origin = new Vector2 (Width / 2, Height / 2);
        }

        //------------------------------------------------------------------
        public override void Update (float elapsed)
        {
            base.Update (elapsed);

            UpdateFrame (elapsed);
        }

        //------------------------------------------------------------------
        public void UpdateFrame (float elapsed)
        {
            if (Paused)
                return;
            TotalElapsed += elapsed;
            if (TotalElapsed > TimePerFrame)
            {
                Frame++;
                // Keep the Frame between 0 and the total frames, minus one.
                //Frame = Frame % framecount;
                TotalElapsed -= TimePerFrame;
            }

            if (Frame > framecount)
                Finish ();
        }

        //------------------------------------------------------------------
        public override void Draw (SpriteBatch batch)
        {
            base.Draw (batch);

            DrawFrame (batch, Position);
        }

        //------------------------------------------------------------------
        public void DrawFrame (SpriteBatch batch, Vector2 screenPos)
        {
            DrawFrame (batch, Frame, screenPos);
        }

        //------------------------------------------------------------------
        public void DrawFrame (SpriteBatch batch, int frame, Vector2 screenPos)
        {
            const int spritesInRow = 8;

            int x = Width * (frame % spritesInRow);
            int y = Height * (frame / spritesInRow);
            Rectangle sourcerect = new Rectangle (x, y, Width, Height);

            batch.Draw (myTexture, Position, sourcerect, Color.White, Rotation, Origin, Scale, SpriteEffects.None, Depth);
        }

        //------------------------------------------------------------------
        public bool IsPaused
        {
            get { return Paused; }
        }

        //------------------------------------------------------------------
        public void Reset ()
        {
            Frame = 0;
            TotalElapsed = 0f;
        }

        //------------------------------------------------------------------
        public void Stop ()
        {
            Pause ();
            Reset ();
        }

        //------------------------------------------------------------------
        public void Play ()
        {
            Paused = false;
        }

        //------------------------------------------------------------------
        public void Pause ()
        {
            Paused = true;
        }
    }
}