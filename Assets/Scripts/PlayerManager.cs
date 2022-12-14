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

    private float collectibleObtainHealthBoost = 25f;    
    // Health decay is currently paused when a player
    // is in a waypoint or when they have lost all
    // of their health and are being dragged back
    // to the path.
    public bool IsHealthDecayPaused { get; private set; }
    private IEnumerator displayHealthDepletedEffect = null;

    public float maxScale = 3f;

    [SerializeField]
    AudioSource dyingSound;

    private void Awake()
    {
        Waypoint.OnWaypointEntered += PauseHealthDecay;
        Waypoint.OnWayPointExited += UnpauseHealthDecay;

        EndGameSequence.OnEndGameSequenceStarted += PauseHealthDecay;
    }

    public void PauseHealthDecay()
    {
        IsHealthDecayPaused = true;
    }

    public void UnpauseHealthDecay()
    {
        IsHealthDecayPaused = false;
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
        bool isDepletedToZero = this.playerHealth > 0f && this.playerHealth - this.healthDecayRate <= 0f;
        if (IsHealthDecayPaused == false)
        {
            this.playerHealth -= this.healthDecayRate;
        }

        if (this.playerHealth < 0f)
        {
            this.playerHealth = 0f;
        }
        this.UpdateGroundTransform();
        if (isDepletedToZero)
        {
            // Player's health just became depleted.
            IsHealthDecayPaused = true;
            OnPlayerHealthDepleted?.Invoke();
            this.displayHealthDepletedEffect = DisplayHealthDepletedEffects();
            StartCoroutine(this.displayHealthDepletedEffect);
        }

        if (!isDepletedToZero && this.GetPlayerHealth() < (this.maxPlayerHealth / 3))
        {
            if (!this.dyingSound.isPlaying)
            {
                this.dyingSound.Play();
            }
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
        if (targetHealth > this.maxPlayerHealth)
        {
            targetHealth = this.maxPlayerHealth;
        }

        if (playerHealth <= 0f && targetHealth >= 0f)
        {
            IsHealthDecayPaused = false;
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
                this.UpdateGroundTransform();
                break;
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

    private void OnDestroy()
    {
        Waypoint.OnWaypointEntered -= PauseHealthDecay;
        Waypoint.OnWayPointExited -= UnpauseHealthDecay;

        EndGameSequence.OnEndGameSequenceStarted -= PauseHealthDecay;
    }
}
