using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Currency : MonoBehaviour {
	string AddCurrencyURL = "http://ivocunha.com/linewar/db_addcurrency.php";
	public Text currentPlayername;
	public Text coins;
	private int _tempCoins;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public IEnumerator AddCurrency(Text playername, Text playercurrency) {
		_tempCoins = int.Parse(coins.text);
		_tempCoins += 10;
		coins.text = _tempCoins.ToString();


		WWWForm form = new WWWForm ();
		form.AddField ("post_player_name", playername.text);
		form.AddField ("post_player_currency", playercurrency.text);
		//form.AddField ("post_player_currency", _tempCoins);

		WWW www = new WWW (AddCurrencyURL, form.data);
		yield return www;
		print (www.text);

		if (www.text == "DATABASE: Currency added ") {
			print ("TRANSACTION: Current Coins: " + _tempCoins);
			//PlayerLoader.instance.GetDataValue (currentPlayername.text, "Currency: ");
		}
	}

	public void AddCoins() {
//		_tempCoins = int.Parse(coins.text);
//		_tempCoins += 10;
//		coins.text = _tempCoins.ToString();
//		print ("TRANSACTION: Current Coins: " + _tempCoins);
		StartCoroutine (AddCurrency (currentPlayername, coins));
		//Debug.LogWarning ("coins being added: " + _tempCoins.ToString());
	}
}