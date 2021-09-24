using UnityEngine;

public class GhostLimit : MonoBehaviour
{
    // seguro se puede hacer un poco mas prolijo pero a estas alturas ya ni ganas 😂
    public bool isHorizontal;
    public float displacement = 0.2f;
    public Vector3 vMovement = new Vector3(0, 0.2f, 0);
    public Vector3 hMovement = new Vector3(0.2f, 0, 0);

    public void Limit(Transform other)
    {
        if (isHorizontal)
        {
            if (other.position.y < transform.position.y)
            {
                other.position = other.position - vMovement;
            }
            else other.position = other.position + vMovement;
        }
        else
        {
            if (other.position.x < transform.position.x)
            {
                other.position = other.position - hMovement;
            }
            else other.position = other.position + hMovement;
        }
    }
}
