using UnityEngine;
using UnityEngine.Events;

public class OnZombieEnter : MonoBehaviour
{
    // lo hice en un script aparte al OnPlayerEnter para separarlo
    // pero se puede hacer en ese script tambien
    public UnityEvent onZombieEnter;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Zombie"))
        {
            onZombieEnter?.Invoke();
        }
    }
}
