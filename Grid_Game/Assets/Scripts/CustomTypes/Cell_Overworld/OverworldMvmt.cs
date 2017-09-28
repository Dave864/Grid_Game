using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum MVMT
{
    TOP = 3,
    LFT = 2,
    BOT = 1, 
    RGT = 0
}

[System.Serializable]
public class OverworldMvmt
{
    // Movement options are coded in 4 bits
    // 1, you can move in that direction
    // 0, you can't move in that direction
    // bit 3: up movement
    // bit 2: left movement
    // bit 1: down movement
    // bit 0: right movement

    // Movement options for the 'ground' layer
    [SerializeField]
    private int mvLyr1;

    // Movemnet options for the next layer up
    [SerializeField]
    private int mvLyr2;

    public OverworldMvmt(int ml1, int ml2)
    {
        mvLyr1 = ml1;
        mvLyr2 = ml2;
    }

    // Copy Consructor
    public OverworldMvmt(OverworldMvmt toCopy)
    {
        mvLyr1 = toCopy.mvLyr1;
        mvLyr2 = toCopy.mvLyr2;
    }

    // Accessor
    // Only recieves boolean values
    public bool this[bool lyr, MVMT dir]
    {
        get
        {
            switch (dir)
            {
                case MVMT.TOP:
                    return CanMvTop(lyr);
                case MVMT.LFT:
                    return CanMvLeft(lyr);
                case MVMT.BOT:
                    return CanMvBot(lyr);
                case MVMT.RGT:
                    return CanMvRight(lyr);
                default:
                    return false;
            }
        }
        set
        {
            switch (dir)
            {
                case MVMT.TOP:
                    SetTop(value, lyr);
                    break;
                case MVMT.LFT:
                    SetLeft(value, lyr);
                    break;
                case MVMT.BOT:
                    SetBot(value, lyr);
                    break;
                case MVMT.RGT:
                    SetRight(value, lyr);
                    break;
                default:
                    break;
            }
        }
    }

    // Helper function used to set movement options
    private void MvSet(int mvMask, int stpMask, bool mv, bool lyr)
    {
        if (lyr) // layer 2
        {
            mvLyr2 = mv ? (mvMask | mvLyr2) : (stpMask & mvLyr2);
        }
        else // layer 1
        {
            mvLyr1 = mv ? (mvMask | mvLyr1) : (stpMask & mvLyr1);
        }
    }

    // Helper function used to check movement options
    private bool MvCheck(int mask, bool lyr)
    {
        if (lyr) // layer 2
        {
            if ((mvLyr2 & mask) == 0)
            {
                return false;
            }
        }
        else // layer 1
        {
            if ((mvLyr1 & mask) == 0)
            {
                return false;
            }
        }
        return true;
    }

    // Set bit 3 of mvLyr lyr to value of mv
    // 0 is layer 1
    // 1 is layer 2
    private void SetTop(bool mv, bool lyr)
    {
        int mvMask = 8; // 1000
        int stpMask = 7; // 0111
        MvSet(mvMask, stpMask, mv, lyr);
    }

    // Set bit 2 of mvLyr lyr to value of mv 
    // 0 is layer 1
    // 1 is layer 2
    private void SetLeft(bool mv, bool lyr)
    {
        int mvMask = 4; // 0100
        int stpMask = 11; // 1011
        MvSet(mvMask, stpMask, mv, lyr);
    }

    // Set bit 1 of mvLyr lyr to value of mv
    // 0 is layer 1
    // 1 is layer 2
    private void SetBot(bool mv, bool lyr)
    {
        int mvMask = 2; // 0010
        int stpMask = 13; // 1101
        MvSet(mvMask, stpMask, mv, lyr);
    }

    // Set bit 0 of mvLyr lyr to value of mv
    // 0 is layer 1
    // 1 is layer 2
    private void SetRight(bool mv, bool lyr)
    {
        int mvMask = 1; // 0001
        int stpMask = 14; // 1110
        MvSet(mvMask, stpMask, mv, lyr);
    }

    // Return if you can move up from the cell
    // 0 is layer 1
    // 1 is layer 2
    private bool CanMvTop(bool lyr)
    {
        int mask = 8; // 1000
        return MvCheck(mask, lyr);
    }

    // Return if you can move left from the cell
    // 0 is layer 1
    // 1 is layer 2
    private bool CanMvLeft(bool lyr)
    {
        int mask = 4; // 0100
        return MvCheck(mask, lyr);
    }

    // Return if you can move down from the cell
    // 0 is layer 1
    // 1 is layer 2
    private bool CanMvBot(bool lyr)
    {
        int mask = 2; // 0010
        return MvCheck(mask, lyr);
    }

    // Return if you can move right from the cell
    // 0 is layer 1
    // 1 is layer 2
    private bool CanMvRight(bool lyr)
    {
        int mask = 1; // 0001
        return MvCheck(mask, lyr);
    }

    // true is lyr 2
    // false is lyr 1
    public int GetMv(bool lyr)
    {
        if (lyr)
        {
            return mvLyr2;
        }
        else
        {
            return mvLyr1;
        }
    }
}
