using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapMaker : MonoBehaviour
{

    // The dimensions of the map
    public int mapRowCnt;
    public int mapColCnt;
    public int secWidth;

    public int Sections
    {
        get
        {
            return mapRowCnt * mapColCnt;
        }
    }

	public int CellCount
    {
        get
        {
            return secWidth * secWidth;
        }
    }

    // Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    // Save the current map
    void SaveMap()
    {

    }

    // Physically load up the map to see its cells
    void LoadMap()
    {

    }
}
