using UnityEngine;

public class DeathManager : MonoBehaviour
{
    public Timer timer;
    private Transform targetPos;

    public SpawnsData currentSpawn;

    void OnTriggerEnter2D(Collider2D collision)
    {
        targetPos = collision.transform;

        if(timer.isUnder3Min == true)
        {
            Respawn();
        }
        else
        {
            Reset();
        }
    }

    public void Respawn()
    {
        Debug.Log("Morreu cedo em. Volta lá para tentar de novo");

        targetPos.position = new Vector3(currentSpawn.positionX, currentSpawn.positionY, currentSpawn.positionZ);
    }

    public void Reset()
    {
        Debug.Log("Se fodeu kkkkk recomeça aí");
    }
}
