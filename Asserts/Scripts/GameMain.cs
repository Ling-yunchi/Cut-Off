using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class GameMain : MonoBehaviour
{
    public GameObject player;
    public GameObject EnemyPrefab;
    public float randomRange = 10f;
    public float freshTime = 1f;
    public GameObject pauseCanvas;
    private bool _canvasVisible = false;
    private int _nowAliveEnemyCount = 0;
    public int maxAliveEnemyCount = 5;

    void Start()
    {
        pauseCanvas.SetActive(_canvasVisible);
        InvokeRepeating(nameof(SpawnEnemy), freshTime, freshTime);
    }

    void Update()
    {
        if (OVRInput.GetDown(OVRInput.Button.Start))
        {
            _canvasVisible = !_canvasVisible;
        }
        pauseCanvas.SetActive(_canvasVisible);
    }

    public void Continue()
    {
        _canvasVisible = false;
    }

    public void Exit()
    {
        _canvasVisible = false;
        SceneManager.LoadScene("Start");
    }

    public void SpawnEnemy()
    {
        if (_nowAliveEnemyCount >= maxAliveEnemyCount)
        {
            return;
        }

        Vector3 randomPos = new Vector3(Random.Range(-randomRange, randomRange), 1,
            Random.Range(-randomRange, randomRange));
        var enemy = Instantiate(EnemyPrefab, randomPos, Quaternion.identity);
        enemy.GetComponent<EnemyController>().target = player.transform;
        enemy.GetComponent<CharacterSlicer>().OnDeadEvent += () => { _nowAliveEnemyCount--; };
        _nowAliveEnemyCount++;
    }
}