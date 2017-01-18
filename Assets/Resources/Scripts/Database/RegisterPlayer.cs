using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class RegisterPlayer : MonoBehaviour {
	//string RegisterPlayerURL = "https://linewar.000webhostapp.com/db_addplayer.php";
	string RegisterPlayerURL = "http://ivocunha.com/linewar/db_addplayer.php";
//	public Button RegisterPlayerButton;

	public string inputPlayertype;
	public Text inputPlayername;
	public Text inputPlayerpassword;

	// Use this for initialization
	void Start () {
	
	}

	// Update is called once per frame
	void Update () {

	}

	public IEnumerator AddPlayer(string type, Text playername, Text password) {
		WWWForm form = new WWWForm ();
		form.AddField ("post_player_type", type);
		form.AddField ("post_player_name", playername.text);
		form.AddField ("post_player_password", password.text);

		WWW www = new WWW (RegisterPlayerURL, form);

		yield return www;

		print (www.text);

		if (www.text == "DATABASE: Player created ") {
			LoginPlayer.instance.LoginButton ();
//			LoginPlayer.LoginButton ();
		}

	}

	public void RegisterButton () {
		StartCoroutine (AddPlayer (inputPlayertype, inputPlayername, inputPlayerpassword));
		// print ("Player registered");
	}
}
