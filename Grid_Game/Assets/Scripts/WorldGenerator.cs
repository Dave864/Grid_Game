using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.IO;

// Manages the creation and destruction of world elements
public class WorldGenerator : MonoBehaviour {

	// Used for grouping together map section cells
	public Transform cur;
	public Transform left;
	public Transform right;
	public Transform top;
	public Transform bot;
	public Transform top_L;
	public Transform top_R;
	public Transform bot_L;
	public Transform bot_R;

	private float curSecOrigin_x = 0.0f;
	private float curSecOrigin_z = 0.0f;
	private int curSec;
	enum SECTION_LOC {CUR, L, R, T, B, TL, TR, BL, BR};

	TextAsset secData;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	}
		
	// create sections
	// destroy sections
}
