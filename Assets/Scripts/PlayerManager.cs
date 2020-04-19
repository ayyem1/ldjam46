using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [SerializeField, Range(0f, 100f)]
    private float maxPlayerHealth = 100f;
    [SerializeField, Range(0f, 100f)]
    private float playerHealth;
    private float healthDecayRate = 0.1f;
    private float healthGrowRate = 0.5f; //Note: This must be bigger than healthDecayRate in order to exit the coroutine

    private float collectibleObtainHealthBoost = 50f;

    private float maxLightIntensity = 10f;
    [SerializeField]
    private Light playerLight;
    

    // Start is called before the first frame update
    void Start()
    {
        this.playerHealth = maxPlayerHealth;
        this.UpdateLightIntensity();
    }

    private void UpdateLightIntensity()
    {
        this.playerLight.intensity = (this.playerHealth / this.maxPlayerHealth) * this.maxLightIntensity;
    }

    private void FixedUpdate()
    {
        this.playerHealth -= this.healthDecayRate;
        if (this.playerHealth < 0)
        {
            this.playerHealth = 0;
        }
        this.UpdateLightIntensity();
    }

    public void CollectibleObtained()
    {
        StartCoroutine(this.IncreasePlayerHealth(this.playerHealth + this.collectibleObtainHealthBoost));         
    }

    public IEnumerator IncreasePlayerHealth(float targetHealth)
    {
        while (this.playerHealth < targetHealth)
        {
            this.playerHealth += this.healthGrowRate;
            this.UpdateLightIntensity();
            yield return null;
        }

        if (this.playerHealth > this.maxPlayerHealth)
        {
            this.playerHealth = this.maxPlayerHealth;
        }
    }
}
