﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

// Window for editing the information of a cell
public class CellDataWindow : EditorWindow
{
    public CellData info;
    public CELLTYPES curType;

    // Sizes and values for movement GUI
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
    private string htReference_path = "Assets/Resources/Materials/GUI Images/Enc Cell Ht Reference.png";
    private string encCell_path = "Assets/Resources/Materials/GUI Images/Encounter Cell.png";

    // Sizes and values for Encounter Map GUI
    private GUIStyle encCellNrm_Style;
    private GUIStyle encCellSel_Style;
    private string encMapOr = "Left - Right";
    private bool type = true;
    private bool ulTog = false;
    private bool blTog = false;
    private bool urTog = false;
    private bool brTog = false;
    private int cellButWdt = 40;
    private int curInd = 0;

    private void OnGUI()
    {
        encCellNrm_Style = new GUIStyle
        {
            alignment = TextAnchor.MiddleCenter,
            fixedHeight = cellButWdt,
            fixedWidth = cellButWdt,
            margin = new RectOffset(2, 2, 2, 2),
        };
        encCellNrm_Style.normal.background = (Texture2D)AssetDatabase.LoadAssetAtPath(encCell_path, typeof(Texture2D));

        encCellSel_Style = new GUIStyle(encCellNrm_Style);
        encCellSel_Style.normal.background = Texture2D.whiteTexture;

        //EditorGUILayout.BeginVertical();
        EditorGUILayout.BeginHorizontal();
        // Movement settings GUI
        // This doesn't change movement settings for some reason
        MovementGui();

        // Where a preview of model should go
        // Encounter map GUI
        EncounterMapGUI();
        EditorGUILayout.EndHorizontal();
        //EditorGUILayout.EndVertical();
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
                Debug.LogError("Tried to access CellData of special cell");
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
        EditorGUILayout.BeginVertical();

        // Top button Label
        if (info.mvmt[lyr, MVMT.TOP])
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

        // Top button
        if (GUILayout.Button(mvArrowUD, GUILayout.Width(mvButLen), GUILayout.Height(mvButWdth)))
        {
            info.mvmt[lyr, MVMT.TOP] = !info.mvmt[lyr, MVMT.TOP];
        }
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        // Left button label
        if (info.mvmt[lyr, MVMT.LFT])
        {
            mvArrowLR = (Texture2D)AssetDatabase.LoadAssetAtPath(mvArrLROn_path, typeof(Texture2D));
        }
        else
        {
            mvArrowLR = (Texture2D)AssetDatabase.LoadAssetAtPath(mvArrLROff_path, typeof(Texture2D));
        }

        // Left button
        if (GUILayout.Button(mvArrowLR, GUILayout.Width(mvButWdth * LRButScale), GUILayout.Height(mvButLen)))
        {
            info.mvmt[lyr, MVMT.LFT] = !info.mvmt[lyr, MVMT.LFT];
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

        // Right button label
        if (info.mvmt[lyr, MVMT.RGT])
        {
            mvArrowLR = (Texture2D)AssetDatabase.LoadAssetAtPath(mvArrLROn_path, typeof(Texture2D));
        }
        else
        {
            mvArrowLR = (Texture2D)AssetDatabase.LoadAssetAtPath(mvArrLROff_path, typeof(Texture2D));
        }

        // Right button
        if (GUILayout.Button(mvArrowLR, GUILayout.Width(mvButWdth * LRButScale), GUILayout.Height(mvButLen)))
        {
            info.mvmt[lyr, MVMT.RGT] = !info.mvmt[lyr, MVMT.RGT];
        }

        EditorGUILayout.EndHorizontal();

        // Bot button label
        if (info.mvmt[lyr, MVMT.BOT])
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

        // Bot button
        if (GUILayout.Button(mvArrowUD, GUILayout.Width(mvButLen), GUILayout.Height(mvButWdth)))
        {
            info.mvmt[lyr, MVMT.BOT] = !info.mvmt[lyr, MVMT.BOT];
        }
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.EndVertical();
    }

    // Interface for editing the layout of a cell's encounter map
    private void EncounterMapGUI()
    {
        GUI.enabled = true;
        Texture2D htReference = (Texture2D)AssetDatabase.LoadAssetAtPath(htReference_path, typeof(Texture2D));

        GUILayout.BeginHorizontal();

        // Height color reference image
        GUILayout.Label(htReference, GUILayout.Width(htReference.width));

        GUILayout.BeginVertical();

        // Button to pick orientation
        if(GUILayout.Button(encMapOr))
        {
            type = !type;
            encMapOr = type ? "Left - Right" : "Up - Down";
        }

        // Create menu to alter a cell's properties
        if (type)
        {
            EncounterMapCellProp(info.encounterMapLR);
        }
        else
        {
            EncounterMapCellProp(info.encounterMapUD);
        }

        // Menu to see trimmed corners
        EncounterMapTrimMenu();

        GUILayout.EndVertical();

        // Create selection grid
        if (type)
        {
            EncounterMapSelGrid(info.encounterMapLR, type);
        }
        else
        {
            EncounterMapSelGrid(info.encounterMapUD, type);
        }

        GUILayout.EndHorizontal();
    }

    // GUI for editing an ecounter cell's properties
    private void EncounterMapCellProp(HexRect hexRect)
    {
        int r = curInd / GlobalVals.ENC_MAP_COL;
        int c = curInd - (r * GlobalVals.ENC_MAP_COL);
        EditorGUILayout.LabelField("Column: " + c.ToString());
        EditorGUILayout.LabelField("Cell: " + r.ToString());
        EditorGUIUtility.labelWidth = 75.0f;
        hexRect[r,c].height = EditorGUILayout.IntSlider("Height:", hexRect[r, c].height, 0, GlobalVals.ENC_MAP_MX_HT);
        EditorGUIUtility.labelWidth = 0.0f;
    }

    // Toggle buttons used to see trimmed corners
    private void EncounterMapTrimMenu()
    {
        GUILayout.BeginVertical("Box");
        EditorGUILayout.LabelField("Trim Corner Preview");
        GUILayout.BeginHorizontal();
        EditorGUIUtility.labelWidth = 70.0f;
        ulTog = EditorGUILayout.Toggle("Top - Left", ulTog);
        urTog = EditorGUILayout.ToggleLeft("Top - Right", urTog);
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        blTog = EditorGUILayout.Toggle("Bot - Left", blTog);
        brTog = EditorGUILayout.ToggleLeft("Bot - Right", brTog);
        EditorGUIUtility.labelWidth = 0.0f;
        GUILayout.EndHorizontal();
        GUILayout.EndVertical();
    }

    // Selection grid for an encounter map
    // true is Left - Right orientation
    // false is Up - Down orientaton
    private void EncounterMapSelGrid(HexRect hexRect, bool type)
    {
        int w = cellButWdt;
        float hVal;
        GUIStyle gUIStyle;
        HexRect trimHexRect = hexRect.TrimCorners(ulTog, urTog, blTog, brTog);

        GUILayout.BeginVertical("Box", GUILayout.Width(w * hexRect.ColCnt()));
        if (type)
        {
            GUILayout.Label("Left-Right Orientation");
        }
        else
        {
            GUILayout.Label("Up-Down Orientation");
        }

        GUILayout.BeginHorizontal(GUILayout.ExpandWidth(false));
        for (int c = 0; c < trimHexRect.ColCnt(); c++)
        {
            GUILayout.BeginVertical();
            for (int r = 0; r < trimHexRect.HexPerCol(); r++)
            {
                // Format layout of encounter cells GUI
                if (trimHexRect.SharpCor())
                {
                    if ((r == 0) && (c % 2 != 0))
                    {
                        GUILayout.Space(w / 2);
                    }
                }
                else
                {
                    if ((r == 0) && (c % 2 == 0))
                    {
                        GUILayout.Space(w / 2);
                    }
                }
                // Display the encounter cells GUI
                if (trimHexRect[r,c] != null)
                {
                    // "Trim" the encounter cell
                    if (trimHexRect[r,c].height < 0)
                    {
                        GUI.color = Color.blue;
                        GUI.enabled = false;
                        GUILayout.Box("", encCellNrm_Style);
                        GUI.enabled = true;
                    }
                    // Create button to interact with encounter cell
                    else
                    {
                        hVal = (trimHexRect[r, c].height / (float)GlobalVals.ENC_MAP_MX_HT) * 0.33f;
                        GUI.color = Color.HSVToRGB(hVal, 1.0f, 1.0f);
                        // Set style for button
                        gUIStyle = (curInd == ((r * GlobalVals.ENC_MAP_COL) + c)) ? encCellSel_Style : encCellNrm_Style;
                        if (GUILayout.Button(trimHexRect[r, c].height.ToString(), gUIStyle))
                        {
                            curInd = (r * GlobalVals.ENC_MAP_COL) + c;
                        }
                    }                   
                }
            }
            GUI.color = Color.white;
            GUILayout.EndVertical();
        }
        GUILayout.EndHorizontal();
        GUILayout.EndVertical();
    }
}
