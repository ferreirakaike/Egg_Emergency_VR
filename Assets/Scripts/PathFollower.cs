using UnityEngine;
using PathCreation;
using Photon.Pun;

// Moves along a path at constant speed.
// Depending on the end of path instruction, will either loop, reverse, or stop at the end of the path.
public class PathFollower : MonoBehaviour
{
    public PathCreator pathCreator;
    public EndOfPathInstruction endOfPathInstruction;
    public float speed = 5;
    float distanceTravelled;
    private NetworkVariablesAndReferences networkVar;

    void Start()
    {
        networkVar = GameObject.Find("Network Interaction Statuses").GetComponent<NetworkVariablesAndReferences>();
    }

    void Update()
    {
        if (pathCreator != null && !networkVar.isGameOver)
        {
            distanceTravelled += speed * Time.deltaTime;
            transform.position = pathCreator.path.GetPointAtDistance(distanceTravelled, endOfPathInstruction);
            transform.rotation = pathCreator.path.GetRotationAtDistance(distanceTravelled, endOfPathInstruction);
        }
        else if (networkVar.isGameOver)
        {
            // Master / Client destroy their own object
            if (this.gameObject.GetComponent<CollectableBehavior>().playerIndex == 1 && !PhotonNetwork.IsMasterClient)
            {
                PhotonNetwork.Destroy(this.gameObject);
            }
            else if (this.gameObject.GetComponent<CollectableBehavior>().playerIndex == 0 && PhotonNetwork.IsMasterClient)
            {
                PhotonNetwork.Destroy(this.gameObject);
            }
        }
    }
}