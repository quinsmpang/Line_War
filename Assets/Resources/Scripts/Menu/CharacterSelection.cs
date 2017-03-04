using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelection : MonoBehaviour {

	[Tooltip("prefab used to represent the character's gameobject model")]
	public GameObject prefab;
	public static CharacterSelection instance;
	private GameObject currentSelection;


	void Awake () {
		if (instance == null)
			instance = this;
	}
		
	public GameObject CurrentSelection {
		get { return currentSelection; }
		set { currentSelection = value; }
	}
}
