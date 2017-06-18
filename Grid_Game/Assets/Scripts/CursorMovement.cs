using UnityEngine;
using System.Collections;

public class CursorMovement : MonoBehaviour {

	// info for how hex cells are placed
	/*private float Hoz_Offset = 1.0f;
	private float Vert_Offset = -0.866f;
	private int Cell_Row_Cnt = 4;*/

	// Use this for initialization
	void Start () {
		if (PlayerPrefs.GetString ("ArenaType") == "double") {
			if (PlayerPrefs.GetInt ("TL") < -1) {
				// left-right arena with player at left
				if (PlayerPrefs.GetInt ("TR") > -1) {
				}
				// up-down arena with player at top
				else {
				}
			} else if (PlayerPrefs.GetInt ("TR") < -1) {
				// left-right arena with player at right
			} else {
				// up-down arena with player at bottom
			}
		} else if (PlayerPrefs.GetString ("ArenaType") == "quad") {
			if (PlayerPrefs.GetInt ("TL") < 0) {
				// player is in the top-left
			} else if (PlayerPrefs.GetInt ("TR") < 0) {
				// player is in the top-right
			} else if (PlayerPrefs.GetInt ("BL") < 0) {
				// player is in the bottom-left
			} else {
				// player is in the bottom-right
			}
		} else {
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
