using UnityEngine;
using UnityEngine.UI;
using System.Collections;

// Handles player movement in the overworld
public class PlayerMovement : MonoBehaviour {

	// player coordinates
	public int Test_Sec;
	public int Test_Row;
	public int Test_Col;
	[HideInInspector] static public int Cur_Sec;
	[HideInInspector] static public int Cur_Row;
	[HideInInspector] static public int Cur_Col;

	// section dimensions
	[HideInInspector] static public float Sec_Width = 10.0f;
	enum SECTION_LOC {CUR, L, R, T, B, TL, TR, BL, BR};

	// rotation and movement information
	private float Rotation = 0f;
	private float Rot_Speed = 200f;
	private float Mov_Speed = 2.5f;
	private bool Moving = false;
	private bool Rotating = false;
	private bool RotButtDown = false;
	private Quaternion Rotate_To = Quaternion.Euler(30f, 0f, 0f);
	enum HELD_DIR_BUTT {VERT, HOZ, NONE};
	HELD_DIR_BUTT Held_Butt;

	// text showing player coordinates
	public Text Coordinates_Text;
	public Text Debug_Text;

	// Initialize initial section and intial row and column
	void Awake () {
		Cur_Sec = Test_Sec;
		if (Test_Row < 0) {
			Cur_Row = 0;
		} else if (Test_Row > (int)Sec_Width - 1) {
			Cur_Row = (int)Sec_Width - 1;
		} else {
			Cur_Row = Test_Row;
		}
		if (Test_Col < 0) {
			Cur_Col = 0;
		} else if (Test_Col > (int)Sec_Width - 1) {
			Cur_Col = (int)Sec_Width - 1;
		} else {
			Cur_Col = Test_Col;
		}
	}

	// Use this for initialization
	void Start () {
		// get the player's y coordinate
		string cellData = SectionData.Cur_Sec [Cur_Row][Cur_Col];
		float height = (float)(char.GetNumericValue (cellData [3]) / 2);
		if (cellData [0] == 'r') {
			height += 0.25f;
		} else if (cellData [0] == 'p') {
			height += 0.5f;
		}
		// place player at coordinates
		transform.position = new Vector3 (transform.position.x + Cur_Col, transform.position.y + height, transform.position.z - Cur_Row);
		Debug_Text.text = "Waiting...";
		DisplayPos ();
		Held_Butt = HELD_DIR_BUTT.NONE;
	}

	// Update is called once per frame
	void Update () {
		// player rotation
		PrevHoldRot ();
		// player movement
		if (!Moving) {
			OrientAxes (-Input.GetAxisRaw ("Horizontal"), Input.GetAxisRaw ("Vertical"));
		}
		DisplayPos ();
	}

	// Updates the display that shows player coordinates
	void DisplayPos () {
		Coordinates_Text.text = "WORLD: " + transform.position.x + ", " + transform.position.y + ", " + transform.position.z
			+ "\nsection: " + Cur_Sec.ToString ()
			+ "\nrow: " + Cur_Row.ToString ()
			+ "\ncol: " + Cur_Col.ToString ();
	}

	// Determines if a directional input is held down
	void HeldDirButton (float hoz_inc, float vert_inc) {
		if (Held_Butt == HELD_DIR_BUTT.HOZ) {
			if (hoz_inc != 0 && vert_inc == 0) {
				Held_Butt = HELD_DIR_BUTT.HOZ;
			} else if (hoz_inc == 0 && vert_inc != 0) {
				Held_Butt = HELD_DIR_BUTT.VERT;
			} else if (hoz_inc != 0 && vert_inc != 0) {
				Held_Butt = HELD_DIR_BUTT.HOZ;
			} else {
				Held_Butt = HELD_DIR_BUTT.NONE;
			}
		} else if (Held_Butt == HELD_DIR_BUTT.VERT) {
			if (hoz_inc != 0 && vert_inc == 0) {
				Held_Butt = HELD_DIR_BUTT.HOZ;
			} else if (hoz_inc == 0 && vert_inc != 0) {
				Held_Butt = HELD_DIR_BUTT.VERT;
			} else if (hoz_inc != 0 && vert_inc != 0) {
				Held_Butt = HELD_DIR_BUTT.VERT;
			} else {
				Held_Butt = HELD_DIR_BUTT.NONE;
			}
		} else {
			if (hoz_inc != 0 && vert_inc == 0) {
				Held_Butt = HELD_DIR_BUTT.HOZ;
			} else if (hoz_inc == 0 && vert_inc != 0) {
				Held_Butt = HELD_DIR_BUTT.VERT;
			} else if (hoz_inc != 0 && vert_inc != 0) {
				Held_Butt = HELD_DIR_BUTT.HOZ;
			}
		}
	}

