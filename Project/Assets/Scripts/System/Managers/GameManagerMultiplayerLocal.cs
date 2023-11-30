using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;

public class GameManagerMultiplayerLocal : GameManager
{
    [SerializeField] private GameObject camera1, camera2;
    [SerializeField] private PlayableDirector[] playable;
    private void Start()
    {
        TurnSystemManager.Instance.OnTurnChanged += ToggleCamera;
        base.Start();
    }

    protected override void StartGame()
    {
        gameMode = GameMode.multiplayerMode;
        Debug.Log(gameMode);
        isActive = true;
    }

    private void OnDisable()
    {
        TurnSystemManager.Instance.OnTurnChanged -= ToggleCamera;
    }

    private void ToggleCamera(object sender, EventArgs e)
    {
        if (TurnSystemManager.Instance.GetTeamTurn() == Team.Team1)
        {
            if (!camera1.activeInHierarchy)
            {
                playable[0].Play();
            }
        }
        else
        {
            if (!camera2.activeInHierarchy)
            {
                playable[1].Play();
            }
        }
    }

    protected override void Character_OnDead(object sender, EventArgs e)
    {
        Character character = (Character)sender;

        GridManager.Instance.ClearCharacterAtTilePosition(character.CharacterTilePosition);

        if (character.GetCharacterTeam() == Team.Team1)
        {
            _spawnManager.SpawnedMedievalTeam.Remove(character.gameObject);
        }
        else
        {
            _spawnManager.SpawnedFutureTeam.Remove(character.gameObject);
        }

        if (_spawnManager.SpawnedMedievalTeam.Count == 0 || _spawnManager.SpawnedFutureTeam.Count == 0)
        {
            if (_spawnManager.SpawnedMedievalTeam.Count > 0)
            {
                message = "COLOMBIAN Team Wins";
            }

            if (_spawnManager.SpawnedFutureTeam.Count > 0)
            {
                message = "SPANISH Team Wins";
            }
            
           StartCoroutine(EndMatch());
        }
    }
}


