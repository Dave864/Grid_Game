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
    private int prevX = 120;
    private int prevY = 0;
    private int prevLen = 500;
    private int prevWdth = 200;

    // position and sizes for movement GUI
    private int boxLen = 40;
    private int mvButWdth = 25;
    private int mvButLen = 40;
    private int elemSpacing = 8;
    private float LRButScale = 1.3f;

    private string mvArrLROn_path = "Assets/Resources/Materials/GUI Images/Movement Arrow On LR.png";
    private string mvArrUDOn_path = "Assets/Resources/Materials/GUI Images/Movement Arrow On UD.png";
    private string mvArrLROff_path = "Assets/Resources/Materials/GUI Images/Movement Arrow Off LR.png";
    private string mvArrUDOff_path = "Assets/Resources/Materials/GUI Images/Movement Arrow Off UD.png";

    public static void AdvCellOpt(CellData cell, CELLTYPES t)
    {
        CellDataWindow window = (CellDataWindow)GetWindow(typeof(CellDataWindow), true, "Cell Data");
        window.info = new CellData(cell);
        window.curType = t;

        window.modelPreviewStyle = new GUIStyle
        {
            stretchWidth = true
        };

        window.modelPreviewStyle.normal.background = EditorGUIUtility.whiteTexture;

        window.ShowAuxWindow();
    }

    private void OnGUI()
    {
        EditorGUILayout.BeginHorizontal();
        // Movement GUI
        MovementGui();
        // Model Preview
        Rect mdlPrevSpace = new Rect(prevX, prevY, prevLen, prevWdth);
        //EditorGUILayout.Space();
        GUILayout.Box("", GUILayout.ExpandWidth(true), GUILayout.Height(prevWdth));
        // Create preview of model
        mdl = info.GetModel();
        if (mdl != null)
        {
            if (modelPreviewEditor == null)
            {
                modelPreviewEditor = Editor.CreateEditor(mdl/*, typeof(CellModelPreview)*/);
            }
            //modelPreviewEditor.OnPreviewGUI(new Rect(prevX, prevY, prevLen, prevWdth), modelPreviewStyle);
            //modelPreviewEditor.OnPreviewGUI(new Rect(120, 0, 500, 200), modelPreviewStyle);
        }
        EditorGUILayout.EndHorizontal();
        // Encounter area GUI
    }

    // Interface for editing movement settings of a cell
    private void MovementGui()
    {
        bool butEnableLyr1;
        bool butEnableLyr2;
        EditorGUILayout.BeginVertical("Box", GUILayout.Width((2 * elemSpacing) + (2 * LRButScale * mvButWdth) + boxLen));
        GUILayout.Label("Movement Settings");
        switch (curType)
        {
            case CELLTYPES.FLOOR:
                // Layer 2 Cell movement editor
                butEnableLyr2 = false;
                // Layer 1 Cell movement editor
                butEnableLyr1 = true;
                break;
            case CELLTYPES.WALL:
                // Layer 2 Cell movement editor
                butEnableLyr2 = false;
                // Layer 1 Cell movement editor
                butEnableLyr1 = false;
                break;
            case CELLTYPES.PLATFORM:
                // Layer 2 Cell movement editor
                butEnableLyr2 = true;
                // Layer 1 Cell movement editor
                butEnableLyr1 = false;
                break;
            case CELLTYPES.RAMP:
                GUI.enabled = true;
                // Layer 2 Cell movement editor
                butEnableLyr2 = true;
                // Layer 1 Cell movement editor
                butEnableLyr1 = true;
                break;
            default:
                GUI.enabled = false;
                // Layer 2 Cell movement editor
                butEnableLyr2 = false;
                // Layer 1 Cell movement editor
                butEnableLyr1 = false;
                Debug.LogError("Tried to access CellData of special");
                break;
        }
        MovementGuiButtons(true, butEnableLyr2);
        // Rotation reference image
        MovementGuiButtons(false, butEnableLyr1);
        EditorGUILayout.EndVertical();
    }

    // Buttons for editing the movement settings of a cell
    // true is layer 2
    // false is layer 1
    private void MovementGuiButtons(bool lyr, bool butEnable)
    {
        Texture2D mvArrowLR;
        Texture2D mvArrowUD;

        GUI.enabled = butEnable;
        Rect mvButGUIRect = EditorGUILayout.BeginVertical();

        // Top button
        if (info.CanMvTop(lyr))
        {
            mvArrowUD = (Texture2D)AssetDatabase.LoadAssetAtPath(mvArrUDOn_path, typeof(Texture2D));
        }
        else
        {
            mvArrowUD = (Texture2D)AssetDatabase.LoadAssetAtPath(mvArrUDOff_path, typeof(Texture2D));
        }
        EditorGUILayout.BeginHorizontal();
        // Space used to position button
        GUILayout.Space((mvButWdth * LRButScale) + elemSpacing);
        if (GUILayout.Button(mvArrowUD, GUILayout.Width(mvButLen), GUILayout.Height(mvButWdth)))
        {
            info.SetTop(!info.CanMvTop(lyr), lyr);
        }
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();

        // Left button
        if (info.CanMvLeft(lyr))
        {
            mvArrowLR = (Texture2D)AssetDatabase.LoadAssetAtPath(mvArrLROn_path, typeof(Texture2D));
        }
        else
        {
            mvArrowLR = (Texture2D)AssetDatabase.LoadAssetAtPath(mvArrLROff_path, typeof(Texture2D));
        }
        if (GUILayout.Button(mvArrowLR, GUILayout.Width(mvButWdth * LRButScale), GUILayout.Height(mvButLen)))
        {
            info.SetLeft(!info.CanMvLeft(lyr), lyr);
        }

        // Layer label
        GUI.enabled = true;
        if(lyr)
        {
            GUILayout.Box("Label 2", GUILayout.Width(boxLen), GUILayout.Height(boxLen));
        }
        else
        {
            GUILayout.Box("Label 1", GUILayout.Width(boxLen), GUILayout.Height(boxLen));
        }
        GUI.enabled = butEnable;

        // Right button
        if (info.CanMvRight(lyr))
        {
            mvArrowLR = (Texture2D)AssetDatabase.LoadAssetAtPath(mvArrLROn_path, typeof(Texture2D));
        }
        else
        {
            mvArrowLR = (Texture2D)AssetDatabase.LoadAssetAtPath(mvArrLROff_path, typeof(Texture2D));
        }
        if (GUILayout.Button(mvArrowLR, GUILayout.Width(mvButWdth * LRButScale), GUILayout.Height(mvButLen)))
        {
            info.SetRight(!info.CanMvRight(lyr), lyr);
        }

        EditorGUILayout.EndHorizontal();

        // Bot button
        if (info.CanMvBot(lyr))
        {
            mvArrowUD = (Texture2D)AssetDatabase.LoadAssetAtPath(mvArrUDOn_path, typeof(Texture2D));
        }
        else
        {
            mvArrowUD = (Texture2D)AssetDatabase.LoadAssetAtPath(mvArrUDOff_path, typeof(Texture2D));
        }

        EditorGUILayout.BeginHorizontal();
        // Space used to position button
        GUILayout.Space((mvButWdth * LRButScale) + elemSpacing);
        if (GUILayout.Button(mvArrowUD, GUILayout.Width(mvButLen), GUILayout.Height(mvButWdth)))
        {
            info.SetBot(!info.CanMvBot(lyr), lyr);
        }
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.EndVertical();
        prevWdth = (int)mvButGUIRect.height;
        prevLen = (int)1.25 * prevWdth;
        prevX = (int)mvButGUIRect.width;
    }
}
