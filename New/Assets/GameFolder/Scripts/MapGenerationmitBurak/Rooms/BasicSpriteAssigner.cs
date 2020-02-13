using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class BasicSpriteAssigner
{

    enum Directions { left, right, up, down }

    public static RoomClass.RoomTile[,] AssignBasicSprites(RoomClass.RoomTile[,] grid)
    {
        CreateWallsUDLR();
        CreateCornerWalls();
        CreateTwoWalls();
        CreateThreeWalls();
        CreatePillars();


        void CreateWallsUDLR()
        {
            for (int x = 2; x < grid.GetLength(0) - 1; x++)
            {
                for (int y = 2; y < grid.GetLength(1) - 1; y++)

                {
                    if (grid[x, y].roomSpriteAbove == RoomClass.RoomSprite.Floor)
                    {

                        if (grid[x - 1, y].roomSpriteAbove == RoomClass.RoomSprite.Border) { grid[x - 1, y].roomSpriteAbove = RoomClass.RoomSprite.WallRight; grid[x - 1, y].roomSpriteUnderneath = RoomClass.RoomSpriteBelow.Floor; }
                        if (grid[x + 1, y].roomSpriteAbove == RoomClass.RoomSprite.Border) { grid[x + 1, y].roomSpriteAbove = RoomClass.RoomSprite.WallLeft; grid[x + 1, y].roomSpriteUnderneath = RoomClass.RoomSpriteBelow.Floor; }
                        if (grid[x, y - 1].roomSpriteAbove == RoomClass.RoomSprite.Border) { grid[x, y - 1].roomSpriteAbove = RoomClass.RoomSprite.WallTop; grid[x, y - 1].roomSpriteUnderneath = RoomClass.RoomSpriteBelow.Floor; }
                        if (grid[x, y + 1].roomSpriteAbove == RoomClass.RoomSprite.Border) { grid[x, y + 1].roomSpriteAbove = RoomClass.RoomSprite.WallBot; grid[x, y + 1].roomSpriteUnderneath = RoomClass.RoomSpriteBelow.Floor; }
                    }

                }
            }
        }


        void CreateCornerWalls()
        {
            for (int x = 2; x < grid.GetLength(0) - 1; x++)
            {
                for (int y = 2; y < grid.GetLength(1) - 1; y++)

                {
                    if (WallCheck(x, y))
                    {

                        if (FloorCheck(x - 1, y) && FloorCheck(x, y + 1) && BorderandWallandCornerCheck(x, y - 1) && BorderandWallandCornerCheck(x + 1, y))
                        {
                            grid[x, y].roomSpriteAbove = RoomClass.RoomSprite.CornerTopLeft;
                            grid[x, y].roomSpriteUnderneath = RoomClass.RoomSpriteBelow.Floor;
                        }

                        if (FloorCheck(x, y + 1) && FloorCheck(x + 1, y) && BorderandWallandCornerCheck(x, y - 1) && BorderandWallandCornerCheck(x - 1, y))
                        {
                            grid[x, y].roomSpriteAbove = RoomClass.RoomSprite.CornerTopRight;
                            grid[x, y].roomSpriteUnderneath = RoomClass.RoomSpriteBelow.Floor;
                        }

                        if (FloorCheck(x - 1, y) && FloorCheck(x, y - 1) && BorderandWallandCornerCheck(x, y + 1) && BorderandWallandCornerCheck(x + 1, y))
                        {
                            grid[x, y].roomSpriteAbove = RoomClass.RoomSprite.CornerBotLeft;
                            grid[x, y].roomSpriteUnderneath = RoomClass.RoomSpriteBelow.Floor;
                        }
                        if (FloorCheck(x + 1, y) && FloorCheck(x, y - 1) && BorderandWallandCornerCheck(x, y + 1) && BorderandWallandCornerCheck(x - 1, y))
                        {
                            grid[x, y].roomSpriteAbove = RoomClass.RoomSprite.CornerBotRight;
                            grid[x, y].roomSpriteUnderneath = RoomClass.RoomSpriteBelow.Floor;
                        }

                    }



                }
            }
        }

        void CreateTwoWalls()
        {
            for (int x = 2; x < grid.GetLength(0) - 1; x++)
            {
                for (int y = 2; y < grid.GetLength(1) - 1; y++)

                {
                    if (WallCheck(x, y) || CornerCheck(x, y))
                    {

                        if (FloorCheck(x - 1, y)  && FloorCheck(x + 1, y)  && BorderandWallandCornerandTwoCheck(x, y - 1) && BorderandWallandCornerandTwoCheck(x, y + 1))
                            grid[x, y].roomSpriteAbove = RoomClass.RoomSprite.twoLeftRight;

                        if (FloorCheck(x, y + 1)&& FloorCheck(x, y + -1) && BorderandWallandCornerandTwoCheck(x + 1, y) && BorderandWallandCornerandTwoCheck(x - 1, y))
                            grid[x, y].roomSpriteAbove = RoomClass.RoomSprite.twoTopBot;

                    }

                }
            }

        }

        void CreateThreeWalls()
        {
            for (int x = 2; x < grid.GetLength(0) - 1; x++)
            {
                for (int y = 2; y < grid.GetLength(1) - 1; y++)

                {
                    if (WallCheck(x, y) || CornerCheck(x, y) || TwoWallCheck(x, y) || ThreeWallCheck(x, y))
                    {

                        if (ThreeEmpty(Directions.down, x, y))
                            grid[x, y].roomSpriteAbove = RoomClass.RoomSprite.threeBot;
                        if (ThreeEmpty(Directions.left, x, y))
                            grid[x, y].roomSpriteAbove = RoomClass.RoomSprite.threeLeft;
                        if (ThreeEmpty(Directions.right, x, y))
                            grid[x, y].roomSpriteAbove = RoomClass.RoomSprite.threeRight;
                        if (ThreeEmpty(Directions.up, x, y))
                            grid[x, y].roomSpriteAbove = RoomClass.RoomSprite.threeTop;

                    }

                }
            }

        }

        void CreatePillars()
        {
            for (int x = 2; x < grid.GetLength(0) - 1; x++)
            {
                for (int y = 2; y < grid.GetLength(1) - 1; y++)

                {
                    if (CornerCheck(x, y) || WallCheck(x, y))
                    {

                        if (FloorCheck(x - 1, y) && FloorCheck(x + 1, y) && FloorCheck(x, y + 1) && FloorCheck(x, y - 1) )
                            grid[x, y].roomSpriteAbove = RoomClass.RoomSprite.Pillar;

                    }



                }
            }

        }

        //all the Checks start
        //
        //
        bool FloorCheck(int posX, int posY)
        {
            return (grid[posX, posY].roomSpriteAbove == RoomClass.RoomSprite.Floor || grid[posX, posY].roomSpriteAbove == RoomClass.RoomSprite.Floor2);
        }

        bool BorderCheck(int posX, int posY)
        {
            if (grid[posX, posY].roomSpriteAbove == RoomClass.RoomSprite.Border) return true;
            else return false;
        }
        bool WallCheck(int posX, int posY)
        {
            if (grid[posX, posY].roomSpriteAbove == RoomClass.RoomSprite.WallBot || grid[posX, posY].roomSpriteAbove == RoomClass.RoomSprite.WallLeft || grid[posX, posY].roomSpriteAbove == RoomClass.RoomSprite.WallRight ||
                grid[posX, posY].roomSpriteAbove == RoomClass.RoomSprite.WallTop) return true;
            else return false;
        }

        bool CornerCheck(int posX, int posY)
        {
            if (grid[posX, posY].roomSpriteAbove == RoomClass.RoomSprite.CornerBotLeft || grid[posX, posY].roomSpriteAbove == RoomClass.RoomSprite.CornerBotRight
                || grid[posX, posY].roomSpriteAbove == RoomClass.RoomSprite.CornerTopLeft || grid[posX, posY].roomSpriteAbove == RoomClass.RoomSprite.CornerTopRight) return true;
            else return false;
        }

        bool TwoWallCheck(int posX, int posY)
        {
            if (grid[posX, posY].roomSpriteAbove == RoomClass.RoomSprite.twoTopBot || grid[posX, posY].roomSpriteAbove == RoomClass.RoomSprite.twoLeftRight) return true;
            else return false;
        }
        bool ThreeWallCheck(int posX, int posY)
        {
            if (grid[posX, posY].roomSpriteAbove == RoomClass.RoomSprite.threeBot || grid[posX, posY].roomSpriteAbove == RoomClass.RoomSprite.threeTop
                || grid[posX, posY].roomSpriteAbove == RoomClass.RoomSprite.threeLeft || grid[posX, posY].roomSpriteAbove == RoomClass.RoomSprite.threeRight) return true;
            else return false;


        }

        //
        //
        //all the Checks by themselves end
        // Checkcombos start
        //
        //

        bool BorderandWallandCornerCheck(int posX, int posY)
        {
            if (BorderCheck(posX, posY) || WallCheck(posX, posY) || CornerCheck(posX, posY))
                return true;
            else return false;
        }
        bool BorderandWallandCornerandTwoCheck(int posX, int posY)
        {
            if (BorderCheck(posX, posY) || WallCheck(posX, posY) || CornerCheck(posX, posY) || TwoWallCheck(posX, posY))
                return true;
            else return false;
        }

        bool WallandCornerandTwoandThreeCheck(int posX, int posY)
        {
            if (BorderCheck(posX, posY) || WallCheck(posX, posY) || CornerCheck(posX, posY) || TwoWallCheck(posX, posY) || ThreeWallCheck(posX, posY))
                return true;
            else return false;
        }

        //Checkcombos end
        //
        //

        bool ThreeEmpty(Directions dir, int posX, int posY)
        {
            switch (dir)
            {
                case Directions.down:
                    if (WallandCornerandTwoandThreeCheck(posX, posY + 1) && FloorCheck(posX, posY - 1) && FloorCheck(posX + 1, posY)
                        && FloorCheck(posX - 1, posY))
                        return true;
                    break;
                case Directions.up:
                    if (WallandCornerandTwoandThreeCheck(posX, posY - 1) && FloorCheck(posX, posY + 1) && FloorCheck(posX + 1, posY)
                        && FloorCheck(posX - 1, posY))
                        return true;
                    break;
                case Directions.left:
                    if (WallandCornerandTwoandThreeCheck(posX + 1, posY) && FloorCheck(posX, posY - 1) && FloorCheck(posX - 1, posY)
                        && FloorCheck(posX, posY + 1))
                        return true;
                    break;
                case Directions.right:
                    if (WallandCornerandTwoandThreeCheck(posX - 1, posY) && FloorCheck(posX, posY - 1) && FloorCheck(posX, posY + 1)
                        && FloorCheck(posX + 1, posY))
                        return true;
                    break;
                default:
                    return false;


            }

            return false;
            
        }
        return grid;
    }
}
