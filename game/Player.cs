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
using OpenTK.Input;
using RiskyKen.ModularShooter.Game.Ship;

namespace RiskyKen.ModularShooter.Game
{
    class Player : IPilot
    {
        private GameCore game = null;
        public SpaceShip ship;


        public Player(GameCore gameRef)
        {
            this.game = gameRef;
            ship = new SpaceShip(game, "resources/ships/player.ship");
            //ship.point = new Point(0,0);
            ship.point = new System.Drawing.Point((GameCore.screenSize.Width / 2) - (ship.sprite.imageRec.Width / 2), GameCore.screenSize.Height - ship.sprite.imageRec.Height);
        }

        public void Update()
        {
            KeyboardState kb = Keyboard.GetState();
            if (kb.IsKeyDown(Key.Down)) { ship.point.Y += 2; }
            if (kb.IsKeyDown(Key.Up)) { ship.point.Y -= 2; }
            if (kb.IsKeyDown(Key.Right)) { ship.point.X += 2; }
            if (kb.IsKeyDown(Key.Left)) { ship.point.X -= 2; }

            if (ship.point.Y < 0) { ship.point.Y = 0; }
            if (ship.point.X < 0) { ship.point.X = 0; }
            if (ship.point.Y > GameCore.screenSize.Height - ship.sprite.imageRec.Height)
            { ship.point.Y = GameCore.screenSize.Height - ship.sprite.imageRec.Height; }
            if (ship.point.X > GameCore.screenSize.Width - ship.sprite.imageRec.Width)
            { ship.point.X = GameCore.screenSize.Width - ship.sprite.imageRec.Width; }

            if (kb.IsKeyDown(Key.X)) { Shoot(kb.IsKeyDown(Key.ControlLeft)); }

            ship.Update();
        }

        private void Shoot(bool altFire)
        {
            ship.Shoot(altFire, 90f, true);
        }

        public void Render()
        {
            ship.Render();
        }

        public void Dispose()
        {

        }

        int IPilot.ShipHealth()
        {
            throw new NotImplementedException();
        }
    }
}
