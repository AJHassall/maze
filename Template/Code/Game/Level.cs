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
    internal class Level : TileMap
    {
        //private int thisLevel;

        /// <summary>
        /// List of tiles that the player cannot walk through
        /// </summary>
        List<int> walls = new List<int>();

        /// <summary>
        /// The location of the minimap
        /// </summary>
        public Rectangle MiniMap;

        // public Rectangle MiniMap;

        /// <summary>
        /// Maze gen class
        /// </summary>
        public MazeGenerator MazeGen = new MazeGenerator();
        public int[,] map;

       



        

        public Level(int currentLevel)
        {
            //thisLevel = currentLevel;

            
          
            
            map = MazeGen.NewMaze();
            GM.tileMapM.Add(this);

            SetMap(240/MazeGen.mazeHeight,240/ MazeGen.mazeLength, map);
            // Set the mini map in the top right corner of the screen
            //MiniMap = new Rectangle(GM.screenSize.Width - MazeGenerator.mazeHeight/2 * TileHeight,MazeGenerator.mazeLength/2 * TileWidth, 0, 0);
            MiniMap = new Rectangle(MazeGen.mazeHeight / 2 * TileHeight, MazeGen.mazeLength / 2 * TileWidth, 0, 0);
             CentreTileMap(MiniMap);


            SetCollisionData();

            new Player(this);



            UseAlphaMap = true;

            FillAlphaMap(1f);

            CreateTiles();


            MessageBus.Instance.Subscribe(ExtraMessageTypes.UpdateMap, UpdateMap);
            RayCast rc = new RayCast(this);



        }

        /// <summary>
        /// Updates the alpha valued of the tile at the given position
        /// </summary>
        private void UpdateMap(Message message)
        {
            int x = (int)((Vector2)message.ObjectValue).X;
            int y = (int)((Vector2)message.ObjectValue).Y;
            AlphaMap[y, x] = 1f;
        }

        /// <summary>
        /// Creates a list of tiles that cannot be walked through
        /// </summary>
        private void SetCollisionData()
        {
            walls.Add(0);
            walls.Add(2);
        }

        /// <summary>
        /// Returns true if at position there is a tile in the list
        /// </summary>
        public bool HitWall(Vector2 position)
        {

           

            int tile = GetGraphic(Location(position.X, position.Y));

            return walls.Contains(tile);
        }

        /// <summary>
        /// If at any of the positions in offsets there is a tile in the collision list return true
        /// </summary>
        public bool HitWall(Vector2 centre, List<Vector2> offsets)
        {
            foreach (Vector2 postion in offsets)
            {
                if (HitWall(postion + centre)) return true;
            }
            return false;
        }

        /// <summary>
        /// Array of tile information
        /// </summary>
        private void CreateTiles()
        {
            MyTileList = new Tile[]
            {
                new Tile(Tex.Rectangle50by50,new Rectangle(0,0, TileWidth, TileHeight), Color.Black),
                new Tile(Tex.Rectangle50by50,new Rectangle(0,0, TileWidth, TileHeight), Color.White),

                new Tile(Tex.Rectangle50by50,new Rectangle(0,0, TileWidth, TileHeight), Color.Yellow),
                new Tile(Tex.Rectangle50by50,new Rectangle(0,0, TileWidth, TileHeight), Color.Red),
                new Tile(Tex.Rectangle50by50,new Rectangle(0,0, TileWidth, TileHeight), Color.Blue),
                new Tile(Tex.Rectangle50by50,new Rectangle(0,0, TileWidth, TileHeight), Color.Green),


               };
        }
    }
}