using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGameSequence : MonoBehaviour
{
    [SerializeField]
    private GameObject endGameCameraObject;
    private bool showingEndGame = false;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (this.showingEndGame && Input.GetMouseButtonDown(0))
        {
            SceneManager.LoadScene("JPS");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            this.GoToEndGameState(other.gameObject);
        }
    }

    private void GoToEndGameState(GameObject playerObject)
    {
        this.endGameCameraObject.SetActive(true);
        this.showingEndGame = true;
    }
}
