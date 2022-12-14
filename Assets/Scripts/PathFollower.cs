using UnityEngine;
using PathCreation;
using Photon.Pun;

// Moves along a path at constant speed.
// Depending on the end of path instruction, will either loop, reverse, or stop at the end of the path.
/// <summary>
/// This class moved the object along a path at a constant speed.
/// </summary>
public class PathFollower : MonoBehaviour
{
    /// <summary>
    /// Reference to the path creator object.
    /// </summary>
    public PathCreator pathCreator;
    /// <summary>
    /// Default behavior is to what happens when the object reaches the end of the path.
    /// </summary>
    public EndOfPathInstruction endOfPathInstruction;
    /// <summary>
    /// Default travel speed of the object.
    /// </summary>
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
            if (!gameObject.tag.Equals("Heart"))
            {
                transform.rotation = pathCreator.path.GetRotationAtDistance(distanceTravelled, endOfPathInstruction);
            }
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