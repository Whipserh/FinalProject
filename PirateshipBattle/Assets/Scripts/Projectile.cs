using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float lifeTime = 3;

    void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
