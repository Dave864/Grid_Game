using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class HexRect
{
    [SerializeField]
    private int cCnt;
    [SerializeField]
    private int hPc;
    [SerializeField]
    private int rad;
    [SerializeField]
    private bool sharpCor;

    [SerializeField]
    private EnctrCell[] hRect;

    // Constructor
    public HexRect(int radius = 0)
    {
        rad = radius;
        hPc = 2 * (radius + 1);
        cCnt = hPc + 1;
        sharpCor = (radius % 2 == 0) ? true : false;
        hRect = new EnctrCell[hPc * cCnt];

        for (int i = 0; i < (hPc * cCnt); i++)
        {
            hRect[i] = null;
        }
    }

    // Copy Constructor
    public HexRect(HexRect toCopy)
    {
        cCnt = toCopy.cCnt;
        hPc = toCopy.hPc;
        sharpCor = toCopy.sharpCor;
        hRect = new EnctrCell[hPc * cCnt];

        for (int i = 0; i < (hPc * cCnt); i++)
        {
            hRect[i] = toCopy.hRect[i];
        }
    }

    // Index Operator
    public EnctrCell this[int rowKey, int colKey]
    {
        get
        {
            if (sharpCor && (colKey % 2 != 0) && (rowKey == (hPc - 1)))
            {
                return null;
            }
            else if (!sharpCor && (colKey % 2 == 0) && (rowKey == 0))
            {
                return null;
            }
            else
            {
                return hRect[(rowKey * cCnt) + colKey];
            }
        }
        set
        {
            if (sharpCor && (colKey % 2 != 0) && (rowKey == (hPc - 1)))
            {
                hRect[(rowKey * cCnt) + colKey] = new EnctrCell();
            }
            else if (!sharpCor && (colKey % 2 == 0) && (rowKey == 0))
            {
                hRect[(rowKey * cCnt) + colKey] = new EnctrCell();
            }
            else
            {
                hRect[(rowKey * cCnt) + colKey] = value;
            }
        }
    }

    // Get row count
    public int HexPerRow()
    {
        return hPc;
    }

    // Get column count
    public int ColCnt()
    {
        return cCnt;
    }

    // Is corner sharp
    public bool SharpCor()
    {
        return sharpCor;
    }

    // Make a HexRect rotated 180 degrees
    public HexRect GetRot180()
    {
        HexRect hRectRot = new HexRect(this);
        if (sharpCor)
        {
            // Iterate through each column
            for (int c = 0; c < cCnt; c++)
            {
                // If column is even index
                if (c % 2 == 0)
                {
                    // for each cell in column
                    for (int r = 0; r < hPc; r++)
                    {
                        hRectRot[r, c] = hRect[(((hPc - 1) - r) * cCnt) + (cCnt - 1) - c];
                    }
                }
                // If column is odd index
                else
                {
                    // for all but bottom cell in column
                    for (int r = 0; r < (hPc - 1); r++)
                    {
                        hRectRot[r, c] = hRect[(((hPc - 2) - r) * cCnt) + (cCnt - 1) - c];
                    }
                }
            }
        }
        else
        {
            // Iterate through each column
            for (int c = 0; c < cCnt; c++)
            {
                // If column is even index
                if (c % 2 == 0)
                {
                    // for all but top cell in column
                    for (int r = 1; r < hPc; r++)
                    {
                        hRectRot[r, c] = hRect[(((hPc - 1) - r) * cCnt) + (cCnt - 1) - c];
                    }
                }
                // If column is odd index
                else
                {
                    // for each cell in column
                    for (int r = 0; r < hPc; r++)
                    {
                        hRectRot[r, c] = hRect[(((hPc - 1) - r) * cCnt) + (cCnt - 1) - c];
                    }
                }
            }
        }
        return hRectRot;
    }

    // Rotate rect 180 degrees
    public void Rot180()
    {
        EnctrCell tmp;
        if (sharpCor)
        {
            // Iterate through half of columns
            for (int c = 0; c <= (cCnt / 2); c++)
            {
                // If column is even index
                if (c % 2 == 0)
                {
                    // for half of cells in column
                    for (int r = 0; r <= (hPc / 2); r++)
                    {
                        tmp = hRect[(r * hPc) + c];
                        hRect[(r * hPc) + c] = hRect[(((hPc - 1) - r) * cCnt) + (cCnt - 1) - c];
                        hRect[(((hPc - 1) - r) * cCnt) + (cCnt - 1) - c] = tmp;
                    }
                }
                // If column is odd index
                else
                {
                    // for half of cells in column not at the bottom
                    for (int r = 0; r <= (hPc / 2); r++)
                    {
                        tmp = hRect[(r * hPc) + c];
                        hRect[(r * hPc) + c] = hRect[(((hPc - 2) - r) * cCnt) + (cCnt - 1) - c];
                        hRect[(((hPc - 2) - r) * cCnt) + (cCnt - 1) - c] = tmp;
                    }
                }
            }
        }
        else
        {
            // Iterate through half of columns
            for (int c = 0; c <= (cCnt / 2); c++)
            {
                // If column is even index
                if (c % 2 == 0)
                {
                    // for half of cells in column not at the top
                    for (int r = 1; r <= (hPc / 2); r++)
                    {
                        tmp = hRect[(r * hPc) + c];
                        hRect[(r * hPc) + c] = hRect[(((hPc - 1) - r) * cCnt) + (cCnt - 1) - c];
                        hRect[(((hPc - 1) - r) * cCnt) + (cCnt - 1) - c] = tmp;
                    }
                }
                // If column is odd index
                else
                {
                    // for half of cells in column
                    for (int r = 0; r <= (hPc / 2); r++)
                    {
                        tmp = hRect[(r * hPc) + c];
                        hRect[(r * hPc) + c] = hRect[(((hPc - 1) - r) * cCnt) + (cCnt - 1) - c];
                        hRect[(((hPc - 1) - r) * cCnt) + (cCnt - 1) - c] = tmp;
                    }
                }
            }
        }
    }

    // Make a HexRect with trimmed corners
    // Argument is true if corner is to be trimmed
    public HexRect TrimCorners(bool TL = false, bool TR = false, bool BL = false, bool BR = false)
    {
        HexRect hRectTrim = new HexRect(this);
        
        // dimensions for corner rectangle
        int tCorRowCnt;
        int bCorRowCnt;
        int corColCnt;

        if (rad == 0)
        {
            // trim top-left corner
            if (TL)
            {
                hRectTrim[0, 0] = new EnctrCell();
            }
            // trim top-right corner
            if (TR)
            {
                hRectTrim[0, (cCnt - 1)] = new EnctrCell();
            }
            // trim bottom-left corner
            if (BL)
            {
                hRectTrim[(hPc - 1), 0] = new EnctrCell();
            }
            // trim bottom-right corner
            if (BR)
            {
                hRectTrim[(hPc - 1), (cCnt - 1)] = new EnctrCell();
            }
        }
        else if (rad == 1)
        {
            // trim top-left corner
            if (TL)
            {
                hRectTrim[0, 1] = new EnctrCell();
                hRectTrim[1, 0] = new EnctrCell();
            }            
            // trim top-right corner
            if (TR)
            {
                hRectTrim[0, (cCnt - 2)] = new EnctrCell();
                hRectTrim[1, (cCnt - 1)] = new EnctrCell();
            }           
            // trim bottom-left corner
            if (BL)
            {
                hRectTrim[(hPc - 1), 0] = new EnctrCell();
                hRectTrim[(hPc - 1), 1] = new EnctrCell();
            }           
            // trim bottom-right corner
            if (BR)
            {
                hRectTrim[(hPc - 1), (cCnt - 1)] = new EnctrCell();
                hRectTrim[(hPc - 1), (cCnt - 2)] = new EnctrCell();
            }            
        }
        else if(sharpCor)
        {
            // top and bottom dimensions are the same
            tCorRowCnt = rad / 2;
            corColCnt = rad - 1;

            for (int r = 0; r < tCorRowCnt; r++)
            {
                for (int c = 0; c < corColCnt; c++)
                {
                    // trim top-left corner
                    if (TL && (c < corColCnt - (2 * r)))
                    {
                        hRectTrim[r, c] = new EnctrCell();
                    }
                    // trim top-right corner
                    if (TR && (c >= 2 * r))
                    {
                        hRectTrim[r, c + (cCnt - corColCnt)] = new EnctrCell();
                    }
                    // trim bottom-left corner
                    if (BL && (c < 2 * (r + 1)))
                    {
                        hRectTrim[r + (hPc - tCorRowCnt), c] = new EnctrCell();
                    }
                    // trim bottom-right corner
                    if (BR && (c >= (corColCnt - (2 * (r + 1)))))
                    {
                        hRectTrim[r + (hPc - tCorRowCnt), c + (cCnt - corColCnt)] = new EnctrCell();
                    }
                }
            }
        }
        else
        {
            tCorRowCnt = ((rad - 1) / 2) + 1;
            bCorRowCnt = (rad - 1) / 2;
            corColCnt = rad - 1;
            for (int r = 0; r < tCorRowCnt; r++)
            {
                for (int c = 0; c < corColCnt; c++)
                {
                    // trim top-left corner
                    if (TL && ((c < corColCnt - (2 * r) + 1) || (c == 0)))
                    {
                        hRectTrim[r, c] = new EnctrCell();
                    }
                    // trim top-right corner
                    if (TR && ((c >= (2 * r) - 1) || (c == corColCnt - 1)))
                    {
                        hRectTrim[r, c + (cCnt - corColCnt)] = new EnctrCell();
                    }
                    if (r < bCorRowCnt)
                    {
                        // trim bottom-left corner
                        if (BL && (c < (r + 1) * 2))
                        {
                            hRectTrim[r + (hPc - tCorRowCnt + 1), c] = new EnctrCell();
                        }
                        // trim bottom-right corner
                        if (BR && (c >= corColCnt - ((r + 1) * 2)))
                        {
                            hRectTrim[r + (hPc - tCorRowCnt + 1), c + (cCnt - corColCnt)] = new EnctrCell();
                        }
                    }
                }
            }
        }
        return hRectTrim;
    }
}
