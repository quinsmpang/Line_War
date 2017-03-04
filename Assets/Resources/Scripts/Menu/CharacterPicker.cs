using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;


public class CharacterPicker : MonoBehaviour {

	public GameObject characterPanel;
	public Button buttonCharacterPicker;
	private bool _isShowing;
	public static CharacterPicker instance;
	public Button buttonCharacterBtn;
	public Image characterIconImg;


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
			btn.onClick.AddListener (delegate {
				SelectCharacter (btn);
			});
		}

		characterIconImg = gameObject.transform.Find("CharacterIcon").GetComponent<Image> ();
		Debug.LogWarning (characterIconImg.name);
	}

	private void SelectCharacter(Button btn) {
		characterIconImg.sprite = btn.transform.Find("Image").GetComponent<Image> ().sprite;
		CharacterSelection.instance.CurrentSelection = btn.GetComponent<CharacterSelection> ().prefab;
		Debug.LogWarning ("Selected Character: " + CharacterSelection.instance.CurrentSelection);
		//Debug.LogWarning ("Selected Character: " + CharacterSelection.instance.CurrentSelection.name);
	}

	void Start() {
		
	}

	void OnEnable() {
		buttonCharacterPicker.onClick.AddListener (ToggleCharacterList);
	}

	void ToggleCharacterList () {
		_isShowing = !_isShowing;
		characterPanel.SetActive(_isShowing);

	}
}