	// Keeps player from rotating when rotate button is held down
	void PrevHoldRot () {
		if (!Rotating) {
			float rot_val = Input.GetAxisRaw ("Rotate");
			if (rot_val != 0) {
				if (!RotButtDown) {
					StartCoroutine (RotatePlayer (rot_val));
					RotButtDown = true;
				}
			} else {
				RotButtDown = false;
			}
		}
	}
		
	// rotate player based on input
	IEnumerator RotatePlayer(float rot) {
		if (rot < 0f) {
			Rotation += 90f;
		} else if (rot > 0f) {
			Rotation -= 90f;
		}
		Rotation = (Rotation == 360 || Rotation == -360) ? 0 : Rotation;
		Rotate_To = Quaternion.Euler (30f, Rotation, 0f);
		Rotating = true;
		while (Quaternion.Angle (transform.rotation, Rotate_To) > 1.0f) {
			transform.rotation = Quaternion.RotateTowards (transform.rotation, Rotate_To, Rot_Speed * Time.deltaTime);
			yield return null;
		}
		Rotating = false;
		yield return null;
	}

	// Adjust input based on player rotation
	void OrientAxes (float hoz_inc, float vert_inc) {
		if (Rotation == 0) {
			StartCoroutine (PrevDiagMove (hoz_inc, vert_inc));
		} else if (Rotation == 90 || Rotation == -270) {
			StartCoroutine (PrevDiagMove (vert_inc, -hoz_inc));
		} else if (Rotation == 180 || Rotation == -180) {
			StartCoroutine (PrevDiagMove (-hoz_inc, -vert_inc));
		} else {
			StartCoroutine (PrevDiagMove (-vert_inc, hoz_inc));
		}
	}

	// Prevent the player from moving diagonally
	IEnumerator PrevDiagMove (float hoz_inc, float vert_inc) {
		Debug_Text.text = "Horizontal Input: " + hoz_inc.ToString () + "\nVertical Input: " + vert_inc.ToString ()
		+ "\nCoordinates: " + transform.position.x + ", " + transform.position.y + ", " + transform.position.z;
		// Determine if an input button is being held down
		HeldDirButton (hoz_inc, vert_inc);
		// Split horizontal and vertical movement
		Moving = true;
		if (hoz_inc != 0 && vert_inc != 0) {
			if (Held_Butt == HELD_DIR_BUTT.HOZ) {
				yield return StartCoroutine (Move (hoz_inc, 0.0f));
				yield return StartCoroutine (Move (0.0f, vert_inc));
			} else if (Held_Butt == HELD_DIR_BUTT.VERT) {
				yield return StartCoroutine (Move (0.0f, vert_inc));
				yield return StartCoroutine (Move (hoz_inc, 0.0f));
			} else {
				yield return StartCoroutine (Move (hoz_inc, 0.0f));
				yield return StartCoroutine (Move (0.0f, vert_inc));
			}
		} else {
			yield return StartCoroutine (Move (hoz_inc, vert_inc));
		}
		Moving = false;
		yield return 0;
	}

