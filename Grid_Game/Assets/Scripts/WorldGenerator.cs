using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.IO;

// Manages the creation and destruction of world elements
public class WorldGenerator : MonoBehaviour {

	// Used for grouping together map section cells
	public Transform Cur;
	public Transform Left;
	public Transform Right;
	public Transform Top;
	public Transform Bot;
	public Transform Top_L;
	public Transform Top_R;
	public Transform Bot_L;
	public Transform Bot_R;

	private float Cur_Sec_Origin_x = 0.0f;
	private float Cur_Sec_Origin_z = 0.0f;
	private int Cur_Sec;
	enum SECTION_LOC {CUR, L, R, T, B, TL, TR, BL, BR};

	TextAsset Sec_Data;

	// Use this for initialization
	void Start () {
		Cur_Sec = PlayerMovement.Cur_Sec;
		LoadWorld ();
	}
	
	// Update is called once per frame
	void Update () {
	}
		
	// Loads the visible sections of the world
	void LoadWorld () {
		//float origin_offset = PlayerMovement.Sec_Width;
		LoadSection (Cur_Sec, SECTION_LOC.CUR);

	}

	// Loads section sec_num of the world at location at coordinates: sec_origin_x, some height, sec_origin_z
	void LoadSection (int sec_num, SECTION_LOC location) {
		Sec_Data = (TextAsset)Resources.Load (GetAreaMapPath (sec_num), typeof(TextAsset));
		int c_col = (int)Cur_Sec_Origin_x;
		int c_row = (int)Cur_Sec_Origin_z;
		float height = 0.0f;
		string[] row_data = Sec_Data.text.Split ('\n');
		if (location == SECTION_LOC.L) {
		} else if (location == SECTION_LOC.R) {
		} else if (location == SECTION_LOC.T) {
		} else if (location == SECTION_LOC.B) {
		} else if (location == SECTION_LOC.TL) {
		} else if (location == SECTION_LOC.TR) {
		} else if (location == SECTION_LOC.BL) {
		} else if (location == SECTION_LOC.BR) {
		} else {
			SectionData.Cur_Sec = new string[row_data.Length][];
			for (int i = 0; i < row_data.Length; i++) {
				SectionData.Cur_Sec [i] = row_data [i].Split (' ');
			}
			foreach (string[] row in SectionData.Cur_Sec) {
				foreach (string cell in row) {
					height = (float)char.GetNumericValue (cell[3]);
					MakeCell(c_row, c_col, height, cell, Cur);
					c_col += 1;
				}
				c_row -= 1;
				c_col = (int)Cur_Sec_Origin_x;
			}
		}
	}

	// Create AreaMap file path for section sec_num
	string GetAreaMapPath (int sec_num) {
		string file_path = "Sections/";
		char c = '0';
		c += (char)(sec_num/10);
		file_path += c;
		c = '0';
		c += (char)(sec_num % 10);
		file_path += c;
		file_path += "/AreaMap";
		return file_path;
	}

	// Creates a section cell in the game space
	void MakeCell (int row, int col, float height, string info, Transform section) {
		string file_path = "Prefabs/";
		float x_rot;
		float y_rot;
		// make flat panel
		//if (char.IsDigit (info [0])) {
			file_path += "Panel_0";
			x_rot = 90f;
			y_rot = 0f;
		//}
		// make special platform
		//else {
		//}
		GameObject cell;
		cell = Instantiate (Resources.Load (file_path), new Vector3 ((float)col, height, (float)row), Quaternion.Euler (x_rot, y_rot, 0)) as GameObject;
		cell.transform.parent = section;
		// AssignTag
	}

	// destroy sections
}
