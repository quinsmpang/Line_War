using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DataInserter : MonoBehaviour {
	string RegisterPlayerURL = "https://linewar.000webhostapp.com/db_addplayer.php";
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

	public IEnumerator RegisterPlayer(string type, Text playername, Text password) {
		WWWForm form = new WWWForm ();
		form.AddField ("post_player_type", type);
		form.AddField ("post_player_name", playername.text);
		form.AddField ("post_player_password", password.text);

		WWW www = new WWW (RegisterPlayerURL, form);

		yield return www;

		print (www.text);

	}

	public void RegisterButton () {
		StartCoroutine (RegisterPlayer (inputPlayertype, inputPlayername, inputPlayerpassword));
		// print ("Player registered");
	}
}
