using UnityEngine;

public class Interactable : MonoBehaviour
{
    public float radius = 3f;

    public Transform interactionTransform;

    public bool isFocus = false;
    Transform player;

    public bool hasInteracted = false;

    public virtual void Interact()
    {

        //Debug.Log("Interacting with " + interactionTransform.name);
    }

    void Update()
    {
        if (isFocus && !hasInteracted)
        {
            float distance = Vector3.Distance(player.position, interactionTransform.position);
            if(distance <= radius)
            {
                //Debug.Log("Interact");
                hasInteracted = true;
                Interact();
                hasInteracted = false;
            }
        }
    }

    public void OnFocused (Transform playerTransform)
    {
        isFocus = true;
        player = playerTransform;
        hasInteracted = false;
    }

    public void OnDeFocused()
    {
        isFocus = false;
        player = null;
        hasInteracted = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(interactionTransform.position, radius);
    }
}
