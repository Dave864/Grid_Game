using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class RandomEncounter : MonoBehaviour {

	private float Encounter_Rate = 85.0f;

	// Use this for initialization
	void Start () {
		PlayerPrefs.SetString ("ArenaType", "none");
		PlayerPrefs.SetInt ("Cur", -1);
		PlayerPrefs.SetInt ("TL", -1);
		PlayerPrefs.SetInt ("TR", -1);
		PlayerPrefs.SetInt ("BL", -1);
		PlayerPrefs.SetInt ("BR", -1);
	}
	
	// Update is called once per frame
	void Update () {
	}

	// Determine the sections that will make up the arena
	void ArenaSections () {
		// Player is at the top row
		if (PlayerMovement.Cur_Row == 0) {
			// Player is at top-left cell
			if (PlayerMovement.Cur_Col == 0) {
				PlayerPrefs.SetInt ("Cur", (int)char.GetNumericValue (SectionData.Cur_Sec [0] [0] [3]));
				// cell to the right is a wall
				if (SectionData.Cur_Sec [0] [1] [0] == 'w') {
					PlayerPrefs.SetString ("ArenaType", "double");
					PlayerPrefs.SetInt ("BL", (int)char.GetNumericValue (SectionData.Cur_Sec [1] [0] [3]));
					PlayerPrefs.SetInt ("TL", -2);
					PlayerPrefs.SetInt ("TR", -1);
					PlayerPrefs.SetInt ("BL", -1);
				}
				// cell to the bottom is a wall
				else if (SectionData.Cur_Sec [1] [0] [0] == 'w') {
					PlayerPrefs.SetString ("ArenaType", "double");
					PlayerPrefs.SetInt ("TR", (int)char.GetNumericValue (SectionData.Cur_Sec [0] [1] [3]));
					PlayerPrefs.SetInt ("TL", -2);
					PlayerPrefs.SetInt ("BL", -1);
					PlayerPrefs.SetInt ("BR", -1);
				}
				// cell to the bottom-right is a wall
				else if (SectionData.Cur_Sec [1] [1] [0] == 'w') {
					PlayerPrefs.SetString ("ArenaType", "double");
					switch (Random.Range (0, 1)) {
					case 1:
						PlayerPrefs.SetInt ("BL", (int)char.GetNumericValue (SectionData.Cur_Sec [1] [0] [3]));
						PlayerPrefs.SetInt ("TL", -2);
						PlayerPrefs.SetInt ("TR", -1);
						PlayerPrefs.SetInt ("BL", -1);
						break;
					default:
						PlayerPrefs.SetInt ("TR", (int)char.GetNumericValue (SectionData.Cur_Sec [0] [1] [3]));
						PlayerPrefs.SetInt ("TL", -2);
						PlayerPrefs.SetInt ("BL", -1);
						PlayerPrefs.SetInt ("BR", -1);
						break;
					}
				}
				// no adjacent cells are walls
				else {
					PlayerPrefs.SetString ("ArenaType", "quad");
					PlayerPrefs.SetInt ("TL", -1);
					PlayerPrefs.SetInt ("TR", (int)char.GetNumericValue (SectionData.Cur_Sec [0] [1] [3]));
					PlayerPrefs.SetInt ("BL", (int)char.GetNumericValue (SectionData.Cur_Sec [1] [0] [3]));
					PlayerPrefs.SetInt ("BR", (int)char.GetNumericValue (SectionData.Cur_Sec [1] [1] [3]));
				}
			}
			// Player is at top-right cell
			else if (PlayerMovement.Cur_Col == PlayerMovement.Sec_Width - 1) {
				PlayerPrefs.SetInt ("Cur", (int)char.GetNumericValue (SectionData.Cur_Sec [0] [(int)PlayerMovement.Sec_Width - 1] [3]));
				// cell to the left is a wall
				if (SectionData.Cur_Sec [0] [(int)PlayerMovement.Sec_Width - 2] [0] == 'w') {
					PlayerPrefs.SetString ("ArenaType", "double");
					PlayerPrefs.SetInt ("BR", (int)char.GetNumericValue (SectionData.Cur_Sec [1] [(int)PlayerMovement.Sec_Width - 1] [3]));
					PlayerPrefs.SetInt ("TL", -1);
					PlayerPrefs.SetInt ("TR", -2);
					PlayerPrefs.SetInt ("BL", -1);
				}
				// cell to the bottom is a wall
				else if (SectionData.Cur_Sec [1] [(int)PlayerMovement.Sec_Width - 1] [0] == 'w') {
					PlayerPrefs.SetString ("ArenaType", "double");
					PlayerPrefs.SetInt ("TL", (int)char.GetNumericValue (SectionData.Cur_Sec [0] [(int)PlayerMovement.Sec_Width - 2] [3]));
					PlayerPrefs.SetInt ("TR", -2);
					PlayerPrefs.SetInt ("BR", -1);
					PlayerPrefs.SetInt ("BL", -1);
				}
				// cell to the bottom-left is a wall
				else if (SectionData.Cur_Sec [1] [(int)PlayerMovement.Sec_Width - 2] [0] == 'w') {
					PlayerPrefs.SetString ("ArenaType", "double");
					switch (Random.Range (0, 1)) {
					case 1:
						PlayerPrefs.SetInt ("BR", (int)char.GetNumericValue (SectionData.Cur_Sec [1] [(int)PlayerMovement.Sec_Width - 1] [3]));
						PlayerPrefs.SetInt ("TL", -1);
						PlayerPrefs.SetInt ("TR", -2);
						PlayerPrefs.SetInt ("BL", -1);
						break;
					default:
						PlayerPrefs.SetInt ("TL", (int)char.GetNumericValue (SectionData.Cur_Sec [0] [(int)PlayerMovement.Sec_Width - 2] [3]));
						PlayerPrefs.SetInt ("TR", -2);
						PlayerPrefs.SetInt ("BR", -1);
						PlayerPrefs.SetInt ("BL", -1);
						break;
					}
				}
				// no adjacent cells are walls
				else {
					PlayerPrefs.SetString ("ArenaType", "quad");
					PlayerPrefs.SetInt ("TR", -1);
					PlayerPrefs.SetInt ("TL", (int)char.GetNumericValue (SectionData.Cur_Sec [0] [(int)PlayerMovement.Sec_Width - 2] [3]));
					PlayerPrefs.SetInt ("BL", (int)char.GetNumericValue (SectionData.Cur_Sec [1] [(int)PlayerMovement.Sec_Width - 2] [3]));
					PlayerPrefs.SetInt ("BR", (int)char.GetNumericValue (SectionData.Cur_Sec [1] [(int)PlayerMovement.Sec_Width - 1] [3]));
				}
			}
			// Player is in top row
			else {
				PlayerPrefs.SetInt ("Cur", (int)char.GetNumericValue (SectionData.Cur_Sec [0] [PlayerMovement.Cur_Col] [3]));
				// cell to the right is a wall
				if (SectionData.Cur_Sec [0] [PlayerMovement.Cur_Col + 1] [0] == 'w') {
					// cell to the left is a wall
					if (SectionData.Cur_Sec [0] [PlayerMovement.Cur_Col - 1] [0] == 'w') {
						PlayerPrefs.SetString ("ArenaType", "double");
						PlayerPrefs.SetInt ("BR", (int)char.GetNumericValue (SectionData.Cur_Sec [1] [PlayerMovement.Cur_Col] [3]));
						PlayerPrefs.SetInt ("BL", -1);
						PlayerPrefs.SetInt ("TL", -1);
						PlayerPrefs.SetInt ("TR", -2);
					}
					// cell to the bottom is a wall
					else if (SectionData.Cur_Sec [1] [PlayerMovement.Cur_Col] [0] == 'w') {
						PlayerPrefs.SetString ("ArenaType", "double");
						PlayerPrefs.SetInt ("TL", (int)char.GetNumericValue (SectionData.Cur_Sec [0] [PlayerMovement.Cur_Col - 1] [3]));
						PlayerPrefs.SetInt ("TR", -2);
						PlayerPrefs.SetInt ("BL", -1);
						PlayerPrefs.SetInt ("BR", -1);
					}
					// cell to the bottom-left is a wall
					else if (SectionData.Cur_Sec [1] [PlayerMovement.Cur_Col - 1] [0] == 'w') {
						PlayerPrefs.SetString ("ArenaType", "double");
						switch (Random.Range (0, 1)) {
						case 1:
							PlayerPrefs.SetInt ("BR", (int)char.GetNumericValue (SectionData.Cur_Sec [1] [PlayerMovement.Cur_Col] [3]));
							PlayerPrefs.SetInt ("BL", -1);
							PlayerPrefs.SetInt ("TL", -1);
							PlayerPrefs.SetInt ("TR", -2);
							break;
						default:
							PlayerPrefs.SetInt ("TL", (int)char.GetNumericValue (SectionData.Cur_Sec [0] [PlayerMovement.Cur_Col - 1] [3]));
							PlayerPrefs.SetInt ("TR", -2);
							PlayerPrefs.SetInt ("BL", -1);
							PlayerPrefs.SetInt ("BR", -1);
							break;
						}
					}
					// no other cells are walls
					else {
						PlayerPrefs.SetString ("ArenaType", "quad");
						PlayerPrefs.SetInt ("TR", -1);
						PlayerPrefs.SetInt ("TL", (int)char.GetNumericValue (SectionData.Cur_Sec [0] [PlayerMovement.Cur_Col - 1] [3]));
						PlayerPrefs.SetInt ("BL", (int)char.GetNumericValue (SectionData.Cur_Sec [1] [PlayerMovement.Cur_Col - 1] [3]));
						PlayerPrefs.SetInt ("BR", (int)char.GetNumericValue (SectionData.Cur_Sec [1] [PlayerMovement.Cur_Col] [3]));
					}
				}
				// cell to the left is a wall
				else if (SectionData.Cur_Sec [0] [PlayerMovement.Cur_Col - 1] [0] == 'w') {
					// cell to the right is a wall
					if (SectionData.Cur_Sec [0] [PlayerMovement.Cur_Col + 1] [0] == 'w') {
						PlayerPrefs.SetString ("ArenaType", "double");
						PlayerPrefs.SetInt ("BL", (int)char.GetNumericValue (SectionData.Cur_Sec [1] [PlayerMovement.Cur_Col] [3]));
						PlayerPrefs.SetInt ("BR", -1);
						PlayerPrefs.SetInt ("TL", -2);
						PlayerPrefs.SetInt ("TR", -1);
					}
					// cell to the bottom is a wall
					else if (SectionData.Cur_Sec [1] [PlayerMovement.Cur_Col] [0] == 'w') {
						PlayerPrefs.SetString ("ArenaType", "double");
						PlayerPrefs.SetInt ("TR", (int)char.GetNumericValue (SectionData.Cur_Sec [0] [PlayerMovement.Cur_Col + 1] [3]));
						PlayerPrefs.SetInt ("BL", -1);
						PlayerPrefs.SetInt ("BR", -1);
						PlayerPrefs.SetInt ("TL", -2);
					}
					// cell to the bottom-right is a wall
					else if (SectionData.Cur_Sec [1] [PlayerMovement.Cur_Col + 1] [0] == 'w') {
						PlayerPrefs.SetString ("ArenaType", "double");
						switch (Random.Range (0, 1)) {
						case 1:
							PlayerPrefs.SetInt ("BL", (int)char.GetNumericValue (SectionData.Cur_Sec [1] [PlayerMovement.Cur_Col] [3]));
							PlayerPrefs.SetInt ("BR", -1);
							PlayerPrefs.SetInt ("TL", -2);
							PlayerPrefs.SetInt ("TR", -1);
							break;
						default:
							PlayerPrefs.SetInt ("TR", (int)char.GetNumericValue (SectionData.Cur_Sec [0] [PlayerMovement.Cur_Col + 1] [3]));
							PlayerPrefs.SetInt ("BL", -1);
							PlayerPrefs.SetInt ("BR", -1);
							PlayerPrefs.SetInt ("TL", -2);
							break;
						}
					}
					// no other cells are walls
					else {
						PlayerPrefs.SetString ("ArenaType", "quad");
						PlayerPrefs.SetInt ("TL", -1);
						PlayerPrefs.SetInt ("TR", (int)char.GetNumericValue (SectionData.Cur_Sec [0] [PlayerMovement.Cur_Col + 1] [3]));
						PlayerPrefs.SetInt ("BL", (int)char.GetNumericValue (SectionData.Cur_Sec [1] [PlayerMovement.Cur_Col] [3]));
						PlayerPrefs.SetInt ("BR", (int)char.GetNumericValue (SectionData.Cur_Sec [1] [PlayerMovement.Cur_Col + 1] [3]));
					}
				}
				// cell to the bottom is a wall
				else if (SectionData.Cur_Sec [1] [PlayerMovement.Cur_Col] [0] == 'w') {
					switch (Random.Range (0, 1)) {
					case 1:
						PlayerPrefs.SetString ("ArenaType", "double");
						PlayerPrefs.SetInt ("TL", (int)char.GetNumericValue (SectionData.Cur_Sec [0] [PlayerMovement.Cur_Col - 1] [3]));
						PlayerPrefs.SetInt ("TR", -2);
						PlayerPrefs.SetInt ("BR", -1);
						PlayerPrefs.SetInt ("BL", -1);
						break;
					default:
						PlayerPrefs.SetString ("ArenaType", "double");
						PlayerPrefs.SetInt ("TR", (int)char.GetNumericValue (SectionData.Cur_Sec [0] [PlayerMovement.Cur_Col + 1] [3]));
						PlayerPrefs.SetInt ("TL", -2);
						PlayerPrefs.SetInt ("BL", -1);
						PlayerPrefs.SetInt ("BR", -1);
						break;
					}
				}
				// cell to the bottom-right is a wall
				else if (SectionData.Cur_Sec [1] [(int)PlayerMovement.Cur_Col + 1] [0] == 'w') {
					PlayerPrefs.SetString ("ArenaType", "quad");
					PlayerPrefs.SetInt ("TR", -1);
					PlayerPrefs.SetInt ("TL", (int)char.GetNumericValue (SectionData.Cur_Sec [0] [PlayerMovement.Cur_Col - 1] [3]));
					PlayerPrefs.SetInt ("BR", (int)char.GetNumericValue (SectionData.Cur_Sec [1] [PlayerMovement.Cur_Col] [3]));
					PlayerPrefs.SetInt ("BL", (int)char.GetNumericValue (SectionData.Cur_Sec [1] [PlayerMovement.Cur_Col - 1] [3]));
				}
				// cell to the bottom-left is a wall
				else if (SectionData.Cur_Sec [1] [(int)PlayerMovement.Cur_Col - 1] [0] == 'w') {
					PlayerPrefs.SetString ("ArenaType", "quad");
					PlayerPrefs.SetInt ("TL", -1);
					PlayerPrefs.SetInt ("TR", (int)char.GetNumericValue (SectionData.Cur_Sec [0] [PlayerMovement.Cur_Col + 1] [3]));
					PlayerPrefs.SetInt ("BL", (int)char.GetNumericValue (SectionData.Cur_Sec [1] [PlayerMovement.Cur_Col] [3]));
					PlayerPrefs.SetInt ("BR", (int)char.GetNumericValue (SectionData.Cur_Sec [1] [PlayerMovement.Cur_Col + 1] [3]));
				}
				// no adjacent cells are walls
				else {
					switch (Random.Range (0, 1)) {
					case 1:
						PlayerPrefs.SetString ("ArenaType", "quad");
						PlayerPrefs.SetInt ("TR", -1);
						PlayerPrefs.SetInt ("TL", (int)char.GetNumericValue (SectionData.Cur_Sec [0] [PlayerMovement.Cur_Col - 1] [3]));
						PlayerPrefs.SetInt ("BR", (int)char.GetNumericValue (SectionData.Cur_Sec [1] [PlayerMovement.Cur_Col] [3]));
						PlayerPrefs.SetInt ("BL", (int)char.GetNumericValue (SectionData.Cur_Sec [1] [PlayerMovement.Cur_Col - 1] [3]));
						break;
					default:
						PlayerPrefs.SetString ("ArenaType", "quad");
						PlayerPrefs.SetInt ("TL", -1);
						PlayerPrefs.SetInt ("TR", (int)char.GetNumericValue (SectionData.Cur_Sec [0] [PlayerMovement.Cur_Col + 1] [3]));
						PlayerPrefs.SetInt ("BL", (int)char.GetNumericValue (SectionData.Cur_Sec [1] [PlayerMovement.Cur_Col] [3]));
						PlayerPrefs.SetInt ("BR", (int)char.GetNumericValue (SectionData.Cur_Sec [1] [PlayerMovement.Cur_Col + 1] [3]));
						break;
					}
				}
			}
		}
		// Player is at the bottom row
		else if (PlayerMovement.Cur_Row == PlayerMovement.Sec_Width - 1) {
			// Player is at bottom-left cell
			if (PlayerMovement.Cur_Col == 0) {
				PlayerPrefs.SetInt ("Cur", (int)char.GetNumericValue (SectionData.Cur_Sec [(int)PlayerMovement.Sec_Width - 1] [0] [3]));
				// cell to the top is a wall
				if (SectionData.Cur_Sec [(int)PlayerMovement.Sec_Width - 2] [0] [0] == 'w') {
				}
				// cell to the right is a wall
				else if (SectionData.Cur_Sec [(int)PlayerMovement.Sec_Width - 1] [1] [0] == 'w') {
				}
				// cell to the top-right is a wall
				else if (SectionData.Cur_Sec [(int)PlayerMovement.Sec_Width - 2] [1] [0] == 'w') {
				}
				// no adjacent cells are walls
				else {
				}
			}
			// Player is at bottom-right cell
			else if (PlayerMovement.Cur_Col == PlayerMovement.Sec_Width - 1) {
				PlayerPrefs.SetInt ("Cur", (int)char.GetNumericValue (SectionData.Cur_Sec [(int)PlayerMovement.Sec_Width - 1] [(int)PlayerMovement.Sec_Width - 1] [3]));
				// cell to the top is a wall
				if (SectionData.Cur_Sec [(int)PlayerMovement.Sec_Width - 2] [(int)PlayerMovement.Sec_Width - 1] [0] == 'w') {
				}
				// cell to the left is a wall
				else if (SectionData.Cur_Sec [(int)PlayerMovement.Sec_Width - 1] [(int)PlayerMovement.Sec_Width - 2] [0] == 'w') {
				}
				// cell to the top-left is a wall
				else if (SectionData.Cur_Sec [(int)PlayerMovement.Sec_Width - 2] [(int)PlayerMovement.Sec_Width - 2] [0] == 'w') {
				}
				// no adjacent cells are walls
				else {
				}
			}
			// Player is in the bottom row
			else {
				PlayerPrefs.SetInt ("Cur", (int)char.GetNumericValue (SectionData.Cur_Sec [(int)PlayerMovement.Sec_Width - 1] [PlayerMovement.Cur_Col] [3]));
				// cell to the left is a wall
				if (SectionData.Cur_Sec [(int)PlayerMovement.Sec_Width - 1] [PlayerMovement.Cur_Col - 1] [0] == 'w') {
				}
				// cell to the right is a wall
				else if (SectionData.Cur_Sec [(int)PlayerMovement.Sec_Width - 1] [PlayerMovement.Cur_Col + 1] [0] == 'w') {
				}
				// cell to the top is a wall
				else if (SectionData.Cur_Sec [(int)PlayerMovement.Sec_Width - 2] [PlayerMovement.Cur_Col] [0] == 'w') {
				}
				// cell to the top-left is a wall
				else if (SectionData.Cur_Sec [(int)PlayerMovement.Sec_Width - 2] [PlayerMovement.Cur_Col - 1] [0] == 'w') {
				}
				// cell to the top-right is a wall
				else if (SectionData.Cur_Sec [(int)PlayerMovement.Sec_Width - 2] [PlayerMovement.Cur_Col + 1] [0] == 'w') {
				}
				// no adjacent cells are walls
				else {
				}
			}
		}
		// Player is at the left-most column
		else if (PlayerMovement.Cur_Col == 0) {
			PlayerPrefs.SetInt ("Cur", (int)char.GetNumericValue (SectionData.Cur_Sec [PlayerMovement.Cur_Row] [0] [3]));
			// cell to the top is a wall
			if (SectionData.Cur_Sec [PlayerMovement.Cur_Row - 1] [0] [0] == 'w') {
			}
			// cell to the bottom is a wall
			else if (SectionData.Cur_Sec [PlayerMovement.Cur_Row + 1] [0] [0] == 'w') {
			}
			// cell to the right is a wall
			else if (SectionData.Cur_Sec [PlayerMovement.Cur_Row] [1] [0] == 'w') {
			}
			// cell to the top-right is a wall
			else if (SectionData.Cur_Sec [PlayerMovement.Cur_Row - 1] [1] [0] == 'w') {
			}
			// cell to the bottom-right is a wall
			else if (SectionData.Cur_Sec [PlayerMovement.Cur_Row + 1] [1] [0] == 'w') {
			}
			// no adjacent cells are walls
			else {
			}
		}
		// Player is at the right-most column
		else if (PlayerMovement.Cur_Col == (int)PlayerMovement.Sec_Width - 1) {
			PlayerPrefs.SetInt ("Cur", (int)char.GetNumericValue (SectionData.Cur_Sec [PlayerMovement.Cur_Row] [(int)PlayerMovement.Sec_Width - 1] [3]));
			// cell to the top is a wall
			if (SectionData.Cur_Sec [PlayerMovement.Cur_Row - 1] [(int)PlayerMovement.Sec_Width - 1] [0] == 'w') {
			}
			// cell to the bottom is a wall
			else if (SectionData.Cur_Sec [PlayerMovement.Cur_Row + 1] [(int)PlayerMovement.Sec_Width - 1] [0] == 'w') {
			}
			// cell to the left is a wall
			else if (SectionData.Cur_Sec [PlayerMovement.Cur_Row] [(int)PlayerMovement.Sec_Width - 2] [0] == 'w') {
			}
			// cell to the top-left is a wall
			else if (SectionData.Cur_Sec [PlayerMovement.Cur_Row - 1] [(int)PlayerMovement.Sec_Width - 1] [0] == 'w') {
			}
			// cell to the bottom-left is a wall
			else if (SectionData.Cur_Sec [PlayerMovement.Cur_Row + 1] [(int)PlayerMovement.Sec_Width - 1] [0] == 'w') {
			}
			// no adjacent cells are walls
			else {
			}
		}
		// Player is not at the edge of the section
		else {
			PlayerPrefs.SetInt ("Cur", (int)char.GetNumericValue (SectionData.Cur_Sec [PlayerMovement.Cur_Row] [PlayerMovement.Cur_Col] [3]));
			// cell to the top is a wall
			if (SectionData.Cur_Sec [PlayerMovement.Cur_Row - 1] [PlayerMovement.Cur_Col] [0] == 'w') {
			}
			// cell to the bottom is a wall
			else if (SectionData.Cur_Sec [PlayerMovement.Cur_Row + 1] [PlayerMovement.Cur_Col] [0] == 'w') {
			}
			// cell to the left is a wall
			else if (SectionData.Cur_Sec [PlayerMovement.Cur_Row] [PlayerMovement.Cur_Col - 1] [0] == 'w') {
			}
			// cell to the right is a wall
			else if (SectionData.Cur_Sec [PlayerMovement.Cur_Row] [PlayerMovement.Cur_Col + 1] [0] == 'w') {
			}
			// cell to the top-left is a wall
			else if (SectionData.Cur_Sec [PlayerMovement.Cur_Row - 1] [PlayerMovement.Cur_Col - 1] [0] == 'w') {
			}
			// cell to the top-right is a wall
			else if (SectionData.Cur_Sec [PlayerMovement.Cur_Row - 1] [PlayerMovement.Cur_Col + 1] [0] == 'w') {
			}
			// cell to the bottom-left is a wall
			else if (SectionData.Cur_Sec [PlayerMovement.Cur_Row + 1] [PlayerMovement.Cur_Col - 1] [0] == 'w') {
			}
			// cell to the bottom-right is a wall
			else if (SectionData.Cur_Sec [PlayerMovement.Cur_Row + 1] [PlayerMovement.Cur_Col + 1] [0] == 'w') {
			}
			// no adjacent cells are walls
			else {
			}
		}
	}

	// Determines when the player enters a random encounter
	public IEnumerator Randomizer () {
		// encounter
		if (Random.Range (0.0f, 100.0f) > Encounter_Rate) {
			SceneManager.LoadScene ("Encounter");
			yield return null;
		}
	}
}
