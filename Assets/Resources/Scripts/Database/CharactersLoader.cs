using UnityEngine;
using System.Collections;

public class CharactersLoader : MonoBehaviour {
	public string[] characters;

	// Use this for initialization
	IEnumerator Start () {
		// WWW charactersData = new WWW("https://linewar.000webhostapp.com/db_characters.php");
		WWW charactersData = new WWW("http://ivocunha.com/linewar/db_getcharacters.php");
		yield return charactersData;
		string charactersDataString = charactersData.text;
		print ("CharactersLoader: " + charactersDataString);
		characters = charactersDataString.Split (';');
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
