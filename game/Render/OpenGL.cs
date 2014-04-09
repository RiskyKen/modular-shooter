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
using System.Drawing;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace RiskyKen.ModularShooter.Game.Render
{
    public delegate void OnLoadedHandler();
    public delegate void OnRenderHandler(object sender, FrameEventArgs e);
    public delegate void OnUpdateHandler(object sender, FrameEventArgs e);

    class OpenGL
    {
        private GameWindow gw;
        private SpriteText loadingText;
        private SpriteText frameRateText;

        public event OnLoadedHandler OnLoaded;
        public event OnRenderHandler OnRender;
        public event OnUpdateHandler OnUpdate;

        public OpenGL(Size size, string title, bool fullscreen)
        {
            gw = new GameWindow(size.Width, size.Height, OpenTK.Graphics.GraphicsMode.Default, "Space Warp", GameWindowFlags.Default);
            gw.WindowBorder = WindowBorder.Fixed;
            gw.Icon = new Icon("icon.ico");
            gw.Resize += new EventHandler<EventArgs>(Resize);
            gw.UpdateFrame += new EventHandler<FrameEventArgs>(UpdateFrame);
            gw.RenderFrame += new EventHandler<FrameEventArgs>(RenderFrame);
            gw.Load += new EventHandler<EventArgs>(Load);
        }

        public void Dispose()
        {
            loadingText.Dispose();
            frameRateText.Dispose();

            gw.Resize -= Resize;
            gw.RenderFrame -= RenderFrame;
            gw.UpdateFrame -= UpdateFrame;
            gw.Load -= Load;
            gw.Dispose();
        }

        void Load(object sender, EventArgs e)
        {
            gw.VSync = VSyncMode.Off;
            gw.TargetUpdatePeriod = 0.005f;
            //gw.TargetUpdatePeriod = 0.01f;
            SetupViewport();

            loadingText = new SpriteText("Loading...", new Rectangle(5, 5, 128, 16),
                new Font("Vardana", 12, GraphicsUnit.Pixel), Color.White, false);
            frameRateText = new SpriteText("FPS: 0", new Rectangle(GameCore.screenSize.Width - 60, 5, 128, 16),
                new Font("Vardana", 10, GraphicsUnit.Pixel), Color.White, true);
            //GL.Hint(HintTarget.PerspectiveCorrectionHint, HintMode.Fastest);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            loadingText.Render();
            gw.SwapBuffers();

            //Thread.Sleep(200);
            if (OnLoaded != null) { OnLoaded.Invoke(); }
        }

        public void Run()
        {
            gw.Run();
        }

        private void SetupViewport()
        {
            int w = gw.ClientRectangle.Width;
            int h = gw.ClientRectangle.Height;
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            GL.Ortho(0, w, h, 0, -1, 1); // Bottom-left corner pixel has coordinate (0, 0)
            GL.Viewport(0, 0, w, h); // Use all of the glControl painting area
            GL.Disable(EnableCap.DepthTest); //Disable Z-Buffer, 2D Rendering
            GL.Disable(EnableCap.CullFace);
            GL.Disable(EnableCap.PointSmooth);
            GL.Disable(EnableCap.DepthTest);
            GL.Disable(EnableCap.CullFace);
        }

        void Resize(object sender, EventArgs e)
        {
            SetupViewport();
        }

        void RenderFrame(object sender, FrameEventArgs e)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            if (OnRender != null) { OnRender.Invoke(sender, e); }

            frameRateText.Render();
            gw.SwapBuffers();
            UpdateFrameRate();
        }

        void UpdateFrame(object sender, FrameEventArgs e)
        {
            if (OnUpdate != null) { OnUpdate.Invoke(sender, e); }
        }

        int frameCount;
        int lastTick;
        int fps;
        public void UpdateFrameRate()
        {
            frameCount += 1;
            if (lastTick + 1000 <= Environment.TickCount)
            {
                fps = frameCount;
                frameCount = 0;
                lastTick = Environment.TickCount;
                frameRateText.UpdateText("FPS:" + fps.ToString());
            }
        }
    }
}
