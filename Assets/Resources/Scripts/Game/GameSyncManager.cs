using UnityEngine;
using System;
using TrueSync;

/**
* @brief Manages boxes instantiation.
**/
public class GameSyncManager : TrueSyncBehaviour {
    

    public static GameSyncManager Instance;

    /**
    * @brief Initial setup when game is started.
    **/
    public override void OnSyncedStart() {
        if (Instance == null) Instance = this;
    }

    public override void OnSyncedUpdate()
    {
        Player.AfterUpdate();
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