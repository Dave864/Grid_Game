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

	TextAsset secData;

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
		LoadSection (Cur_Sec, Cur_Sec_Origin_x, Cur_Sec_Origin_z, SECTION_LOC.CUR);
	}

	// Loads section sec_num of the world at location at coordinates: sec_origin_x, some height, sec_origin_z
	void LoadSection (int sec_num, float sec_origin_x, float sec_origin_z, SECTION_LOC location) {
	}
	// destroy sections
}
