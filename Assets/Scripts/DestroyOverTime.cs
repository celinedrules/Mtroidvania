using UnityEngine;

public class DestroyOverTime : MonoBehaviour
{
    [SerializeField] private float lifeTime;

    private void Start()
    {
        Destroy(gameObject, lifeTime);
    }
}
