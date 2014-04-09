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
using RiskyKen.ModularShooter.Game.Render;

namespace RiskyKen.ModularShooter.Game.Ship
{
    class SpaceShip
    {
        private GameCore game;
        public string name;
        public Health health;
        public Gun[] guns;
        public Sprite sprite;
        public Point point;

        public SpaceShip(GameCore gameRef, Sprite sprite)
        {
            this.game = gameRef;
            this.sprite = sprite;
            health = new Health(10);
            guns = new Gun[1];
            guns[0] = new Gun(game,77,27);
        }

        public SpaceShip(GameCore gameRef, string filePath)
        {
            this.game = gameRef;
            Load(filePath);
        }

        public void Load(string filePath)
        {
            string platformPath = Globals.MakePathPlatformSpecific(filePath);
            if (!File.Exists(platformPath))
            { throw new FileNotFoundException("File not found.", platformPath); }

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
                            name = split[1];
                            break;
                        case "image":
                            sprite = game.resources.LoadSprite(split[1]);
                            break;
                        case "health":
                            health = new Health(int.Parse(split[1]));
                            break;
                        case "guntype":
                            ReadGun(split[1]);
                            break;
                    }
                }
            }
            sr.Close();
            sr.Dispose();
        }

        private void ReadGun(string gunInfo)
        {
            string[] line = gunInfo.Split(Convert.ToChar("|"));
            string gunType = line[0];
            int gunX = int.Parse(line[1]);
            int gunY = int.Parse(line[2]);

            if (guns == null) { guns = new Gun[1]; }
            else { Array.Resize(ref guns, guns.GetUpperBound(0) + 2); }
            Gun newGun = new Gun(game, gunX, gunY);
            guns[guns.GetUpperBound(0)] = newGun;
        }

        public void Damage(int amount)
        {
            health.current -= amount;
            if (health.current < 0)
            {
                health.current = 0;
            }
        }

        public void Update()
        {
            if (guns != null)
            {
                foreach (Gun gun in guns)
                {
                    gun.Update();
                }
            }
        }

        public void Render()
        {
            sprite.Render(point);
            if (guns != null)
            {
                foreach (Gun gun in guns)
                {
                    gun.Render();
                }
            }
        }

        public void Shoot(bool altFire, float angle, bool playerIsOwner)
        {
            if (guns == null) { return; }
            if (guns[0].OnCoolDown()) { return; }
            if (altFire)
            {
                for (int i = -2; i <= 2; i++)
                {
                    guns[0].RapidShoot(point.X, point.Y, angle + (i * 3f), 4f, playerIsOwner);
                }
                guns[0].EndRapidShoot();
            }
            else
            {
                if (playerIsOwner)
                { guns[0].Shoot(point.X, point.Y, angle, 4f, playerIsOwner); }
                else
                { guns[0].Shoot(point.X, point.Y, angle, 4f, playerIsOwner); }
                
            }
        }

        public void Dispose()
        {
            sprite.Dispose();
        }

        public bool HasHit(Bullet bullet)
        {
            if (Sprite.HasHit(point, new Point((int)bullet.position.X, (int)bullet.position.Y), sprite, bullet.sprite))
            {
                return true;
            }
            return false;
        }

        public class Health
        {
            public int current;
            public int max;

            public Health(int max)
            {
                this.max = max;
                current = max;
            }
        }
    }
}
