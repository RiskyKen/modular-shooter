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
using RiskyKen.ModularShooter.Game.Render;

namespace RiskyKen.ModularShooter.Game
{
    class Gun
    {
        private GameCore game;
        
        private Sprite sprite;
        private int lastFireTick;
        private int fireRate;
        public int x { get; private set; }
        public int y { get; private set; }

        public Gun(GameCore gameRef)
        {
            this.game = gameRef;
            sprite = game.resources.LoadSprite("resources/gfx/bullet.png");
            fireRate = 150;
        }

        public Gun(GameCore gameRef, int x, int y)
        {
            this.game = gameRef;
            sprite = game.resources.LoadSprite("resources/gfx/bullet.png");
            fireRate = 150;
            this.x = x;
            this.y = y;
        }

        public void Render()
        {
            //sprite.Render(x,y);
        }

        public void Shoot(int x, int y, float angle, float speed, bool playerIsOwner)
        {
            if (lastFireTick + fireRate > Environment.TickCount)
            { return; }
            lastFireTick = Environment.TickCount;
            SpawnBullet(x + this.x, y + this.y, angle, speed, playerIsOwner);
        }

        public void RapidShoot(int x, int y, float angle, float speed, bool playerIsOwner)
        {
            if (lastFireTick + fireRate > Environment.TickCount)
            { return; }
            SpawnBullet(x + this.x, y + this.y, angle, speed, playerIsOwner);
        }

        private void SpawnBullet(int x, int y, float angle, float speed, bool playerIsOwner)
        {
            Bullet bullet = new Bullet(sprite, playerIsOwner);
            bullet.position = new PointF(x - bullet.sprite.imageRec.Width / 2, y - bullet.sprite.imageRec.Height / 2);
            bullet.angle = angle;
            bullet.speed = speed;
            game.level.AddBullet(bullet);
        }

        public void EndRapidShoot()
        {
            lastFireTick = Environment.TickCount;
        }

        public bool OnCoolDown()
        {
            return lastFireTick + 150 > Environment.TickCount;
        }

        public void Update()
        {

        }
    }
}
