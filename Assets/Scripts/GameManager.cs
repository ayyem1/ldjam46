using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                GameManager[] instances = FindObjectsOfType<GameManager>();
                if (instances.Length != 1)
                {
                    throw new System.Exception(
                        $"Invalid number of GameManager instances in the current scene. Count={instances.Length}");
                }

                instance = instances[0];
            }

            return instance;
        }
    }

    private static GameManager instance;

    static GameManager() { }

    private GameManager() { }

    private void Awake()
    {
        // Do we need this?
        DontDestroyOnLoad(this);
    }
}
