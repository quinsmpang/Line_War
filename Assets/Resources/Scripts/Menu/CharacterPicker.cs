using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;


public class CharacterPicker : MonoBehaviour {

	public GameObject characterPanel;
	public Button buttonCharacterPicker;
	private bool _isShowing;
	public static CharacterPicker instance;
	//public Sprite[] characterIconsList;
	//public Sprite characterIcon;

	public Dictionary<string, Button> characterNameToButtonDict = new Dictionary<string, Button>();
	// LIST TO DISPLAY IN INSPECTOR
	//public List<Button> characterButtonList = new List<Button> ();

	void Awake() {
		instance = this;
		//characterPanel.SetActive (true);
		Button[] buttons = characterPanel.transform.GetComponentsInChildren<Button> ();
		foreach (Button btn in buttons) {
			characterNameToButtonDict.Add (btn.name, btn);
			// LIST TO DISPLAY IN INSPECTOR
			//characterButtonList.Add (btn);
			Debug.Log ("Character added to dictionary: " + btn.name + "");
		}

	}

	void Start() {
		// FIXME: Not sure what this is.  Where does playerIconsList get assigned?
		//characterIcon = characterIconsList[int.Parse(LoginPlayer.instance.currentPlayerPlayerIcon)];
	}

	void OnEnable() {
		buttonCharacterPicker.onClick.AddListener (ToggleCharacterList);
//		foreach (KeyValuePair<string, Button> entry in characterNameToButtonDict)
//		{
//			characterNameToButtonDict [entry.Key].interactable = false;
//		}
	}

	void ToggleCharacterList () {
		_isShowing = !_isShowing;
		characterPanel.SetActive(_isShowing);

	}
}

