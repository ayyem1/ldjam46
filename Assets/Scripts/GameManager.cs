using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public FadeTransition fader;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        this.fader.FadeFromBlack();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
