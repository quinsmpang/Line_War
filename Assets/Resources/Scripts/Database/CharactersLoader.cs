using UnityEngine;
using System.Collections;

public class CharactersLoader : MonoBehaviour {
	public string[] characters;
	public static CharactersLoader instance;

	// Use this for initialization
	IEnumerator Start () {
		instance = this;
		WWW charactersData = new WWW("http://ivocunha.com/linewar/db_getcharacters.php");
		yield return charactersData;
		string charactersDataString = charactersData.text;
		print ("CharactersLoader: " + charactersDataString);
		characters = charactersDataString.Split (';');
		//Debug.LogWarning (GetDataValue(characters[1], "Name: "));
		Debug.LogWarning ("Total characters: " + characters.Length);
		for (int x = 0; x< characters.Length-1; x++) {
			Debug.LogWarning ("Character from database: " + GetDataValue(characters[x], "Name: "));
//			if (GameObject.Find ("MenuController")) {
//				Debug.LogWarning("Found the character button named: " + GetDataValue(characters[x], "Name: "));
//			}
			//CharacterPicker.instance.characters[0] = GetDataValue(characters[x], "Name: ");
			//if (GameObject.Find ("Canvas/MainPanel/PanelCharacters/"+GetDataValue(characters[x], "Name: ")))
		}
	}

	string GetDataValue(string data, string index) {
		string value = data.Substring (data.IndexOf(index) + index.Length);
		if (value.Contains("|")) {
			value = value.Remove (value.IndexOf("|"));
		}
		return value;
	}
}