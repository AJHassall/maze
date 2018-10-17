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
    internal class MazeGenerator
    {
        /// <summary>
        /// 0 = Wall
        /// 1 = Passage
        /// </summary>
        public int[,] Maze;

        public int mazeHeight = 18, mazeLength = 18;


        /// <summary>
        /// Current position that the Maze Generator is inspecting;
        /// Current position of Dig()
        /// </summary>
        private int X, Y;

        public Point startPoint;
        private Point currentPos;

        //static bool EndRoom = false;
        ///<summary> 1=North, 2=South,  4=East, 8=West</summary>///
        static int[] directions = new int[] { 1, 2, 4, 8 };

        public Stack mazeStack;

        public MazeGenerator()
        {
            
        }

        /// <summary>
        /// Generates a new maze using recursive backtracking
        /// </summary>
        public int[,] NewMaze()
        {
            mazeStack = new Stack(mazeLength * mazeHeight);
            int[,] map = new int[,] { {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 },

                {0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0 },
                {0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0 },
                {0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0 },
                {0,1,1,1,1,1,1,0,1,1,1,1,1,1,1,2,1,1,1,1,1,1,1,0 },
                {0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0 },
                {0,1,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0 },
                {0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0 },
                {0,1,1,1,1,1,1,1,1,1,1,1,1,1,2,1,1,1,1,1,1,1,1,0 },
                {0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,1,1,1,1,0 },
                {0,1,1,1,1,1,1,1,1,1,0,1,1,1,1,1,1,1,1,1,1,1,1,0 },
                {0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0 },
                {0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0 },
                {0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0 },
                {0,1,1,1,1,1,1,1,1,1,1,1,1,2,1,1,1,1,1,1,1,1,1,0 },
                {0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0 },
                {0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0 },
                {0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0 },
                {0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,1,1,1,1,1,1,1,0 },
                {0,1,1,1,1,1,1,1,1,1,1,0,1,1,1,1,1,1,1,1,1,1,1,0},
                {0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0},
                {0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0 },
                {0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0 },
              { 0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 },
            };
            X = GM.r.Next(1, mazeLength - 2);
            Y = GM.r.Next(1, mazeHeight - 2);


            mazeStack.Push(new Point(X, Y));

            startPoint = new Point(X, Y);

            Maze = new int[mazeHeight, mazeLength];

            Maze[Y, X] = 1;


            Dig();



            return Maze;
        }
        /// <summary>
        /// The worker method just carves a path that direction picker says. Calls self until it reaches start point.
        /// </summary>
        private void Dig()
        {


            //X = position.X;
            //Y = position.Y;
            int direction = directionPicker();



            //North
            if (direction == 1)
            {
               --Y;
                Maze[Y, X] = 1;
            //    Dig(new Point(X, Y));
            }
            //South
            else if (direction == 2)
            {
                ++Y;
                Maze[Y, X] = 1;

           //     Dig(new Point(X, Y));
            }
            //West
            else if (direction == 4)
            {
                --X ;
                Maze[Y, X] = 1;
             //   Dig(new Point(X, Y));
            }
            //East
            else if (direction == 8)
            {
                ++X ;
                Maze[Y, X] = 1;
            //    Dig(new Point(X, Y));


            }
            else if (direction == 16)
            {
                //if (!EndRoom)
                //{
                //    Maze[Y , X] = 2;
                    

                //    EndRoom = true;
                //}

                currentPos = mazeStack.Pop();
                if (X != startPoint.X || Y != startPoint.Y)
                {
                   
                    X = currentPos.X;
                    Y = currentPos.Y;
                    //   Dig(currentPos);

                    
                }

            }

            if (mazeStack._sp > 0)
            {
                Dig();
                 //  GM.eventM.DelayCall(0.02f, Dig);

            }

        }

        //private void _Dig()
        //{
        //    Dig();
        //}

        /// <summary>
        /// Checks a 2x3 area around each direction of the square and returns a random direction that is safe to travel
        /// safe to travel means it will not cut into a visited square and there is not a visited square in the 2x3 area
        /// </summary>
        ///
        /// <returns></returns>
        private int directionPicker()
        {
            List<int> directions = new List<int>();
            int[] directionsToArray;

            bool canNorth = true;
            bool canSouth = true;
            bool canEast = true;
            bool canWest = true;

            int safe;






            //North
            for (int i = X - 1; i < X + 2; i++)
            {
                if (Y - 1 == 0 || Maze[Y - 1, i] == 1|| Maze[Y - 2, i] == 1)
                {
                   canNorth = false;
                }
            }

            //South
            for (int i = X - 1; i < X +2; i++)
            {
                if (Y + 1 == mazeHeight - 1 || Maze[Y + 1, i] == 1|| Maze[Y + 2, i] == 1)
                {
                    canSouth = false;
                }
            }
            //West
            for (int i = Y - 1; i < Y + 2; i++)
            {
                if ( X - 1 <= 0 || Maze[i, X - 1] == 1 ||Maze[i, X - 2] == 1)
                {
                    canWest = false;
                }
            }

            //East
            for (int i = Y - 1; i < Y + 2; i++)

            {
                if ( X + 1 >= mazeLength - 1 || Maze[i, X + 1] ==1 || Maze[i, X + 2] == 1)
                {
                    canEast = false;
                }
            }






            if (canNorth)
                directions.Add(1);
            if (canSouth)
                directions.Add(2);
            if (canWest)
                directions.Add(4);
            if (canEast)
                directions.Add(8);







            if (canNorth || canSouth || canEast || canWest)
                mazeStack.Push(new Point(X, Y));
            directionsToArray = directions.ToArray();


            if (directions.Count == 0)
            {
                return 16;
            }
            safe = (directions[GM.r.Next(directions.Count)]);
            return safe;


        }




        /// <summary>
        /// Checks Points adjacent to current position
        /// </summary>
        /// <returns>True if there is atleast 1 wall adjacent to current position</returns>
        private bool checkNeighbours()
        {
            int Neighbours = 0;

            bool isNeighbours = false;

            for (int i = Y - 1; i < Y + 1; i++)
            {
                for (int j = X - 1; j < X + 1; j++)
                {
                    if (Maze[i, j] == 0)
                    {
                        Neighbours++;
                    }
                }
            }
            if (Neighbours > 1)
            {
                isNeighbours = true;
            }


            return isNeighbours;
        }
    }
}