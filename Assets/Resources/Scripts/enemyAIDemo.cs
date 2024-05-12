using UnityEngine;
using System.Collections;
using UnityEngine.AI;

public class enemyAIDemo : MonoBehaviour
{
    [HideInInspector] public PatrolStateDemo patrolState;
    [HideInInspector] public AlertStateDemo  alertState;
    [HideInInspector] public AttackStateDemo attackState;
    [SerializeField] private GameObject[] Collectibles;

    [HideInInspector] public IEnemyStateDemo  currentState;

    [HideInInspector] public NavMeshAgent navMeshAgent;
    public AudioSource audioSource;
    public AudioClip laser_gun;
    bool itemSpawned;


    public Light myLight;
    public float life = 100;
    public float timeBetweenShots = 1.0f;
    public float damageForce = 10;
    public float rotationTime = 3.0f;
    public float shotHeight = 0.5f;
    public Transform[] wayPoints;

    void Start()
    {
        // AI States.
        alertState  = new AlertStateDemo(this);
        patrolState = new PatrolStateDemo(this);
        attackState = new AttackStateDemo(this);
        audioSource = GetComponent<AudioSource>();
        currentState = patrolState;
        navMeshAgent = GetComponent<NavMeshAgent>();
        audioSource = GetComponent<AudioSource>();
    }
    
    void Update()
    {
    // Since our states don't inherit from
    // MonoBehaviour, its update is not called
    // automatically, and we'll take care of it
    // us to call it every frame.
        currentState.UpdateState();

        if(life <= 0)
        {
            Destroy(this.gameObject);
            if (!itemSpawned)
            {
                Spawn();
                itemSpawned = true;
            }
        }
    }

    public void Spawn()
    {
        Instantiate(Collectibles[Random.Range(0,2)],transform.position,Quaternion.identity);
    }

    // Since our states don't inherit from
    // MonoBehaviour, we'll have to let them know
    // when something enters, stays,  or leaves our
    // trigger.

    public void Hit(float damage)
    {
        life -= damage;
        currentState.Impact();
        Debug.Log("Enemy hit: " + life);
      
    }
    void OnTriggerEnter(Collider col)
    {
        currentState.OnTriggerEnter(col);
    }

    void OnTriggerStay(Collider col)
    {
        currentState.OnTriggerStay(col);
    }

    void OnTriggerExit(Collider col)
    {
        currentState.OnTriggerExit(col);
    }
}
