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

	// rotation and movement information
	/*private float rotation = 0f;
	private float rotSpeed = 200f;
	private float movSpeed = 2f;
	private bool moving = false;
	private bool rotating = false;
	private Quaternion rotateTo = Quaternion.Euler(45f, 0f, 0f);*/

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
		/*string cellData = SectionData.Cur_Sec [cur_row][cur_col];
		float height = (float)char.GetNumericValue (cellData [3])/2;
		if (cellData [0] == 'r') {
			height += 0.25;
		} else if (cellData [0] == 'p') {
			height += 0.5;
		}*/
		// place player at coordinates
		transform.position = new Vector3 (transform.position.x + Cur_Col, transform.position.y, transform.position.z + Cur_Row);
		DisplayPos ();
	}

	// Update is called once per frame
	void Update () {
		// receive player input
	}

	// Updates the display that shows player coordinates
	void DisplayPos () {
		Coordinates_Text.text = "WORLD: " + transform.position.x + ", " + transform.position.y + ", " + transform.position.z
			+ "\nsection: " + Cur_Sec.ToString ()
			+ "\nrow: " + Cur_Row.ToString ()
			+ "\ncol: " + Cur_Col.ToString ();
	}

	// rotate player based on input
	// move player based on input and player rotation
	// update section and coordinates
}
