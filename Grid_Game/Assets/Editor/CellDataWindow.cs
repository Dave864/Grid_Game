﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

// Window for editing the information of a cell
public class CellDataWindow : EditorWindow
{
    Editor modelPreviewEditor;
    GUIStyle modelPreviewStyle;
    GameObject mdl;

    public CellData info;
    public CELLTYPES curType;

    private int lyrx = 60;
    private int lyr1y = 225;
    private int lyr2y = 50;
    private int boxLen = 40;
    private int mvButWdth = 25;
    private int mvButLen = 40;
    private float LRButScale = 1.3f;

    private string mvArrLROn_path = "Assets/Resources/Materials/GUI Images/Movement Arrow On LR.png";
    private string mvArrUDOn_path = "Assets/Resources/Materials/GUI Images/Movement Arrow On UD.png";
    private string mvArrLROff_path = "Assets/Resources/Materials/GUI Images/Movement Arrow Off LR.png";
    private string mvArrUDOff_path = "Assets/Resources/Materials/GUI Images/Movement Arrow Off UD.png";

    public static void advCellOpt(CellData cell, CELLTYPES t)
    {
        CellDataWindow window = (CellDataWindow)GetWindow(typeof(CellDataWindow), true, "Cell Data");
        window.info = new CellData(cell);
        window.curType = t;

        window.modelPreviewStyle = new GUIStyle();
        window.modelPreviewStyle.stretchWidth = true;
        window.modelPreviewStyle.normal.background = EditorGUIUtility.whiteTexture;

        window.ShowAuxWindow();
    }

    private void OnGUI()
    {
        switch (curType)
        {
            case CELLTYPES.FLOOR:
                // Layer 2 Cell movement editor
                GUI.Box(new Rect(lyrx, lyr2y, boxLen, boxLen), "Layer 2");
                GUI.enabled = false;
                MovementGui(true);
                // Layer 1 Cell movement editor
                GUI.enabled = true;
                GUI.Box(new Rect(lyrx, lyr1y, boxLen, boxLen), "Layer 1");
                MovementGui(false);
                break;
            case CELLTYPES.WALL:
                // Layer 2 Cell movement editor
                GUI.Box(new Rect(lyrx, lyr2y, boxLen, boxLen), "Layer 2");
                GUI.enabled = false;
                MovementGui(true);
                // Layer 1 Cell movement editor
                GUI.enabled = true;
                GUI.Box(new Rect(lyrx, lyr1y, boxLen, boxLen), "Layer 1");
                GUI.enabled = false;
                MovementGui(false);
                break;
            case CELLTYPES.PLATFORM:
                // Layer 2 Cell movement editor
                GUI.enabled = true;
                GUI.Box(new Rect(lyrx, lyr2y, boxLen, boxLen), "Layer 2");
                MovementGui(true);
                // Layer 1 Cell movement editor
                GUI.Box(new Rect(lyrx, lyr1y, boxLen, boxLen), "Layer 1");
                GUI.enabled = false;
                MovementGui(false);
                break;
            case CELLTYPES.RAMP:
                GUI.enabled = true;
                // Layer 2 Cell movement editor
                GUI.Box(new Rect(lyrx, lyr2y, boxLen, boxLen), "Layer 2");
                MovementGui(true);
                // Layer 1 Cell movement editor
                GUI.Box(new Rect(lyrx, lyr1y, boxLen, boxLen), "Layer 1");
                MovementGui(false);
                break;
            default:
                GUI.enabled = false;
                // Layer 2 Cell movement editor
                GUI.Box(new Rect(lyrx, lyr2y, boxLen, boxLen), "Layer 2");
                MovementGui(true);
                // Layer 1 Cell movement editor
                GUI.Box(new Rect(lyrx, lyr1y, boxLen, boxLen), "Layer 1");
                MovementGui(false);
                Debug.LogError("Tried to access CellData of special");
                break;
        }
        
        // Create preview of model
        mdl = info.getModel();
        if(mdl != null)
        {
            if(modelPreviewEditor == null)
            {
                modelPreviewEditor = Editor.CreateEditor(mdl);
            }
            modelPreviewEditor.OnPreviewGUI(new Rect(lyrx + (3 * boxLen), lyr2y - mvButWdth, 400, 300), modelPreviewStyle);
        }
    }

    // Interface for editing the movement options of a cell
    // true is layer 2
    // false is layer 1
    private void MovementGui(bool lyr)
    {
        Texture2D mvArrowLR;
        Texture2D mvArrowUD;
        Vector2 pos;

        // Top
        if (info.canMvTop(lyr))
        {
            mvArrowUD = (Texture2D)AssetDatabase.LoadAssetAtPath(mvArrUDOn_path, typeof(Texture2D));
        }
        else
        {
            mvArrowUD = (Texture2D)AssetDatabase.LoadAssetAtPath(mvArrUDOff_path, typeof(Texture2D));
        }
        pos = (lyr) ? new Vector2(lyrx, lyr2y - mvButWdth) : new Vector2(lyrx, lyr1y - mvButWdth);
        if(GUI.Button(new Rect(pos, new Vector2(mvButLen, mvButWdth)), mvArrowUD))
        {
            info.setTop(!info.canMvTop(lyr), lyr);
        }

        // Left
        if (info.canMvLeft(lyr))
        {
            mvArrowLR = (Texture2D)AssetDatabase.LoadAssetAtPath(mvArrLROn_path, typeof(Texture2D));
        }
        else
        {
            mvArrowLR = (Texture2D)AssetDatabase.LoadAssetAtPath(mvArrLROff_path, typeof(Texture2D));
        }
        pos = (lyr) ? new Vector2(lyrx - (mvButWdth * LRButScale), lyr2y) : new Vector2(lyrx - (mvButWdth * LRButScale), lyr1y);
        if (GUI.Button(new Rect(pos, new Vector2(mvButWdth * LRButScale, mvButLen)), mvArrowLR))
        {
            info.setLeft(!info.canMvLeft(lyr), lyr);
        }

        // Bot
        if (info.canMvBot(lyr))
        {
            mvArrowUD = (Texture2D)AssetDatabase.LoadAssetAtPath(mvArrUDOn_path, typeof(Texture2D));
        }
        else
        {
            mvArrowUD = (Texture2D)AssetDatabase.LoadAssetAtPath(mvArrUDOff_path, typeof(Texture2D));
        }
        pos = (lyr) ? new Vector2(lyrx, lyr2y + mvButLen) : new Vector2(lyrx, lyr1y + mvButLen);
        if (GUI.Button(new Rect(pos, new Vector2(mvButLen, mvButWdth)), mvArrowUD))
        {
            info.setBot(!info.canMvBot(lyr), lyr);
        }

        // Right
        if (info.canMvRight(lyr))
        {
            mvArrowLR = (Texture2D)AssetDatabase.LoadAssetAtPath(mvArrLROn_path, typeof(Texture2D));
        }
        else
        {
            mvArrowLR = (Texture2D)AssetDatabase.LoadAssetAtPath(mvArrLROff_path, typeof(Texture2D));
        }
        pos = (lyr) ? new Vector2(lyrx + mvButLen, lyr2y) : new Vector2(lyrx + mvButLen, lyr1y);
        if (GUI.Button(new Rect(pos, new Vector2(mvButWdth * LRButScale, mvButLen)), mvArrowLR))
        {
            info.setRight(!info.canMvRight(lyr), lyr);
        }
    }
}
