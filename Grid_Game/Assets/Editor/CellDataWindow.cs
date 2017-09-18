using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

// Window for editing the information of a cell
public class CellDataWindow : EditorWindow
{
    public CellData info;
    public CELLTYPES curType;

    // position and size for model preview
    /*
    private int prevX;
    private int prevY = 4;
    private int prevLen = 500;
    private int prevWdth = 200;
    */

    // position and sizes for movement GUI
    private int boxLen = 40;
    private int mvButWdth = 25;
    private int mvButLen = 40;
    private int elemSpacing = 8;
    private float LRButScale = 1.3f;

    // GUI images
    private string mvArrLROn_path = "Assets/Resources/Materials/GUI Images/Movement Arrow On LR.png";
    private string mvArrUDOn_path = "Assets/Resources/Materials/GUI Images/Movement Arrow On UD.png";
    private string mvArrLROff_path = "Assets/Resources/Materials/GUI Images/Movement Arrow Off LR.png";
    private string mvArrUDOff_path = "Assets/Resources/Materials/GUI Images/Movement Arrow Off UD.png";
    private string mvReference_path = "Assets/Resources/Materials/GUI Images/Cell Movement Reference.png";

    public static void AdvCellOpt(CellData cell, CELLTYPES t)
    {
        CellDataWindow window = (CellDataWindow)GetWindow(typeof(CellDataWindow), true, "Cell Data");
        window.info = new CellData(cell);
        window.curType = t;

        window.ShowAuxWindow();
    }

    private void OnGUI()
    {
        EditorGUILayout.BeginVertical();
        EditorGUILayout.BeginHorizontal();
        // Movement GUI
        MovementGui();

        // Where a preview of model should go
        EditorGUILayout.EndHorizontal();
        // Encounter map GUI
        EncounterMapGUI();
        EditorGUILayout.EndVertical();
    }

    // Interface for editing movement settings of a cell
    private void MovementGui()
    {
        bool butEnableLyr1;
        bool butEnableLyr2;
        Texture2D mvReference = (Texture2D)AssetDatabase.LoadAssetAtPath(mvReference_path, typeof(Texture2D));

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
        GUI.enabled = true;
        GUILayout.Label(mvReference, GUILayout.Height(100), GUILayout.Width((2 * elemSpacing) + (2 * LRButScale * mvButWdth) + boxLen));

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
        Rect movemntGUIRect = EditorGUILayout.BeginVertical();
        // prevX = (int)(movemntGUIRect.width + (2.5 * elemSpacing));

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
            GUILayout.Box("Layer 2", GUILayout.Width(boxLen), GUILayout.Height(boxLen));
        }
        else
        {
            GUILayout.Box("Layer 1", GUILayout.Width(boxLen), GUILayout.Height(boxLen));
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
    }

    // Interface for editing the layout of a cell's encounter map
    private void EncounterMapGUI()
    {
        GUIContent[] selGridLR = new GUIContent[0];
        GUIContent[] selGridUD = new GUIContent[0];

        GUILayout.BeginHorizontal();
        // Height color reference image
        // Current cell reference

        // Left-Right orientation of map
        
        // Up-Down orientation of map

        GUILayout.EndHorizontal();
    }
}
