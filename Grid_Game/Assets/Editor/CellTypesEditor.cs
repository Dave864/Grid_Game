using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine;
using UnityEditor;

public enum CELLTYPES
{
    FLOOR = 0,
    WALL = 1,
    PLATFORM = 2,
    RAMP = 3,
    SPECIAL = 4
}

[CustomEditor(typeof(CellTypes))]
public class CellTypesEditor : Editor
{
    private Texture2D advOptButImg;
    private CellTypes cellTypesRef;

    private List<CellData> curCellList;

    public CELLTYPES curList = 0;

    private void OnEnable()
    {
        advOptButImg = (Texture2D)AssetDatabase.LoadAssetAtPath("Assets/Resources/Materials/GUI Images/Adv_Cell_Opt.png", typeof(Texture2D));
        cellTypesRef = (CellTypes)target;
        GetList();
    }

    public override void OnInspectorGUI()
    {
        // Popup that lets you choose between the different cell lists
        curList = (CELLTYPES)EditorGUILayout.EnumPopup("Types: ", curList);

        // Update the currently displayed list
        GetList();

        // Display the elements of the current list
        DispList();

        // Button that lets you add another cell type to the current list
        EditorGUI.BeginChangeCheck();
        if (GUILayout.Button("Add new cell"))
        {
            AddCell();
        }
        if(EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(target, "Added a new cell type");
        }

        if(GUI.changed)
        {
            EditorUtility.SetDirty(cellTypesRef);
        }
    }

    // Displays each element in the current list
    void DispList()
    {
        CellData curInfo;
        Texture2D mdlPrev;
        bool cellRemoved;
        for (int ind = 0; ind < curCellList.Count; ind++)
        {
            curInfo = curCellList[ind];
            cellRemoved = false;
            EditorGUILayout.BeginHorizontal("Box");

            // Displays a preview of the model
            if(curInfo.GetModel() != null)
            {
                mdlPrev = AssetPreview.GetAssetPreview(curInfo.GetModel());
                if(mdlPrev == null)
                {
                    mdlPrev = AssetPreview.GetMiniThumbnail(curInfo.GetModel());
                }
                GUILayout.Label(mdlPrev, GUILayout.MaxWidth(50), GUILayout.MaxHeight(50));
            }
            else
            {
                GUILayout.Box("No Model", GUILayout.Width(50), GUILayout.Height(50));
            }
            EditorGUILayout.BeginVertical(GUILayout.Height(50));
            EditorGUILayout.BeginHorizontal();

            // Button for advanced options
            GUI.enabled = (curInfo.GetModel() != null) ? true : false;
            // Begin Change Check
            if (GUILayout.Button(new GUIContent(advOptButImg), GUILayout.MaxWidth(30), GUILayout.MaxHeight(30), GUILayout.ExpandWidth(false)))
            {
                //curInfo = new CellData(advOptMenu(curInfo));
                AdvOptMenu(curInfo);
            }
            // End Change check
            EditorGUILayout.BeginVertical();

            // Field to insert the model to use
            EditorGUIUtility.labelWidth = 50.0f;
            EditorGUIUtility.fieldWidth = 150.0f;
            EditorGUI.BeginChangeCheck();
            GUI.enabled = true;
            curInfo.SetModel((GameObject)EditorGUILayout.ObjectField("Model:", curInfo.GetModel(), typeof(GameObject), false));
            if(EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(target, "Set a model for a cell");
            }

            // Get the path to the 3D model; record and display path
            curInfo.SetPath(AssetDatabase.GetAssetPath(curInfo.GetModel()));
            EditorGUILayout.LabelField("Path:", curInfo.GetPath());

            EditorGUILayout.EndVertical();
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();  

            // Button that removes the cell from the current list
            EditorGUI.BeginChangeCheck();
            GUI.enabled = true;
            if (GUILayout.Button("Remove", GUILayout.MinWidth(50), GUILayout.MaxWidth(75)))
            {
                RemoveCell(ind);
                cellRemoved = true;
            }
            if(EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(target, "Removed an item from the cell");
            }

            // Button to clear the info of the cell
            if (!cellRemoved)
            {
                EditorGUI.BeginChangeCheck();
                GUI.enabled = (curInfo.GetModel() != null) ? true : false;
                if (GUILayout.Button("Clear", GUILayout.MinWidth(50), GUILayout.ExpandWidth(true)))
                {
                    curInfo = new CellData((int)curList);
                }
                curCellList[ind] = new CellData(curInfo);
                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(target, "Cleared the information of a cell");
                }
            }
            EditorGUILayout.EndVertical();
            EditorGUILayout.EndVertical();
            EditorGUILayout.EndHorizontal();
            GUI.enabled = true;
        }
        SetList();
    }

    // Add a new cell to the current list
    void AddCell()
    {
        Debug.Log("Added another cell");
        curCellList.Add(new CellData((int)curList));
    }

    // Remove a cell from the current list
    void RemoveCell(int ind)
    {
        Debug.Log("A cell has been removed");
        curCellList.Remove(curCellList[ind]);
    }

    // Menu for altering the information of a cell
    void AdvOptMenu(CellData cell)
    {
        if(curList != CELLTYPES.SPECIAL)
        {
            Debug.Log("Change standard cell");
            CellDataWindow.AdvCellOpt(cell, curList);
        }
        else
        {
            Debug.Log("Attempt to change special cell");
        }
        //return null;
    }

    // Changes the current list being displayed
    void GetList()
    {
        switch (curList)
        {
            case CELLTYPES.FLOOR:
                Debug.Log("Current List: Floors");
                curCellList = cellTypesRef.floors;
                break;
            case CELLTYPES.WALL:
                Debug.Log("Current List: Walls");
                curCellList = cellTypesRef.walls;
                break;
            case CELLTYPES.PLATFORM:
                Debug.Log("Current List: Platform");
                curCellList = cellTypesRef.platforms;
                break;
            case CELLTYPES.RAMP:
                Debug.Log("Current List: Ramp");
                curCellList = cellTypesRef.ramps;
                break;
            case CELLTYPES.SPECIAL:
                Debug.Log("Current List: Special");
                curCellList = cellTypesRef.special;
                break;
            default:
                Debug.LogError("Unkown cell list type!");
                curCellList = null;
                break;
        }
    }

    void SetList()
    {
        switch (curList)
        {
            case CELLTYPES.FLOOR:
                Debug.Log("Saved changes to Floors list");
                cellTypesRef.floors = curCellList;
                break;
            case CELLTYPES.WALL:
                Debug.Log("Saved changes to Walls list");
                cellTypesRef.walls = curCellList;
                break;
            case CELLTYPES.PLATFORM:
                Debug.Log("Saved changes to Platforms list");
                cellTypesRef.platforms = curCellList;
                break;
            case CELLTYPES.RAMP:
                Debug.Log("Saved changes to Ramps list");
                cellTypesRef.ramps = curCellList;
                break;
            case CELLTYPES.SPECIAL:
                Debug.Log("Saved changes to Special list");
                cellTypesRef.special = curCellList;
                break;
            default:
                Debug.LogError("Unknown cell list type!");
                break;
        }
    }
}
