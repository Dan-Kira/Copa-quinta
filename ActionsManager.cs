using System.Collections;
using UnityEngine;

public class ActionsManager : MonoBehaviour
{
    [SerializeField] private int grabDistance;
    public LayerMask grabLayer;
    public KeyCode actionKey;
    public float actionCooldown;
    private float lastActionTime; 

    public PlayerController player;
    
    DistanceJoint2D joint;
    private Collider2D target;
    private bool isHooked;
    private bool isPunished;

    private void Awake()
    {
        joint = GetComponent<DistanceJoint2D>();

        joint.enabled = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(actionKey) && Time.time >= lastActionTime + actionCooldown)
        {
            TryToGrab();
        }

        if(Input.GetKeyUp(actionKey) && isHooked)
        {
            ReleaseHook();
        }
    }

    public void TryToGrab()
    {
        lastActionTime = Time.time;

        target = Physics2D.OverlapCircle(transform.position, grabDistance, grabLayer);

        if(target == null)
        {
            if(!isPunished)
            {
                StartCoroutine(FailedActionRoutine(3, 2.5f));
            }

            return;
        }

        if(target.CompareTag("Competitor"))
        {
            GrabCompetitor(target.transform);
        }
        else if(target.CompareTag("HookPoint"))
        {
            GrabHook(target.transform);
            isHooked = true;
        }
    }

    public void GrabCompetitor(Transform grabedObject)
    {
        Rigidbody2D targetRb = grabedObject.GetComponent<Rigidbody2D>();

        Vector2 pushDirection = (grabedObject.position.x > transform.position.x) ? Vector2.left : Vector2.right;

        targetRb.AddForce(pushDirection * 8f, ForceMode2D.Impulse);
    }

    public void GrabHook(Transform grabbedHook)
    {
        joint.enabled = true;

        joint.connectedAnchor = grabbedHook.position;

        joint.distance = Vector2.Distance(transform.position, grabbedHook.position);
    }
    public void ReleaseHook()
    {
        joint.enabled = false;
        isHooked = false;
    }

    public IEnumerator FailedActionRoutine(int speedPenalty, float duration)
    {        
        isPunished = true;

        float originalSpeed = player.moveSpeed;

        player.moveSpeed -= speedPenalty;

        yield return new WaitForSeconds(duration);

        player.moveSpeed = originalSpeed;

        isPunished = false;
    }
}
