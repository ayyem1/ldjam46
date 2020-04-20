using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    public enum CollectibleState { Active, Collected }

    public CollectibleState state = CollectibleState.Active;

    [SerializeField]
    private MeshRenderer meshRenderer;

    [SerializeField]
    private ParticleSystem particles;

    const string alphaThresholdName = "Vector1_49FF6D3F";
    private float fillRate = 3.0f;

    private void Start()
    {
        this.meshRenderer.material.SetFloat(alphaThresholdName, 1.0f);
    }

    public void Update()
    {
        switch (this.state)
        {
            case CollectibleState.Active:

                break;
            case CollectibleState.Collected:

                break;
            default:
                Debug.LogError("Invalid Collectible state");
                break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (this.state == CollectibleState.Active && other.tag == "Player")
        {
            StartCoroutine(this.ExecuteCollectionEffects());
            this.CollectibleObtained(other.gameObject);
        }
    }

    private void CollectibleObtained(GameObject playerObject)
    {
        this.state = CollectibleState.Collected;
        //Do animation stuff here
        PlayerManager player = playerObject.GetComponent<PlayerManager>();

        if (player != null)
        {
            player.CollectibleObtained();
        }
    }

    private IEnumerator ExecuteCollectionEffects()
    {
        this.particles.Play();

        while (this.meshRenderer.material.GetFloat(alphaThresholdName) > 0.0f)
        {
            float currentFloat = this.meshRenderer.material.GetFloat(alphaThresholdName);
            this.meshRenderer.material.SetFloat(alphaThresholdName, currentFloat - (Time.deltaTime * this.fillRate));
            yield return null;
        }
    }
}
