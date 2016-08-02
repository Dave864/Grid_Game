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
	// private float Mov_Speed = 2f;
	// private bool Moving = false;
	private bool Rotating = false;
	private bool RotButtDown = false;
	private Quaternion Rotate_To = Quaternion.Euler(30f, 0f, 0f);

	// text showing player coordinates
	public Text Coordinates_Text;

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
		DisplayPos ();
	}

	// Update is called once per frame
	void Update () {
		// player rotation
		PrevHoldRot ();
		// player movement
		OrientAxes(Input.GetAxisRaw("Vertical"), Input.GetAxisRaw("Horizontal"));
	}

	// Updates the display that shows player coordinates
	void DisplayPos () {
		Coordinates_Text.text = "WORLD: " + transform.position.x + ", " + transform.position.y + ", " + transform.position.z
			+ "\nsection: " + Cur_Sec.ToString ()
			+ "\nrow: " + Cur_Row.ToString ()
			+ "\ncol: " + Cur_Col.ToString ();
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

	// Matches up input to the player's orientation
	void OrientAxes (float vert_step, float hoz_step) {
		if (Rotation == 0f) {
		} else if (Rotation == 90f || Rotation == -270f) {
		} else if (Rotation == 180f || Rotation == -180f) {
		} else {
		}
		SplitMovement ((int) vert_step, (int) hoz_step);
	}

	// Prevents diagonal movement
	void SplitMovement (int vert_step, int hoz_step) {
		if (vert_step != 0 && hoz_step != 0) {
			// move horizontally
			GetDestCoordinates(0, hoz_step);
			// move vertically
			GetDestCoordinates(vert_step, 0);
		} else {
			GetDestCoordinates (vert_step, hoz_step);
		}
	}

	// Gets the destiniation x,z coordinates for the current movement step
	void GetDestCoordinates (int vert_step, int hoz_step) {
		// vertical movement
		if (vert_step != 0) {
			int dest_row = Cur_Row + vert_step;
			if (dest_row < 0) {
				// move to top section
			} else if (dest_row >= (int)Sec_Width) {
				// move to bottom section
			} else {
				// stay in current section
				GetStartCellType(SECTION_LOC.CUR, Cur_Col, dest_row);
			}
		}
		// horizontal movement
		else {
			int dest_col = Cur_Col + hoz_step;
			if (dest_col < 0) {
				// move to left section
			} else if (dest_col >= (int)Sec_Width) {
				// move to right section
			} else {
				// stay in current section
				GetStartCellType(SECTION_LOC.CUR, dest_col, Cur_Row);
			}
		}
	}

	// Determines which type of cell the player is on
	void GetStartCellType (SECTION_LOC dest_sec, int x_dest, int z_dest) {
		if (SectionData.Cur_Sec [Cur_Row] [Cur_Col] [0] == 'p') {
			PlatformStart (dest_sec, x_dest, z_dest);
		} else if (SectionData.Cur_Sec [Cur_Row] [Cur_Col] [0] == 'r') {
			RampStart (dest_sec, x_dest, z_dest);
		} else {
			FloorStart (dest_sec, x_dest, z_dest);
		}
	}

	// Determines how the player moves when starting on a floor cell
	void FloorStart (SECTION_LOC dest_sec, int x_dest, int z_dest) {
		if (dest_sec == SECTION_LOC.T) {
		} else if (dest_sec == SECTION_LOC.B) {
		} else if (dest_sec == SECTION_LOC.L) {
		} else if (dest_sec == SECTION_LOC.R) {
		} else if (dest_sec == SECTION_LOC.TL) {
		} else if (dest_sec == SECTION_LOC.TR) {
		} else if (dest_sec == SECTION_LOC.BL) {
		} else if (dest_sec == SECTION_LOC.BR) {
		} else {
		}
	}

	// Determines how the player moves when starting on a platform
	void PlatformStart (SECTION_LOC dest_sec, int x_dest, int z_dest) {
		if (dest_sec == SECTION_LOC.T) {
		} else if (dest_sec == SECTION_LOC.B) {
		} else if (dest_sec == SECTION_LOC.L) {
		} else if (dest_sec == SECTION_LOC.R) {
		} else if (dest_sec == SECTION_LOC.TL) {
		} else if (dest_sec == SECTION_LOC.TR) {
		} else if (dest_sec == SECTION_LOC.BL) {
		} else if (dest_sec == SECTION_LOC.BR) {
		} else {
		}
	}

	// Determines how the player moves when starting on a ramp
	void RampStart (SECTION_LOC dest_sec, int x_dest, int z_dest) {
		if (dest_sec == SECTION_LOC.T) {
		} else if (dest_sec == SECTION_LOC.B) {
		} else if (dest_sec == SECTION_LOC.L) {
		} else if (dest_sec == SECTION_LOC.R) {
		} else if (dest_sec == SECTION_LOC.TL) {
		} else if (dest_sec == SECTION_LOC.TR) {
		} else if (dest_sec == SECTION_LOC.BL) {
		} else if (dest_sec == SECTION_LOC.BR) {
		} else {
		}
	}

	// Moves the player to the destination
	void Move () {
	}

	// rotate player based on input
	IEnumerator RotatePlayer(float rot) {
		if (rot < 0f) {
			Rotation += 90f;
		} else if (rot > 0f) {
			Rotation -= 90f;
		}
		Rotate_To = Quaternion.Euler (30f, Rotation, 0f);
		Rotating = true;
		while (Quaternion.Angle (transform.rotation, Rotate_To) > 1.0f) {
			transform.rotation = Quaternion.RotateTowards (transform.rotation, Rotate_To, Rot_Speed * Time.deltaTime);
			yield return null;
		}
		Rotating = false;
		yield return null;
	}

	// move player based on input and player rotation
	// update section and coordinates
}
