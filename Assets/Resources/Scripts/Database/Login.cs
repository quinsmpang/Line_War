using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Login : MonoBehaviour {
	string LoginURL = "https://linewar.000webhostapp.com/db_login.php";
	public Button LoginPlayerButton;

	public Text inputPlayername;
	public Text inputPlayerpassword;

//	public bool loggedin = false;

	// Use this for initialization
	void Start () {
		
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
	}

	public void LoginButton () {
		StartCoroutine (LoginToDB (inputPlayername, inputPlayerpassword));
	//	print ("Player Logged in");
	}
}
