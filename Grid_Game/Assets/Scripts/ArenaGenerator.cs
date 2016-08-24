using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class ArenaGenerator : MonoBehaviour {

	// Use this for initialization
	void Start () {
		StartCoroutine (Wait ());
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	// Wait for x seconds
	IEnumerator Wait(){
		yield return new WaitForSeconds (1);
		SceneManager.LoadScene ("OverWorld");
	}
}
