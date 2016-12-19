﻿using UnityEngine;
using System.Collections;

public class DataInserter : MonoBehaviour {
	string CreatePlayerURL = "http://localhost/linewar/addplayer.php";

	public string inputPlayertype;
	public string inputPlayername;
	public string inputPlayerpassword;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Space)) {
			CreateUser (inputPlayertype, inputPlayername, inputPlayerpassword);
			print ("user created");
		}
	}

	public void CreateUser(string type, string playername, string password) {
		WWWForm form = new WWWForm ();
		form.AddField ("post_player_type", type);
		form.AddField ("post_player_name", playername);
		form.AddField ("post_player_password", password);

		WWW www = new WWW (CreatePlayerURL, form);
	}
}
