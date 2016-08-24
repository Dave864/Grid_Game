using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class RandomEncounter : MonoBehaviour {

	private float Encounter_Rate = 85.0f;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	}

	public IEnumerator Randomizer(){
		// encounter
		if (Random.Range (0.0f, 100.0f) > Encounter_Rate) {
			SceneManager.LoadScene ("Encounter");
			yield return null;
		}
	}
}
