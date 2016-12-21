using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DataInserter : MonoBehaviour {
	string RegisterPlayerURL = "http://localhost/linewar/addplayer.php";
	public Button RegisterPlayerButton;

	public string inputPlayertype;
	public Text inputPlayername;
	public Text inputPlayerpassword;

	// Use this for initialization
	void Start () {
	
	}

	// Update is called once per frame
	void Update () {

	}

	public void RegisterPlayer(string type, Text playername, Text password) {
		WWWForm form = new WWWForm ();
		form.AddField ("post_player_type", type);
		form.AddField ("post_player_name", playername.text);
		form.AddField ("post_player_password", password.text);

		WWW www = new WWW (RegisterPlayerURL, form);
	}

	public void RegisterButton () {
		RegisterPlayer (inputPlayertype, inputPlayername, inputPlayerpassword);
		//print ("Player created");
	}
}
