using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Currency : MonoBehaviour {
	public Text coins;
	private int _tempCoins;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void AddCoins() {
		_tempCoins = int.Parse(coins.text);
		_tempCoins += 10;
		coins.text = _tempCoins.ToString();
		print ("TRANSACTION: Current Coins: " + _tempCoins);
	}
}