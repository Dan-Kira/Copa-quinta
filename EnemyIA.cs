using UnityEngine;

public class EnemyIA : MonoBehaviour
{
    public enum NiveisDaIA
    {
        Nivel1, Nivel2, Nivel3, Nivel4, Nivel5
    }
    public NiveisDaIA NivelAtual;

    [Header("Decisão")]
    [SerializeField] private float mistakeChance;
    [SerializeField] private float decisionTime;

    [Header("Movimentação")]
    private Rigidbody2D rb;
    [SerializeField] private Transform actualObjective;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float aceleration;
    [SerializeField] private float deceleration;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Update()
    {
        MovimentarInimigo();
    }

    private void FixedUpdate()
    {
        switch (NivelAtual)
        {
            case NiveisDaIA.Nivel1 :
                mistakeChance = 0.75f;
                TomarDecisao(mistakeChance);
                break;
            case NiveisDaIA.Nivel2:

                break;
            case NiveisDaIA.Nivel3:

                break;
            case NiveisDaIA.Nivel4:

                break;
            case NiveisDaIA.Nivel5:

                break;
        }
    }

    public void MovimentarInimigo()
    {
        float distanceToTarget = Vector2.Distance(transform.position, actualObjective.position);

        if (distanceToTarget < 0.01f) 
        {
            rb.linearVelocity = new Vector2(rb.linearVelocityX * moveSpeed * Time.deltaTime, rb.linearVelocityY);
        }
    }

public void TomarDecisao(float mistakeChance)
    {

    }
}
