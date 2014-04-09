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

using System.Collections.Generic;
using System.Drawing;
using OpenTK;
using RiskyKen.ModularShooter.Game.Render;
using RiskyKen.ModularShooter.Game.Ship;

namespace RiskyKen.ModularShooter.Game
{
    class GameCore
    {
        private OpenGL openGl;
        public static readonly Size screenSize = new Size(800, 600);

        public List<ShipPart> shipParts;

        public Resources resources;
        public Player player;
        public Stars stars;
        public Level level;
        public GameState gameState { get; private set; }

        public GameCore()
        {
            gameState = GameState.FIRST_LOAD;
            openGl = new OpenGL(screenSize, "Space Warp", false);
            openGl.OnLoaded += new OnLoadedHandler(OnLoaded);
            openGl.OnRender +=new OnRenderHandler(OnRender);
            openGl.OnUpdate +=new OnUpdateHandler(OnUpdate);
        }

        private void OnLoaded()
        {
            resources = new Resources();
            player = new Player(this);
            stars = new Stars(this);
            level = new Level(this, "resources/levels/level-1.lvl");
            gameState = GameState.LEVEL;
        }

        public void Dispose()
        {
            stars.Dispose();
            player.Dispose();
            resources.Dispose();
            openGl.Dispose();
        }

        void OnRender(object sender, FrameEventArgs e)
        {
            switch (gameState)
            {
                case GameState.LEVEL:
                    stars.Render();
                    level.Render();
                    player.Render();
                    break;
            }
        }

        void OnUpdate(object sender, FrameEventArgs e)
        {
            switch (gameState)
            {
                case GameState.LEVEL:
                    stars.Update();
                    level.Update();
                    player.Update();
                    break;
            }
        }

        public void Run()
        {
            openGl.Run();
        }
    }
}
