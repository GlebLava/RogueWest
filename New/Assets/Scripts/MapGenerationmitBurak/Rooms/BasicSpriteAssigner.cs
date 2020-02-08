using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class BasicSpriteAssigner 
{

    enum Directions { left, right, up, down }

    public static RoomClass.RoomSprite[,] AssignBasicSprites(RoomClass.RoomSprite[,] grid)
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
                    if (grid[x, y] == RoomClass.RoomSprite.Floor)
                    {

                        if (grid[x - 1, y] == RoomClass.RoomSprite.Border) grid[x - 1, y] = RoomClass.RoomSprite.WallRight;
                        if (grid[x + 1, y] == RoomClass.RoomSprite.Border) grid[x + 1, y] = RoomClass.RoomSprite.WallLeft;
                        if (grid[x, y - 1] == RoomClass.RoomSprite.Border) grid[x, y - 1] = RoomClass.RoomSprite.WallTop;
                        if (grid[x, y + 1] == RoomClass.RoomSprite.Border) grid[x, y + 1] = RoomClass.RoomSprite.WallBot;
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

                        if (grid[x - 1, y] == RoomClass.RoomSprite.Floor && grid[x, y + 1] == RoomClass.RoomSprite.Floor && EmptyandWallandCornerCheck(x, y - 1) && EmptyandWallandCornerCheck(x + 1, y))
                            grid[x, y] = RoomClass.RoomSprite.CornerTopLeft;

                        if (grid[x, y + 1] == RoomClass.RoomSprite.Floor && grid[x + 1, y] == RoomClass.RoomSprite.Floor && EmptyandWallandCornerCheck(x, y - 1) && EmptyandWallandCornerCheck(x - 1, y))
                            grid[x, y] = RoomClass.RoomSprite.CornerTopRight;

                        if (grid[x - 1, y] == RoomClass.RoomSprite.Floor && grid[x, y - 1] == RoomClass.RoomSprite.Floor && EmptyandWallandCornerCheck(x, y + 1) && EmptyandWallandCornerCheck(x + 1, y))
                            grid[x, y] = RoomClass.RoomSprite.CornerBotLeft;
                        if (grid[x + 1, y] == RoomClass.RoomSprite.Floor && grid[x, y - 1] == RoomClass.RoomSprite.Floor && EmptyandWallandCornerCheck(x, y + 1) && EmptyandWallandCornerCheck(x - 1, y))
                            grid[x, y] = RoomClass.RoomSprite.CornerBotRight;

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

                        if (grid[x - 1, y] == RoomClass.RoomSprite.Floor && grid[x + 1, y] == RoomClass.RoomSprite.Floor && EmptyandWallandCornerandTwoCheck(x, y - 1) && EmptyandWallandCornerandTwoCheck(x, y + 1))
                            grid[x, y] = RoomClass.RoomSprite.twoLeftRight;

                        if (grid[x, y + 1] == RoomClass.RoomSprite.Floor && grid[x, y + -1] == RoomClass.RoomSprite.Floor && EmptyandWallandCornerandTwoCheck(x + 1, y) && EmptyandWallandCornerandTwoCheck(x - 1, y))
                            grid[x, y] = RoomClass.RoomSprite.twoTopBot;

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
                            grid[x, y] = RoomClass.RoomSprite.threeBot;
                        if (ThreeEmpty(Directions.left, x, y))
                            grid[x, y] = RoomClass.RoomSprite.threeLeft;
                        if (ThreeEmpty(Directions.right, x, y))
                            grid[x, y] = RoomClass.RoomSprite.threeRight;
                        if (ThreeEmpty(Directions.up, x, y))
                            grid[x, y] = RoomClass.RoomSprite.threeTop;

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

                        if (grid[x - 1, y] == RoomClass.RoomSprite.Floor && grid[x + 1, y] == RoomClass.RoomSprite.Floor && grid[x, y + 1] == RoomClass.RoomSprite.Floor && grid[x, y - 1] == RoomClass.RoomSprite.Floor)
                            grid[x, y] = RoomClass.RoomSprite.Pillar;

                    }



                }
            }

        }

        //all the Checks start
        //
        //
        bool EmptyCheck(int posX, int posY)
        {
            if (grid[posX, posY] == RoomClass.RoomSprite.Border) return true;
            else return false;
        }
        bool WallCheck(int posX, int posY)
        {
            if (grid[posX, posY] == RoomClass.RoomSprite.WallBot || grid[posX, posY] == RoomClass.RoomSprite.WallLeft || grid[posX, posY] == RoomClass.RoomSprite.WallRight ||
                grid[posX, posY] == RoomClass.RoomSprite.WallTop) return true;
            else return false;
        }

        bool CornerCheck(int posX, int posY)
        {
            if (grid[posX, posY] == RoomClass.RoomSprite.CornerBotLeft || grid[posX, posY] == RoomClass.RoomSprite.CornerBotRight
                || grid[posX, posY] == RoomClass.RoomSprite.CornerTopLeft || grid[posX, posY] == RoomClass.RoomSprite.CornerTopRight) return true;
            else return false;
        }

        bool TwoWallCheck(int posX, int posY)
        {
            if (grid[posX, posY] == RoomClass.RoomSprite.twoTopBot || grid[posX, posY] == RoomClass.RoomSprite.twoLeftRight) return true;
            else return false;
        }
        bool ThreeWallCheck(int posX, int posY)
        {
            if (grid[posX, posY] == RoomClass.RoomSprite.threeBot || grid[posX, posY] == RoomClass.RoomSprite.threeTop
                || grid[posX, posY] == RoomClass.RoomSprite.threeLeft || grid[posX, posY] == RoomClass.RoomSprite.threeRight) return true;
            else return false;


        }

        //
        //
        //all the Checks by themselves end
        // Checkcombos start
        //
        //

        bool EmptyandWallandCornerCheck(int posX, int posY)
        {
            if (EmptyCheck(posX, posY) || WallCheck(posX, posY) || CornerCheck(posX, posY))
                return true;
            else return false;
        }
        bool EmptyandWallandCornerandTwoCheck(int posX, int posY)
        {
            if (EmptyCheck(posX, posY) || WallCheck(posX, posY) || CornerCheck(posX, posY) || TwoWallCheck(posX, posY))
                return true;
            else return false;
        }

        bool WallandCornerandTwoandThreeCheck(int posX, int posY)
        {
            if (EmptyCheck(posX, posY) || WallCheck(posX, posY) || CornerCheck(posX, posY) || TwoWallCheck(posX, posY) || ThreeWallCheck(posX, posY))
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
                    if (WallandCornerandTwoandThreeCheck(posX, posY + 1) && grid[posX, posY - 1] == RoomClass.RoomSprite.Floor && grid[posX + 1, posY] == RoomClass.RoomSprite.Floor
                        && grid[posX - 1, posY] == RoomClass.RoomSprite.Floor)
                        return true;
                    break;
                case Directions.up:
                    if (WallandCornerandTwoandThreeCheck(posX, posY - 1) && grid[posX, posY + 1] == RoomClass.RoomSprite.Floor && grid[posX + 1, posY] == RoomClass.RoomSprite.Floor
                        && grid[posX - 1, posY] == RoomClass.RoomSprite.Floor)
                        return true;
                    break;
                case Directions.left:
                    if (WallandCornerandTwoandThreeCheck(posX + 1, posY) && grid[posX, posY - 1] == RoomClass.RoomSprite.Floor && grid[posX - 1, posY] == RoomClass.RoomSprite.Floor
                        && grid[posX, posY + 1] == RoomClass.RoomSprite.Floor)
                        return true;
                    break;
                case Directions.right:
                    if (WallandCornerandTwoandThreeCheck(posX - 1, posY) && grid[posX, posY - 1] == RoomClass.RoomSprite.Floor && grid[posX, posY + 1] == RoomClass.RoomSprite.Floor
                        && grid[posX + 1, posY] == RoomClass.RoomSprite.Floor)
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
