using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LoginPlayer : MonoBehaviour {
	// string LoginURL = "https://linewar.000webhostapp.com/db_login.php";
	string LoginURL = "http://ivocunha.com/linewar/db_login.php";
	public Button LoginPlayerButton;

	public Text inputPlayername;
	public Text inputPlayerpassword;

	public static bool loggedin = false;
	public static LoginPlayer instance;

	// Use this for initialization
	void Start () {
		Debug.Log ("loggedin is " + loggedin);
		instance = this;
	}
	
	// Update is called once per frame
	void Update () {
		//if (Input.GetKeyDown(KeyCode.Space)) StartCoroutine(LoginToDB(inputPlayername, inputPlayerpassword));
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
		} else {
			loggedin = false;
			Debug.Log ("loggedin is " + loggedin);
		}
	}

	public void LoginButton () {
		StartCoroutine (LoginToDB (inputPlayername, inputPlayerpassword));
	//	print ("Player Logged in");
	}
}
