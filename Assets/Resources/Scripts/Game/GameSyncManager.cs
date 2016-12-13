using UnityEngine;
using TrueSync;

/**
* @brief Manages boxes instantiation.
**/
public class GameSyncManager : TrueSyncBehaviour {
    
    [Tooltip("Distance from center of tap point to location of force applied to reactive objects")]
    [SerializeField]
    private FP _tapEffectRadius = new FP(3);

    public FP TapEffectRadius
    {
        get { return _tapEffectRadius; }
        set { _tapEffectRadius = value; }
    }


    [Tooltip("Amount of force applied to object near tapping")]
    [SerializeField]
    private FP _tapEffectForce = new FP(5);

    public FP TapEffectForce
    {
        get { return _tapEffectForce; }
        set { _tapEffectForce = value; }
    }


    //[Tooltip("Players spawn this distance from center")]
    //[SerializeField]
    //private float _playerSpawnDistanceFromCenter = 7.5f;

    //[Tooltip("Materials used to display local player's highlight area")]
    //[SerializeField]
    //private Material _localPlayerHighlightMaterial;

    //[Tooltip("Material used to display all other player's highlight areas")]
    //[SerializeField]
    //private Material _otherPlayerHighlightMaterial;

    public static GameSyncManager Instance;

    /**
    * @brief Initial setup when game is started.
    **/
    public override void OnSyncedStart() {
        if (Instance == null) Instance = this;
    }
    
    /**
    * @brief Logs a text when game is paused.
    **/
    public override void OnGamePaused() {
        Debug.Log("Game Paused");
    }

    /**
    * @brief Logs a text when game is unpaused.
    **/
    public override void OnGameUnPaused() {
        Debug.Log("Game UnPaused");
    }

    /**
    * @brief Logs a text when game is ended.
    **/
    public override void OnGameEnded() {
        Debug.Log("Game Ended");
    }

    /**
    * @brief When a player get disconnected all objects belonging to him are destroyed.
    **/
    public override void OnPlayerDisconnection(int playerId) {
        TrueSyncManager.RemovePlayer(playerId);
    }

}