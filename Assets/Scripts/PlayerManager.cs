using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [SerializeField, Range(0f, 100f)]
    private float maxPlayerHealth = 100f;
    [SerializeField, Range(0f, 100f)]
    private float playerHealth;
    private float healthDecayRate = 0.01f;

    private float collectibleObtainHealthBoost = 10f;

    // Start is called before the first frame update
    void Start()
    {
        this.playerHealth = maxPlayerHealth;
    }

    private void FixedUpdate()
    {
        this.playerHealth -= this.healthDecayRate;
    }

    public void CollectibleObtained()
    {
        this.playerHealth += this.collectibleObtainHealthBoost;
        if (this.playerHealth > this.maxPlayerHealth)
        {
            this.playerHealth = this.maxPlayerHealth;
        }
    }
}
