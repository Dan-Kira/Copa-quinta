using System.Collections;
using UnityEngine;

public class InteractionsManager : MonoBehaviour
{
    public Timer internalTimer;
    public PlayerController player;
    public AdaptationControl adaptationControl;
    public SpawnController initialSpawn;
    public GameFinish gameFinish;

    public LayerMask spawnsLayer;

    public void Awake()
    {
        if(player.spawnPoint == null)
        {
            player.spawnPoint = initialSpawn.spawnData;
        }

        if(gameFinish == null)
        {
            gameFinish = FindAnyObjectByType<GameFinish>();
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
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
            player.spawnPoint = initialSpawn.spawnData;

            return;
        }

        Collider2D spawnReached = Physics2D.OverlapCircle(transform.position, 2f, spawnsLayer);
        SpawnController spawn = spawnReached.GetComponent<SpawnController>();

        if(spawn.name != player.spawnPoint.name)
        {
            player.spawnPoint = spawn.spawnData;
            Debug.Log("Novo spawn alcançado: " + spawn.name);
        }
    }

    public void DeathRoutine()
    {
        player.transform.position = new Vector3(player.spawnPoint.positionX, player.spawnPoint.positionY, player.spawnPoint.positionZ);

        adaptationControl.deathCount++;
    }

    public void WinGame()
    {
        adaptationControl.timeCosted += internalTimer.currentTime;
        adaptationControl.SaveAndUpdate();


        gameFinish.ArmazenateInfos(internalTimer.currentTime, gameObject.name);
        StartCoroutine(VictoryRoutine());
    }

    IEnumerator VictoryRoutine()
    {
        yield return null;

        gameObject.SetActive(false);
    }
}
