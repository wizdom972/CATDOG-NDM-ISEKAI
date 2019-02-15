using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetrisGameManager : MonoBehaviour
{
    public GameObject[] blockPrefabs;

    public static int score = 0;

    // Start is called before the first frame update
    void Start()
    {
        makeNextBlock();
    }

    public void makeNextBlock()
    {
        int i = Random.Range(0, blockPrefabs.Length);
        Instantiate(blockPrefabs[i], transform.position, Quaternion.identity);
    }
}
