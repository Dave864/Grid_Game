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

	// Find out which edge of the section the player is at
	void WhichEdge () {
		// Player is at the top row
		if (PlayerMovement.Cur_Row == 0) {
			ArenaSectionTop ();
		}
		// Player is at the bottom row
		else if (PlayerMovement.Cur_Row == PlayerMovement.Sec_Width - 1) {
			ArenaSectionBottom ();
		}
		// Player is at the left-most column
		else if (PlayerMovement.Cur_Col == 0) {
			ArenaSectionLeft ();
		}
		// Player is at the right-most column
		else if (PlayerMovement.Cur_Col == (int)PlayerMovement.Sec_Width - 1) {
			ArenaSectionRight ();
		}
		// Player is not at the edge of the section
		else {
			ArenaSectionCenter ();
		}
	}

	// Determine the sections that will make up the arena when the player is in the top row
	void ArenaSectionTop () {
		// Player is at top-left cell
		if (PlayerMovement.Cur_Col == 0) {
			PlayerPrefs.SetInt ("Cur", (int)char.GetNumericValue (SectionData.Cur_Sec [0] [0] [3]));
			// cell to the right is a wall
			if (SectionData.Cur_Sec [0] [1] [0] == 'w') {
				ArenaDoubleUp ();
			}
			// cell to the bottom is a wall
			else if (SectionData.Cur_Sec [1] [0] [0] == 'w') {
				ArenaDoubleLeft ();
			}
			// cell to the bottom-right is a wall
			else if (SectionData.Cur_Sec [1] [1] [0] == 'w') {
				switch (Random.Range (0, 1)) {
				case 1:
					ArenaDoubleUp ();
					break;
				default:
					ArenaDoubleLeft ();
					break;
				}
			}
			// no adjacent cells are walls
			else {
				ArenaQuadTL ();
			}
		}
		// Player is at top-right cell
		else if (PlayerMovement.Cur_Col == PlayerMovement.Sec_Width - 1) {
			PlayerPrefs.SetInt ("Cur", (int)char.GetNumericValue (SectionData.Cur_Sec [0] [(int)PlayerMovement.Sec_Width - 1] [3]));
			// cell to the left is a wall
			if (SectionData.Cur_Sec [0] [(int)PlayerMovement.Sec_Width - 2] [0] == 'w') {
				ArenaDoubleUp ();
			}
			// cell to the bottom is a wall
			else if (SectionData.Cur_Sec [1] [(int)PlayerMovement.Sec_Width - 1] [0] == 'w') {
				ArenaDoubleRight ();
			}
			// cell to the bottom-left is a wall
			else if (SectionData.Cur_Sec [1] [(int)PlayerMovement.Sec_Width - 2] [0] == 'w') {
				switch (Random.Range (0, 1)) {
				case 1:
					ArenaDoubleUp ();
					break;
				default:
					ArenaDoubleRight ();
					break;
				}
			}
			// no adjacent cells are walls
			else {
				ArenaQuadTR ();
			}
		}
		// Player is in top row
		else {
			PlayerPrefs.SetInt ("Cur", (int)char.GetNumericValue (SectionData.Cur_Sec [0] [PlayerMovement.Cur_Col] [3]));
			// cell to the right is a wall
			if (SectionData.Cur_Sec [0] [PlayerMovement.Cur_Col + 1] [0] == 'w') {
				// cell to the left is a wall
				if (SectionData.Cur_Sec [0] [PlayerMovement.Cur_Col - 1] [0] == 'w') {
					ArenaDoubleUp ();
				}
				// cell to the bottom is a wall
				else if (SectionData.Cur_Sec [1] [PlayerMovement.Cur_Col] [0] == 'w') {
					ArenaDoubleRight ();
				}
				// cell to the bottom-left is a wall
				else if (SectionData.Cur_Sec [1] [PlayerMovement.Cur_Col - 1] [0] == 'w') {
					switch (Random.Range (0, 1)) {
					case 1:
						ArenaDoubleUp ();
						break;
					default:
						ArenaDoubleRight ();
						break;
					}
				}
				// no other cells are walls
				else {
					ArenaQuadTR ();
				}
			}
			// cell to the left is a wall
			else if (SectionData.Cur_Sec [0] [PlayerMovement.Cur_Col - 1] [0] == 'w') {
				// cell to the right is a wall
				if (SectionData.Cur_Sec [0] [PlayerMovement.Cur_Col + 1] [0] == 'w') {
					ArenaDoubleUp ();
				}
				// cell to the bottom is a wall
				else if (SectionData.Cur_Sec [1] [PlayerMovement.Cur_Col] [0] == 'w') {
					ArenaDoubleLeft ();
				}
				// cell to the bottom-right is a wall
				else if (SectionData.Cur_Sec [1] [PlayerMovement.Cur_Col + 1] [0] == 'w') {
					switch (Random.Range (0, 1)) {
					case 1:
						ArenaDoubleUp ();
						break;
					default:
						ArenaDoubleLeft ();
						break;
					}
				}
				// no other cells are walls
				else {
					ArenaQuadTL ();
				}
			}
			// cell to the bottom is a wall
			else if (SectionData.Cur_Sec [1] [PlayerMovement.Cur_Col] [0] == 'w') {
				switch (Random.Range (0, 1)) {
				case 1:
					ArenaDoubleDown ();
					break;
				default:
					ArenaDoubleLeft ();
					break;
				}
			}
			// cell to the bottom-right is a wall
			else if (SectionData.Cur_Sec [1] [(int)PlayerMovement.Cur_Col + 1] [0] == 'w') {
				ArenaQuadTR ();
			}
			// cell to the bottom-left is a wall
			else if (SectionData.Cur_Sec [1] [(int)PlayerMovement.Cur_Col - 1] [0] == 'w') {
				ArenaQuadTL ();
			}
			// no adjacent cells are walls
			else {
				switch (Random.Range (0, 1)) {
				case 1:
					ArenaQuadTR ();
					break;
				default:
					ArenaQuadTL ();
					break;
				}
			}
		}
	}

	// Determine the sections that will make up the arena when the player is in the bottom row
	void ArenaSectionBottom () {
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

	// Determine the sections that will make up the arena when the player is in the left-most column
	void ArenaSectionLeft () {
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

	// Determine the sections that will make up the arena when the player is in the right-most column
	void ArenaSectionRight () {
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

	// Determine the sections that will make up the arena when the player is not at the edge of the section
	void ArenaSectionCenter () {
		PlayerPrefs.SetInt ("Cur", (int)char.GetNumericValue (SectionData.Cur_Sec [PlayerMovement.Cur_Row] [PlayerMovement.Cur_Col] [3]));
		// cell to the top is a wall
		if (SectionData.Cur_Sec [PlayerMovement.Cur_Row - 1] [PlayerMovement.Cur_Col] [0] == 'w') {
			// cell to the bottom is a wall
			if (SectionData.Cur_Sec [PlayerMovement.Cur_Row + 1] [PlayerMovement.Cur_Col] [0] == 'w') {
				// cell to the left is a wall
				if (SectionData.Cur_Sec [PlayerMovement.Cur_Row] [PlayerMovement.Cur_Col - 1] [0] == 'w') {
					ArenaDoubleLeft ();
				}
				// cell to the right is a wall
				else if (SectionData.Cur_Sec [PlayerMovement.Cur_Row] [PlayerMovement.Cur_Col + 1] [0] == 'w') {
					ArenaDoubleRight ();
				}
				// cells to the left and right are not walls
				else {
					switch (Random.Range (0, 1)) {
					case 1:
						ArenaDoubleLeft ();
						break;
					default:
						ArenaDoubleRight ();
						break;
					}
				}
			}
			// cell to the bottom-left is a wall
			else if (SectionData.Cur_Sec [PlayerMovement.Cur_Row + 1] [PlayerMovement.Cur_Col - 1] [0] == 'w') {
				// cell to the left is a wall
				if (SectionData.Cur_Sec [PlayerMovement.Cur_Row] [PlayerMovement.Cur_Col - 1] [0] == 'w') {
					// cell to the right is a wall
					if (SectionData.Cur_Sec [PlayerMovement.Cur_Row] [PlayerMovement.Cur_Col + 1] [0] == 'w') {
						ArenaDoubleUp ();
					} else {
						ArenaQuadTL ();
					}
				}
				// cell to the right is a wall
				else if (SectionData.Cur_Sec [PlayerMovement.Cur_Row] [PlayerMovement.Cur_Col + 1] [0] == 'w') {
					switch (Random.Range (0, 1)) {
					case 1:
						ArenaDoubleRight ();
						break;
					default:
						ArenaDoubleUp ();
						break;
					}
				}
				// cell to the bottom-right is a wall
				else if (SectionData.Cur_Sec [PlayerMovement.Cur_Row + 1] [PlayerMovement.Cur_Col + 1] [0] == 'w') {
					switch (Random.Range (0, 2)) {
					case 1:
						ArenaDoubleRight ();
						break;
					case 2:
						ArenaDoubleUp ();
						break;
					default:
						ArenaDoubleLeft ();
						break;
					}
				}
				// choose between a double cell or quad cell arena
				else {
					switch (Random.Range (0, 1)) {
					case 1:
						ArenaDoubleRight ();
						break;
					default:
						ArenaQuadTL ();
						break;
					}
				}
			}
			// cell to the bottom-right is a wall
			else if (SectionData.Cur_Sec [PlayerMovement.Cur_Row + 1] [PlayerMovement.Cur_Col + 1] [0] == 'w') {
				// cell to the right is a wall
				if (SectionData.Cur_Sec [PlayerMovement.Cur_Row] [PlayerMovement.Cur_Col + 1] [0] == 'w') {
					// cell to the left is a wall
					if (SectionData.Cur_Sec [PlayerMovement.Cur_Row] [PlayerMovement.Cur_Col - 1] [0] == 'w') {
						ArenaDoubleUp ();
					} else {
						ArenaQuadTR ();
					}
				}
				// cell to the left is a wall
				else if (SectionData.Cur_Sec [PlayerMovement.Cur_Row] [PlayerMovement.Cur_Col - 1] [0] == 'w') {
					switch (Random.Range (0, 1)) {
					case 1:
						ArenaDoubleLeft ();
						break;
					default:
						ArenaDoubleUp ();
						break;
					}
				}
				// cell to the bottom-left is a wall
				else if (SectionData.Cur_Sec [PlayerMovement.Cur_Row + 1] [PlayerMovement.Cur_Col - 1] [0] == 'w') {
					switch (Random.Range (0, 2)) {
					case 1:
						ArenaDoubleLeft ();
						break;
					case 2:
						ArenaDoubleUp ();
						break;
					default:
						ArenaDoubleRight ();
						break;
					}
				}
				// choose between a double cell or quad cell arena
				else {
					switch (Random.Range (0, 1)) {
					case 1:
						ArenaDoubleLeft ();
						break;
					default:
						ArenaQuadTR ();
						break;
					}
				}
			}
			// no lower adjacent cells are walls
			else {
				switch (Random.Range (0, 1)) {
				case 1:
					ArenaQuadTR ();
					break;
				default:
					ArenaQuadTL ();
					break;
				}
			}
		}
		// cell to the bottom is a wall
		else if (SectionData.Cur_Sec [PlayerMovement.Cur_Row + 1] [PlayerMovement.Cur_Col] [0] == 'w') {
			// cell to the top is a wall
			if (SectionData.Cur_Sec [PlayerMovement.Cur_Row - 1] [PlayerMovement.Cur_Col] [0] == 'w') {
				// cell to the left is a wall
				if (SectionData.Cur_Sec [PlayerMovement.Cur_Row] [PlayerMovement.Cur_Col - 1] [0] == 'w') {
					ArenaDoubleLeft ();
				}
				// cell to the right is a wall
				else if (SectionData.Cur_Sec [PlayerMovement.Cur_Row] [PlayerMovement.Cur_Col + 1] [0] == 'w') {
					ArenaDoubleRight ();
				}
				// cells to the left and right are not walls
				else {
					switch (Random.Range (0, 1)) {
					case 1:
						ArenaDoubleLeft ();
						break;
					default:
						ArenaDoubleRight ();
						break;
					}
				}
			}
			// cell to the left is a wall
			else if (SectionData.Cur_Sec [PlayerMovement.Cur_Row] [PlayerMovement.Cur_Col - 1] [0] == 'w') {
				// cell to the right is a wall
				if (SectionData.Cur_Sec [PlayerMovement.Cur_Row] [PlayerMovement.Cur_Col + 1] [0] == 'w') {
					ArenaDoubleDown ();
				}
				// cell to the top-right is a wall
				else if (SectionData.Cur_Sec [PlayerMovement.Cur_Row - 1] [PlayerMovement.Cur_Col + 1] [0] == 'w') {
					switch (Random.Range (0, 1)) {
					case 1:
						ArenaDoubleDown ();
						break;
					default:
						ArenaDoubleRight ();
						break;
					}
				} else {
					ArenaQuadBR ();
				}
			}
			// cell to the right is a wall
			else if (SectionData.Cur_Sec [PlayerMovement.Cur_Row] [PlayerMovement.Cur_Col + 1] [0] == 'w') {
				// cell to the top-left is a wall
				if (SectionData.Cur_Sec [PlayerMovement.Cur_Row - 1] [PlayerMovement.Cur_Col - 1] [0] == 'w') {
					switch (Random.Range (0, 1)) {
					case 1:
						ArenaDoubleDown ();
						break;
					default:
						ArenaDoubleRight ();
						break;
					}
				} else {
					ArenaQuadBR ();
				}
			}
			// cell to the top-left is a wall
			else if (SectionData.Cur_Sec [PlayerMovement.Cur_Row - 1] [PlayerMovement.Cur_Col - 1] [0] == 'w') {
				// cell to the top-right is a wall
				if (SectionData.Cur_Sec [PlayerMovement.Cur_Row - 1] [PlayerMovement.Cur_Col + 1] [0] == 'w') {
					switch (Random.Range (0, 2)) {
					case 1:
						ArenaDoubleDown ();
						break;
					case 2:
						ArenaDoubleRight ();
						break;
					default:
						ArenaDoubleLeft ();
						break;
					}
				} else {
					ArenaQuadBL ();
				}
			}
			// cell to the top-right is a wall
			else if (SectionData.Cur_Sec [PlayerMovement.Cur_Row - 1] [PlayerMovement.Cur_Col + 1] [0] == 'w') {
				ArenaQuadBR ();
			}
			// no upper adjacent cells are walls
			else {
				PlayerPrefs.SetString ("ArenaType", "quad");
				switch (Random.Range (0, 1)) {
				case 1:
					ArenaQuadBR ();
					break;
				default:
					ArenaQuadBL ();
					break;
				}
			}
		}
		// cell to the left is a wall
		else if (SectionData.Cur_Sec [PlayerMovement.Cur_Row] [PlayerMovement.Cur_Col - 1] [0] == 'w') {
			// cell to the right is a wall
			if (SectionData.Cur_Sec [PlayerMovement.Cur_Row] [PlayerMovement.Cur_Col + 1] [0] == 'w') {
				switch (Random.Range (0, 1)) {
				case 1:
					ArenaDoubleDown ();
					break;
				default:
					ArenaDoubleLeft ();
					break;
				}
			}
			// cell to the top-right is a wall
			else if (SectionData.Cur_Sec [PlayerMovement.Cur_Row - 1] [PlayerMovement.Cur_Col + 1] [0] == 'w') {
				// cell to the bottom-right is a wall
				if (SectionData.Cur_Sec [PlayerMovement.Cur_Row + 1] [PlayerMovement.Cur_Col + 1] [0] == 'w') {
					switch (Random.Range (0, 2)) {
					case 1:
						ArenaDoubleDown ();
						break;
					case 2:
						ArenaDoubleUp ();
						break;
					default:
						ArenaDoubleLeft ();
						break;
					}
				} else {
					switch (Random.Range (0, 1)) {
					case 1:
						ArenaDoubleDown ();
						break;
					default:
						ArenaQuadTL ();
						break;
					}
				}
			}
			// cell to the bottom-right is a wall
			else if (SectionData.Cur_Sec [PlayerMovement.Cur_Row + 1] [PlayerMovement.Cur_Col + 1] [0] == 'w') {
				switch (Random.Range (0, 1)) {
				case 1:
					ArenaDoubleUp ();
					break;
				default:
					ArenaQuadBL ();
					break;
				}
			} else {
				switch (Random.Range (0, 1)) {
				case 1:
					ArenaQuadTL ();
					break;
				default:
					ArenaQuadBL ();
					break;
				}
			}
		}
		// cell to the right is a wall
		else if (SectionData.Cur_Sec [PlayerMovement.Cur_Row] [PlayerMovement.Cur_Col + 1] [0] == 'w') {
			// cell to the top-left is a wall
			if (SectionData.Cur_Sec [PlayerMovement.Cur_Row - 1] [PlayerMovement.Cur_Col - 1] [0] == 'w') {
				// cell to the bottom-left is a wall
				if (SectionData.Cur_Sec [PlayerMovement.Cur_Row + 1] [PlayerMovement.Cur_Col - 1] [0] == 'w') {
					switch (Random.Range (0, 2)) {
					case 1:
						ArenaDoubleDown ();
						break;
					case 2:
						ArenaDoubleUp ();
						break;
					default:
						ArenaDoubleRight ();
						break;
					}
				} else {
					switch (Random.Range (0, 1)) {
					case 1:
						ArenaDoubleDown ();
						break;
					default:
						ArenaQuadTR ();
						break;
					}
				}
			}
			// cell to the bottom-left is a wall
			else if (SectionData.Cur_Sec [PlayerMovement.Cur_Row + 1] [PlayerMovement.Cur_Col - 1] [0] == 'w') {
				switch (Random.Range (0, 1)) {
				case 1:
					ArenaDoubleUp ();
					break;
				default:
					ArenaQuadBR ();
					break;
				}
			} else {
				switch (Random.Range (0, 1)) {
				case 1:
					ArenaQuadBR ();
					break;
				default:
					ArenaQuadTR ();
					break;
				}
			}
		}
		// cell to the top-left is a wall
		else if (SectionData.Cur_Sec [PlayerMovement.Cur_Row - 1] [PlayerMovement.Cur_Col - 1] [0] == 'w') {
			// cell to the top-right is a wall
			if (SectionData.Cur_Sec [PlayerMovement.Cur_Row - 1] [PlayerMovement.Cur_Col + 1] [0] == 'w') {
				// cell to the bottom-right is a wall
				if (SectionData.Cur_Sec [PlayerMovement.Cur_Row - 1] [PlayerMovement.Cur_Col + 1] [0] == 'w') {
					// cell to the bottom-left is a wall
					if (SectionData.Cur_Sec [PlayerMovement.Cur_Row - 1] [PlayerMovement.Cur_Col - 1] [0] == 'w') {
						switch (Random.Range (0, 3)) {
						case 1:
							ArenaDoubleLeft ();
							break;
						case 2:
							ArenaDoubleUp ();
							break;
						case 3:
							ArenaDoubleRight ();
							break;
						default:
							ArenaDoubleDown ();
							break;
						}
					} else {
						switch (Random.Range (0, 2)) {
						case 1:
							ArenaDoubleDown ();
							break;
						case 2:
							ArenaDoubleLeft ();
							break;
						default:
							ArenaQuadTR ();
							break;
						}
					}
				}
				// cell to the bottom-left is a wall
				else if (SectionData.Cur_Sec [PlayerMovement.Cur_Row - 1] [PlayerMovement.Cur_Col + 1] [0] == 'w') {
					switch (Random.Range (0, 2)) {
					case 1:
						ArenaDoubleRight ();
						break;
					case 2:
						ArenaDoubleDown ();
						break;
					default:
						ArenaQuadTL ();
						break;
					}
				} else {
					switch (Random.Range (0, 2)) {
					case 1:
						ArenaQuadTR ();
						break;
					case 2:
						ArenaQuadTL ();
						break;
					default:
						ArenaDoubleRight ();
						break;
					}
				}
			}
			// cell to the bottom-right is a wall
			else if (SectionData.Cur_Sec [PlayerMovement.Cur_Row + 1] [PlayerMovement.Cur_Col + 1] [0] == 'w') {
				// cell to the bottom-left is a wall
				if (SectionData.Cur_Sec [PlayerMovement.Cur_Row + 1] [PlayerMovement.Cur_Col - 1] [0] == 'w') {
					switch (Random.Range (0, 2)) {
					case 1:
						ArenaDoubleRight ();
						break;
					case 2:
						ArenaDoubleUp ();
						break;
					default:
						ArenaQuadBL ();
						break;
					}
				} else {
					switch (Random.Range (0, 1)) {
					case 1:
						ArenaQuadBL ();
						break;
					default:
						ArenaQuadTR ();
						break;
					}
				}
			}
			// cell to the bottom-left is a wall
			else if (SectionData.Cur_Sec [PlayerMovement.Cur_Row + 1] [PlayerMovement.Cur_Col - 1] [0] == 'w') {
				switch (Random.Range (0, 2)) {
				case 1:
					ArenaDoubleRight ();
					break;
				case 2:
					ArenaQuadBL ();
					break;
				default:
					ArenaQuadTL ();
					break;
				}
			}
			// no other adjacent cells is a wall
			else {
				switch(Random.Range(0,2)){
				case 1:
					ArenaQuadBL ();
					break;
				case 2:
					ArenaQuadTL ();
					break;
				default:
					ArenaQuadTR ();
					break;
				}
			}
		}
		// cell to the top-right is a wall
		else if (SectionData.Cur_Sec [PlayerMovement.Cur_Row - 1] [PlayerMovement.Cur_Col + 1] [0] == 'w') {
			// cell to the bottom-right is a wall
			if (SectionData.Cur_Sec [PlayerMovement.Cur_Row + 1] [PlayerMovement.Cur_Col + 1] [0] == 'w') {
				// cell to the bottom-left is a wall
				if (SectionData.Cur_Sec [PlayerMovement.Cur_Row + 1] [PlayerMovement.Cur_Col - 1] [0] == 'w') {
					switch (Random.Range (0, 2)) {
					case 1:
						ArenaDoubleLeft ();
						break;
					case 2:
						ArenaDoubleUp ();
						break;
					default:
						ArenaQuadBR ();
						break;
					}
				} else {
					switch (Random.Range (0, 2)) {
					case 1:
						ArenaDoubleLeft ();
						break;
					case 2:
						ArenaQuadBR ();
						break;
					default:
						ArenaQuadTR ();
						break;
					}
				}
			}
			// cell to the bottom-left is a wall
			else if (SectionData.Cur_Sec [PlayerMovement.Cur_Row + 1] [PlayerMovement.Cur_Col - 1] [0] == 'w') {
				switch (Random.Range (0, 1)) {
				case 1:
					ArenaQuadBR ();
					break;
				default:
					ArenaQuadTL ();
					break;
				}
			}
			// no other adjacent cells is a wall
			else {
				switch (Random.Range (0, 2)) {
				case 1:
					ArenaQuadBR ();
					break;
				case 2:
					ArenaQuadTR ();
					break;
				default:
					ArenaQuadTL ();
					break;
				}
			}
		}
		// cell to the bottom-left is a wall
		else if (SectionData.Cur_Sec [PlayerMovement.Cur_Row + 1] [PlayerMovement.Cur_Col - 1] [0] == 'w') {
			// cell to the bottom-right is a wall
			if (SectionData.Cur_Sec [PlayerMovement.Cur_Row + 1] [PlayerMovement.Cur_Col + 1] [0] == 'w') {
				switch (Random.Range (0, 2)) {
				case 1:
					ArenaDoubleUp ();
					break;
				case 2:
					ArenaQuadBR ();
					break;
				default:
					ArenaQuadBL ();
					break;
				}
			}
			// no other adjacent cells is a wall
			else {
				switch (Random.Range (0, 2)) {
				case 1:
					ArenaQuadBR ();
					break;
				case 2:
					ArenaQuadBL ();
					break;
				default:
					ArenaQuadTL ();
					break;
				}
			}
		}
		// cell to the bottom-right is a wall
		else if (SectionData.Cur_Sec [PlayerMovement.Cur_Row + 1] [PlayerMovement.Cur_Col + 1] [0] == 'w') {
			switch (Random.Range (0, 2)) {
			case 1:
				ArenaQuadBR ();
				break;
			case 2:
				ArenaQuadBL ();
				break;
			default:
				ArenaQuadTL ();
				break;
			}
		}
		// no adjacent cells are walls
		else {
			switch (Random.Range (0, 3)) {
			case 1:
				ArenaQuadTL ();
				break;
			case 2:
				ArenaQuadTR ();
				break;
			case 3:
				ArenaQuadBL ();
				break;
			default:
				ArenaQuadBR ();
				break;
			}
		}
	}

	// set up double left-right arena with player at left
	void ArenaDoubleLeft(){
		PlayerPrefs.SetString ("ArenaType", "double");
		PlayerPrefs.SetInt ("TL", -2);
		PlayerPrefs.SetInt ("TR", (int)char.GetNumericValue (SectionData.Cur_Sec [PlayerMovement.Cur_Row] [PlayerMovement.Cur_Col - 1] [3]));
		PlayerPrefs.SetInt ("BL", -1);
		PlayerPrefs.SetInt ("BR", -1);
	}

	// set up double left-right arena with player at right
	void ArenaDoubleRight(){
		PlayerPrefs.SetString ("ArenaType", "double");
		PlayerPrefs.SetInt ("TL", (int)char.GetNumericValue (SectionData.Cur_Sec [PlayerMovement.Cur_Row] [PlayerMovement.Cur_Col + 1] [3]));
		PlayerPrefs.SetInt ("TR", -2);
		PlayerPrefs.SetInt ("BL", -1);
		PlayerPrefs.SetInt ("BR", -1);
	}

	// set up double up-down arena with player at up
	void ArenaDoubleUp(){
		PlayerPrefs.SetString ("ArenaType", "double");
		PlayerPrefs.SetInt ("TL", -2);
		PlayerPrefs.SetInt ("TR", -1);
		PlayerPrefs.SetInt ("BL", (int)char.GetNumericValue (SectionData.Cur_Sec [PlayerMovement.Cur_Row - 1] [PlayerMovement.Cur_Col] [3]));
		PlayerPrefs.SetInt ("BR", -1);
	}

	// set up double up-down arena with player at down
	void ArenaDoubleDown(){
		PlayerPrefs.SetString ("ArenaType", "double");
		PlayerPrefs.SetInt ("TL", (int)char.GetNumericValue (SectionData.Cur_Sec [PlayerMovement.Cur_Row + 1] [PlayerMovement.Cur_Col] [3]));
		PlayerPrefs.SetInt ("TR", -1);
		PlayerPrefs.SetInt ("BL", -1);
		PlayerPrefs.SetInt ("BR", -2);
	}

	// set up quad arena with player at top-left
	void ArenaQuadTL(){
		PlayerPrefs.SetString ("ArenaType", "quad");
		PlayerPrefs.SetInt ("TL", -1);
		PlayerPrefs.SetInt ("TR", (int)char.GetNumericValue (SectionData.Cur_Sec [PlayerMovement.Cur_Row] [PlayerMovement.Cur_Col + 1] [3]));
		PlayerPrefs.SetInt ("BL", (int)char.GetNumericValue (SectionData.Cur_Sec [PlayerMovement.Cur_Row + 1] [PlayerMovement.Cur_Col] [3]));
		PlayerPrefs.SetInt ("BR", (int)char.GetNumericValue (SectionData.Cur_Sec [PlayerMovement.Cur_Row + 1] [PlayerMovement.Cur_Col + 1] [3]));
	}

	// set up quad arena with player at top-right
	void ArenaQuadTR(){
		PlayerPrefs.SetString ("ArenaType", "quad");
		PlayerPrefs.SetInt ("TL", (int)char.GetNumericValue (SectionData.Cur_Sec [PlayerMovement.Cur_Row] [PlayerMovement.Cur_Col - 1] [3]));
		PlayerPrefs.SetInt ("TR", -1);
		PlayerPrefs.SetInt ("BL", (int)char.GetNumericValue (SectionData.Cur_Sec [PlayerMovement.Cur_Row + 1] [PlayerMovement.Cur_Col - 1] [3]));
		PlayerPrefs.SetInt ("BR", (int)char.GetNumericValue (SectionData.Cur_Sec [PlayerMovement.Cur_Row + 1] [PlayerMovement.Cur_Col] [3]));
	}

	// set up quad arena with player at bottom-left
	void ArenaQuadBL(){
		PlayerPrefs.SetString ("ArenaType", "quad");
		PlayerPrefs.SetInt ("TL", (int)char.GetNumericValue (SectionData.Cur_Sec [PlayerMovement.Cur_Row - 1] [PlayerMovement.Cur_Col] [3]));
		PlayerPrefs.SetInt ("TR", (int)char.GetNumericValue (SectionData.Cur_Sec [PlayerMovement.Cur_Row - 1] [PlayerMovement.Cur_Col + 1] [3]));
		PlayerPrefs.SetInt ("BL", -1);
		PlayerPrefs.SetInt ("BR", (int)char.GetNumericValue (SectionData.Cur_Sec [PlayerMovement.Cur_Row] [PlayerMovement.Cur_Col + 1] [3]));
	}

	// set up quad arena with player at bottom-right
	void ArenaQuadBR(){
		PlayerPrefs.SetString ("ArenaType", "quad");
		PlayerPrefs.SetInt ("TL", (int)char.GetNumericValue (SectionData.Cur_Sec [PlayerMovement.Cur_Row - 1] [PlayerMovement.Cur_Col - 1] [3]));
		PlayerPrefs.SetInt ("TR", (int)char.GetNumericValue (SectionData.Cur_Sec [PlayerMovement.Cur_Row - 1] [PlayerMovement.Cur_Col] [3]));
		PlayerPrefs.SetInt ("BL", (int)char.GetNumericValue (SectionData.Cur_Sec [PlayerMovement.Cur_Row] [PlayerMovement.Cur_Col - 1] [3]));
		PlayerPrefs.SetInt ("BR", -1);
	}

	// Determines when the player enters a random encounter
	public IEnumerator Randomizer () {
		// encounter
		if (Random.Range (0.0f, 100.0f) > Encounter_Rate) {
			WhichEdge ();
			SceneManager.LoadScene ("Encounter");
			yield return null;
		}
	}
}
