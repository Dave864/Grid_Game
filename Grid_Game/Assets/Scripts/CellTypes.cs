using System.Collections;
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
    // bit 1: right movement
    // bit 0: down movement

    // Movement options for the 'ground' layer
    [SerializeField]
    private int mvLyr1;
    // Movemnet options for the next layer up
    [SerializeField]
    private int mvLyr2;

    // Arena map for the cell
    // TODO: need to plan out the data structure

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
        switch (type)
        {
            // Default floor
            case 0:
                mvLyr1 = 15; // 1111
                mvLyr2 = 0; // 0000
                break;
            // Default wall
            case 1:
                mvLyr1 = 0; // 0000
                mvLyr2 = 0; // 0000
                break;
            // Default platform
            case 2:
                mvLyr1 = 0; // 0000
                mvLyr2 = 15; // 1111
                break;
            // Default ramp
            case 3:
                mvLyr1 = 15; // 1111
                mvLyr2 = 15; // 1111
                break;
            default:
                mvLyr1 = 0; // 0000
                mvLyr2 = 0; // 0000
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
    }

    // Helper function used to set movement options
    private void mvSet(int mvMask, int stpMask, bool mv, bool lyr)
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
    private bool mvCheck(int mask, bool lyr)
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
    public void setTop(bool mv, bool lyr)
    {
        int mvMask = 8; // 1000
        int stpMask = 7; // 0111
        mvSet(mvMask, stpMask, mv, lyr);
    }

    // Set bit 0 of mvLyr lyr to value of mv
    // 0 is layer 1
    // 1 is layer 2
    public void setBot(bool mv, bool lyr)
    {
        int mvMask = 1; // 0001
        int stpMask = 14; // 1110
        mvSet(mvMask, stpMask, mv, lyr);
    }

    // Set bit 2 of mvLyr lyr to value of mv 
    // 0 is layer 1
    // 1 is layer 2
    public void setLeft(bool mv, bool lyr)
    {
        int mvMask = 4; // 0100
        int stpMask = 11; // 1011
        mvSet(mvMask, stpMask, mv, lyr);
    }

    // Set bit 1 of mvLyr lyr to value of mv
    // 0 is layer 1
    // 1 is layer 2
    public void setRight(bool mv, bool lyr)
    {
        int mvMask = 2; // 0010
        int stpMask = 13; // 1101
        mvSet(mvMask, stpMask, mv, lyr);
    }

    // Return if you can move up from the cell
    // 0 is layer 1
    // 1 is layer 2
    public bool canMvTop(bool lyr)
    {
        int mask = 8; // 1000
        return mvCheck(mask, lyr);
    }

    // Return if you can move down from the cell
    // 0 is layer 1
    // 1 is layer 2
    public bool canMvBot(bool lyr)
    {
        int mask = 1; // 0001
        return mvCheck(mask, lyr);
    }

    // Return if you can move left from the cell
    // 0 is layer 1
    // 1 is layer 2
    public bool canMvLeft(bool lyr)
    {
        int mask = 4; // 0100
        return mvCheck(mask, lyr);
    }

    // Return if you can move right from the cell
    // 0 is layer 1
    // 1 is layer 2
    public bool canMvRight(bool lyr)
    {
        int mask = 4; // 0010
        return mvCheck(mask, lyr);
    }

    // Set the path to some string
    public void setPath(string m)
    {
        path = m;
    }

    // Set the 3D model of the cell
    public void setModel(GameObject m)
    {
        model = m;
    }

    public GameObject getModel()
    {
        return model;
    }

    public string getPath()
    {
        return path;
    }
}

[ExecuteInEditMode]
[System.Serializable]
public class CellTypes : MonoBehaviour
{
    // Containers for each type of cell
    public List<CellData> floors;
    public List<CellData> walls;
    public List<CellData> platforms;
    public List<CellData> ramps;
    public List<CellData> special;

    // constructor
    public CellTypes()
    {
        floors = new List<CellData>();
        walls = new List<CellData>();
        platforms = new List<CellData>();
        ramps = new List<CellData>();
        special = new List<CellData>();
    }

    // Copy constructor
    public CellTypes(CellTypes toCopy)
    {
        floors = new List<CellData>(toCopy.floors);
        walls = new List<CellData>(toCopy.walls);
        platforms = new List<CellData>(toCopy.platforms);
        ramps = new List<CellData>(toCopy.ramps);
        special = new List<CellData>(toCopy.special);
    }
}
