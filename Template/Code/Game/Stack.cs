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


    /// <summary>
    /// Creates a stack of points for use in a maze;
    /// </summary>
    class Stack
    {




        /// <summary>
        /// maximum number of items in the stack
        /// </summary>
        private int max;
        /// <summary>
        /// current position of the stack pointer. Will always be pointing to an empty location
        /// </summary>
        private int sp;
        /// <summary>
        /// contains the stack
        /// </summary>
        private Point[] stack;


        /// <summary>
        /// Returns the current position of the stackpointer
        /// </summary>
        public int _sp
        {
            get { return sp; }
        }



        /// <summary>
        /// Constructs a stack of points based on the size of the maze;
        /// </summary>
        /// <param name="stackSize">Size of the stack</param>
        public Stack(int stackSize)
        {
            stack = new Point[stackSize];
            sp = 0;
            max = stackSize;
        }

        /// <summary>
        /// Adds item to stack new if stack is full throws an exception
        /// </summary>
        /// <param name="item">Point to add to stack</param>
        public void Push(Point item)
        {
            if (sp < max)
            {
                stack[sp] = item;
                sp++;
            }

            else throw new Exception("stack full");


        }

        /// <summary>
        /// Removes item from the top of the stack if stack is empty throws exception
        /// </summary>
        /// <returns></returns>
        public Point Pop()
        {
            if (sp > 0)
            {
                return stack[--sp];
            }

            else throw new Exception("stack empty");
        }

        /// <summary>
        /// Looks at the item at the top of the stack without changing the stack pointer
        /// </summary>
        public Point Peak()
        {
            if (sp > 0)
            {
                return stack[sp - 1];


            }

            else throw new Exception("stack empty");
        }

    }
}