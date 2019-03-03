using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TetrisGameManager : MonoBehaviour
{
    public GameObject[] blockPrefabs;
    public GameObject gameOverScreen;
    public EventManager eventManager;
    public static int score = 0;
    public static int scoreModifier = 1;


    // Start is called before the first frame update
    void Start()
    {
        eventManager = GameObject.Find("EventManager").GetComponent<EventManager>();
        makeNextBlock();
    }

    public void makeNextBlock()
    {
        int i = Random.Range(0, blockPrefabs.Length);
        Instantiate(blockPrefabs[i], transform.position, Quaternion.identity);
    }


    private IEnumerator _StartGameClosingProcess()
    {
        scoreModifier = 2;
        eventManager.ExecuteOneScript();
        gameOverScreen.SetActive(true);
        yield return new WaitForSeconds(3f);
        eventManager.SetActiveEventSceneThings(true);
        SceneManager.SetActiveScene(eventManager.gameObject.scene);
        SceneManager.UnloadSceneAsync(gameObject.scene);
    }
}
