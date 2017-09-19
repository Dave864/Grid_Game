using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnctrCell
{
    public int height;
    // Texture
    // Default 3D model

    // Constructor
    public EnctrCell(int h = -1)
    {
        height = h;
    }

    // Copy Constructor
    public EnctrCell(EnctrCell toCopy)
    {
        if (toCopy != null)
        {
            height = toCopy.height;
        }
    }
}
