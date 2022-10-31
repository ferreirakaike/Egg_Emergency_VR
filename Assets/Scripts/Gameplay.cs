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
    public ParticleSystem expl;

    private float difficulty;
    private GameObject a;
    private float currentTime;
    private float previousTime;

    void OnEnable()
    {
        Debug.Log("Starting Coroutine to spawn objects");
        previousTime = currentTime;
        StartCoroutine(collectableWave());
        switch(MainMenu.difficulty) 
        {
            case Difficulty.Easy:
                startingDifficulty = 1.5f;
                spawnTime = 2.5f;
                break;
            case Difficulty.Medium:
                startingDifficulty = 2.75f;
                spawnTime = 2.25f;
                break;
            case Difficulty.Hard:
                startingDifficulty = 4f;
                spawnTime = 2f;
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
        a.SetActive(true);

        var script = a.GetComponent<PathFollower>();
        script.speed = difficulty;
        var script2 = a.GetComponent<CollectableBehavior>();
        script2.explosion = expl;
        
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
            currentTime = Time.time;
            float deltaTime = currentTime - previousTime;
            difficulty = startingDifficulty + (deltaTime / 1700);
            if (difficulty > 8.0f) {
                difficulty = 8.0f;
            }
            spawnTime = spawnTime - (deltaTime / 2000) ;
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
