using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellTypes : MonoBehaviour
{

    List<string> defaults = new List<string> { "D", "DC", "DI", "DW" };
    
    // Containers for each type of cell
    Dictionary<string, GameObject> floors = new Dictionary<string, GameObject>();
    Dictionary<string, GameObject> walls = new Dictionary<string, GameObject>();
    Dictionary<string, GameObject> platforms = new Dictionary<string, GameObject>();
    Dictionary<string, GameObject> ramps = new Dictionary<string, GameObject>();
    Dictionary<string, GameObject> special = new Dictionary<string, GameObject>();

	// Use this for initialization
	void Start () {
        // Load up default cells
        floors.Add("D", Instantiate(Resources.Load("Prefabs/Panel_0")) as GameObject);
        ramps.Add("D", Instantiate(Resources.Load("Prefabs/ramp")) as GameObject);
        walls.Add("DC", Instantiate(Resources.Load("Prefabs/wall_c")) as GameObject);
        walls.Add("DI", Instantiate(Resources.Load("Prefabs/wall_i")) as GameObject);
        walls.Add("DW", Instantiate(Resources.Load("Prefabs/wall_w")) as GameObject);
        platforms.Add("DC", Instantiate(Resources.Load("Prefabs/platform_c")) as GameObject);
        platforms.Add("DI", Instantiate(Resources.Load("Prefabs/platform_i")) as GameObject);
        platforms.Add("DW", Instantiate(Resources.Load("Prefabs/platform_w")) as GameObject);

        // Load up rest of cells
    }

    // Add new floor
    void NewFloor(string newKey, GameObject newFloor)
    {

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

    }

    // Remove platform cell
    void RmvPlat(string key)
    {
        // Don't remove the defaults
        if (!defaults.Contains(key))
        { platforms.Remove(key); }
        else { Debug.Log("Can't remove a default cell"); }
    }

    // Add special cell
    // Remove special cell
}
