﻿using UnityEngine;
using System.Collections;

public class DataLoader : MonoBehaviour {
	public string[] characters;

	// Use this for initialization
	IEnumerator Start () {
		WWW charactersData = new WWW("http://localhost/linewar/characters.php");
		yield return charactersData;
		string charactersDataString = charactersData.text;
		print (charactersDataString);
		characters = charactersDataString.Split (';');
		print (GetDataValue(characters[0], "Name:"));
	}

	string GetDataValue(string data, string index) {
		string value = data.Substring (data.IndexOf(index) + index.Length);
		if (value.Contains("|")) {
			value = value.Remove (value.IndexOf("|"));
		}
		return value;
	}
}
