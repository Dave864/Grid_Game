﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CellData
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

    // Arena map info for the cell
    // TODO: need to test for the "optimal" size
    // TODO: need datatype to allow for detailed encounter maps
    private int cols = 9;
    private int cellsPerCol = 8;
    public int[,] encounterMapUD;
    public int[,] encounterMapLR;

    // The path for the cell model
    [SerializeField]
    private string path;
    // The model for the 3D object
    [SerializeField]
    private GameObject model;

    // Constructor
    public CellData(int type)
    {
        model = null;
        path = "";
        encounterMapUD = new int[cols, cellsPerCol];
        encounterMapLR = new int[cols, cellsPerCol];
        switch (type)
        {
            // Default floor
            case 0:
                mvLyr1 = 15; // 1111
                mvLyr2 = 0; // 0000
                for (int c = 0; c < cellsPerCol; c++)
                {
                    for (int cl = 0; cl < cols; cl++)
                    {
                        if ((c == 0) && (cl % 2 == 0))
                        {
                            encounterMapUD[cl, c] = -1;
                            encounterMapLR[cl, c] = -1;
                        }
                        else
                        {
                            encounterMapUD[cl, c] = 0;
                            encounterMapLR[cl, c] = 0;
                        }
                    }
                }
                break;
            // Default wall
            case 1:
                mvLyr1 = 0; // 0000
                mvLyr2 = 0; // 0000
                for (int c = 0; c < cellsPerCol; c++)
                {
                    for (int cl = 0; cl < cols; cl++)
                    {
                        if ((c == 0) && (cl % 2 == 0))
                        {
                            encounterMapUD[cl, c] = -1;
                            encounterMapLR[cl, c] = -1;
                        }
                        else
                        {
                            encounterMapUD[cl, c] = 0;
                            encounterMapLR[cl, c] = 0;
                        }
                    }
                }
                break;
            // Default platform
            case 2:
                mvLyr1 = 0; // 0000
                mvLyr2 = 15; // 1111
                for (int c = 0; c < cellsPerCol; c++)
                {
                    for (int cl = 0; cl < cols; cl++)
                    {
                        if ((c == 0) && (cl % 2 == 0))
                        {
                            encounterMapUD[cl, c] = -1;
                            encounterMapLR[cl, c] = -1;
                        }
                        else
                        {
                            // Borders
                            // Inner Border
                            // Interior
                            encounterMapUD[cl, c] = 0;
                            encounterMapLR[cl, c] = 0;
                        }
                    }
                }
                break;
            // Default ramp
            case 3:
                mvLyr1 = 15; // 1111
                mvLyr2 = 15; // 1111
                for (int c = 0; c < cellsPerCol; c++)
                {
                    for (int cl = 0; cl < cols; cl++)
                    {
                        if ((c == 0) && (cl % 2 == 0))
                        {
                            encounterMapUD[cl, c] = -1;
                            encounterMapLR[cl, c] = -1;
                        }
                        else
                        {
                            encounterMapUD[cl, c] = (c == 0) ? cellsPerCol - 1 : cellsPerCol - c;
                            encounterMapLR[cl, c] = cols - c;
                        }
                    }
                }
                break;
            default:
                mvLyr1 = 0; // 0000
                mvLyr2 = 0; // 0000
                for (int c = 0; c < cellsPerCol; c++)
                {
                    for (int cl = 0; cl < cols; cl++)
                    {
                        if ((c == 0) && (cl % 2 == 0))
                        {
                            encounterMapUD[cl, c] = -1;
                            encounterMapLR[cl, c] = -1;
                        }
                        else
                        {
                            encounterMapUD[cl, c] = 0;
                            encounterMapLR[cl, c] = 0;
                        }
                    }
                }
                break;
        }
    }

    // Copy constructor
    public CellData(CellData toCopy)
    {
        mvLyr1 = toCopy.mvLyr1;
        mvLyr2 = toCopy.mvLyr2;
        path = toCopy.path;
        model = toCopy.model;
        encounterMapUD = toCopy.encounterMapUD;
        encounterMapLR = toCopy.encounterMapLR;
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
    public void SetTop(bool mv, bool lyr)
    {
        int mvMask = 8; // 1000
        int stpMask = 7; // 0111
        MvSet(mvMask, stpMask, mv, lyr);
    }

    // Set bit 2 of mvLyr lyr to value of mv 
    // 0 is layer 1
    // 1 is layer 2
    public void SetLeft(bool mv, bool lyr)
    {
        int mvMask = 4; // 0100
        int stpMask = 11; // 1011
        MvSet(mvMask, stpMask, mv, lyr);
    }

    // Set bit 1 of mvLyr lyr to value of mv
    // 0 is layer 1
    // 1 is layer 2
    public void SetBot(bool mv, bool lyr)
    {
        int mvMask = 2; // 0010
        int stpMask = 13; // 1101
        MvSet(mvMask, stpMask, mv, lyr);
    }

    // Set bit 0 of mvLyr lyr to value of mv
    // 0 is layer 1
    // 1 is layer 2
    public void SetRight(bool mv, bool lyr)
    {
        int mvMask = 1; // 0001
        int stpMask = 14; // 1110
        MvSet(mvMask, stpMask, mv, lyr);
    }

    // Return if you can move up from the cell
    // 0 is layer 1
    // 1 is layer 2
    public bool CanMvTop(bool lyr)
    {
        int mask = 8; // 1000
        return MvCheck(mask, lyr);
    }

    // Return if you can move left from the cell
    // 0 is layer 1
    // 1 is layer 2
    public bool CanMvLeft(bool lyr)
    {
        int mask = 4; // 0100
        return MvCheck(mask, lyr);
    }

    // Return if you can move down from the cell
    // 0 is layer 1
    // 1 is layer 2
    public bool CanMvBot(bool lyr)
    {
        int mask = 2; // 0010
        return MvCheck(mask, lyr);
    }

    // Return if you can move right from the cell
    // 0 is layer 1
    // 1 is layer 2
    public bool CanMvRight(bool lyr)
    {
        int mask = 1; // 0001
        return MvCheck(mask, lyr);
    }

    // Set the path to the model
    public void SetPath(string m)
    {
        path = m;
    }

    // Set the 3D model of the cell
    public void SetModel(GameObject m)
    {
        model = m;
    }

    public GameObject GetModel()
    {
        return model;
    }

    public string GetPath()
    {
        return path;
    }
}
