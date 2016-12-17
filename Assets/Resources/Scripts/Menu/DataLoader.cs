using UnityEngine;
using System.Collections;

public class DataLoader : MonoBehaviour {
	public string[] players;

	// Use this for initialization
	IEnumerator Start () {
		WWW playerData = new WWW("http://localhost/linewar/player.php");
		yield return playerData;
		string playerDataString = playerData.text;
		print (playerDataString);
		players = playerDataString.Split (';');
		print (GetDataValue(players[1], "Name:"));
	}

	string GetDataValue(string data, string index) {
		string value = data.Substring (data.IndexOf(index) + index.Length);
		if (value.Contains("|")) {
			value = value.Remove (value.IndexOf("|"));
		}
		return value;
	}
}
