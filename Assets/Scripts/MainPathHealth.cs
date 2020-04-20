using System.Collections;
using UnityEngine;

public class MainPathHealth : MonoBehaviour
{
    [SerializeField] private GameObject playerObject = null;
    // If player's health is greater than this, we won't grant.
    [SerializeField] private float grantCutoff = 20f;
    [SerializeField] private float targetHealth = 50f;

    private PlayerManager player;
    private Bounds grantObjectBounds;

    private IEnumerator grantCoroutine = null;

    private bool hasPlayerLeftMainPath = false;

    private void Start()
    {
        player = playerObject.GetComponent<PlayerManager>();

        MeshRenderer renderer = GetComponent<MeshRenderer>();
        grantObjectBounds = renderer.bounds;

        // If player starts on main path, they haven't left.
        if (IsObjectInPullSource())
        {
            hasPlayerLeftMainPath = false;
        }
        else
        {
            // Otherwise, they are already outside.
            hasPlayerLeftMainPath = true;
        }
    }


    private void Update()
    {
        if (IsObjectInPullSource())
        {
            player.PauseHealthDecay();
            hasPlayerLeftMainPath = false;
        }
        else if (!hasPlayerLeftMainPath)
        {
            player.UnpauseHealthDecay();
        }

        if (grantCoroutine == null)
        {
            float currentHealth = player.GetPlayerHealth();
            if (currentHealth < grantCutoff && IsObjectInPullSource())
            {
                grantCoroutine = player.IncreasePlayerHealth(targetHealth, () =>
                {
                    grantCoroutine = null;
                });
                StartCoroutine(grantCoroutine);
            }
        }

    }

    private bool IsObjectInPullSource()
    {
        float objectToPullX = playerObject.transform.position.x;
        bool isGreaterThanMin = objectToPullX > grantObjectBounds.min.x;
        bool isLessThanMax = objectToPullX < grantObjectBounds.max.x;
        return isGreaterThanMin && isLessThanMax;
    }
}
