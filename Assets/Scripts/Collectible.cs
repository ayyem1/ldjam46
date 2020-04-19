using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    public enum CollectibleState { Active, Collected }

    public CollectibleState state = CollectibleState.Active;
    [SerializeField]
    private MeshRenderer mesh;
    [SerializeField]
    private Material activeMaterial;
    [SerializeField]
    private Material collectedMaterial;

    public void Update()
    {
        switch (this.state)
        {
            case CollectibleState.Active:
                if (this.mesh.material != this.activeMaterial)
                {
                    this.mesh.material = this.activeMaterial;
                }
                break;
            case CollectibleState.Collected:
                if (this.mesh.material != this.collectedMaterial)
                {
                    this.mesh.material = this.collectedMaterial;
                }
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
}