	// Gets the cell type of the destination and updates Cur_Row, Cur_Col, and Cur_Sec values
	string GetEndCell (ref SECTION_LOC dest_sec, int hoz_inc, int vert_inc) {
		int dest_row = Cur_Row - vert_inc;
		int dest_col = Cur_Col + hoz_inc;
		string destCellInfo = "";
		// vertical movement
		if (vert_inc != 0) {
			if (dest_row < 0) {
				// Move to top section
				destCellInfo = SectionData.Cur_Sec [0] [Cur_Col];
				dest_sec = SECTION_LOC.T;
			} else if (dest_row >= (int)Sec_Width) {
				// Move to bottom section
				destCellInfo = SectionData.Cur_Sec [(int)Sec_Width-1] [Cur_Col];
				dest_sec = SECTION_LOC.B;
			} else {
				// Stay in current section
				destCellInfo = SectionData.Cur_Sec [dest_row] [Cur_Col];
				Cur_Row -= vert_inc;
			}
		}
		// horzontal movement
		else if (hoz_inc != 0) {
			if (dest_col < 0) {
				// Move to left section
				destCellInfo = SectionData.Cur_Sec [Cur_Row] [0];
				dest_sec = SECTION_LOC.L;
			} else if (dest_col >= (int)Sec_Width) {
				// Move to right section
				destCellInfo = SectionData.Cur_Sec [Cur_Row] [(int)Sec_Width-1];
				dest_sec = SECTION_LOC.R;
			} else {
				// Stay in current section
				destCellInfo = SectionData.Cur_Sec [Cur_Row] [dest_col];
				Cur_Col += hoz_inc;
			}
		} else {
			destCellInfo = SectionData.Cur_Sec [dest_row] [dest_col];
			Cur_Col += hoz_inc;
			Cur_Row -= vert_inc;
		}
		return destCellInfo;
	}

	// Alters the destTransform when the start cell is a floor
	void FloorStart (ref Vector3 destTransform, string startCellInfo, int hoz_inc, int vert_inc) {
		SECTION_LOC dest_sec = SECTION_LOC.CUR;
		string destCellInfo = GetEndCell (ref dest_sec, hoz_inc, vert_inc);
		if (dest_sec != SECTION_LOC.CUR) {
			destTransform = transform.position;
			return;
		}
		if (destCellInfo [0] == 'w') {
			destTransform = transform.position;
		} else if (destCellInfo [0] == 'p') {
		} else if (destCellInfo [0] == 'r') {
		} else {
		}
	}

	// Alters the destTransform when the start cell is a platform
	void PlatformStart (ref Vector3 destTransform, string startCellInfo, int hoz_inc, int vert_inc) {
	}

	// Alters the destTransform when the start cell is a ramp
	void RampStart (ref Vector3 destTransform, string startCellInfo, int hoz_inc, int vert_inc) {
	}

	// Move the player to destTransform
	IEnumerator Move (float hoz_inc, float vert_inc) {
		string startCellInfo = SectionData.Cur_Sec [Cur_Row] [Cur_Col];
		Vector3 destTransform = new Vector3 (transform.position.x + hoz_inc, transform.position.y, transform.position.z + vert_inc);
		Debug_Text.text = "Horizontal Input: " + hoz_inc.ToString () + "\nVertical Input: " + vert_inc.ToString ()
			+ "\nCoordinates: " + destTransform.x + ", " + destTransform.y + ", " + destTransform.z;
		if (startCellInfo [0] == 'p') {
			PlatformStart (ref destTransform, startCellInfo, (int) hoz_inc, (int) vert_inc);
		} else if (startCellInfo [0] == 'r') {
			RampStart (ref destTransform, startCellInfo, (int) hoz_inc, (int) vert_inc);
		} else {
			FloorStart (ref destTransform, startCellInfo, (int) hoz_inc, (int) vert_inc);
		}
		while (Vector3.Distance (transform.position, destTransform) != 0.0f) {
			transform.position = Vector3.MoveTowards (transform.position, destTransform, Mov_Speed * Time.deltaTime);
			yield return null;
		}
	}

	// update section and coordinates
}
