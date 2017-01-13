using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerLoader : MonoBehaviour {
	public string[] currentPlayer;
	public string currentPlayerID;
	public string currentPlayerType;
	public string currentPlayerName;
	public string currentPlayerPassword;
	public string currentPlayerPlayerIcon;
	public string currentPlayerCurrency;

	public Sprite[] playerIconsList;
	public Image playerIcon;
	public Text coinsText;

	// Use this for initialization
	IEnumerator Start () {
		// WWW charactersData = new WWW("https://linewar.000webhostapp.com/db_characters.php");
		WWW playerData = new WWW("http://ivocunha.com/linewar/db_player.php");
		yield return playerData;
		string playerDataString = playerData.text;
		print ("PlayerLoader: " + playerDataString);
		currentPlayer = playerDataString.Split (';');
		currentPlayerID = GetDataValue(currentPlayer[0], "ID: ");
		currentPlayerType = GetDataValue(currentPlayer[0], "Type: ");
		currentPlayerName = GetDataValue(currentPlayer[0], "Name: ");
		currentPlayerPassword = GetDataValue(currentPlayer[0], "Password: ");
		currentPlayerPlayerIcon = GetDataValue(currentPlayer[0], "PlayerIcon: ");
		currentPlayerCurrency = GetDataValue(currentPlayer[0], "Currency: ");

		playerIcon.overrideSprite = playerIconsList[int.Parse(currentPlayerPlayerIcon)];
		coinsText.text = (currentPlayerCurrency);

		print ("Current Player ID: " + currentPlayerID);
		print ("Current Player Type: " + currentPlayerType);
		print ("Current Player Name: " + currentPlayerName);
		print ("Current Player Password: " + currentPlayerPassword);
		print ("Current Player PlayerIcon: " + currentPlayerPlayerIcon);
		print ("Current Player Currency: " + currentPlayerCurrency);
	}

	string GetDataValue(string data, string index) {
		string value = data.Substring (data.IndexOf(index) + index.Length);
		if (value.Contains("|")) {
			value = value.Remove (value.IndexOf("|"));
		}
		return value;
	}
}
