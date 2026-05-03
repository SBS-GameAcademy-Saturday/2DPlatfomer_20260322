using UnityEngine;

public class Follower : MonoBehaviour
{
    public Transform followTarget;
    public Vector3 offset;

    // Update is called once per frame
    void Update()
    {
        transform.position  = followTarget.position + offset;
    }
}
