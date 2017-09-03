using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexRect<D>
{
    private int col;
    private int hPc;
    private int rad;
    private bool sharpCor;

    private D[,] hRect;

    // Constructor
    public HexRect(int radius = 0)
    {
        rad = radius;
        hPc = 2 * (radius + 1);
        col = hPc + 1;
        sharpCor = (radius % 2 == 0) ? true : false;
        hRect = new D[hPc, col];

        for (int r = 0; r < hPc; r++)
        {
            for (int c = 0; c < col; c++)
            {
                hRect[r, c] = default(D);
            }
        }
    }

    // Copy Constructor
    public HexRect(HexRect<D> toCopy)
    {
        col = toCopy.col;
        hPc = toCopy.hPc;
        sharpCor = toCopy.sharpCor;
        hRect = new D[hPc, col];
        for (int r = 0; r < hPc; r++)
        {
            for (int c = 0; c < col; c++)
            {
                hRect[r, c] = toCopy.hRect[r, c];
            }
        }
    }

    // Index Operator
    public D this[int rowKey, int colKey]
    {
        get
        {
            if (sharpCor && ((colKey + 1) % 2 == 0) && (rowKey == (hPc - 1)))
            {
                return default(D);
            }
            else if (!sharpCor && (colKey % 2 == 0) && (rowKey == 0))
            {
                return default(D);
            }
            else
            {
                return hRect[rowKey, colKey];
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
                hRect[rowKey, colKey] = value;
            }
        }
    }

    // Make a HexRect rotated 180 degrees
    public HexRect<D> GetRot180()
    {
        HexRect<D> hRectRot = new HexRect<D>(this);
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
                        hRectRot[r, c] = hRect[(hPc - 1) - r, (col - 1) - c];
                    }
                }
                // If column is odd index
                else
                {
                    // for all but bottom cell in column
                    for (int r = 0; r < (hPc - 1); r++)
                    {
                        hRectRot[r, c] = hRect[(hPc - 2) - r, (col - 1) - c];
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
                        hRectRot[r, c] = hRect[(hPc - 1) - r, (col - 1) - c];
                    }
                }
                // If column is odd index
                else
                {
                    // for each cell in column
                    for (int r = 0; r < hPc; r++)
                    {
                        hRectRot[r, c] = hRect[(hPc - 1) - r, (col - 1) - c];
                    }
                }
            }
        }
        return hRectRot;
    }

    // Rotate rect 180 degrees
    public void Rot180()
    {
        D tmp;
        if (sharpCor)
        {
            // Iterate through half of columns
            for (int c = 0; c <= (col / 2); c++)
            {
                // If column is even index
                if (c % 2 == 0)
                {
                    // for half of cells in column
                    for (int r = 0; r <= (hPc / 2); r++)
                    {
                        tmp = hRect[r, c];
                        hRect[r, c] = hRect[(hPc - 1) - r, (col - 1) - c];
                        hRect[(hPc - 1) - r, (col - 1) - c] = tmp;
                    }
                }
                // If column is odd index
                else
                {
                    // for half of cells in column not at the bottom
                    for (int r = 0; r <= (hPc / 2); r++)
                    {
                        tmp = hRect[r, c];
                        hRect[r, c] = hRect[(hPc - 2) - r, (col - 1) - c];
                        hRect[(hPc - 2) - r, (col - 1) - c] = tmp;
                    }
                }
            }
        }
        else
        {
            // Iterate through half of columns
            for (int c = 0; c <= (col / 2); c++)
            {
                // If column is even index
                if (c % 2 == 0)
                {
                    // for half of cells in column not at the top
                    for (int r = 1; r <= (hPc / 2); r++)
                    {
                        tmp = hRect[r, c];
                        hRect[r, c] = hRect[(hPc - 1) - r, (col - 1) - c];
                        hRect[(hPc - 1) - r, (col - 1) - c] = tmp;
                    }
                }
                // If column is odd index
                else
                {
                    // for half of cells in column
                    for (int r = 0; r <= (hPc / 2); r++)
                    {
                        tmp = hRect[r, c];
                        hRect[r, c] = hRect[(hPc - 1) - r, (col - 1) - c];
                        hRect[(hPc - 1) - r, (col - 1) - c] = tmp;
                    }
                }
            }
        }
    }

    // Make a HexRect with trimmed corners
    // Argument is true if corner is to be trimmed
    public HexRect<D> TrimCorners(bool TL = false, bool TR = false, bool BL = false, bool BR = false)
    {
        HexRect<D> hRectTrim = new HexRect<D>(this);
        
        // dimensions for corner rectangle
        int tCorRowCnt;
        int bCorRowCnt;
        int corColCnt;

        if (rad == 0)
        {
            // trim top-left corner
            hRectTrim[0, 0] = default(D);
            // trim top-right corner
            hRectTrim[0, (col - 1)] = default(D);
            // trim bottom-left corner
            hRectTrim[(hPc - 1), 0] = default(D);
            // trim bottom-right corner
            hRectTrim[(hPc - 1), (col - 1)] = default(D);
        }
        else if (rad == 1)
        {
            // trim top-left corner
            hRectTrim[0, 1] = default(D);
            hRectTrim[1, 0] = default(D);
            // trim top-right corner
            hRectTrim[0, (col - 2)] = default(D);
            hRectTrim[1, (col - 1)] = default(D);
            // trim bottom-left corner
            hRectTrim[(hPc - 1), 0] = default(D);
            hRectTrim[(hPc - 1), 1] = default(D);
            // trim bottom-right corner
            hRectTrim[(hPc - 1), (col - 1)] = default(D);
            hRectTrim[(hPc - 1), (col - 2)] = default(D);
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
                        hRectTrim[r, c] = default(D);
                    }
                    // trim top-right corner
                    else if (TR && (c >= (2 * r)))
                    {
                        hRectTrim[r, c + (col - corColCnt - 1)] = default(D);
                    }
                    // trim bottom-left corner
                    else if (BL && (c < 2 * (r + 1)))
                    {
                        hRectTrim[r + (tCorRowCnt - hPc - 1), c] = default(D);
                    }
                    // trim bottom-riht corner
                    else if (BR && (c >= corColCnt - (2 * (c + 1))))
                    {
                        hRectTrim[r + (tCorRowCnt - hPc - 1), c + (col - corColCnt - 1)] = default(D);
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
                    if (TL)
                    {
                        hRectTrim[r, c] = default(D);
                    }
                    // trim top-right corner
                    else if (TR)
                    {
                        hRectTrim[r, c + (col - corColCnt - 1)] = default(D);
                    }
                    if (r < bCorRowCnt)
                    {
                        // trim bottom-left corner
                        if (BL)
                        {
                            hRectTrim[r + (tCorRowCnt - hPc - 1), c] = default(D);
                        }
                        // trim bottom-riht corner
                        else if (BR)
                        {
                            hRectTrim[r + (tCorRowCnt - hPc - 1), c + (col - corColCnt - 1)] = default(D);
                        }
                    }
                }
            }
        }
        return hRectTrim;
    }
}
