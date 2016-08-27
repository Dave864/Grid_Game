using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class ArenaGenerator : MonoBehaviour {

	// groups together cells that make up the arena
	public Transform TopLeft;
	public Transform TopRight;
	public Transform BotLeft;
	public Transform BotRight;

	// info for placing the hex cells
	private float Hoz_Offset = 1.0f;
	private float Vert_Offset = -0.866f;
	private int Cell_Row_Cnt = 4;

	private ArenaSectionMap TL_Map;
	private ArenaSectionMap TR_Map;
	private ArenaSectionMap BL_Map;
	private ArenaSectionMap BR_Map;

	public Text Debug_Text;

	TextAsset Sec_Data;

	// Initialize data values
	void Awake () {
		TL_Map = TopLeft.GetComponent<ArenaSectionMap> ();
		TR_Map = TopRight.GetComponent<ArenaSectionMap> ();
		BL_Map = BotLeft.GetComponent<ArenaSectionMap> ();
		BR_Map = BotRight.GetComponent<ArenaSectionMap> ();
	}

	// Use this for initialization
	void Start () {
		InitCellMap (TL_Map);
		InitCellMap (TR_Map);
		InitCellMap (BL_Map);
		InitCellMap (BR_Map);
		TEST ();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKey (KeyCode.Escape)) {
			SceneManager.LoadScene ("OverWorld");
		}
	}

	// Initialize Cell_Map of sections
	void InitCellMap (ArenaSectionMap Section) {
		Section.Cell_Map = new ArenaSectionMap.Cell[Cell_Row_Cnt][];
		for (uint i = 0; i < Cell_Row_Cnt; i++) {
			Section.Cell_Map [i] = new ArenaSectionMap.Cell[Cell_Row_Cnt];
		}
	}

	// TEST LOADING SECTIONS
	void TEST () {
		Sec_Data = (TextAsset)Resources.Load (GetCellInfoPath (), typeof(TextAsset));
		string[] cell_row = Sec_Data.text.Split ('\n');
		string[][] cell_info = new string[cell_row.Length][];
		for (int i = 0; i < cell_info.Length; i++) {
			cell_info [i] = cell_row [i].Split (' ');
		}
		for (int row = 0; row < cell_info.Length; row++) {
			for (int col = 0; col < cell_info [row].Length; col++) {
				int height = (int)char.GetNumericValue (cell_info [row] [col] [1]);
				MakeCell (row, (float)col, height);
			}
		}
	}

	// make arena cell
	void MakeCell(int row, float col, int height){
		string cell_object_path = "Prefabs/arena_cell";
		col = (row % 2 != 0) ? col + 0.5f : col;
		GameObject cell_object = Instantiate (Resources.Load (cell_object_path), new Vector3 (col * Hoz_Offset, (float)height / 8, (float)row * Vert_Offset), Quaternion.Euler (0, 90.0f, 0)) as GameObject;
		cell_object.transform.localScale += new Vector3 (0, (float)height * 1.0f, 0);
		cell_object.transform.SetParent (TopLeft);
		cell_object.tag = "Arena Cell";
	}

	// Get file path for CellInfo
	string GetCellInfoPath(){
		string file_path = "Sections/";
		char c = '0';
		if (PlayerPrefs.GetInt ("Section") != 0) {
			c += (char)(PlayerPrefs.GetInt ("Section") / 10);
			file_path += c;
			c = '0';
			c += (char)(PlayerPrefs.GetInt ("Section") % 10);
			file_path += c;
		} else {
			file_path += "00";
		}
		file_path += '/';
		file_path += PlayerPrefs.GetInt ("Row");
		file_path += PlayerPrefs.GetInt ("Column");
		Debug_Text.text = file_path;
		return file_path;
	}
}
