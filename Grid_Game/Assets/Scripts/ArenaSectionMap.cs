using UnityEngine;
using System.Collections;

public class ArenaSectionMap : MonoBehaviour {

	public class Cell{
		// adjacent cells
		public Cell node_0;
		public Cell node_1;
		public Cell node_2;
		public Cell node_3;
		public Cell node_4;
		public Cell node_5;

		public int sec_key;

		// physical object in the game world
		public GameObject arena_object;

		public Cell(int num){
			node_0 = null;
			node_1 = null;
			node_2 = null;
			node_3 = null;
			node_4 = null;
			node_5 = null;
			sec_key = num;
		}

		public void LinkNode0 (Cell adj = null){
			this.node_0 = adj;
			if (adj != null) {
				adj.node_3 = this;
			}
		}

		public void LinkNode1 (Cell adj = null){
			this.node_1 = adj;
			if (adj != null) {
				adj.node_4 = this;
			}
		}

		public void LinkNode2 (Cell adj = null){
			this.node_2 = adj;
			if (adj != null) {
				adj.node_5 = this;
			}
		}

		public void LinkNode3 (Cell adj = null){
			this.node_3 = adj;
			if (adj != null) {
				adj.node_0 = this;
			}
		}

		public void LinkNode4 (Cell adj = null){
			this.node_4 = adj;
			if (adj != null) {
				adj.node_4 = this;
			}
		}

		public void LinkNode5 (Cell adj = null){
			this.node_5 = adj;
			if (adj != null) {
				adj.node_2 = this;
			}
		}
	}

	public Cell[][] Cell_Map;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
