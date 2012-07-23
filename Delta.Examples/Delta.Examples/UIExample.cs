﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Delta.UI;
using Delta.UI.Controls;

namespace Delta.Examples
{

    public class UIExample : ExampleBase
    {
        public UIExample() : base("UIExample")
        {
            Panel pnl = new Panel();
            pnl.Position = new Point(50, 50);
            pnl.Size = new Point(40, 100);
            Button btn = new Button();
            btn.AutoSize = true;
            btn.Text.Append("Hi, I'm a button!");
            pnl.Add(btn);
            G.UI.HUD.Add(pnl);
        }

        protected override void Initialize()
        {

            base.Initialize();
        }

        protected override void LoadContent()
        {

            base.LoadContent();
        }

        protected override void Update(GameTime gameTime)
        {

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            base.Draw(gameTime);
        }
    }
}
