using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CellData
{
    // Arena map info for the cell
    // TODO: need to test for the "optimal" size
    public HexRect encounterMapUD;
    public HexRect encounterMapLR;

    // Movement options for oveworld cell
    public OverworldMvmt mvmt;

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
        encounterMapUD = new HexRect(GlobalVals.ENC_MAP_RAD);
        encounterMapLR = new HexRect(GlobalVals.ENC_MAP_RAD);
        int cellsPerCol = GlobalVals.ENC_MAP_HPC;
        int cols = GlobalVals.ENC_MAP_COL;
        int r, c;

        switch (type)
        {
            // Default floor
            case 0:
                mvmt = new OverworldMvmt(15, 0);
                for (int i = 0; i < (cellsPerCol * cols); i++)
                {
                    r = i / cols;
                    c = i - (r * cols);
                    encounterMapUD[r, c] = new EnctrCell(0);
                    encounterMapLR[r, c] = new EnctrCell(0);
                }
                break;
            // Default wall
            case 1:
                mvmt = new OverworldMvmt(0, 0);
                for (int i = 0; i < (cellsPerCol * cols); i++)
                {
                    r = i / cols;
                    c = i - (r * cols);
                    encounterMapUD[r, c] = new EnctrCell(0);
                    encounterMapLR[r, c] = new EnctrCell(0);
                }
                break;
            // Default platform
            case 2:
                mvmt = new OverworldMvmt(0, 15);
                for (int i = 0; i < (cellsPerCol * cols); i++)
                {
                    r = i / cols;
                    c = i - (r * cols);
                    // Borders
                    if (r == 0 || r == (cellsPerCol - 1) || c == 0 || c == (cols - 1))
                    {
                        encounterMapUD[r, c] = new EnctrCell(0);
                        encounterMapLR[r, c] = new EnctrCell(0);
                    }
                    // Center
                    else
                    {
                        encounterMapUD[r, c] = new EnctrCell(GlobalVals.ENC_MAP_MX_HT);
                        encounterMapLR[r, c] = new EnctrCell(GlobalVals.ENC_MAP_MX_HT);
                    }
                }
                break;
            // Default ramp
            case 3:
                mvmt = new OverworldMvmt(15, 15);
                for (int i = 0; i < (cellsPerCol * cols); i++)
                {
                    r = i / cols;
                    c = i - (r * cols);
                    // Up-Down orientation
                    encounterMapUD[r, c] = new EnctrCell(GlobalVals.ENC_MAP_HPC - r);
                    // Left-Right orientation
                    encounterMapLR[r, c] = new EnctrCell(c);
                }
                break;
            default:
                mvmt = new OverworldMvmt(0, 0);
                for (int i = 0; i < (cellsPerCol * cols); i++)
                {
                    r = i / cols;
                    c = i - (r * cols);
                    encounterMapUD[r, c] = null;
                    encounterMapLR[r, c] = null;
                }
                break;
        }
    }

    // Copy constructor
    public CellData(CellData toCopy)
    {
        path = toCopy.path;
        model = toCopy.model;
        mvmt = new OverworldMvmt(toCopy.mvmt);
        encounterMapUD = new HexRect(toCopy.encounterMapUD);
        encounterMapLR = new HexRect(toCopy.encounterMapLR);
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
