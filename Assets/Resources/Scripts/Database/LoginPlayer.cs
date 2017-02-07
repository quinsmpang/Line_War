using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LoginPlayer : MonoBehaviour {
	string LoginURL = "http://ivocunha.com/linewar/db_login.php";
	public Button LoginPlayerButton;

	public Text inputPlayername;
	public Text inputPlayerpassword;
	public Text coinsText;

	public static bool loggedin = false;
	public static LoginPlayer instance;
	public static LocalPlayer DBData;


	// Use this for initialization
	void Start () {
		Debug.Log ("loggedin is " + loggedin);
		instance = this;
		if (DBData == null)
		{
			DBData = new LocalPlayer ();
			DBData.coinsText = coinsText;
		}
	}

	IEnumerator LoginToDB (Text playername, Text playerpassword) {
		WWWForm form = new WWWForm ();

		form.AddField ("post_player_name", playername.text);
		form.AddField ("post_player_password", playerpassword.text);

		WWW www = new WWW (LoginURL, form);

		yield return www;

		print (www.text);

		if (www.text == "DATABASE: login successful") {
			loggedin = true;
			Debug.Log ("loggedin is " + loggedin);
			Menu.instance.MainPanel_SetLobby ("demo_boxes;Scenes/Game");
			StartCoroutine (DBData.LoadPlayerData(inputPlayername.text));
		} else {
			loggedin = false;
			Debug.Log ("loggedin is " + loggedin);
		}
	}

	public void LoginButton () {
		StartCoroutine (LoginToDB (inputPlayername, inputPlayerpassword));
	}


	public class LocalPlayer {
		public string[] currentPlayer;
		public string currentPlayerID;
		public string currentPlayerType;
		public string currentPlayerName;
		public string currentPlayerPassword;
		public string currentPlayerPlayerIcon;
		public string currentPlayerCurrency;

		//public Sprite[] playerIconsList;
		public Sprite playerIcon;
		public Text coinsText;


		public IEnumerator LoadPlayerData(string playerName) {
			WWWForm form = new WWWForm ();
			form.AddField ("post_player_name", playerName);
			WWW playerData = new WWW("http://ivocunha.com/linewar/db_getplayer.php", form.data);
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

			// FIXME: Not sure what this is.  Where does playerIconsList get assigned?
			//playerIcon = playerIconsList[int.Parse(currentPlayerPlayerIcon)];
			coinsText.text = (currentPlayerCurrency);

			print ("Current Player ID: " + currentPlayerID);
			print ("Current Player Type: " + currentPlayerType);
			print ("Current Player Name: " + currentPlayerName);
			print ("Current Player Password: " + currentPlayerPassword);
			print ("Current Player PlayerIcon: " + currentPlayerPlayerIcon);
			print ("Current Player Currency: " + currentPlayerCurrency);
		}

		private string GetDataValue(string data, string index) {
			string value = data.Substring (data.IndexOf(index) + index.Length);
			if (value.Contains("|")) {
				value = value.Remove (value.IndexOf("|"));
			}
			return value;
		}
	}
}
