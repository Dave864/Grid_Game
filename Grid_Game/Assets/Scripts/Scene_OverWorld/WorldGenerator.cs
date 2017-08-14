﻿using UnityEngine;
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

	bool loading = false;

	TextAsset Sec_Data;

	// Use this for initialization
	void Start () {
		Cur_Sec = PlayerMovement.Cur_Sec;
		InitializeSections ();
		LoadWorld ();
	}
	
	// Update is called once per frame
	void Update () {
		if (!loading) {
			int sec_diff = PlayerMovement.Cur_Sec - Cur_Sec;
			Cur_Sec = PlayerMovement.Cur_Sec;
			// player moves to top section
			if (sec_diff == -10) {
				Cur_Sec_Origin_z += PlayerMovement.Sec_Width;
				StartCoroutine (ClearSections (SECTION_LOC.B));
			}
			// player moves to bottom section
			else if (sec_diff == 10) {
				Cur_Sec_Origin_z -= PlayerMovement.Sec_Width;
				StartCoroutine (ClearSections (SECTION_LOC.T));
			}
			// player moves to left section
			else if (sec_diff == -1) {
				Cur_Sec_Origin_x -= PlayerMovement.Sec_Width;
				StartCoroutine (ClearSections (SECTION_LOC.R));
			}
			// player moves to right section
			else if (sec_diff == 1) {
				Cur_Sec_Origin_x += PlayerMovement.Sec_Width;
				StartCoroutine (ClearSections (SECTION_LOC.L));
			}
			// player stays in cur section
			else {
			}
		}
	}

	// Initializes section data maps to null
	void InitializeSections () {
		SectionData.Cur_Sec = new string[(int)PlayerMovement.Sec_Width][];
		SectionData.L_Sec = new string[(int)PlayerMovement.Sec_Width][];
		SectionData.R_Sec = new string[(int)PlayerMovement.Sec_Width][];
		SectionData.T_Sec = new string[(int)PlayerMovement.Sec_Width][];
		SectionData.B_Sec = new string[(int)PlayerMovement.Sec_Width][];
		SectionData.TL_Sec = new string[(int)PlayerMovement.Sec_Width][];
		SectionData.TR_Sec = new string[(int)PlayerMovement.Sec_Width][];
		SectionData.BL_Sec = new string[(int)PlayerMovement.Sec_Width][];
		SectionData.BR_Sec = new string[(int)PlayerMovement.Sec_Width][];
		for (uint r = 0; r < (uint)PlayerMovement.Sec_Width; r++) {
			SectionData.Cur_Sec [r] = new string[(int)PlayerMovement.Sec_Width];
			SectionData.L_Sec [r] = new string[(int)PlayerMovement.Sec_Width];
			SectionData.R_Sec [r] = new string[(int)PlayerMovement.Sec_Width];
			SectionData.T_Sec [r] = new string[(int)PlayerMovement.Sec_Width];
			SectionData.B_Sec [r] = new string[(int)PlayerMovement.Sec_Width];
			SectionData.TL_Sec [r] = new string[(int)PlayerMovement.Sec_Width];
			SectionData.TR_Sec [r] = new string[(int)PlayerMovement.Sec_Width];
			SectionData.BL_Sec [r] = new string[(int)PlayerMovement.Sec_Width];
			SectionData.BR_Sec [r] = new string[(int)PlayerMovement.Sec_Width];
			for (uint c = 0; c < (uint)PlayerMovement.Sec_Width; c++) {
				SectionData.Cur_Sec [r] = null;
				SectionData.L_Sec [r] [c] = null;
				SectionData.R_Sec [r] [c] = null;
				SectionData.T_Sec [r] [c] = null;
				SectionData.B_Sec [r] [c] = null;
				SectionData.TL_Sec [r] [c] = null;
				SectionData.TR_Sec [r] [c] = null;
				SectionData.BL_Sec [r] [c] = null;
				SectionData.BR_Sec [r] [c] = null;
			}
		}
	}
		
	// Loads the visible sections of the world
	void LoadWorld () {
		LoadSection (Cur_Sec, SECTION_LOC.CUR);
		LoadSection (Cur_Sec - 1, SECTION_LOC.L);
		LoadSection (Cur_Sec + 1, SECTION_LOC.R);
		LoadSection (Cur_Sec - 10, SECTION_LOC.T);
		LoadSection (Cur_Sec + 10, SECTION_LOC.B);
		LoadSection (Cur_Sec - 11, SECTION_LOC.TL);
		LoadSection (Cur_Sec - 9, SECTION_LOC.TR);
		LoadSection (Cur_Sec + 9, SECTION_LOC.BL);
		LoadSection (Cur_Sec + 11, SECTION_LOC.BR);
	}

	// Loads section sec_num of the world at location if sec_num is valid
	void LoadSection (int sec_num, SECTION_LOC location) {
		if (sec_num < 0 || !Directory.Exists (GetSectionPath (sec_num))) {
			return;
		}
		Sec_Data = (TextAsset)Resources.Load (GetAreaMapPath (sec_num), typeof(TextAsset));
		int c_col = (int)Cur_Sec_Origin_x;
		int c_row = (int)Cur_Sec_Origin_z;
		int origin_offset = (int)PlayerMovement.Sec_Width;
		float height = 0.0f;
		string[] row_data = Sec_Data.text.Split ('\n');
		if (location == SECTION_LOC.L) {
			if (Left.childCount == 0) {
				c_col -= origin_offset;
				SectionData.L_Sec = new string[row_data.Length][];
				for (int i = 0; i < row_data.Length; i++) {
					SectionData.L_Sec [i] = row_data [i].Split (' ');
				}
				foreach (string[] row in SectionData.L_Sec) {
					foreach (string cell in row) {
						height = (float)(char.GetNumericValue (cell [3]) / 2);
						MakeCell (c_row, c_col, height, cell, Left);
						c_col += 1;
					}
					c_row -= 1;
					c_col = (int)Cur_Sec_Origin_x - origin_offset;
				}
			}
		} else if (location == SECTION_LOC.R) {
			if (Right.childCount == 0) {
				c_col += origin_offset;
				SectionData.R_Sec = new string[row_data.Length][];
				for (int i = 0; i < row_data.Length; i++) {
					SectionData.R_Sec [i] = row_data [i].Split (' ');
				}
				foreach (string[] row in SectionData.R_Sec) {
					foreach (string cell in row) {
						height = (float)(char.GetNumericValue (cell [3]) / 2);
						MakeCell (c_row, c_col, height, cell, Right);
						c_col += 1;
					}
					c_row -= 1;
					c_col = (int)Cur_Sec_Origin_x + origin_offset;
				}
			}
		} else if (location == SECTION_LOC.T) {
			if (Top.childCount == 0) {
				c_row += origin_offset;
				SectionData.T_Sec = new string[row_data.Length][];
				for (int i = 0; i < row_data.Length; i++) {
					SectionData.T_Sec [i] = row_data [i].Split (' ');
				}
				foreach (string[] row in SectionData.T_Sec) {
					foreach (string cell in row) {
						height = (float)(char.GetNumericValue (cell [3]) / 2);
						MakeCell (c_row, c_col, height, cell, Top);
						c_col += 1;
					}
					c_row -= 1;
					c_col = (int)Cur_Sec_Origin_x;
				}
			}
		} else if (location == SECTION_LOC.B) {
			if (Bot.childCount == 0) {
				c_row -= origin_offset;
				SectionData.B_Sec = new string[row_data.Length][];
				for (int i = 0; i < row_data.Length; i++) {
					SectionData.B_Sec [i] = row_data [i].Split (' ');
				}
				foreach (string[] row in SectionData.B_Sec) {
					foreach (string cell in row) {
						height = (float)(char.GetNumericValue (cell [3]) / 2);
						MakeCell (c_row, c_col, height, cell, Bot);
						c_col += 1;
					}
					c_row -= 1;
					c_col = (int)Cur_Sec_Origin_x;
				}
			}
		} else if (location == SECTION_LOC.TL) {
			if (Top_L.childCount == 0) {
				c_col -= origin_offset;
				c_row += origin_offset;
				SectionData.TL_Sec = new string[row_data.Length][];
				for (int i = 0; i < row_data.Length; i++) {
					SectionData.TL_Sec [i] = row_data [i].Split (' ');
				}
				foreach (string[] row in SectionData.TL_Sec) {
					foreach (string cell in row) {
						height = (float)(char.GetNumericValue (cell [3]) / 2);
						MakeCell (c_row, c_col, height, cell, Top_L);
						c_col += 1;
					}
					c_row -= 1;
					c_col = (int)Cur_Sec_Origin_x - origin_offset;
				}
			}
		} else if (location == SECTION_LOC.TR) {
			if (Top_R.childCount == 0) {
				c_col += origin_offset;
				c_row += origin_offset;
				SectionData.TR_Sec = new string[row_data.Length][];
				for (int i = 0; i < row_data.Length; i++) {
					SectionData.TR_Sec [i] = row_data [i].Split (' ');
				}
				foreach (string[] row in SectionData.TR_Sec) {
					foreach (string cell in row) {
						height = (float)(char.GetNumericValue (cell [3]) / 2);
						MakeCell (c_row, c_col, height, cell, Top_R);
						c_col += 1;
					}
					c_row -= 1;
					c_col = (int)Cur_Sec_Origin_x + origin_offset;
				}
			}
		} else if (location == SECTION_LOC.BL) {
			if (Bot_L.childCount == 0) {
				c_col -= origin_offset;
				c_row -= origin_offset;
				SectionData.BL_Sec = new string[row_data.Length][];
				for (int i = 0; i < row_data.Length; i++) {
					SectionData.BL_Sec [i] = row_data [i].Split (' ');
				}
				foreach (string[] row in SectionData.BL_Sec) {
					foreach (string cell in row) {
						height = (float)(char.GetNumericValue (cell [3]) / 2);
						MakeCell (c_row, c_col, height, cell, Bot_L);
						c_col += 1;
					}
					c_row -= 1;
					c_col = (int)Cur_Sec_Origin_x - origin_offset;
				}
			}
		} else if (location == SECTION_LOC.BR) {
			if (Bot_R.childCount == 0) {
				c_col += origin_offset;
				c_row -= origin_offset;
				SectionData.BR_Sec = new string[row_data.Length][];
				for (int i = 0; i < row_data.Length; i++) {
					SectionData.BR_Sec [i] = row_data [i].Split (' ');
				}
				foreach (string[] row in SectionData.BR_Sec) {
					foreach (string cell in row) {
						height = (float)(char.GetNumericValue (cell [3]) / 2);
						MakeCell (c_row, c_col, height, cell, Bot_R);
						c_col += 1;
					}
					c_row -= 1;
					c_col = (int)Cur_Sec_Origin_x + origin_offset;
				}
			}
		} else {
			SectionData.Cur_Sec = new string[row_data.Length][];
			for (int i = 0; i < row_data.Length; i++) {
				SectionData.Cur_Sec [i] = row_data [i].Split (' ');
			}
			foreach (string[] row in SectionData.Cur_Sec) {
				foreach (string cell in row) {
					height = (float)(char.GetNumericValue (cell [3]) / 2);
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

	// Create Section file path for sec_num
	string GetSectionPath (int sec_num){
		string file_path = "Assets/Resources/Sections/";
		char c = '0';
		c += (char)(sec_num / 10);
		file_path += c;
		c = '0';
		c += (char)(sec_num % 10);
		file_path += c;
		return file_path;
	}

	// Create file path to prefab listed in info
	string GetPrefabPath (string info) {
		string file_path;
		if (info [0] == 'p') {
			file_path = "platform_";
			file_path += info [1];
		} else if (info [0] == 'w') {
			file_path = "wall_";
			file_path += info [1];
		} else {
			file_path = "ramp";
		}
		return file_path;
	}

	// Decides how much higher/lower a cell needs to be based on its type
	float HeightOffset (string cell) {
		float offset = 0.0f;
		if (cell [0] == 'w') {
			offset = (cell[1] == 'o') ? 0.0f : 0.25f;
		} else if (cell [0] == 'r') {
			offset = 0.25f;
		}
		return offset;
	}

	// Assigns the appropriate tag to cell
	void AssignTag (GameObject cell, string info) {
		if (info [0] == 'p') {
			cell.tag = "Platform";
		} else if (info [0] == 'w') {
			cell.tag = "Wall";
		} else if (info [0] == 'r') {
			cell.tag = "Ramp";
		} else {
			cell.tag = "Floor";
		}
	}

	// Creates a section cell in the game space
	void MakeCell (int row, int col, float height, string info, Transform section) {
		string file_path = "Prefabs/";
		float x_rot;
		float y_rot;
		// make flat panel
		if (char.IsDigit (info [0])) {
			file_path += "Panel_0";
			x_rot = 90f;
			y_rot = 0f;
		}
		// make special platform
		else {
			file_path += GetPrefabPath (info);
			height += HeightOffset (info);
			x_rot = (info.Substring (0, 2) == "wo") ? 90f : 0f;
			y_rot = 90f*(float)(char.GetNumericValue(info[2]));
		}
		GameObject cell;
		cell = Instantiate (Resources.Load (file_path), new Vector3 ((float)col, height, (float)row), Quaternion.Euler (x_rot, y_rot, 0)) as GameObject;
		cell.transform.parent = section;
		AssignTag (cell, info);
	}

	// make cells of start the cells of end
	void ChangeSection(Transform start, Transform end){
		uint i = 0;
		GameObject[] cell_list = new GameObject[start.childCount];
		foreach (Transform cell in start) {
			cell_list [i] = cell.gameObject;
			i++;
		}
		for (i = 0; i < cell_list.Length; i++) {
			cell_list [i].transform.SetParent (end);
		}
	}

	// move cells in a section row up a section row
	void MoveCellsUp(){
		ClearSections (SECTION_LOC.T);
		// move cur row up
		ChangeSection (Left, Top_L);
		ChangeSection (Cur, Top);
		ChangeSection (Right, Top_R);
		// move bot row up
		ChangeSection (Bot_L, Left);
		ChangeSection (Bot, Cur);
		ChangeSection (Bot_R, Right);
	}
	// move cells in a section row down a section row
	void MoveCellsDown(){
		ClearSections (SECTION_LOC.B);
		// move cur row down
		ChangeSection (Left, Bot_L);
		ChangeSection (Cur, Bot);
		ChangeSection (Right, Bot_R);
		// move top row down
		ChangeSection(Top_L, Left);
		ChangeSection (Top, Cur);
		ChangeSection (Top_R, Right);
	}
	// move cells in a section column left a section column
	void MoveCellsLeft(){
		ClearSections (SECTION_LOC.L);
		// move cur column left
		ChangeSection (Top, Top_L);
		ChangeSection (Cur, Left);
		ChangeSection (Bot, Bot_L);
		// move right column left
		ChangeSection (Top_R, Top);
		ChangeSection (Right, Cur);
		ChangeSection (Bot_R, Bot);
	}
	// move cells in a section column right a section column
	void MoveCellsRight(){
		ClearSections (SECTION_LOC.R);
		// move cur column right
		ChangeSection (Top, Top_R);
		ChangeSection (Cur, Right);
		ChangeSection (Bot, Bot_R);
		// move left column right
		ChangeSection (Top_L, Top);
		ChangeSection (Left, Cur);
		ChangeSection (Bot_L, Bot);
	}

	// destroy cells in Transform section
	void ClearCells(Transform section){
		uint i = 0;
		GameObject[] cell_list = new GameObject[section.childCount];
		foreach (Transform cell in section) {
			cell_list [i] = cell.gameObject;
			i++;
		}
		section.DetachChildren ();
		for (i = 0; i < cell_list.Length; i++) {
			Destroy (cell_list [i]);
		}
	}

	// Empties sections of cells
	IEnumerator ClearSections(SECTION_LOC section_group){
		loading = true;
		// move to the bottom section row
		if (section_group == SECTION_LOC.T) {
			// change current row cell data to bottom row
			for (uint r = 0; r < (uint)PlayerMovement.Sec_Width; r++) {
				for (uint c = 0; c < (uint)PlayerMovement.Sec_Width; c++) {
					// copy the cur row cell data into the top row
					SectionData.TL_Sec [r] [c] = SectionData.L_Sec [r] [c];
					SectionData.T_Sec [r] [c] = SectionData.Cur_Sec [r] [c];
					SectionData.TR_Sec [r] [c] = SectionData.R_Sec  [r] [c];
					// copy the bot row cell data into the cur row
					SectionData.L_Sec [r] [c] = SectionData.BL_Sec [r] [c];
					SectionData.Cur_Sec [r] [c] = SectionData.B_Sec [r] [c];
					SectionData.R_Sec [r] [c] = SectionData.BR_Sec [r] [c];
					// clear out the bot row cell data
					SectionData.BL_Sec [r] [c] = null;
					SectionData.B_Sec [r] [c] = null;
					SectionData.BR_Sec [r] [c] = null;
				}
			}
			// clear out the top row cell game objects
			ClearCells (Top_L);
			ClearCells (Top);
			ClearCells (Top_R);
			MoveCellsUp ();
			// Load new bot row
			LoadSection (Cur_Sec + 9, SECTION_LOC.BL);
			LoadSection (Cur_Sec + 10, SECTION_LOC.B);
			LoadSection (Cur_Sec + 11, SECTION_LOC.BR);
		}
		// move to the top section row
		else if (section_group == SECTION_LOC.B) {
			// change current row cell data to top row
			for (uint r = 0; r < (uint)PlayerMovement.Sec_Width; r++) {
				for (uint c = 0; c < (uint)PlayerMovement.Sec_Width; c++) {
					// copy cur row cell data into the bot row
					SectionData.BL_Sec [r] [c] = SectionData.L_Sec [r] [c];
					SectionData.B_Sec [r] [c] = SectionData.Cur_Sec [r] [c];
					SectionData.BR_Sec [r] [c] = SectionData.R_Sec [r] [c];
					// copy the top row cell data into the cur row
					SectionData.L_Sec [r] [c] = SectionData.TL_Sec [r] [c];
					SectionData.Cur_Sec [r] [c] = SectionData.T_Sec [r] [c];
					SectionData.R_Sec [r] [c] = SectionData.TR_Sec [r] [c];
					// clear out the top row cell data
					SectionData.TL_Sec [r] [c] = null;
					SectionData.T_Sec [r] [c] = null;
					SectionData.TR_Sec [r] [c] = null;
				}
			}
			// clear out the bot row cell game objects
			ClearCells (Bot_L);
			ClearCells (Bot);
			ClearCells (Bot_R);
			MoveCellsDown ();
			// load new top row
			LoadSection (Cur_Sec - 11, SECTION_LOC.TL);
			LoadSection (Cur_Sec - 10, SECTION_LOC.T);
			LoadSection (Cur_Sec - 9, SECTION_LOC.TR);
		}
		// move to the right section column
		else if (section_group == SECTION_LOC.L) {
			// change current column cell data to the right column
			for (uint r = 0; r < (uint)PlayerMovement.Sec_Width; r++) {
				for (uint c = 0; c < (uint)PlayerMovement.Sec_Width; c++) {
					// copy cur column cell data into the left column
					SectionData.TL_Sec [r] [c] = SectionData.T_Sec [r] [c];
					SectionData.L_Sec [r] [c] = SectionData.Cur_Sec [r] [c];
					SectionData.BL_Sec [r] [c] = SectionData.B_Sec [r] [c];
					// copy the right column cell data into cur column
					SectionData.T_Sec [r] [c] = SectionData.TR_Sec [r] [c];
					SectionData.Cur_Sec [r] [c] = SectionData.R_Sec [r] [c];
					SectionData.B_Sec [r] [c] = SectionData.BR_Sec [r] [c];
					// clear out the right column cell data
					SectionData.TR_Sec [r] [c] = null;
					SectionData.R_Sec [r] [c] = null;
					SectionData.BR_Sec [r] [c] = null;
				}
			}
			// clear out the left column cell game objects
			ClearCells (Top_L);
			ClearCells (Left);
			ClearCells (Bot_L);
			MoveCellsLeft ();
			// load new right section column
			LoadSection (Cur_Sec - 9, SECTION_LOC.TR);
			LoadSection (Cur_Sec + 1, SECTION_LOC.R);
			LoadSection (Cur_Sec + 11, SECTION_LOC.BR);
		}
		// move to the left section column
		else {
			// change current column data to the left column
			for (uint r = 0; r < (uint)PlayerMovement.Sec_Width; r++) {
				for (uint c = 0; c < (uint)PlayerMovement.Sec_Width; c++) {
					// copy cur column cell data into the right column
					SectionData.TR_Sec [r] [c] = SectionData.T_Sec [r] [c];
					SectionData.R_Sec [r] [c] = SectionData.Cur_Sec [r] [c];
					SectionData.BR_Sec [r] [c] = SectionData.B_Sec [r] [c];
					// copy the left column cell data into cur column
					SectionData.T_Sec [r] [c] = SectionData.TL_Sec [r] [c];
					SectionData.Cur_Sec [r] [c] = SectionData.L_Sec [r] [c];
					SectionData.B_Sec [r] [c] = SectionData.BL_Sec [r] [c];
					// clear out the left column cell data
					SectionData.TL_Sec [r] [c] = null;
					SectionData.L_Sec [r] [c] = null;
					SectionData.BL_Sec [r] [c] = null;
				}
			}
			// clear out the right column cell game objects
			ClearCells (Top_R);
			ClearCells (Right);
			ClearCells (Bot_R);
			MoveCellsRight ();
			// load new left section column
			LoadSection (Cur_Sec - 11, SECTION_LOC.TL);
			LoadSection (Cur_Sec - 1, SECTION_LOC.L);
			LoadSection (Cur_Sec + 9, SECTION_LOC.BL);
		}
		loading = false;
		yield return null;
	}
}
