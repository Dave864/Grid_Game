using System.Collections;
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

    // position and size for model preview
    private int prevX = 20;
    private int prevY = 20;
    private int prevLen;
    private int prevWdth;

    // position for movement GUI
    private int mvGuiGroupPosX;
    private int mvGuiGroupPosY;
    private int boxLen = 40;
    private int mvButWdth = 25;
    private int mvButLen = 40;
    private float LRButScale = 1.3f;
    private int lyrx;
    private int lyr1y;
    private int lyr2y;

    private string mvArrLROn_path = "Assets/Resources/Materials/GUI Images/Movement Arrow On LR.png";
    private string mvArrUDOn_path = "Assets/Resources/Materials/GUI Images/Movement Arrow On UD.png";
    private string mvArrLROff_path = "Assets/Resources/Materials/GUI Images/Movement Arrow Off LR.png";
    private string mvArrUDOff_path = "Assets/Resources/Materials/GUI Images/Movement Arrow Off UD.png";

    private void Awake()
    {
        mvGuiGroupPosX = prevX + 400;
        mvGuiGroupPosY = prevY;
        lyrx = mvGuiGroupPosX + 20 + mvButWdth;
        lyr2y = mvGuiGroupPosY + 20 + mvButWdth;
        lyr1y = lyr2y + (2*boxLen) + (2*mvButWdth);
        prevLen = mvGuiGroupPosX - prevX;
        prevWdth = mvGuiGroupPosY + lyr1y + boxLen + mvButWdth;
    }

    public static void AdvCellOpt(CellData cell, CELLTYPES t)
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
        // Movement GUI
        GUI.Box(new Rect(lyrx, lyr2y, boxLen, boxLen), "Layer 2");
        GUI.Box(new Rect(lyrx, lyr1y, boxLen, boxLen), "Layer 1");
        switch (curType)
        {
            case CELLTYPES.FLOOR:
                // Layer 2 Cell movement editor
                GUI.enabled = false;
                MovementGui(true);
                // Layer 1 Cell movement editor
                GUI.enabled = true;
                MovementGui(false);
                break;
            case CELLTYPES.WALL:
                // Layer 2 Cell movement editor
                GUI.enabled = false;
                MovementGui(true);
                // Layer 1 Cell movement editor
                MovementGui(false);
                break;
            case CELLTYPES.PLATFORM:
                // Layer 2 Cell movement editor
                GUI.enabled = true;
                MovementGui(true);
                // Layer 1 Cell movement editor
                GUI.enabled = false;
                MovementGui(false);
                break;
            case CELLTYPES.RAMP:
                GUI.enabled = true;
                // Layer 2 Cell movement editor
                MovementGui(true);
                // Layer 1 Cell movement editor
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
        GUI.enabled = true;

        // Create preview of model
        mdl = info.GetModel();
        if(mdl != null)
        {
            if(modelPreviewEditor == null)
            {
                modelPreviewEditor = Editor.CreateEditor(mdl);
            }
            modelPreviewEditor.OnPreviewGUI(new Rect(prevX, prevY, prevLen, prevWdth), modelPreviewStyle);
        }

        // Encounter area GUI
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
        if (info.CanMvTop(lyr))
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
            info.SetTop(!info.CanMvTop(lyr), lyr);
        }

        // Left
        if (info.CanMvLeft(lyr))
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
            info.SetLeft(!info.CanMvLeft(lyr), lyr);
        }

        // Bot
        if (info.CanMvBot(lyr))
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
            info.SetBot(!info.CanMvBot(lyr), lyr);
        }

        // Right
        if (info.CanMvRight(lyr))
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
            info.SetRight(!info.CanMvRight(lyr), lyr);
        }
    }
}
