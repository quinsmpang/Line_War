using UnityEngine;
using System.Collections;

public class PlayerConfig : MonoBehaviour {

    [Tooltip("Character Prefab used for players")]
    [SerializeField]
    private GameObject _characterPrefab;

    public GameObject CharacterPrefab
    {
        get { return _characterPrefab; }
    }

    [Tooltip("Character distance from the center of the world, where the lines intersect")]
    [SerializeField]
    private float _characterDistanceFromCenter;

    public float CharacterDistanceFromCenter
    {
        get { return _characterDistanceFromCenter; }
    }

    [Tooltip("Distance from center of screen to screen edge, used for sizing the highlighted player areas")]
    private static float _distanceFromCenterToScreenEdge = 50f;

    public float DistanceFromCenterToScreenEdge
    {
        get { return _distanceFromCenterToScreenEdge; }
        set { _distanceFromCenterToScreenEdge = value; }
    }

    public static PlayerConfig Instance;


    void Awake()
    {
        if (Instance == null)
            Instance = this;
    }


}
