using UnityEngine;
using System.Collections;

public class PlayerSkillLoader : MonoBehaviour {
	public string[] playerSkills;

	// Use this for initialization
	IEnumerator Start () {
		// WWW charactersData = new WWW("https://linewar.000webhostapp.com/db_characters.php");
		WWW playerSkillsData = new WWW("http://ivocunha.com/linewar/db_playerskills.php");
		yield return playerSkillsData;
		string playerSkillsDataString = playerSkillsData.text;
		print ("PlayerSkillLoader: " + playerSkillsDataString);
		playerSkills = playerSkillsDataString.Split (';');
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
