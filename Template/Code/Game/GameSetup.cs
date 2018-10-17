using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
//--engine import
using Engine7;
using Template.Title;

namespace Template.Game
{
    class GameSetup : BasicSetup
    {
        float gameTime =0;
        public GameSetup() : base(false)
        {
            GM.engineM.DebugDisplay = Debug.fps | Debug.version;
            GM.engineM.ScreenColour = Color.Red;
           

            new Level(1);

            
        }

        public override void Logic()
        {
            //display code
            GM.textM.Draw(FontBank.gradius, gameTime.ToString(), GM.screenSize.Center.X, 30, TextAtt.Top);
            gameTime += GM.eventM.Delta;

            if (GM.inputM.KeyPressed(Keys.Escape))
            {
                BackToTitle();
                

            }
        }


        private static void BackToTitle()
        {
            GM.ClearAllManagedObjects();
            GM.active = new TitleSetup();
        }
    }
}
