﻿using System.Collections;
using System.Collections.Generic;
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
    private CellTypes cellTypesRef;

    private List<CellData> curStrList;
    private int listSz;
    //private char keyBase;

    public CELLTYPES curList = 0;

    private void OnEnable()
    {
        cellTypesRef = (CellTypes)target;
        getList();
    }

    public override void OnInspectorGUI()
    {        
        // Popup that lets you choose between the different cell lists
        curList = (CELLTYPES)EditorGUILayout.EnumPopup("Types: ", curList);

        // Update the currently displayed list
        getList();

        // Display the elements of the current list
        dispList();

        // Button that lets you add another cell type to the current list
        if (GUILayout.Button("Add new cell"))
        {
            addCell();
        }
    }

    // Displays each element in the current list
    void dispList()
    {
        CellData curInfo;
        // GameObject mObj;
        for (int ind = 0; ind < curStrList.Count; ind++)
        {
            curInfo = curStrList[ind];
            EditorGUILayout.BeginHorizontal();
            // Intended to be an image of the model
            GUILayout.Label(AssetDatabase.GetCachedIcon(curInfo.getPath()), GUILayout.MinWidth(30));
            EditorGUILayout.BeginVertical(GUILayout.Height(50));
            // Field to insert the model to use
            curInfo.setModel((GameObject)EditorGUILayout.ObjectField("Model", curInfo.getModel(), typeof(GameObject), false, GUILayout.MinWidth(150)));
            // Get the path to the 3D model and save it
            curInfo.setPath(AssetDatabase.GetAssetPath(curInfo.getModel()));
            EditorGUILayout.LabelField("Model Path", curInfo.getPath(), GUILayout.MinWidth(150));
            curStrList[ind] = new CellData(curInfo);
            
            // Button that removes the cell from the current list
            if (GUILayout.Button("Remove Cell", GUILayout.Width(150)))
            {
                removeCell(ind);
            }
            EditorGUILayout.EndVertical();
            EditorGUILayout.EndHorizontal();
        }
        setList();
    }

    // Add a new cell to the current list
    void addCell()
    {
        Debug.Log("Added another cell");
        //string newKey = keyBase + curStrList.Count.ToString();
        //curStrList.Add(newKey, "NEW CELL");
        //curStrList.Add("NEW CELL");
        curStrList.Add(new CellData((int)curList));
        setList();
    }

    // Remove a cell from the current list
    void removeCell(int ind)
    {
        Debug.Log("A cell has been removed");
        curStrList.Remove(curStrList[ind]);
    }

    // Changes the current list being displayed
    void getList()
    {
        switch (curList)
        {
            case CELLTYPES.FLOOR:
                Debug.Log("Current List: Floors");
                curStrList = cellTypesRef.floors;
                // keyBase = 'F';
                break;
            case CELLTYPES.WALL:
                Debug.Log("Current List: Walls");
                curStrList = cellTypesRef.walls;
                // keyBase = 'W';
                break;
            case CELLTYPES.PLATFORM:
                Debug.Log("Current List: Platform");
                curStrList = cellTypesRef.platforms;
                // keyBase = 'P';
                break;
            case CELLTYPES.RAMP:
                Debug.Log("Current List: Ramp");
                curStrList = cellTypesRef.ramps;
                // keyBase = 'R';
                break;
            case CELLTYPES.SPECIAL:
                Debug.Log("Current List: Special");
                curStrList = cellTypesRef.special;
                // keyBase = 'S';
                break;
            default:
                Debug.LogError("Unkown cell list type!");
                curStrList = null;
                break;
        }
        listSz = curStrList.Count;
    }

    // Update the current list
    void setList()
    {
        switch (curList)
        {
            case CELLTYPES.FLOOR:
                cellTypesRef.floors = new List<CellData>(curStrList);
                break;
            case CELLTYPES.WALL:
                cellTypesRef.walls = new List<CellData>(curStrList);
                break;
            case CELLTYPES.PLATFORM:
                cellTypesRef.platforms = new List<CellData>(curStrList);
                break;
            case CELLTYPES.RAMP:
                cellTypesRef.ramps = new List<CellData>(curStrList);
                break;
            case CELLTYPES.SPECIAL:
                cellTypesRef.special = new List<CellData>(curStrList);
                break;
            default:
                Debug.LogError("Unable to add cell! Unkown cell list type!");
                break;
        }
    }
}