using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;


public class Gameplay : MonoBehaviour
{
    public GameObject collectablePrefab;
    public GameObject deterrentPrefab;
    public float spawnTime = 1.0f;
    public float startingDifficulty = 2.0f;
    public PathCreator path;
    public PathCreator leftPath;
    public PathCreator rightPath;
    public EndOfPathInstruction end;

    private float difficulty;
    private GameObject a;

    void OnEnable()
    {
        Debug.Log("Starting Coroutine to spawn objects");
        StartCoroutine(collectableWave());
        switch(MainMenu.difficulty) 
        {
            case Difficulty.Easy:
                startingDifficulty = 1.5f;
                spawnTime = 2.5f;
                break;
            case Difficulty.Medium:
                startingDifficulty = 3.5f;
                spawnTime = 1.75f;
                break;
            case Difficulty.Hard:
                startingDifficulty = 5.5f;
                spawnTime = 1.25f;
                break;
        }
    }

    private void spawnCollectable() {
		if (GameplayManager.gameIsOver) {
			return;
		}
		
        float deterrentChance = 15.0f;
        int deterrentRoll = Random.Range(0, 100);
        if (deterrentRoll < (int)deterrentChance) {
            a = Instantiate(deterrentPrefab) as GameObject;
            a.tag = "Deterrent";
        }
        else {
            a = Instantiate(collectablePrefab) as GameObject;
            a.tag = "Collectable";
        }


        
        var script = a.GetComponent<PathFollower>();
        script.speed = difficulty;
        
        int chosenPath = Random.Range(0, 3);

        script.endOfPathInstruction = end;
        if (chosenPath == 0) {
            script.pathCreator = leftPath;
        }
        else if (chosenPath == 1) {
            script.pathCreator = path;
        }
        else if (chosenPath == 2) {
            script.pathCreator = rightPath;
        }
    }

    IEnumerator collectableWave() {
        while(true) {
            difficulty = startingDifficulty + Time.time / 60;
            if (difficulty > 8.0f) {
                difficulty = 8.0f;
            }
            spawnTime = spawnTime - (Time.time / 60) ;
            if (spawnTime < 0.25f) {
                spawnTime = 0.25f;
            }
            print("Difficulty: " + difficulty);
            print("Spawn Time: " + spawnTime);
            yield return new WaitForSeconds(spawnTime);
            spawnCollectable();
        }
    }

}
