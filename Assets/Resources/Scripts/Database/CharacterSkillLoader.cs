using UnityEngine;
using System.Collections;

public class CharacterSkillLoader : MonoBehaviour {
	public string[] characterSkills;

	// Use this for initialization
	IEnumerator Start () {
		WWW characterSkillsData = new WWW("http://ivocunha.com/linewar/db_getcharacterskills.php");
		yield return characterSkillsData;
		string characterSkillsDataString = characterSkillsData.text;
		print ("CharacterSkillLoader: " + characterSkillsDataString);
		characterSkills = characterSkillsDataString.Split (';');
//		print (GetDataValue(characters[1], "Name: "));
	}

	string GetDataValue(string data, string index) {
		string value = data.Substring (data.IndexOf(index) + index.Length);
		if (value.Contains("|")) {
			value = value.Remove (value.IndexOf("|"));
		}
		return value;
	}
}