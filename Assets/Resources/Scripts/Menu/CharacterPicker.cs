using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CharacterPicker : MonoBehaviour {

	public GameObject characterList;
	public Button buttonCharacter;
	private bool _isShowing;

	void OnEnable() {
		buttonCharacter.onClick.AddListener (ToggleCharacterList);
	}

	void ToggleCharacterList () {
		_isShowing = !_isShowing;
		characterList.SetActive(_isShowing);
	}
}

