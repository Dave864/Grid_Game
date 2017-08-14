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

	enum ARENA_MAP {TL, TR, BL, BR};

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
		CreateArena ();
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

	// Create the arena for the encounter
	void CreateArena(){
		int base_height = PlayerPrefs.GetInt ("Cur");
		if (PlayerPrefs.GetString ("ArenaType") == "double") {
			if (PlayerPrefs.GetInt ("TL") < -1) {
				// left-right arena with player at left
				if (PlayerPrefs.GetInt ("TR") > -1) {
					Debug_Text.text = "left-right arena w/ player at left\n";
					base_height = (base_height >= PlayerPrefs.GetInt ("TR")) ? PlayerPrefs.GetInt ("TR") : base_height;
					LoadSection (ARENA_MAP.TL, TopLeft, TL_Map);
					LoadSection (ARENA_MAP.TR, TopRight, TR_Map, 0, 1);
					TopLeft.transform.position = new Vector3 (0, (float)(PlayerPrefs.GetInt ("Cur") - base_height), 0);
					TopRight.transform.position = new Vector3 (Cell_Row_Cnt * Hoz_Offset, ((float)PlayerPrefs.GetInt ("TR") - base_height), 0);
				}
				// up-down arena with player at top
				else {
					Debug_Text.text = "top-bot arena w/ player at top\n";
					base_height = (base_height >= PlayerPrefs.GetInt ("BL")) ? PlayerPrefs.GetInt ("BL") : base_height;
					LoadSection (ARENA_MAP.TL, TopLeft, TL_Map);
					LoadSection (ARENA_MAP.BL, BotLeft, BL_Map, 1, 0);
					TopLeft.transform.position = new Vector3 (0, (float)(PlayerPrefs.GetInt ("Cur") - base_height), 0);
					BotLeft.transform.position = new Vector3 (0, (float)(PlayerPrefs.GetInt ("BL") - base_height), Cell_Row_Cnt * Vert_Offset);
				}
			} else if (PlayerPrefs.GetInt ("TR") < -1) {
				// left-right arena with player at right
				Debug_Text.text = "left-right arena w/ player at right\n";
				base_height = (base_height >= PlayerPrefs.GetInt ("TL")) ? PlayerPrefs.GetInt ("TL") : base_height;
				LoadSection (ARENA_MAP.TL, TopLeft, TL_Map, 0, -1);
				LoadSection(ARENA_MAP.TR, TopRight, TR_Map);
				TopRight.transform.position = new Vector3 (0, (float)(PlayerPrefs.GetInt ("Cur") - base_height), 0);
				TopLeft.transform.position = new Vector3 (-Cell_Row_Cnt * Hoz_Offset, (float)(PlayerPrefs.GetInt ("TL") - base_height), 0);
			} else {
				// up-down arena with player at bottom
				Debug_Text.text = "top-bot arena w/ player at bottom\n";
				base_height = (base_height >= PlayerPrefs.GetInt ("TL")) ? PlayerPrefs.GetInt ("TL") : base_height;
				LoadSection (ARENA_MAP.TL, TopLeft, TL_Map, -1, 0);
				LoadSection (ARENA_MAP.BL, BotLeft, BL_Map);
				BotLeft.transform.position = new Vector3 (0, (float)(PlayerPrefs.GetInt ("Cur") - base_height), 0);
				TopLeft.transform.position = new Vector3 (0, (float)(PlayerPrefs.GetInt ("TL") - base_height), -4 * Vert_Offset);
			}
		} else if (PlayerPrefs.GetString ("ArenaType") == "quad") {
			// player is in top-left
			if (PlayerPrefs.GetInt ("TL") < 0) {
				base_height = (base_height >= PlayerPrefs.GetInt ("TR")) ? PlayerPrefs.GetInt ("TR") : base_height;
				base_height = (base_height >= PlayerPrefs.GetInt ("BL")) ? PlayerPrefs.GetInt ("BL") : base_height;
				base_height = (base_height >= PlayerPrefs.GetInt ("BR")) ? PlayerPrefs.GetInt ("BR") : base_height;
				LoadSection (ARENA_MAP.TL, TopLeft, TL_Map);
				LoadSection (ARENA_MAP.TR, TopRight, TR_Map, 0, 1);
				LoadSection (ARENA_MAP.BL, BotLeft, BL_Map, 1, 0);
				LoadSection (ARENA_MAP.BR, BotRight, BR_Map, 1, 1);
				TopLeft.transform.position = new Vector3 (0, (float)(PlayerPrefs.GetInt ("Cur") - base_height), 0);
				TopRight.transform.position = new Vector3 (Cell_Row_Cnt * Hoz_Offset, (float)(PlayerPrefs.GetInt ("TR") - base_height), 0);
				BotLeft.transform.position = new Vector3 (0, (float)(PlayerPrefs.GetInt("BL") - base_height), Cell_Row_Cnt * Vert_Offset);
				BotRight.transform.position = new Vector3 (Cell_Row_Cnt * Hoz_Offset, (float)(PlayerPrefs.GetInt ("BR") - base_height), Cell_Row_Cnt * Vert_Offset);
			}
			// player is in top-right
			else if (PlayerPrefs.GetInt ("TR") < 0) {
				base_height = (base_height >= PlayerPrefs.GetInt ("TL")) ? PlayerPrefs.GetInt ("TL") : base_height;
				base_height = (base_height >= PlayerPrefs.GetInt ("BL")) ? PlayerPrefs.GetInt ("BL") : base_height;
				base_height = (base_height >= PlayerPrefs.GetInt ("BR")) ? PlayerPrefs.GetInt ("BR") : base_height;
				LoadSection (ARENA_MAP.TL, TopLeft, TL_Map, 0, -1);
				LoadSection (ARENA_MAP.TR, TopRight, TR_Map);
				LoadSection (ARENA_MAP.BL, BotLeft, BL_Map, 1, -1);
				LoadSection (ARENA_MAP.BR, BotRight, BR_Map, 1, 0);
				TopLeft.transform.position = new Vector3 (-Cell_Row_Cnt * Hoz_Offset, (float)(PlayerPrefs.GetInt ("TL") - base_height), 0);
				TopRight.transform.position = new Vector3 (0, (float)(PlayerPrefs.GetInt ("Cur") - base_height), 0);
				BotLeft.transform.position = new Vector3 (-Cell_Row_Cnt * Hoz_Offset, (float)(PlayerPrefs.GetInt("BL") - base_height), Cell_Row_Cnt * Vert_Offset);
				BotRight.transform.position = new Vector3 (0, (float)(PlayerPrefs.GetInt ("BR") - base_height), Cell_Row_Cnt * Vert_Offset);
			}
			// player is in bottom-left
			else if (PlayerPrefs.GetInt ("BL") < 0) {
				base_height = (base_height >= PlayerPrefs.GetInt ("TL")) ? PlayerPrefs.GetInt ("TL") : base_height;
				base_height = (base_height >= PlayerPrefs.GetInt ("TR")) ? PlayerPrefs.GetInt ("TR") : base_height;
				base_height = (base_height >= PlayerPrefs.GetInt ("BR")) ? PlayerPrefs.GetInt ("BR") : base_height;
				LoadSection (ARENA_MAP.TL, TopLeft, TL_Map, -1, 0);
				LoadSection (ARENA_MAP.TR, TopRight, TR_Map, -1, 1);
				LoadSection (ARENA_MAP.BL, BotLeft, BL_Map);
				LoadSection (ARENA_MAP.BR, BotRight, BR_Map, 0, 1);
				TopLeft.transform.position = new Vector3 (0, (float)(PlayerPrefs.GetInt ("TL") - base_height), -Cell_Row_Cnt * Vert_Offset);
				TopRight.transform.position = new Vector3 (Cell_Row_Cnt * Hoz_Offset, (float)(PlayerPrefs.GetInt ("TR") - base_height), -Cell_Row_Cnt * Vert_Offset);
				BotLeft.transform.position = new Vector3 (0, (float)(PlayerPrefs.GetInt ("Cur") - base_height), 0);
				BotRight.transform.position = new Vector3 (Cell_Row_Cnt * Hoz_Offset, (float)(PlayerPrefs.GetInt ("BR") - base_height), 0);
			}
			// player is in bottom-right
			else {
				base_height = (base_height >= PlayerPrefs.GetInt ("TL")) ? PlayerPrefs.GetInt ("TL") : base_height;
				base_height = (base_height >= PlayerPrefs.GetInt ("TR")) ? PlayerPrefs.GetInt ("TR") : base_height;
				base_height = (base_height >= PlayerPrefs.GetInt ("BL")) ? PlayerPrefs.GetInt ("BL") : base_height;
				LoadSection (ARENA_MAP.TL, TopLeft, TL_Map, -1, -1);
				LoadSection (ARENA_MAP.TR, TopRight, TR_Map, -1, 0);
				LoadSection (ARENA_MAP.BR, BotLeft, BL_Map, 0, -1);
				LoadSection (ARENA_MAP.BR, BotRight, BR_Map);
				TopLeft.transform.position = new Vector3 (-Cell_Row_Cnt * Hoz_Offset, (float)(PlayerPrefs.GetInt ("TL") - base_height), -Cell_Row_Cnt * Vert_Offset);
				TopRight.transform.position = new Vector3 (0, (float)(PlayerPrefs.GetInt ("TR") - base_height), -Cell_Row_Cnt * Vert_Offset);
				BotLeft.transform.position = new Vector3 (-Cell_Row_Cnt * Hoz_Offset, (float)(PlayerPrefs.GetInt ("BL") - base_height), 0);
				BotRight.transform.position = new Vector3 (0, (float)(PlayerPrefs.GetInt ("Cur") - base_height), 0);
			}
		} else {
			LoadSection (ARENA_MAP.TL, TopLeft, TL_Map);
		}
	}

	// Loads the sec_area
	void LoadSection (ARENA_MAP type, Transform section, ArenaSectionMap sec_map, int r = 0, int c = 0) {
		Sec_Data = (TextAsset)Resources.Load (GetCellInfoPath (r, c), typeof(TextAsset));
		string[] cell_row = Sec_Data.text.Split ('\n');
		for (int row = 0; row < cell_row.Length; row++) {
			int col = 0;
			foreach (string cell_data in cell_row[row].Split(' ')) {
				int height = (int)char.GetNumericValue (cell_data [1]);
				sec_map.Cell_Map [row] [col] = new ArenaSectionMap.Cell ((10 * row) + col, MakeCell ((float)row, (float)col, (float)height, section));
				LinkNodes (type, sec_map, row, col);
				col++;
			}
		}
	}

	// Make arena cell
	GameObject MakeCell(float row, float col, float height, Transform section){
		string cell_object_path = "Prefabs/arena_cell";
		col = (row % 2 != 0) ? col + 0.5f : col;
		GameObject cell_object = Instantiate (Resources.Load (cell_object_path), new Vector3 (col * Hoz_Offset, height / 8, row * Vert_Offset), Quaternion.Euler (0, 90.0f, 0)) as GameObject;
		cell_object.transform.localScale += new Vector3 (0, height * 1.0f, 0);
		cell_object.transform.SetParent (section);
		cell_object.tag = "Arena Cell";
		return cell_object;
	}

	// Connect nodes of adjacent cells
	void LinkNodes(ARENA_MAP type, ArenaSectionMap sec_map, int row, int col){
		if (PlayerPrefs.GetString ("ArenaType") == "double") {
			if (PlayerPrefs.GetInt ("TL") < -1) {
				// left-right arena with player at left
				if (PlayerPrefs.GetInt ("TR") > -1) {
					LinkLR (type, sec_map, row, col);
				}
				// up-down arena with player at top
				else {
					LinkUD (type, sec_map, row, col);
				}
			} else if (PlayerPrefs.GetInt ("TR") < -1) {
				// left-right arena with player at right
				LinkLR (type, sec_map, row, col);
			} else {
				// up-down arena with player at bottom
				LinkUD (type, sec_map, row, col);
			}
		} else if (PlayerPrefs.GetString ("ArenaType") == "quad") {
			LinkQuad (type, sec_map, row, col);
		} else {
		}
	}

	// Link nodes of left-right arena
	void LinkLR(ARENA_MAP type, ArenaSectionMap sec_map, int row, int col){
		if (row == 0) {
			sec_map.Cell_Map [row] [col].LinkNode0 ();
			sec_map.Cell_Map [row] [col].LinkNode1 ();
		} else if (row == Cell_Row_Cnt - 1) {
			sec_map.Cell_Map [row] [col].LinkNode3 ();
			sec_map.Cell_Map [row] [col].LinkNode4 ();
		}
		if (type == ARENA_MAP.TL) {
			if (row > 0) {
				if (row % 2 == 0) {
					if (col == 0) {
						sec_map.Cell_Map [row] [col].LinkNode0 ();
						sec_map.Cell_Map [row] [col].LinkNode1 (sec_map.Cell_Map [row - 1] [col]);
						sec_map.Cell_Map [row] [col].LinkNode5 ();
					} else {
						sec_map.Cell_Map [row] [col].LinkNode0 (sec_map.Cell_Map [row - 1] [col - 1]);
						sec_map.Cell_Map [row] [col].LinkNode1 (sec_map.Cell_Map [row - 1] [col]);
						sec_map.Cell_Map [row] [col].LinkNode5 (sec_map.Cell_Map [row] [col - 1]);
					}
				} else {
					if (col == 0) {
						sec_map.Cell_Map [row] [col].LinkNode0 (sec_map.Cell_Map [row - 1] [col]);
						sec_map.Cell_Map [row] [col].LinkNode1 (sec_map.Cell_Map [row - 1] [col + 1]);
						sec_map.Cell_Map [row] [col].LinkNode5 ();
					} else {
						sec_map.Cell_Map [row] [col].LinkNode0 (sec_map.Cell_Map [row - 1] [col]);
						sec_map.Cell_Map [row] [col].LinkNode1 ();
						sec_map.Cell_Map [row] [col].LinkNode5 (sec_map.Cell_Map [row] [col - 1]);
					}
				}
			}	
		} else {
			if (row > 0) {
				if (row % 2 == 0) {
					if (col == 0) {
						sec_map.Cell_Map [row] [col].LinkNode0 ();
						sec_map.Cell_Map [row] [col].LinkNode1 (sec_map.Cell_Map [row - 1] [col]);
						sec_map.Cell_Map [row] [col].LinkNode5 (TL_Map.Cell_Map [row] [Cell_Row_Cnt - 1]);
					} else {
						sec_map.Cell_Map [row] [col].LinkNode0 (sec_map.Cell_Map [row - 1] [col - 1]);
						sec_map.Cell_Map [row] [col].LinkNode1 (sec_map.Cell_Map [row - 1] [col]);
						sec_map.Cell_Map [row] [col].LinkNode5 (sec_map.Cell_Map [row] [col - 1]);
						if (col == Cell_Row_Cnt - 1) {
							sec_map.Cell_Map [row] [col].LinkNode2 ();
						}
					}
				} else {
					if (col == 0) {
						sec_map.Cell_Map [row] [col].LinkNode0 (TL_Map.Cell_Map [row - 1] [Cell_Row_Cnt - 1]);
						sec_map.Cell_Map [row] [col].LinkNode1 (sec_map.Cell_Map [row - 1] [col]);
					} else if (col == Cell_Row_Cnt - 1) {
						sec_map.Cell_Map [row] [col].LinkNode0 (TL_Map.Cell_Map [row - 1] [Cell_Row_Cnt - 1]);
						sec_map.Cell_Map [row] [col].LinkNode1 ();
						sec_map.Cell_Map [row] [col].LinkNode2 ();
						sec_map.Cell_Map [row] [col].LinkNode3 ();
						sec_map.Cell_Map [row] [col].LinkNode5 (sec_map.Cell_Map [row] [col - 1]);
					} else {
						sec_map.Cell_Map [row] [col].LinkNode0 (TL_Map.Cell_Map [row - 1] [Cell_Row_Cnt - 1]);
						sec_map.Cell_Map [row] [col].LinkNode1 (sec_map.Cell_Map [row - 1] [col]);
						sec_map.Cell_Map [row] [col].LinkNode5 (sec_map.Cell_Map [row] [col - 1]);
					}
				}
			}
		}
	}

	// Link nodes of up-down arena
	void LinkUD(ARENA_MAP type, ArenaSectionMap sec_map, int row, int col){
		if (col == 0) {
			if (row % 2 == 0) {
				sec_map.Cell_Map [row] [col].LinkNode0 ();
				sec_map.Cell_Map [row] [col].LinkNode5 ();
				sec_map.Cell_Map [row] [col].LinkNode4 ();
			} else {
				sec_map.Cell_Map [row] [col].LinkNode5 ();
			}
		} else if (col == Cell_Row_Cnt - 1) {
			if (row % 2 == 0) {
				sec_map.Cell_Map [row] [col].LinkNode2 ();
			} else {
				sec_map.Cell_Map [row] [col].LinkNode1 ();
				sec_map.Cell_Map [row] [col].LinkNode2 ();
				sec_map.Cell_Map [row] [col].LinkNode3 ();
			}
		}
		if (type == ARENA_MAP.TL) {
			if (row == 0) {
				sec_map.Cell_Map [row] [col].LinkNode1 ();
				if (col > 0) {
					sec_map.Cell_Map [row] [col].LinkNode0 ();
					sec_map.Cell_Map [row] [col].LinkNode5 (sec_map.Cell_Map [row] [col - 1]);
					if (col == Cell_Row_Cnt - 1) {
						sec_map.Cell_Map [row] [col].LinkNode2 ();
					}
				}
			} else {
				if (col == 0) {
					if (row % 2 == 0) {
						sec_map.Cell_Map [row] [col].LinkNode1 (sec_map.Cell_Map [row - 1] [col]);
					} else {
						sec_map.Cell_Map [row] [col].LinkNode0 (sec_map.Cell_Map [row - 1] [col]);
						sec_map.Cell_Map [row] [col].LinkNode1 (sec_map.Cell_Map [row - 1] [col + 1]);
					}
				} else {
					sec_map.Cell_Map [row] [col].LinkNode5 (sec_map.Cell_Map [row] [col - 1]);
					if (row % 2 == 0) {
						sec_map.Cell_Map [row] [col].LinkNode0 (sec_map.Cell_Map [row - 1] [col - 1]);
						sec_map.Cell_Map [row] [col].LinkNode1 (sec_map.Cell_Map [row - 1] [col]);
					} else {
						if (col != Cell_Row_Cnt - 1) {
							sec_map.Cell_Map [row] [col].LinkNode1 (sec_map.Cell_Map [row - 1] [col + 1]);
						} else {
							sec_map.Cell_Map [row] [col].LinkNode1 ();
						}
						sec_map.Cell_Map [row] [col].LinkNode0 (sec_map.Cell_Map [row - 1] [col]);
					}
				}
			}
		} else {
			if (row == 0) {
				sec_map.Cell_Map [row] [col].LinkNode1 (TL_Map.Cell_Map [Cell_Row_Cnt - 1] [col]);
				if (col > 0) {
					sec_map.Cell_Map [row] [col].LinkNode0 (TL_Map.Cell_Map [Cell_Row_Cnt - 1] [col - 1]);
					sec_map.Cell_Map [row] [col].LinkNode5 (sec_map.Cell_Map [row] [col - 1]);
					if (col == Cell_Row_Cnt - 1) {
						sec_map.Cell_Map [row] [col].LinkNode2 ();
					}
				}
			} else {
				sec_map.Cell_Map [row] [col].LinkNode5 (sec_map.Cell_Map [row] [col - 1]);
				if (row % 2 == 0) {
					sec_map.Cell_Map [row] [col].LinkNode0 (sec_map.Cell_Map [row - 1] [col - 1]);
					sec_map.Cell_Map [row] [col].LinkNode1 (sec_map.Cell_Map [row - 1] [col]);
				} else {
					if (col != Cell_Row_Cnt - 1) {
						sec_map.Cell_Map [row] [col].LinkNode1 (sec_map.Cell_Map [row - 1] [col + 1]);
					} else {
						sec_map.Cell_Map [row] [col].LinkNode1 ();
						sec_map.Cell_Map [row] [col].LinkNode3 ();
						sec_map.Cell_Map [row] [col].LinkNode4 ();
					}
					sec_map.Cell_Map [row] [col].LinkNode0 (sec_map.Cell_Map [row - 1] [col]);
				}
			}
		}
	}

	// Link nodes of a quad arena
	void LinkQuad(ARENA_MAP type, ArenaSectionMap sec_map, int row, int col){
		if (type == ARENA_MAP.TL) {
			// top row connects with null
			// left column connects with null
		} else if (type == ARENA_MAP.TR) {
			// top row connects with null
			// left column connects with TL
		} else if (type == ARENA_MAP.BL) {
			// top row connects with TL
			// left column connects with null
		} else {
			// top row connects with TR
			// left column connects with BL
		}
	}

	// Get file path for CellInfo
	string GetCellInfoPath(int row, int col){
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
		file_path += PlayerPrefs.GetInt ("Row") + row;
		file_path += PlayerPrefs.GetInt ("Column") + col;
		return file_path;
	}
}
