using UnityEngine;
public class ObjectSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject @object;

    [SerializeField]
    private Transform spawnTransform;

    [SerializeField]
    private bool enableDestroyTimer;

    [SerializeField]
    private float destroyInSeconds;

    public void Spawn()
    {
        var obj = Instantiate(@object, spawnTransform.position, spawnTransform.rotation);

        if (enableDestroyTimer)
            Destroy(obj, destroyInSeconds);
    }
}
