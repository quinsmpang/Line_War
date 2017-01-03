using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerLoader : MonoBehaviour {
	public string[] currentPlayer;

//	string RegisterPlayerURL = "http://ivocunha.com/db_addplayer.php";
////	public Button RegisterPlayerButton;
//
//	public string inputPlayertype;
//	public Text inputPlayername;
//	public Text inputPlayerpassword;

	// Use this for initialization
	IEnumerator Start () {
		// WWW charactersData = new WWW("https://linewar.000webhostapp.com/db_characters.php");
		WWW playerData = new WWW("http://ivocunha.com/db_player.php");
		yield return playerData;
		string playerDataString = playerData.text;
		print ("PlayerLoader: " + playerDataString);
		currentPlayer = playerDataString.Split (';');
//		print (GetDataValue(characters[1], "Name:"));
	}

//	public IEnumerator GetPlayer(string type, Text playername, Text password) {
//		WWWForm form = new WWWForm ();
//		form.AddField ("post_player_type", type);
//		form.AddField ("post_player_name", playername.text);
//		form.AddField ("post_player_password", password.text);
//
//		WWW www = new WWW (RegisterPlayerURL, form);
//
//		yield return www;
//
//		print (www.text);
//
//	}

	string GetDataValue(string data, string index) {
		string value = data.Substring (data.IndexOf(index) + index.Length);
		if (value.Contains("|")) {
			value = value.Remove (value.IndexOf("|"));
		}
		return value;
	}
}
