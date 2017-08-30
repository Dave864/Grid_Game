using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexRect<D>
{
    private int col;
    private int hPc;
    private bool sharpCor;

    private D[,] rect;

    // Constructor
    public HexRect(int radius = 0)
    {
        hPc = 2 * (radius + 1);
        col = hPc + 1;
        sharpCor = (radius % 2 == 0) ? true : false;
        rect = new D[hPc, col];

        for (int r = 0; r < hPc; r++)
        {
            for(int c = 0; c < col; c++)
            {
                rect[r, c] = default(D);
            }
        }
    }

    // Copy Constructor
    public HexRect(HexRect<D> toCopy)
    {
        col = toCopy.col;
        hPc = toCopy.hPc;
        sharpCor = toCopy.sharpCor;
        rect = new D[hPc, col];
        for (int r = 0; r < hPc; r++)
        {
            for (int c = 0; c < col; c++)
            {
                rect[r, c] = toCopy.rect[r, c];
            }
        }
    }

    // Index Operator
    public D this[int rowKey, int colKey]
    {
        get
        {
            if(sharpCor && ((colKey + 1) % 2 == 0) && (rowKey == (hPc - 1)))
            {
                return default(D);
            }
            else if(!sharpCor && (colKey % 2 == 0) && (rowKey == 0))
            {
                return default(D);
            }
            else
            {
                return rect[rowKey, colKey];
            }
        }
        set
        {
            if (sharpCor && ((colKey + 1) % 2 == 0) && (rowKey == (hPc - 1)))
            {
                return;
            }
            else if (!sharpCor && (colKey % 2 == 0) && (rowKey == 0))
            {
                return;
            }
            else
            {
                rect[rowKey, colKey] = value;
            }
        }
    }

    // Make a HexRect rotated 180 degrees
    public HexRect<D> getRot180()
    {
        HexRect<D> rotHex = new HexRect<D>(this);
        if (sharpCor)
        {
            // Iterate through each column
            for (int c = 0; c < col; c++)
            {
                // If column is even index
                if (c % 2 == 0)
                {
                    // for each cell in column
                    for (int r = 0; r < hPc; r++)
                    {
                        rotHex[r, c] = rect[(hPc - 1) - r, (col - 1) - c];
                    }
                }
                // If column is odd index
                else
                {
                    // for all but bottom cell in column
                    for (int r = 0; r < (hPc-1); r++)
                    {
                        rotHex[r, c] = rect[(hPc - 2) - r, (col - 1) - c];
                    }
                }
            }
        }
        else
        {
            // Iterate through each column
            for (int c = 0; c < col; c++)
            {
                // If column is even index
                if (c % 2 == 0)
                {
                    // for all but top cell in column
                    for (int r = 1; r < hPc; r++)
                    {
                        rotHex[r, c] = rect[(hPc - 1) - r, (col - 1) - c];
                    }
                }
                // If column is odd index
                else
                {
                    // for each cell in column
                    for (int r = 0; r < hPc; r++)
                    {
                        rotHex[r, c] = rect[(hPc - 1) - r, (col - 1) - c];
                    }
                }
            }
        }
        return rotHex;
    }

    // Rotate rect 180 degrees
    public void rot180()
    {
        D tmp;
        if (sharpCor)
        {
            // Iterate through half of columns
            for (int c = 0; c < (col/2); c++)
            {
                // If column is even index
                if (c % 2 == 0)
                {
                    // for each cell in column
                    for (int r = 0; r < (hPc/2); r++)
                    {
                        
                    }
                }
                // If column is odd index
                else
                {
                    // for all but bottom cell in column
                    for (int r = 0; r < (hPc/2); r++)
                    {

                    }
                }
            }
        }
        else
        {
            // Iterate through half of columns
            for (int c = 0; c < (col/2); c++)
            {
                // If column is even index
                if (c % 2 == 0)
                {
                    // for all but top cell in column
                    for (int r = 1; r < hPc; r++)
                    {

                    }
                }
                // If column is odd index
                else
                {
                    // for each cell in column
                    for (int r = 0; r < hPc; r++)
                    {

                    }
                }
            }
        }
    }
}
