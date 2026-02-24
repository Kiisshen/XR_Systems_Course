using UnityEngine;

public class BounceEffectScript : MonoBehaviour
{
    public float lifeTime = 5f;

    void Start()
    {
        AudioSource.PlayClipAtPoint(
            GetComponent<AudioSource>().clip,
            transform.position
        );
    }

    void Update()
    {
        if(lifeTime > 0)
        {
            lifeTime -= Time.deltaTime;
        }
        if(lifeTime <= 0)
        {
            Destroy(transform.gameObject);
        }
    }
}
