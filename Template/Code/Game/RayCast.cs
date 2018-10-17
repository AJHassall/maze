
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
using Template.Game;


namespace Template.Game
{
    




    internal class RayCast : Line
    {
        //double fovDiv2 = 30;
        private Vector2 rayStart;
        private Vector2 playerRotation;
        //private int counter = 100;

        /// <summary>
        /// X resolution of game window. Less will be faster as it will cast less rays.
        /// </summary>
        private int displayWidth = 900;
        private int displayHeight = 600;

        /// <summary>
        /// List of lines that are used to create three dimensional effect. The list is updated at the end of the raycast event
        /// </summary>
        private Line[] gameLines;
        /// <summary>
        /// Similar to GameRays. The lines are used to draw a view cone on the player
        /// </summary>
        private Line[] miniMapLines;
        //private Line line;
        int[,] map;
      

        Vector2 RayEnd;
        int tileWidth;
        int tileHeight;




        /// <summary>
        /// Constructor for the raycaster. This will create the lines that are used to render the game. Creates a raycast event and subscribes to the players location and rotation.
        /// </summary>
        /// <param name="level">The Level that will be raycast.</param>
        public RayCast(Level level)
        {
            tileWidth = level.TileWidth;
            tileHeight = level.TileHeight;

            MessageBus.Instance.Subscribe(ExtraMessageTypes.PlayerRotation, PlayerRotation);
            MessageBus.Instance.Subscribe(ExtraMessageTypes.PlayerPosition, RayStart);


            //createLines();

            this.map = level.map;

            GM.eventM.AddEvent(new Event(1 / 60f, "raycast", RayCasting));

            gameLines = createLines();
            miniMapLines = createLines();




        }



        /// <summary>
        /// Creates an array of lines. the lines start and end are both 0,0
        /// </summary>
        private Line[] createLines()
        {
            Line[] lineList = new Line[displayWidth];

            for (int i = 0; i < displayWidth; i++)
            {
                lineList[i] = new Line(Vector3.Zero, Vector3.Zero);
                GM.engineM.AddLine(lineList[i]);
            }



            return lineList;
        }


        /// <summary>
        /// Reaction to a message that tells this to update the players rotation. This just sets the rays rotation to be equal to the players.
        /// </summary>
        private void PlayerRotation(Message message)
        {
            playerRotation = (Vector2)message.ObjectValue;
        }

        private void RayStart(Message message)
        {
            rayStart = (Vector2)message.ObjectValue;
            
        }

