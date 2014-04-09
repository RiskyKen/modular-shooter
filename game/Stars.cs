#region "License"
//Modular Shooter - Modular 2D shooter game.
//Copyright (C) 2014 RiskyKen

//This program is free software; you can redistribute it and/or modify
//it under the terms of the GNU General Public License as published by
//the Free Software Foundation; either version 2 of the License, or
//(at your option) any later version.

//This program is distributed in the hope that it will be useful,
//but WITHOUT ANY WARRANTY; without even the implied warranty of
//MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
//GNU General Public License for more details.

//You should have received a copy of the GNU General Public License along
//with this program; if not, write to the Free Software Foundation, Inc.,
//51 Franklin Street, Fifth Floor, Boston, MA 02110-1301 USA.
#endregion

using System;
using OpenTK.Graphics.OpenGL;

namespace RiskyKen.ModularShooter.Game
{
    class Stars
    {
        private Star[] gameStars = null;
        private GameCore game = null;

        public Stars(GameCore game)
        {
            this.game = game;
            GenerateStars(100);
        }

        public void GenerateStars(int numberOfStars)
        {
            Random rnd = new Random();
            gameStars = new Star[numberOfStars];

            for (int i = 0; i <= gameStars.GetUpperBound(0); i++)
            {
                gameStars[i].x = rnd.Next(GameCore.screenSize.Width);
                gameStars[i].y = rnd.Next(GameCore.screenSize.Height);
                gameStars[i].xV = 0;
                float num = (rnd.Next(8) + 1) + 2;
                gameStars[i].yV += num / 5;
            }
        }

        public void Dispose()
        { }

        public void Update()
        {
            if (gameStars == null) { return; }
            for (int i = 0; i <= gameStars.GetUpperBound(0); i++)
            {
                gameStars[i].x += gameStars[i].xV;
                gameStars[i].y += gameStars[i].yV;
                if (gameStars[i].x < 0) { gameStars[i].x += GameCore.screenSize.Width; }
                if (gameStars[i].y < 0) { gameStars[i].y += GameCore.screenSize.Height; }
                if (gameStars[i].y > GameCore.screenSize.Height) { gameStars[i].y -= GameCore.screenSize.Height; }
            }
        }

        public void Render()
        {
            if (gameStars == null) { return; }
            GL.PointSize(2f);
            GL.Begin(PrimitiveType.Points);
            foreach (Star star in gameStars)
            {
                GL.Vertex2(star.x, star.y);
            }
            GL.End();
        }

        private struct Star
        {
            public float x;
            public float y;
            public float xV;
            public float yV;
        }
    }
}
