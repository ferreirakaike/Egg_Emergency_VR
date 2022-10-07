using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableSpawner : MonoBehaviour
{
    public GameObject collectable;
    private float _timer = 0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        _timer++;

        if (_timer > 100f)
        {
            SpawnCollectable();
            _timer = 0;
        }
    }
    
    void SpawnCollectable()
    {
        int rand = Random.Range(-1, 1);
        Vector3 temp = this.transform.position;
        temp.x += rand;
        Instantiate(collectable, temp, this.transform.rotation);
    }
}
