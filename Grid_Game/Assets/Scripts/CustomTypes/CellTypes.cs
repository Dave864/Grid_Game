using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
