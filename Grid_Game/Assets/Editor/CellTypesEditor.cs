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

public static class SaveLoadCell
{
    public static CellTypes cellsOfSec = new CellTypes();

    public static void save()
    {
        string filePth = Application.persistentDataPath + "/" + CellTypes.cur.gameObject.name + "_cells.mapc";
        cellsOfSec = CellTypes.cur;
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(filePth);
        bf.Serialize(file, SaveLoadCell.cellsOfSec);
        file.Close();
    }

    public static bool load()
    {
        string filePth = Application.persistentDataPath + "/" + CellTypes.cur.gameObject.name + "_cells.mapc";
        if (File.Exists(filePth))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(filePth, FileMode.Open);
            SaveLoadCell.cellsOfSec = (CellTypes)bf.Deserialize(file);
            file.Close();
            return true;
        }
        return false;
    }

    public static void destroy()
    {
        string filePth = Application.persistentDataPath + "/" + CellTypes.cur.gameObject.name + "_cells.mapc";
        if (File.Exists(filePth))
        {
            File.Delete(filePth);
            AssetDatabase.Refresh();
        }
    }
}

[CustomEditor(typeof(CellTypes))]
public class CellTypesEditor : Editor
{
    private Texture2D advOptBut;
    private CellTypes cellTypesRef;

    private List<CellData> curStrList;

    public CELLTYPES curList = 0;

    private void OnEnable()
    {
        advOptBut = (Texture2D)AssetDatabase.LoadAssetAtPath("Assets/Resources/Materials/Adv_Cell_Opt.png", typeof(Texture2D));
        CellTypes.cur = (CellTypes)target;
        cellTypesRef = /*(SaveLoadCell.load()) ? SaveLoadCell.cellsOfSec :*/ (CellTypes)target;
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
        EditorGUI.BeginChangeCheck();
        if (GUILayout.Button("Add new cell"))
        {
            addCell();
        }
        if(EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(target, "Added a new cell type");
        }
    }

    // Displays each element in the current list
    void dispList()
    {
        CellData curInfo;
        for (int ind = 0; ind < curStrList.Count; ind++)
        {
            curInfo = curStrList[ind];
            EditorGUILayout.BeginHorizontal("Box");

            // Displays a preview of the model
            if(curInfo.getModel() != null)
            {
                GUILayout.Label(AssetPreview.GetAssetPreview(curInfo.getModel()), GUILayout.MaxWidth(50), GUILayout.MaxHeight(50));
            }
            else
            {
                GUILayout.Box("No Model", GUILayout.MaxWidth(50), GUILayout.MaxHeight(50));
            }
            EditorGUILayout.BeginVertical(GUILayout.Height(50));
            EditorGUILayout.BeginHorizontal();

            // Button for advanced options
            GUI.enabled = (curInfo.getModel() != null) ? true : false;
            // Begin Change Check
            if (GUILayout.Button(new GUIContent(advOptBut), GUILayout.MaxWidth(30), GUILayout.MaxHeight(30)))
            {
                advOptMenu();
            }
            // End Change check
            EditorGUILayout.BeginVertical();

            // Field to insert the model to use
            EditorGUIUtility.labelWidth = 50.0f;
            EditorGUIUtility.fieldWidth = 150.0f;
            EditorGUI.BeginChangeCheck();
            GUI.enabled = true;
            curInfo.setModel((GameObject)EditorGUILayout.ObjectField("Model:", curInfo.getModel(), typeof(GameObject), false));
            if(EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(target, "Set a model for a cell");
            }
           
            // Get the path to the 3D model; record and display path
            curInfo.setPath(AssetDatabase.GetAssetPath(curInfo.getModel()));
            EditorGUILayout.LabelField("Path:", curInfo.getPath());

            EditorGUILayout.EndVertical();
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();

            // Button to clear the info of the cell
            EditorGUI.BeginChangeCheck();
            GUI.enabled = (curInfo.getModel() != null) ? true : false;
            if(GUILayout.Button("Clear Cell", GUILayout.Width(100)))
            {
                curInfo = new CellData((int)curList);
            }
            curStrList[ind] = new CellData(curInfo);
            if(EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(target, "Cleared the information of a cell");
            }
            
            // Button that removes the cell from the current list
            EditorGUI.BeginChangeCheck();
            GUI.enabled = true;
            if (GUILayout.Button("Remove Cell", GUILayout.Width(150)))
            {
                removeCell(ind);
            }
            if(EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(target, "Removed an item from the cell");
            }

            EditorGUILayout.EndVertical();
            EditorGUILayout.EndVertical();
            EditorGUILayout.EndHorizontal();
        }
    }

    // Add a new cell to the current list
    void addCell()
    {
        Debug.Log("Added another cell");
        curStrList.Add(CreateInstance<CellData>());
        //curStrList.Add(new CellData((int)curList));
    }

    // Remove a cell from the current list
    void removeCell(int ind)
    {
        Debug.Log("A cell has been removed");
        curStrList.Remove(curStrList[ind]);
    }

    // Menu for altering the information of a cell
    void advOptMenu()
    {
        if(curList != CELLTYPES.SPECIAL)
        {
            Debug.Log("Change standard cell");
        }
        else
        {
            Debug.Log("Attempt to change special cell");
        }
    }

    // Changes the current list being displayed
    void getList()
    {
        switch (curList)
        {
            case CELLTYPES.FLOOR:
                Debug.Log("Current List: Floors");
                curStrList = cellTypesRef.floors;
                break;
            case CELLTYPES.WALL:
                Debug.Log("Current List: Walls");
                curStrList = cellTypesRef.walls;
                break;
            case CELLTYPES.PLATFORM:
                Debug.Log("Current List: Platform");
                curStrList = cellTypesRef.platforms;
                break;
            case CELLTYPES.RAMP:
                Debug.Log("Current List: Ramp");
                curStrList = cellTypesRef.ramps;
                break;
            case CELLTYPES.SPECIAL:
                Debug.Log("Current List: Special");
                curStrList = cellTypesRef.special;
                break;
            default:
                Debug.LogError("Unkown cell list type!");
                curStrList = null;
                break;
        }
    }
}
