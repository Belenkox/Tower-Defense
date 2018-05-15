using System;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Base Enemy class
/// </summary>
public abstract class Enemy : MonoBehaviour, IDamagable<Enemy>, ISelectable
{

    public HexTile currentlyOn { get; set; }                    //Tile this enemy is currently on
    public int Damage { get; set; }                             //Every enemy has damage?
    public GameObject bloodPrefab;
    //IDamagable implementation
    public event Action<Enemy> OnDeath;
    public abstract void TakeDamage(int amount);
    public int CurrentHealth { get; set; }

    //Enemy states
    public abstract void Idle();                                //Do nothing
    public abstract void Move(HexTile startTile);               //Just move along given path
    public abstract void Attack();

    protected IEnemyState currentState;
    public float Speed { get; protected set; }

    public Color SelectionColor { get { return Color.red; } }

    protected Healthbar healthBar;

    protected PlayEnemySFX soundEffectPlayer;

    protected void Awake()
    {
        GameManager.OnGameOver += Idle;
        healthBar = GetComponent<Healthbar>();
        soundEffectPlayer = GetComponent<PlayEnemySFX>();
        Idle();                                                 //Initial state is Idle
    }

    protected void Update()
    {
        if (currentState != null)
            currentState.UpdateState();
    }

    protected void Die()
    {
        PlayerStats.Instance.EnemyKilled();
        if (OnDeath != null)
            OnDeath(this);
        healthBar.RemoveHealthbar();
        soundEffectPlayer.Play(SoundType.EnemyDeath);
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        GameManager.OnGameOver -= Idle;
    }

    protected void GiveReward(int moneyReward)
    {
        PlayerStats.Instance.ChangeMoney(moneyReward);
    }
    
    public float startSpeed = 0.1f;
    public float freeze = 0;
    public float freezeDuration = 5;
    public float enemySpeed; 

    public void Freeze()
    {
        enemySpeed = Speed; // save start enemy speed
        Speed = startSpeed * freeze;
        Invoke("ResetFreeze", freezeDuration);
    }
    
    private void ResetFreeze()
    {
        Speed = enemySpeed;
    }
}