        /// <summary>
        /// Ray Cast event. This happens every frame of the screen. Starts at the left line of the screen and moves right. Updates the line of each ray at the end.
        /// </summary>
        private void RayCasting()
        {

            //x and y start position
            double positionX = rayStart.X / tileWidth, positionY = rayStart.Y / tileHeight;

            double AdditionDirectionX = -playerRotation.Y, AdditionalDirectionY = playerRotation.X;



            for (int x = 0; x < displayWidth; x++)
            {
                //x and y location in array
                int mapX = (int)(positionX);
                int mapY = (int)(positionY);

                //calculate ray position and direction

                // CameraX is a number between 1 and 1. 1 is the right of the screen.
                double cameraX = 2 * (x / (double)displayWidth) - 1; //x-coordinate in camera space
                double rayDirX = playerRotation.X + AdditionDirectionX * cameraX;
                double rayDirY = playerRotation.Y + AdditionalDirectionY * cameraX;


                //length of ray from current position to next x or y-side
                double sideDistX;
                double sideDistY;

                //The number of blocks left or right the ray moves if the ray goes up or down 1 block
                //deltaX per DeltaY
                double deltaDistX = Math.Abs(1 / rayDirX);
                //deltaY per DeltaX
                double deltaDistY = Math.Abs(1 / rayDirY);
                double perpWallDist;

                //what direction to step in x or y-direction (either +1 or -1)
                int stepX;
                int stepY;

                int hit = 0; //was there a wall hit?
                int side = -1; //was a NS or a EW wall hit?
                               //calculate step and initial sideDist
                if (rayDirX < 0)
                {
                    stepX = -1;
                    sideDistX = (positionX - mapX) * deltaDistX;
                }
                else
                {
                    stepX = 1;
                    sideDistX = (mapX + 1.0 - positionX) * deltaDistX;
                }
                if (rayDirY < 0)
                {
                    stepY = -1;
                    sideDistY = (positionY - mapY) * deltaDistY;
                }
                else
                {
                    stepY = 1;
                    sideDistY = (mapY + 1.0 - positionY) * deltaDistY;
                }
                //perform DDA
                while (hit == 0)
                {
                    //jump to next map square, OR in x-direction, OR in y-direction
                    if (sideDistX < sideDistY)
                    {
                        sideDistX += deltaDistX;
                        mapX += stepX;
                        side = 0;
                    }
                    else
                    {
                        sideDistY += deltaDistY;
                        mapY += stepY;
                        side = 1;
                    }
                    //Check if ray has hit a wall
                    if (map[mapY, mapX] == 0 || map[mapY, mapX] == 2) hit = 1;

                    RayEnd = new Vector2(mapX, mapY);
                    MessageBus.Instance.BroadcastMessage(ExtraMessageTypes.UpdateMap, RayEnd);

                    //MessageBus.Instance.BroadcastMessage(ExtraMessageTypes.UpdateMap, newmap);
                }
                //Calculate distance projected on camera direction 
                if (side == 0) perpWallDist = (mapX - positionX + (1 - stepX) / 2) / rayDirX;
                else perpWallDist = (mapY - positionY + (1 - stepY) / 2) / rayDirY;

                //Calculate height of line to draw on screen
                int lineLength = (int)(displayHeight / perpWallDist);

                //calculate lowest and highest pixel to fill in current stripe
                int lineMax = -lineLength / 2 + displayHeight / 2;
                if (lineMax < 0) lineMax = 0;
                int lineMin = lineLength / 2 + displayHeight / 2;
                if (lineMin >= displayHeight) lineMin = displayHeight - 1;

                //choose wall color

                Color color = Color.White;

                if (map[mapY, mapX] == 2)
                {


                    color = Color.Sienna;

                }

                //give x and y sides different brightness
                //if (side == 1) { color = new Color(color.R / 2, color.G / 2, color.B / 2);
                if (side == 1)
                {


                    color = new Color(color.R / 2, color.G / 2, color.B / 2);

                }

                


                //draw the pixels of the stripe as a vertical line
                UpdateLine(x, lineMax + 100, lineMin + 100, color, positionX * tileWidth, positionY * tileWidth, rayDirX, rayDirY, perpWallDist);


                //GM.textM.DrawQuick(FontBank.imagic, "o", mapX*10, mapY*10, TextAtt.Centred);
                //GM.textM.DrawQuick(FontBank.arcadeLarge, new Vector2((float)rayDirX, (float)rayDirY).ToString(), 500, 500, TextAtt.Centred);
                //GM.textM.DrawQuick(FontBank.arcadeLarge, new Vector2((float)rayDirX, (float)rayDirY).ToString(), 500, 500, TextAtt.Centred);
                //GM.textM.DrawQuick(FontBank.arcadeLarge, new Vector2(mapX, mapY).ToString(), 700, 700, TextAtt.Centred);
            }

        }


        /// <summary>
        /// Updates the lines at x in the line arrays. Updates both the Game lines and the Minimap lines
        /// </summary>
        /// <param name="x"> which line to update</param>
        private void UpdateLine(int x, int drawEnd, int drawStart, Color color, double posX, double posY, double dirX, double dirY, double perpWallDist)
        {




            gameLines[x].Vertices = new Vector3[] { new Vector3(x + 240, drawEnd, 0), new Vector3(x + 240, drawStart, 0) };
            //gameLines[x].Length = 1;
            gameLines[x].Settings.Wash = color;

            miniMapLines[x].Vertices = new Vector3[] { new Vector3((int)posX, (int)posY, 0),
                                    new Vector3(new Vector2((int)posX, (int)posY) + new Vector2((float) dirX*(float)perpWallDist* tileWidth,
                                    (float)dirY* (float)perpWallDist* tileHeight), 0) };

            miniMapLines[x].Settings.Wash = Color.Green;


        }



        //private void verLine(int x, int drawStart, int drawEnd, Color color)
        //{
        //    Line l = new Line(new Vector3(x + 500, drawStart, 0), new Vector3(x + 500, drawEnd, 0));
        //    l.Settings.Wash = color;
        //    GM.engineM.AddLine(l);
        //}

        private void clearlines()
        {
            GM.engineM.ClearLineList();
        }
    }

}