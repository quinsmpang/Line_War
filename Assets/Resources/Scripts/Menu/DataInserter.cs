using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DataInserter : MonoBehaviour {
	string CreatePlayerURL = "http://localhost/linewar/addplayer.php";
	public Button CreatePlayerButton;

	public string inputPlayertype;
	public Text inputPlayername;
	public Text inputPlayerpassword;

	// Use this for initialization
	void Start () {
	
	}

	// Update is called once per frame
	void Update () {

	}

	public void CreatePlayer(string type, Text playername, Text password) {
		WWWForm form = new WWWForm ();
		form.AddField ("post_player_type", type);
		form.AddField ("post_player_name", playername.text);
		form.AddField ("post_player_password", password.text);

		WWW www = new WWW (CreatePlayerURL, form);
	}

	public void CreateButton () {
		CreatePlayer (inputPlayertype, inputPlayername, inputPlayerpassword);
		print ("user created");
	}
}
