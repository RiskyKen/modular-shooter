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
using OpenTK.Graphics.OpenGL;
using System.Drawing.Imaging;

namespace RiskyKen.ModularShooter.Game.Render
{
    class SpriteText
    {
        public int texture;
        private RectangleF spriteRec;
        private string text;
        private Font font;
        private Color colour;
        private bool shadow;

        public SpriteText(string text, RectangleF rectangle, Font font, Color colour, bool shadow)
        {
            spriteRec = rectangle;
            this.text = text;
            this.font = font;
            this.colour = colour;
            this.shadow = shadow;

            GL.GenTextures(1, out texture);
            GL.BindTexture(TextureTarget.Texture2D, texture);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);

            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, (int)spriteRec.Width, (int)spriteRec.Height, 0,
                OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, IntPtr.Zero);

            UpdateSprite();
        }

        public void Dispose()
        {
            GL.BindTexture(TextureTarget.Texture2D, 0);
            GL.DeleteTexture(texture);
        }

        public void UpdateText(string text)
        {
            if (this.text != text)
            {
                this.text = text;
                UpdateSprite();
            }
        }

        private void UpdateSprite()
        {
            Bitmap bitmap = new Bitmap((int)spriteRec.Width, (int)spriteRec.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            bitmap.SetResolution(96, 96);
            if (this.shadow)
            {
                Graphics.FromImage(bitmap).DrawString(text, font, new SolidBrush(Color.Gray),
                    new RectangleF(-1, -1, spriteRec.Width, spriteRec.Height));
            }
            Graphics.FromImage(bitmap).DrawString(text, font, new SolidBrush(colour), new RectangleF(new PointF(), spriteRec.Size));
            BitmapData data = bitmap.LockBits(new Rectangle(0, 0, (int)spriteRec.Width, (int)spriteRec.Height),
                ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, data.Width, data.Height, 0,
                OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);
            bitmap.UnlockBits(data);
            bitmap.Dispose();
        }

        public void Render(string text, Point pos)
        {
            //imageRec.Location = pos;
            spriteRec.Location = pos;
            Render();
        }

        public void Render(string text, int x, int y)
        {
            Render(text, new Point(x, y));
        }

        public void Render()
        {
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
            GL.Enable(EnableCap.Texture2D);
            GL.BindTexture(TextureTarget.Texture2D, texture);

            GL.Begin(PrimitiveType.TriangleStrip);

            GL.TexCoord2(0, 0);
            GL.Vertex2(spriteRec.X, spriteRec.Y);

            GL.TexCoord2(1, 0);
            GL.Vertex2(spriteRec.X + spriteRec.Width, spriteRec.Y);

            GL.TexCoord2(0, 1);
            GL.Vertex2(spriteRec.X, spriteRec.Y + spriteRec.Height);

            GL.TexCoord2(1, 1);
            GL.Vertex2(spriteRec.X + spriteRec.Width, spriteRec.Y + spriteRec.Height);

            GL.End();
            GL.Disable(EnableCap.Blend);
            GL.Disable(EnableCap.Texture2D);
        }
    }
}
