using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;


public class CharacterPicker : MonoBehaviour {

	public GameObject characterPanel;
	public Button buttonCharacterPicker;
	private bool _isShowing;

	public Dictionary<string, Button> characterNameToButtonDict = new Dictionary<string, Button>();
	// LIST TO DISPLAY IN INSPECTOR
	//public List<Button> characterButtonList = new List<Button> ();

	void Start() {
		//characterPanel.SetActive (true);
		Button[] buttons = characterPanel.transform.GetComponentsInChildren<Button> ();
		foreach (Button btn in buttons) {
			characterNameToButtonDict.Add (btn.name, btn);
			// LIST TO DISPLAY IN INSPECTOR
			//characterButtonList.Add (btn);
			Debug.LogWarning ("Button: " + btn.name);
		}

	}

	void OnEnable() {
		buttonCharacterPicker.onClick.AddListener (ToggleCharacterList);
		foreach (KeyValuePair<string, Button> entry in characterNameToButtonDict)
		{
			characterNameToButtonDict [entry.Key].interactable = false;
		}
	}

	void ToggleCharacterList () {
		_isShowing = !_isShowing;
		characterPanel.SetActive(_isShowing);

	}
}

