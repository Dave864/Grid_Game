using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
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
[CanEditMultipleObjects]
public class CellTypesEditor : Editor
{
    private CellDataWindow cellDataW;
    private Texture2D advOptButImg;
    private CellTypes cellTypesRef;

    private List<CellData> curCellList;

    private CELLTYPES curList = 0;

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
            Undo.RecordObject(target, "Added a new cell to list");
        }
    }

    // Displays each element in the current list
    private void DispList()
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

            EditorGUILayout.BeginVertical();
            EditorGUILayout.BeginHorizontal();

            // Button for settings of the cell
            GUI.enabled = (curInfo.GetModel() != null) ? true : false;
            EditorGUI.BeginChangeCheck();
            if (GUILayout.Button(new GUIContent(advOptButImg), GUILayout.MaxWidth(30), GUILayout.MaxHeight(30), GUILayout.ExpandWidth(false)))
            {
                CellDataSettings(curInfo);
            }
            // End Change check
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(target, "Changed properties of the cell");
            }

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

            // Movement GUI
            //MvmtGUI(true, curInfo);
            //MvmtGUI(false, curInfo);
            //EncMapGUI(curInfo);

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
                Undo.RecordObject(target, "Removed a cell from the list");
            }

            // Button to clear the info of the cell
            if (!cellRemoved)
            {
                GUI.enabled = (curInfo.GetModel() != null) ? true : false;
                EditorGUI.BeginChangeCheck();
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

            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndVertical();
            EditorGUILayout.EndHorizontal();

            GUI.enabled = true;
        }
    }

    // Add a new cell to the current list
    private void AddCell()
    {
        Debug.Log("Added another cell");
        curCellList.Add(new CellData((int)curList));
    }

    // Remove a cell from the current list
    private void RemoveCell(int ind)
    {
        Debug.Log("A cell has been removed");
        curCellList.Remove(curCellList[ind]);
    }

    // GUI for changeing the first cell of an encounter map
    private void EncMapGUI(CellData cell)
    {
        cell.encounterMapLR[0, 0].height = EditorGUILayout.IntSlider("Height:", cell.encounterMapLR[0, 0].height, 0, GlobalVals.ENC_MAP_MX_HT);
    }

    // GUI for changing movement settings of a cell
    private void MvmtGUI(bool lyr, CellData cell)
    {
        EditorGUILayout.BeginVertical();
        if (lyr)
        {
            EditorGUILayout.LabelField("Layer 2");
        }
        else
        {
            EditorGUILayout.LabelField("Layer 1");
        }
        EditorGUIUtility.fieldWidth = 10.0f;
        EditorGUILayout.BeginHorizontal();
        GUILayout.Space(20.0f);
        Debug.Log(cell.mvmt[lyr, MVMT.TOP]);
        cell.mvmt[lyr, MVMT.TOP] = EditorGUILayout.Toggle(cell.mvmt[lyr, MVMT.TOP]);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        cell.mvmt[lyr, MVMT.LFT] = EditorGUILayout.Toggle(cell.mvmt[lyr, MVMT.LFT]);
        cell.mvmt[lyr, MVMT.RGT] = EditorGUILayout.Toggle(cell.mvmt[lyr, MVMT.RGT]);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        GUILayout.Space(20.0f);
        cell.mvmt[lyr, MVMT.BOT] = EditorGUILayout.Toggle(cell.mvmt[lyr, MVMT.BOT]);
        EditorGUILayout.EndHorizontal();
        EditorGUIUtility.fieldWidth = 0.0f;

        EditorGUILayout.EndVertical();
    }

    // Menu for altering the information of a cell
    private void CellDataSettings(CellData cell)
    {
        if(curList != CELLTYPES.SPECIAL)
        {
            // Begin Change Check
            EditorGUI.BeginChangeCheck();

            Debug.Log("Change standard cell");
            cellDataW = (CellDataWindow)EditorWindow.GetWindow(typeof(CellDataWindow), true, "Cell Data");
            cellDataW.info = cell;
            cellDataW.curType = curList;
            cellDataW.Show(true);
        }
        else
        {
            Debug.Log("Attempt to change special cell");
        }
    }

    // Changes the current list being displayed
    private void GetList()
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
}
