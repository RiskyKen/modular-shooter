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
    class Bullet
    {
        public PointF position;
        public float angle;
        public float speed;
        public bool dead;
        public Sprite sprite;
        public bool playerIsOwner;

        public Bullet(Sprite sprite, bool playerIsOwner)
        {
            dead = false;
            this.sprite = sprite;
            this.playerIsOwner = playerIsOwner;
        }

        public void Update(Level level)
        {
            if (position.X > GameCore.screenSize.Width | position.X < -sprite.imageRec.Width)
            {
                dead = true;
                //return;
            }
            if (position.Y > GameCore.screenSize.Height | position.Y < -sprite.imageRec.Height)
            {
                dead = true;
               // return;
            }

            if (playerIsOwner)
            {
                //if (level.npc.ship.HasHit(this))
                //{ dead = true; }
            }
            else
            {
                if (ModularShooter.gameCore.player.ship.HasHit(this))
                { dead = true; }
            }


            position = Trig.MoveTo(position, speed, angle);
        }

        public void Render()
        {
            sprite.Render((int)position.X, (int)position.Y);
        }
    }
}
