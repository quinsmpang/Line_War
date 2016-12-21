using UnityEngine;
using System;
using System.Collections;
using TrueSync;


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
    [SerializeField]
    private static float _distanceFromCenterToScreenEdge = 150f;

    public float DistanceFromCenterToScreenEdge
    {
        get { return _distanceFromCenterToScreenEdge; }
        set { _distanceFromCenterToScreenEdge = value; }
    }

    [Tooltip("Distance from center of tap point to location of force applied to reactive objects")]
    [SerializeField]
    private FP _tapEffectRadius;

    public FP TapEffectRadius
    {
        get { return _tapEffectRadius; }
        set { _tapEffectRadius = value; }
    }

    [Tooltip("Minimum tapping distance required to avoid penalty, must be less than TapEffectRadius")]
    [SerializeField]
    private FP _minTapDistance;

    public FP MinTapDistance
    {
        get { return _minTapDistance; }
        set { _minTapDistance = value; }
    }



    [Tooltip("Amount of force applied to object near tapping")]
    [SerializeField]
    private FP _tapEffectForce;

    public FP TapEffectForce
    {
        get { return _tapEffectForce; }
        set { _tapEffectForce = value; }
    }

    [Tooltip("Amount of force applied to line if tap is closer then MinTapDistance")]
    [SerializeField]
    private FP _penaltyEffectForce;

    public FP PenaltyEffectForce
    {
        get { return _penaltyEffectForce; }
        set { _penaltyEffectForce = value; }
    }


    [Tooltip("Friction applied to moving lines")]
    public LineMotionFriction LineFriction;

    [Serializable]
    public class LineMotionFriction
    {
        [SerializeField]
        private float _dragCoefficient = 0.15f;

        public FP GetVelocity(FP currentAngularVelocity)
        {
            FP result = 0;
            if (currentAngularVelocity > _dragCoefficient * TrueSyncManager.DeltaTime)
                result = currentAngularVelocity - _dragCoefficient * TrueSyncManager.DeltaTime;
            return result;
        }
    }

    [Tooltip("Materials used for player areas, position of material represents material used for same player's position. Game can only support a player count up to the number of items in this array.")]
    public Material[] PlayerAreaMaterials;



    public static PlayerConfig Instance;


    void Awake()
    {
        if (Instance == null)
            Instance = this;
    }


}
