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
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;


namespace RiskyKen.ModularShooter.Game.Render
{
    class Sprite
    {

        public int texture;
        public Rectangle imageRec;
        private Rectangle spriteRec;
        public int loadCount;
        public string filePath;
        public byte[,] collisionData; 

        public Sprite(string filePath)
        {
            this.filePath = (string)filePath.Clone();

            string platformPath = Globals.MakePathPlatformSpecific(filePath);

            if (!File.Exists(platformPath))
            { throw new FileNotFoundException("File not found.", platformPath); }

            GL.GenTextures(1, out texture);
            GL.BindTexture(TextureTarget.Texture2D, texture);

            Bitmap bitmap1 = new Bitmap(platformPath);
            imageRec = new Rectangle(0, 0, bitmap1.Width, bitmap1.Height);

            LoadCollisionData(bitmap1);
            
            Bitmap bitmap2 = new Bitmap(Power(imageRec.Width), Power(imageRec.Height),
                System.Drawing.Imaging.PixelFormat.Format32bppPArgb);

            bitmap1.SetResolution(96, 96);
            Graphics.FromImage(bitmap2).DrawImageUnscaled(bitmap1, 0, 0);
            bitmap1.Dispose();

            spriteRec = new Rectangle(0, 0, bitmap2.Width, bitmap2.Height);

            BitmapData data = bitmap2.LockBits(new Rectangle(0, 0, spriteRec.Width, spriteRec.Height),
                ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, data.Width, data.Height, 0,
                OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);
            
            bitmap2.UnlockBits(data);
            bitmap2.Dispose();
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            loadCount = 1;
        }

        public static bool HasHit(Point point1, Point point2, Sprite sprite1, Sprite sprite2)
        {
            Rectangle rec1 = new Rectangle(point1, sprite1.imageRec.Size);
            Rectangle rec2 = new Rectangle(point2, sprite2.imageRec.Size);

            if (rec1.IntersectsWith(rec2))
            {
                return CheckCollisionData(rec1, rec2, sprite1, sprite2);
                //return true;
            }

            return false;
        }

        private static bool CheckCollisionData(Rectangle rec1, Rectangle rec2, Sprite sprite1, Sprite sprite2)
        {
            Rectangle inter = rec1;
            inter.Intersect(rec2);

            for (int ix = inter.X; ix < inter.X + inter.Width; ix++)
            {
                for (int iy = inter.Y; iy < inter.Y + inter.Height; iy++)
                {
                    if (sprite1.collisionData[ix - rec1.X, iy - rec1.Y] >= 96)
                    {
                        if (sprite2.collisionData[ix - rec2.X, iy - rec2.Y] >= 150)
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        public void Dispose()
        {
            GL.BindTexture(TextureTarget.Texture2D, 0);
            GL.DeleteTexture(texture);
        }

        private void LoadCollisionData(Bitmap bitmap)
        {
            collisionData = new byte[bitmap.Width, bitmap.Height];
            for (int ix = 0; ix < bitmap.Width; ix++)
            {
                for (int iy = 0; iy < bitmap.Height; iy++)
                {
                    collisionData[ix, iy] = bitmap.GetPixel(ix, iy).A;
                }
            }
        }

        private static int Power(int value)
        {
            int v = value; // compute the next highest power of 2 of 32-bit v

            v--;
            v |= v >> 1;
            v |= v >> 2;
            v |= v >> 4;
            v |= v >> 8;
            v |= v >> 16;
            v |= v >> 32;
            v |= v >> 64;
            v |= v >> 128;
            v++;
            return v;
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

        public void Render(Point pos)
        {
            imageRec.Location = pos;
            spriteRec.Location = pos;
            Render();
        }

        public void Render(int x, int y)
        {
            Render(new Point(x, y));
        }
    }
}
