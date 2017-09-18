using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnctrCell
{
    public int height;
    // Texture
    // Default 3D model

    // Constructor
    public EnctrCell(int h = 0)
    {
        height = h;
    }

    // Copy Constructor
    public EnctrCell(EnctrCell toCopy)
    {
        height = toCopy.height;
    }
}
