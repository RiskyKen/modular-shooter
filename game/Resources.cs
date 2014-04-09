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
using RiskyKen.ModularShooter.Game.Render;

namespace RiskyKen.ModularShooter.Game
{
    class Resources
    {
        private List<Sprite> sprites;

        public Resources()
        {
            sprites = new List<Sprite>();
        }

        public Sprite LoadSprite(string filePath)
        {
            Sprite sprite = GetSprite(filePath);
            if (sprite != null)
            {
                sprite.loadCount += 1;
                return sprite;
            }
            else
            {
                sprite = new Sprite(filePath);
                sprites.Add(sprite);
                return sprite;
            }
        }

        public void UnloadSprite(string filePath)
        {
            for (int i = 0; i < sprites.Count; i++)
            {
                if (sprites[i].filePath == filePath)
                {
                    sprites[i].loadCount -= 1;
                    if (sprites[i].loadCount <= 0)
                    {
                        sprites[i].Dispose();
                        sprites.RemoveAt(i);
                        return;
                    }
                }
            }
        }

        private Sprite GetSprite(string filePath)
        {
            foreach (Sprite sprite in sprites)
            {
                if (sprite.filePath == filePath)
                { return sprite; }
            }
            return null;
        }

        public void Dispose()
        {
            foreach (Sprite sprite in sprites)
            {
                sprite.Dispose();
            }
            sprites.Clear();
        }
    }
}
