using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;
using Photon.Pun;
using Photon.Realtime;


public class Gameplay : MonoBehaviourPunCallbacks
{
    public float spawnTime = 1.0f;
    private float startingDifficulty;
    public PathCreator path;
    public PathCreator leftPath;
    public PathCreator rightPath;
    public PathCreator path2;
    public PathCreator leftPath2;
    public PathCreator rightPath2;
    public EndOfPathInstruction end;
    private float difficulty;
    private GameObject a;
    private float currentTime;
    private float previousTime;
    private NetworkVariablesAndReferences networkVar;
    private float deterrentChance = 15.0f;

    public override void OnEnable()
    {
        base.OnEnable();
        networkVar = GameObject.Find("Network Interaction Statuses").GetComponent<NetworkVariablesAndReferences>();
        Debug.Log("Starting Coroutine to spawn objects");
        previousTime = currentTime;
        switch(MainMenu.difficulty) 
        {
            case Difficulty.Easy:
                startingDifficulty = 1.75f;
                spawnTime = 3.5f;
                break;
            case Difficulty.Medium:
                startingDifficulty = 2.75f;
                spawnTime = 2.85f;
                break;
            case Difficulty.Hard:
                startingDifficulty = 4f;
                spawnTime = 2f;
                break;
        }

        // determine send time and send settings on master only
        if(PhotonNetwork.IsMasterClient)
        {
            StartCoroutine(collectableWave());
        }
    }

    IEnumerator collectableWave() {
        while(!networkVar.isGameOver) {
            currentTime = Time.time;
            float deltaTime = currentTime - previousTime;
            difficulty = startingDifficulty + (deltaTime / 2700);
            if (difficulty > 8.0f) {
                difficulty = 8.0f;
            }
            spawnTime = spawnTime - (deltaTime / 3000) ;
            if (spawnTime < 0.25f) {
                spawnTime = 0.25f;
            }
            print("Difficulty: " + difficulty);
            print("Spawn Time: " + spawnTime);
            yield return new WaitForSeconds(spawnTime);
            int deterrentRoll = Random.Range(0, 100);
            int chosenPath = Random.Range(0, 3);
            photonView.RPC("spawnCollectable", RpcTarget.All, deterrentRoll, chosenPath);
        }
    }

    [PunRPC]
    private void spawnCollectable(int deterrentRoll, int chosenPath) {
		if (GameplayManager.gameIsOver) {
			return;
		}
		
        if (deterrentRoll < (int)deterrentChance) {
            a = PhotonNetwork.Instantiate("Deterrent_Bomb", transform.position, new Quaternion(-90,0,0,0)) as GameObject;
            a.tag = "Deterrent";
        }
        else {
            a = PhotonNetwork.Instantiate("Collectable", transform.position, new Quaternion(-90,0,0,0)) as GameObject;
            a.tag = "Collectable";
        }
        // Since object is spawned using PhotonNetwork.Instantiate, let photon handle viewID assignment
        
        var script = a.GetComponent<PathFollower>();
        script.speed = difficulty;
        var script2 = a.GetComponent<CollectableBehavior>();
        
        script.endOfPathInstruction = end;
        
        // master uses path left, path, right.
        // client uses path left2, path2, right2
        if (PhotonNetwork.IsMasterClient)
        {
            // set this to 0 or 1for multiplayer
            script2.playerIndex = 0;
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
        else
        {
            // set this to 0 or 1for multiplayer
            script2.playerIndex = 1;
            if (chosenPath == 0) {
                script.pathCreator = leftPath2;
            }
            else if (chosenPath == 1) {
                script.pathCreator = path2;
            }
            else if (chosenPath == 2) {
                script.pathCreator = rightPath2;
            }
        }
        a.SetActive(true);
    }

}
