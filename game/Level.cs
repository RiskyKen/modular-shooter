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
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.IO;

namespace RiskyKen.ModularShooter.Game
{
    class Level
    {
        public string Name { get; private set; }
        private GameCore game = null;
        public List<Npc> npcs = null;
        private List<Bullet> bullets = null;
        private List<string> waves = null;
        private bool levelLoaded = false;

        public Level(GameCore gameRef)
        {
            this.game = gameRef;
        }

        public Level(GameCore gameRef, string filePath) : this(gameRef)
        {
            LoadLevel(filePath);
        }

        public void LoadLevel(string filePath)
        {
            string platformPath = Globals.MakePathPlatformSpecific(filePath);
            if (!File.Exists(platformPath))
            { throw new FileNotFoundException("File not found.", platformPath); }

            UnloadLevel();

            bullets = new List<Bullet>();
            npcs = new List<Npc>();
            waves = new List<string>();

            StreamReader sr = new StreamReader(platformPath);

            while (!sr.EndOfStream)
            {
                string line = sr.ReadLine();
                if (line.Contains("="))
                {
                    string[] split = line.Split(Convert.ToChar("="));

                    switch (split[0])
                    {
                        case "name":
                            Name = split[1];
                            break;
                        case "star-count":
                            game.stars.GenerateStars(int.Parse(split[1]));
                            break;
                        case "wave":
                            waves.Add(split[1]);
                            break;
                    }
                }
            }
            sr.Close();
            sr.Dispose();

            levelLoaded = true;
        }

        private void UnloadLevel()
        {
            levelLoaded = false;
            if (npcs != null)
            {
                for (int i = 0; i < npcs.Count; i++)
                {
                    npcs[i].Dispose();
                }
                npcs.Clear();
                npcs = null;
            }
            if (bullets != null)
            {
                bullets.Clear();
                bullets = null;
            }
            if (waves != null)
            {
                waves.Clear();
                waves = null;
            }
        }

        public void Update()
        {
            if (!levelLoaded) { return; }
            for (int i = 0; i < npcs.Count; i++)
            { npcs[i].Update(); }
            for (int i = 0; i < bullets.Count; i++)
            {
                bullets[i].Update(this);

                if (bullets[i].dead)
                { bullets.RemoveAt(i); }
            }
        }

        public void Render()
        {
            if (!levelLoaded) { return; }
            for (int i = 0; i < npcs.Count; i++)
            { npcs[i].Render(); }
            for (int i = 0; i < bullets.Count; i++)
            { bullets[i].Render(); }
        }

        public void AddBullet(Bullet bullet)
        {
            bullets.Add(bullet);
        }


        public void Dispose()
        {
            UnloadLevel();
        }
    }
}
