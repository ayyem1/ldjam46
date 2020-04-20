using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Assertions;

public class PlayerManager : MonoBehaviour
{
    public static Action OnPlayerHealthDepleted;
    public static Action OnPlayerHealthRestoredFromEmpty;
    public static Action OnPlayerSpectating;

    [SerializeField, Range(0f, 100f)]
    private float maxPlayerHealth = 100f;
    [SerializeField, Range(0f, 100f)]
    private float playerHealth;
    [SerializeField, Range(0f, 100f)] 
    private float healthDecayRate = 0.1f;
    [SerializeField, Range(0f, 100f)]
    private float healthGrowthRate = 0.5f; //Note: This must be bigger than healthDecayRate in order to exit the coroutine
    [SerializeField]
    private Transform playerGroundTransform = null;

    private float collectibleObtainHealthBoost = 50f;    
    // Health decay is currently paused when a player
    // is in a waypoint or when they have lost all
    // of their health and are being dragged back
    // to the path.
    private bool isHealthDecayPaused = false;
    private IEnumerator displayHealthDepletedEffect = null;

    public float maxScale = 3f;

    private void Awake()
    {
        Waypoint.OnWaypointEntered += PauseHealthDecay;
        Waypoint.OnWayPointExited += UnpauseHealthDecay;
    }

    private void OnDestroy()
    {
        Waypoint.OnWaypointEntered -= PauseHealthDecay;
        Waypoint.OnWayPointExited -= UnpauseHealthDecay;
    }

    public void PauseHealthDecay()
    {
        isHealthDecayPaused = true;
    }

    public void UnpauseHealthDecay()
    {
        isHealthDecayPaused = false;
    }

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
        if (isHealthDecayPaused == false)
        {
            this.playerHealth -= this.healthDecayRate;
        }

        if (this.playerHealth < 0f)
        {
            this.playerHealth = 0f;
        }
        this.UpdateGroundTransform();
        if (this.playerHealth <= 0f && !isHealthDecayPaused)
        {
            // Player's health just became depleted.
            isHealthDecayPaused = true;
            OnPlayerHealthDepleted?.Invoke();
            this.displayHealthDepletedEffect = DisplayHealthDepletedEffects();
            StartCoroutine(this.displayHealthDepletedEffect);
        }
    }

    private IEnumerator DisplayHealthDepletedEffects()
    {
        // TODO: Replace this dummy code.
        yield return new WaitForSeconds(2.0f);
        OnPlayerSpectating?.Invoke();
    }

    public void CollectibleObtained()
    {
        StartCoroutine(this.IncreasePlayerHealth(this.playerHealth + this.collectibleObtainHealthBoost));         
    }


    public IEnumerator IncreasePlayerHealth(float targetHealth, Action callback = null)
    {
        if (playerHealth <= 0f && targetHealth >= 0f)
        {
            isHealthDecayPaused = false;
            // We should prevent this in our game design,
            // but if a player collects health while we are
            // playing the health depleted animation, we will
            // stop it.
            StopCoroutine(this.displayHealthDepletedEffect);
            OnPlayerHealthRestoredFromEmpty?.Invoke();
        }

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

        if (callback != null)
        {
            callback();
        }
    }

    public float GetPlayerHealth()
    {
        return this.playerHealth;
    }
}
