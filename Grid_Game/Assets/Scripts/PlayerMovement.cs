using UnityEngine;
using UnityEngine.UI;
using System.Collections;

// Handles player movement in the overworld
public class PlayerMovement : MonoBehaviour {

	// player coordinates
	public int test_sec;
	public int test_row;
	public int test_col;
	[HideInInspector] static public int cur_sec;
	[HideInInspector] static public int cur_row;
	[HideInInspector] static public int cur_col;

	// section dimensions
	[HideInInspector] static public float sec_width = 10.0f;

	// rotation and movement information
	/*private float rotation = 0f;
	private float rotSpeed = 200f;
	private float movSpeed = 2f;
	private bool moving = false;
	private bool rotating = false;
	private Quaternion rotateTo = Quaternion.Euler(45f, 0f, 0f);*/

	// text showing player coordinates
	public Text CoordinatesText;

	// Initialize initial section and intial row and column
	void Awake () {
		cur_sec = test_sec;
		if (test_row < 0) {
			cur_row = 0;
		} else if (test_row > (int)sec_width - 1) {
			cur_row = (int)sec_width - 1;
		} else {
			cur_row = test_row;
		}
		if (test_col < 0) {
			cur_col = 0;
		} else if (test_col > (int)sec_width - 1) {
			cur_col = (int)sec_width - 1;
		} else {
			cur_col = test_col;
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
		transform.position = new Vector3 (transform.position.x + cur_col, transform.position.y, transform.position.z + cur_row);
		DisplayPos ();
	}

	// Update is called once per frame
	void Update () {
		// receive player input
	}

	// Updates the display that shows player coordinates
	void DisplayPos () {
		CoordinatesText.text = "Section: " + cur_sec.ToString ()
		+ " row: " + cur_row.ToString ()
		+ " col: " + cur_col.ToString ()
		+ "\nWorld Coordinates:\n\t" + transform.position.x + ", " + transform.position.y + ", " + transform.position.z;
	}

	// rotate player based on input
	// move player based on input and player rotation
	// update section and coordinates
}
