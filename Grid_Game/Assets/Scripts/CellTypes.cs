using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct CellData
{
    // Movement options are coded in 4 bits
    // 1, you can move in that direction
    // 0, you can't move in that direction
    // bit 3: up movement
    // bit 2: left movement
    // bit 1: right movement
    // bit 0: down movement

    // Movement options for the 'ground' layer
    int mvLyr1;
    // Movemnet options for the next layer up
    int mvLyr2;
    // Path to the 3D object
    private string path;

    CellData(string p, int type)
    {
        mvLyr1 = 0;
        mvLyr2 = 0;
        path = p;
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
    void setTop(bool mv, bool lyr)
    {
        int mvMask = 8; // 1000
        int stpMask = 7; // 0111
        mvSet(mvMask, stpMask, mv, lyr);
    }

    // Set bit 0 of mvLyr lyr to value of mv
    // 0 is layer 1
    // 1 is layer 2
    void setBot(bool mv, bool lyr)
    {
        int mvMask = 1; // 0001
        int stpMask = 14; // 1110
        mvSet(mvMask, stpMask, mv, lyr);
    }

    // Set bit 2 of mvLyr lyr to value of mv 
    // 0 is layer 1
    // 1 is layer 2
    void setLeft(bool mv, bool lyr)
    {
        int mvMask = 4; // 0100
        int stpMask = 11; // 1011
        mvSet(mvMask, stpMask, mv, lyr);
    }

    // Set bit 1 of mvLyr lyr to value of mv
    // 0 is layer 1
    // 1 is layer 2
    void setRight(bool mv, bool lyr)
    {
        int mvMask = 2; // 0010
        int stpMask = 13; // 1101
        mvSet(mvMask, stpMask, mv, lyr);
    }

    // Return if you can move up from the cell
    // 0 is layer 1
    // 1 is layer 2
    bool canMvTop(bool lyr)
    {
        int mask = 8; // 1000
        return mvCheck(mask, lyr);
    }

    // Return if you can move down from the cell
    // 0 is layer 1
    // 1 is layer 2
    bool canMvBot(bool lyr)
    {
        int mask = 1; // 0001
        return mvCheck(mask, lyr);
    }

    // Return if you can move left from the cell
    // 0 is layer 1
    // 1 is layer 2
    bool canMvLeft(bool lyr)
    {
        int mask = 4; // 0100
        return mvCheck(mask, lyr);
    }

    // Return if you can move right from the cell
    // 0 is layer 1
    // 1 is layer 2
    bool canMvRight(bool lyr)
    {
        int mask = 4; // 0010
        return mvCheck(mask, lyr);
    }

    string getPath()
    {
        return path;
    }
}

[ExecuteInEditMode]
public class CellTypes : MonoBehaviour
{

    private List<string> defaults = new List<string> { "D", "DC", "DI", "DW" };

    // Containers for each type of cell
    /*public Dictionary<string, GameObject> floors = new Dictionary<string, GameObject>();
    public Dictionary<string, GameObject> walls = new Dictionary<string, GameObject>();
    public Dictionary<string, GameObject> platforms = new Dictionary<string, GameObject>();
    public Dictionary<string, GameObject> ramps = new Dictionary<string, GameObject>();
    public Dictionary<string, GameObject> special = new Dictionary<string, GameObject>();*/

    // Debugging containers to test editor
    public List<string> floors;// = new Dictionary<string, string>();
    public List<string> walls;// = new Dictionary<string, string>();
    public List<string> platforms;// = new Dictionary<string, string>();
    public List<string> ramps;// = new Dictionary<string, string>();
    public List<string> special;// = new Dictionary<string, string>();

	// Use this for initialization
	void LoadTypes ()
    {
        // Load up default cells
        /*floors.Add("D", Instantiate(Resources.Load("Prefabs/Panel_0")) as GameObject);
        ramps.Add("D", Instantiate(Resources.Load("Prefabs/ramp")) as GameObject);
        walls.Add("DC", Instantiate(Resources.Load("Prefabs/wall_c")) as GameObject);
        walls.Add("DI", Instantiate(Resources.Load("Prefabs/wall_i")) as GameObject);
        walls.Add("DW", Instantiate(Resources.Load("Prefabs/wall_w")) as GameObject);
        platforms.Add("DC", Instantiate(Resources.Load("Prefabs/platform_c")) as GameObject);
        platforms.Add("DI", Instantiate(Resources.Load("Prefabs/platform_i")) as GameObject);
        platforms.Add("DW", Instantiate(Resources.Load("Prefabs/platform_w")) as GameObject);
        */

        // Load up rest of cells
    }

    /*
    // Add new floor
    void NewFloor(string newKey, GameObject newFloor)
    {
        floors[newKey] = newFloor;
    }

    // Remove floor cell
    void RmvFloor(string key)
    {
        // Don't remove the defaults
        if(!defaults.Contains(key))
        { floors.Remove(key); }
        else { Debug.Log("Can't remove a default cell"); }
    }

    // Add new wall
    void NewWall(string newKey, GameObject newWall)
    {
        walls[newKey] = newWall;
    }

    // Remove wall cell
    void RmvWall(string key)
    {
        // Don't remove the defaults
        if (!defaults.Contains(key))
        { walls.Remove(key); }
        else { Debug.Log("Can't remove a default cell"); }
    }

    // Add new platform
    void NewPlat(string newKey, GameObject newPlat)
    {
        platforms[newKey] = newPlat;
    }

    // Remove platform cell
    void RmvPlat(string key)
    {
        // Don't remove the defaults
        if (!defaults.Contains(key))
        { platforms.Remove(key); }
        else { Debug.Log("Can't remove a default cell"); }
    }
    */
    // Add special cell
    // Remove special cell
}