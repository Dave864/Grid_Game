using System.Collections;
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
    public CELLTYPES curList = 0;

    private void OnEnable()
    {
        
    }

    public override void OnInspectorGUI()
    {
        // Popup that lets you choose between the different cell lists
        curList = (CELLTYPES)EditorGUILayout.EnumPopup("Types: ", curList);

        // Update the currently displayed list
        string[] curListDisplay = getList();

        // Button that lets you add another cell type to the current list
        if (GUILayout.Button("Add new cell"))
        {

        }
    }

    string[] getList()
    {
        CellTypes cellTypesRef = (CellTypes)target;

        switch (curList)
        {
            case CELLTYPES.FLOOR:
                Debug.Log("Current List: Floors");
                return cellTypesRef.floors;
            case CELLTYPES.WALL:
                Debug.Log("Current List: Walls");
                return cellTypesRef.walls;
            case CELLTYPES.PLATFORM:
                Debug.Log("Current List: Platform");
                return cellTypesRef.platforms;
            case CELLTYPES.RAMP:
                Debug.Log("Current List: Ramp");
                return cellTypesRef.ramps;
            case CELLTYPES.SPECIAL:
                Debug.Log("Current List: Special");
                return cellTypesRef.special;
            default:
                Debug.LogError("Unkown cell list type");
                return null;
        }
    }
}
