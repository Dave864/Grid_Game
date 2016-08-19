using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameInfo : MonoBehaviour {

	// World location data
	public int Cur_Sec;
	public int Cur_Row;
	public int Cur_Col;

	// Initialize initial section and intial row and column
	void Awake() {
		DontDestroyOnLoad (this);
		if (Scene.Equals (SceneManager.GetActiveScene (), SceneManager.GetSceneByName ("OverWorld"))) {
			PlayerMovement.Cur_Sec = Cur_Sec;
			if (Cur_Row < 0) {
				PlayerMovement.Cur_Row = 0;
			} else if (Cur_Row > (int)PlayerMovement.Sec_Width - 1) {
				PlayerMovement.Cur_Row = (int)PlayerMovement.Sec_Width - 1;
			} else {
				PlayerMovement.Cur_Row = Cur_Row;
			}
			if (Cur_Col < 0) {
				PlayerMovement.Cur_Col = 0;
			} else if (Cur_Col > (int)PlayerMovement.Sec_Width - 1) {
				PlayerMovement.Cur_Col = (int)PlayerMovement.Sec_Width - 1;
			} else {
				PlayerMovement.Cur_Col = Cur_Col;
			}
		}
	}

	// Use this for initialization
	void Start () {
		
	}

	// Update is called once per frame
	void Update () {
		if (Scene.Equals(SceneManager.GetActiveScene(), SceneManager.GetSceneByName("OverWorld"))) {
			Cur_Sec = PlayerMovement.Cur_Sec;
			Cur_Row = PlayerMovement.Cur_Row;
			Cur_Col = PlayerMovement.Cur_Col;
		}
	}
}
