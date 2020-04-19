using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class PlayerManager : MonoBehaviour
{
    [SerializeField, Range(0f, 100f)]
    private float maxPlayerHealth = 100f;
    [SerializeField, Range(0f, 100f)]
    private float playerHealth;
    [SerializeField, Range(0f, 100f)] 
    private float healthDecayRate = 0.1f;
    [SerializeField, Range(0f, 100f)]
    private float healthGrowthRate = 0.5f; //Note: This must be bigger than healthDecayRate in order to exit the coroutine

    private float collectibleObtainHealthBoost = 50f;

    public float maxScale = 3f;
    [SerializeField]
    private Transform playerGroundTransform = null;
    

    // Start is called before the first frame update
    void Start()
    {
        Assert.IsTrue(healthGrowthRate > healthDecayRate);
        this.playerHealth = maxPlayerHealth;
        this.UpdateGroundTransform();
    }

    private void UpdateGroundTransform()
    {
        float scaleSize = (this.playerHealth / this.maxPlayerHealth) * this.maxScale;
        this.playerGroundTransform.localScale = new Vector3(scaleSize, this.playerGroundTransform.localScale.y, scaleSize);
    }

    private void Update()
    {
        this.playerHealth -= this.healthDecayRate;
        if (this.playerHealth < 0f)
        {
            this.playerHealth = 0f;
        }
        this.UpdateGroundTransform();
        if (this.playerHealth <= 0f)
        {
            Debug.LogError("GAME OVER YEEAAAHH");
        }
    }

    public void CollectibleObtained()
    {
        StartCoroutine(this.IncreasePlayerHealth(this.playerHealth + this.collectibleObtainHealthBoost));         
    }

    public IEnumerator IncreasePlayerHealth(float targetHealth)
    {
        while (this.playerHealth < targetHealth)
        {
            this.playerHealth += this.healthGrowthRate;
            if (this.playerHealth > this.maxPlayerHealth)
            {
                this.playerHealth = this.maxPlayerHealth;
            }

            this.UpdateGroundTransform();
            yield return null;
        }

        
    }
}
