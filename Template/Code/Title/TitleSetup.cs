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
//--engine import
using Engine7;
using Template.Game;
/*
step 1 add title graphic
step 2 add press start graphic
step 3 add showscores
step 4 add start logic and method
*/
namespace Template.Title
{
    /// <summary>
    /// generates graphs for display
    /// </summary>
    public class TitleSetup : BasicSetup
    {
        /// <summary>
        /// constructor
        /// </summary>
        public TitleSetup() : base(true)
        {
            GM.engineM.DebugDisplay = Debug.version;
            GM.engineM.ScreenColour = new Color(GM.r.Next(255), GM.r.Next(255), GM.r.Next(255) );
            GM.textM.DrawAsSprites(FontBank.arcadeLarge, "Press space to begin", GM.screenSize.Center.X, GM.screenSize.Center.Y, TextAtt.Centred,0, null);
            

        }

        /// <summary>
        /// 
        /// </summary>
        public override void Logic()
        {
            if (GM.inputM.KeyPressed(Keys.Space))
            {
                StartGame();
            }
            if (GM.inputM.KeyPressed(Keys.D2))
            {
                GM.ClearAllManagedObjects();
                GM.CloseSystem();
            }

        }

        private static void StartGame()
        {
            //tidy up before moving to another mode
            GM.ClearAllManagedObjects();
            GM.active = new GameSetup();
        }
    }
}
