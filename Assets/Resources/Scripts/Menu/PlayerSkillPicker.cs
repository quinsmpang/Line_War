using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerSkillPicker : MonoBehaviour {

	public GameObject characterList; // Assign in inspector
	public Button buttonPlayerSkill1;
	public Button buttonPlayerSkill2;
	private bool _isShowing;

	void OnEnable() {
		buttonPlayerSkill1.onClick.AddListener (ToggleCharactersList);
		buttonPlayerSkill2.onClick.AddListener (ToggleCharactersList);
	}

	void ToggleCharactersList () {
		_isShowing = !_isShowing;
		characterList.SetActive(_isShowing);
	}
}

