using UnityEngine;
using System.Collections;

public class SkillLoader : MonoBehaviour {
	public string[] skillsList;

	// Use this for initialization
	IEnumerator Start () {
		WWW skillsData = new WWW("http://ivocunha.com/linewar/db_getskills.php");
		yield return skillsData;
		string skillsDataString = skillsData.text;
		print ("PlayerSkillLoader: " + skillsDataString);
		skillsList = skillsDataString.Split (';');
		//print (GetDataValue(skillsList[1], "Name: "));
	}

	string GetDataValue(string data, string index) {
		string value = data.Substring (data.IndexOf(index) + index.Length);
		if (value.Contains("|")) {
			value = value.Remove (value.IndexOf("|"));
		}
		return value;
	}
}
