using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelTransitionTrigger : Interactable
{
    private PlayerController _player;
    private DungeonGenerator _dungeonGenerator;

    private void Awake()
    {
        _player = FindObjectOfType<PlayerController>();
        _dungeonGenerator = FindObjectOfType<DungeonGenerator>();
    }

    public override void OnInteract()
    {
        _player.transform.position = new Vector3(0, _player.transform.position.y, 0);
        _dungeonGenerator.GenerateDungeon();
    }
}
