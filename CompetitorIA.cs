using System.Collections;
using UnityEngine;

public class CompetitorIA : MonoBehaviour
{
    [Header("Velocidade")]
    [SerializeField] private float velocidadeMin = 6f;
    [SerializeField] private float velocidadeMax = 12f;
    [SerializeField] private float velocidadeBase = 9f;
    private float currentSpeed;

    [Header("Pulo")]
    [SerializeField] private float jumpForce = 12f;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float groundCheckRadius = 0.1f;
    
    private bool jumpRequested;
    private bool isGrounded;

    [Header("Movimento")]
    public Transform[] waypoints;
    private int currentWaypoint;
    
    public Transform playerTransform1;
    public Transform playerTransform2;
    private float currentDistance;
    [SerializeField] private float maxPlayerDistance;
    private Rigidbody2D rb;

    [Header("Interagir")]
    public Timer internalTimer;
    private SpawnsData currentSpawn;
    public SpawnController initialSpawn;
    public GameFinish gameFinish;

    public LayerMask spawnsLayer;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        
        if(currentSpawn == null)
        {
            currentSpawn = initialSpawn.spawnData;
        }

        if(gameFinish == null)
        {
            gameFinish = FindAnyObjectByType<GameFinish>();
        }
    }

    void Update()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
    }

    void FixedUpdate()
    {
        DefinirVelocidade();

        MovementToWaypoint();

        if(isGrounded)
            TryToJump();
    }

    public void DefinirVelocidade()
    {   
        if(playerTransform2 == null)
        {
            currentDistance = transform.position.x - playerTransform1.position.x;
        }
        else
        {
            currentDistance = transform.position.x - (playerTransform1.position.x + playerTransform2.position.x) / 2;
        }

        if(currentDistance >= maxPlayerDistance)
        {
            currentSpeed = velocidadeMin;
        }
        else if(currentDistance >= -maxPlayerDistance)
        {
            currentSpeed = velocidadeMax;
        }
        else
        {
            currentSpeed = velocidadeBase;
        }
    }

    public void MovementToWaypoint()
    {
        Transform target = waypoints[currentWaypoint];
        Vector2 direction = ((Vector2)target.position - rb.position).normalized;
        
        float VelocityX = direction.x * currentSpeed;

        rb.linearVelocity = new Vector2(VelocityX, rb.linearVelocity.y);

        if((transform.position.y - direction.y) > 1.5f)
        {
            jumpRequested = true;
        }
    }

    public void TryToJump()
    {
        if (!jumpRequested) return;

        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        jumpRequested = false;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Waypoint")
        {
            currentWaypoint = (currentWaypoint + 1) % waypoints.Length;
        }
        if(collision.tag == "SpawnPoint")
        {
            DefineSpawn();
        }
        else if(collision.tag == "DeathPoint")
        {
            DeathRoutine();
        }
        else if(collision.tag == "WinningPoint")
        {
            WinGame();
        }
    }

    public void DefineSpawn()
    {
        if(!internalTimer.isUnder3Min)
        {
            currentSpawn = initialSpawn.spawnData;

            return;
        }

        Collider2D spawnReached = Physics2D.OverlapCircle(transform.position, 2f, spawnsLayer);
        SpawnController spawn = spawnReached.GetComponent<SpawnController>();

        if(spawn.name != currentSpawn.name)
        {
            currentSpawn = spawn.spawnData;
            Debug.Log("Novo spawn alcançado: " + spawn.name);
        }
    }

    public void DeathRoutine()
    {
        transform.position = new Vector3(currentSpawn.positionX, currentSpawn.positionY, currentSpawn.positionZ);
    }

    public void WinGame()
    {
        gameFinish.ArmazenateInfos(internalTimer.currentTime, gameObject.name);
        StartCoroutine(VictoryRoutine());
    }

    IEnumerator VictoryRoutine()
    {
        yield return null;

        gameObject.SetActive(false);
    }
}
