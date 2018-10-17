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
    internal class Player : Sprite



    {
        

        private Level level;
        /// <summary>
        /// current level instance of the mazegen
        /// </summary>
        MazeGenerator mazeGen;
        //int[,] maze;



        /// <summary>
        /// rotation speed in degrees per second
        /// </summary>
        int rotationSpeed = 200;

        /// <summary>
        /// walk speed in pixels per second
        /// </summary>
        int walkSpeed = 60;
        private string direction;

        //private string direction;
        //private int tileHeightDiv2;
        //private int tileWidthDiv2;

        public Player(Level level)
        {
            this.level = level;
            //maze =  level.map;
            mazeGen = level.MazeGen;

            
            int tileHeight = level.TileHeight;
            int tileWidth = level.TileWidth;

        

            GM.engineM.AddSprite(this);

            //tileWidthDiv2 = tileWidth / 2;
            //tileHeightDiv2 = tileHeight / 2;


            X = level.MiniMap.X - (mazeGen.mazeLength) * tileWidth / 2 + tileWidth / 2 + mazeGen.startPoint.X * tileWidth;
            Y = level.MiniMap.Y - (mazeGen.mazeHeight) * tileHeight / 2 + tileHeight / 2 + mazeGen.startPoint.Y * tileHeight;

            


            //Player spawn when tilemap is centered on screen size
            //X = GM.screenSize.Center.X - MazeGenerator.mazeLength / 2 * tileHeight + MazeGenerator.startPoint.X * tileHeight;
            //Y = GM.screenSize.Center.Y - MazeGenerator.mazeHeight / 2 * tileWidth + MazeGenerator.startPoint.Y * tileWidth;

            UpdateBeforeLimit = true;

            //Align = Align.centre;


            Frame.Define(Tex.Triangle);
            Sprite bottom = new Sprite();


            Shape = Shape.rectangle;

            //SX = 10;
            //SY = 20;
            SX = 0.2f;
            SY = 0.2f;


            Wash = Color.Blue;
            //RotationAngle = RotationHelper.AngleFromDirection(new Vector2(0, -1));
            UpdateCallBack += Controls;
            

            //RayCast rc = new RayCast(this, maze, 300, level.Location(this), RotationHelper.Direction2DFromAngle(this.RotationAngle, 0));


        }





        //private void SetOffSets()
        //{
        //    offsetLeft.Add(new Vector2());


        //}

        /// <summary>
        /// Gets keyboard input of player
        /// </summary>
        private void Controls()
        {
            //Facing();
            //float _RotationAngle = RotationAngle;



            //GM.textM.DrawQuick(FontBank.arcadePixel, RotationHelper.Direction2DFromAngle(RotationAngle,0).ToString(), 200, 400, TextAtt.Centred);
            //GM.textM.DrawQuick(FontBank.arcadePixel, "@@@@", 400, 800, TextAtt.Centred);


            //   GM.textM.DrawQuick(FontBank.arcadePixel, direction, 200, 600, TextAtt.Centred);
            Velocity.X = 0;
            Velocity.Y = 0;
            if (GM.inputM.KeyDown(Keys.Right))
            {
                RotationAngle += rotationSpeed * GM.eventM.Delta;

            }

            if (GM.inputM.KeyDown(Keys.Left))
            {
                RotationAngle -= rotationSpeed * GM.eventM.Delta;
            }

            //GM.textM.DrawQuick(FontBank.arcadePixel, ".",(Position2D.X+RotationHelper.Direction2DFromAngle(RotationAngle,0).X*3), (Position2D.Y + RotationHelper.Direction2DFromAngle(RotationAngle, 0).Y*3), TextAtt.Centred);
            if (GM.inputM.KeyDown(Keys.A))
            {


                //RotationHelper.VelocityInCurrentDirection(this, walkSpeed, -90);
                // Move(walkSpeed, -90);
                if (!level.HitWall(collistionRadius(-90)))
                {
                    //RotationHelper.VelocityInCurrentDirection(this, walkSpeed, 0);
                    Move(walkSpeed, -90);
                }
            }

            if (GM.inputM.KeyDown(Keys.D))
            {

                //RotationHelper.VelocityInCurrentDirection(this, -walkSpeed, -90);
                //Move(walkSpeed, 90);
                if (!level.HitWall(collistionRadius(90)))
                {
                    //RotationHelper.VelocityInCurrentDirection(this, walkSpeed, 0);
                    Move(walkSpeed, 90);
                }
            }

            if (GM.inputM.KeyDown(Keys.W))
            {
                if (!level.HitWall(collistionRadius(0)))
                {
                    //RotationHelper.VelocityInCurrentDirection(this, walkSpeed, 0);
                    Move(walkSpeed, 0);
                }

            }

            if (GM.inputM.KeyDown(Keys.S))
            {

                //RotationHelper.VelocityInCurrentDirection(this, -walkSpeed, 0);
                //Move(-walkSpeed, 0);
                if (!level.HitWall(collistionRadius(180)))
                {
                    //RotationHelper.VelocityInCurrentDirection(this, walkSpeed, 0);
                    Move(-walkSpeed, 0);
                }

            }



            MessageBus.Instance.BroadcastMessage(ExtraMessageTypes.PlayerPosition, Position2D);
            MessageBus.Instance.BroadcastMessage(ExtraMessageTypes.PlayerRotation, RotationHelper.Direction2DFromAngle(RotationAngle, 0));

            //GM.textM.DrawQuick(FontBank.arcadeLarge, RotationHelper.Direction2DFromAngle(RotationAngle, 0).ToString(), 500, 500, TextAtt.Centred);

            //line.
        }
        /// <summary>
        /// Returns a point that is infront of the sprite. It is infront when the additional angle is 0
        /// </summary>
        /// <param name="additionalAngle">angle away from normal</param>
        /// <returns></returns>
        private Vector2 collistionRadius(int additionalAngle)
        {
            //return new Vector2(Position2D.X + RotationHelper.Direction2DFromAngle(RotationAngle, additionalAngle).X * 10, Position2D.Y + RotationHelper.Direction2DFromAngle(RotationAngle, additionalAngle).Y * 10);
            return (Position2D + RotationHelper.Direction2DFromAngle(RotationAngle, additionalAngle)*6);
        }


        /// <summary>
        /// Returns a string of what quadrant the player sprite is facing.
        /// </summary>
        private string Facing()
        {
            

            if (RotationAngle<90 && RotationAngle >=0)
            {
                direction = "NorthEast";
            }

            else if (RotationAngle < 180 && RotationAngle >= 90)
            {
                direction = "SouthEast";
            }
            else if (RotationAngle < 270 && RotationAngle >= 180)
            {
                direction = "SouthWest";
            }
            else if (RotationAngle < 360 && RotationAngle >= 270)
            {
                direction = "NorthWest";
            }

            return direction;
        }

        /// <summary>
        /// Moves the player in the angle specified
        /// </summary>
        /// <param name="additionalAngle">0 is directly infront of the player 90 is to the right of the player</param>
        /// <param name="speed">velocity in direction</param>
        private void Move(int speed, int additionalAngle)
        {
            RotationHelper.VelocityInCurrentDirection(this, speed, additionalAngle);
        }



    }
}