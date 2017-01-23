using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CharacterSkillPicker : MonoBehaviour {

	public GameObject characterSkillList;
	public Button buttonCharacterSkill1;
	public Button buttonCharacterSkill2;
	public Button buttonCharacterSkill3;
	private bool _isShowing;

	void OnEnable() {
		buttonCharacterSkill1.onClick.AddListener (ToggleCharacterSkillList);
		buttonCharacterSkill2.onClick.AddListener (ToggleCharacterSkillList);
		buttonCharacterSkill3.onClick.AddListener (ToggleCharacterSkillList);
	}

	void ToggleCharacterSkillList () {
		_isShowing = !_isShowing;
		characterSkillList.SetActive(_isShowing);
	}
}

