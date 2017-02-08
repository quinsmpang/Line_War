using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CharacterPicker : MonoBehaviour {

	public GameObject characterPanel;
	public Button buttonCharacterPicker;
	private bool _isShowing;
	public Button[] characters;

	void Start() {
		
	}

	void OnEnable() {
		buttonCharacterPicker.onClick.AddListener (ToggleCharacterList);
		for (int x = 0; x<characters.Length; x++) {
			characters[x].interactable = false;
		}
	}

	void ToggleCharacterList () {
		_isShowing = !_isShowing;
		characterPanel.SetActive(_isShowing);

	}
}

