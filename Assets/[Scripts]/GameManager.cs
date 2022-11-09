using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    
    public GameState gameState;

    public static event Action<GameState> OnGameStateChanged;

    void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        UpdateGameState(GameState.PlayerTurn);
    }

    public void UpdateGameState(GameState newState)
    {
        switch(newState)
        {
            case GameState.WaitingForOpponent:
                break;
            case GameState.PlayerTurn:
                HandlePlayerTurn();
                break;
            case GameState.WaitTurn:
                break;
            case GameState.Victory:
                break;
            case GameState.Lose:
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
        }
        OnGameStateChanged?.Invoke(newState);
    }

    private void HandlePlayerTurn()
    {
        throw new NotImplementedException();
    }
}

public enum GameState
{
    WaitingForOpponent,
    PlayerTurn,
    WaitTurn,
    Victory,
    Lose
}
