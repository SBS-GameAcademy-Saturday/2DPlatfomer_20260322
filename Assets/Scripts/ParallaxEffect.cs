using UnityEngine;

public class ParallaxEffect : MonoBehaviour
{
    // 카메라
    public Camera camera;

    // 얼만큼 느리게 혹은 빠르게 움직이게 할건지
    public float parallaxFactor = 1.1f;

    // 시작 위치
    Vector2 startingPosition;

    // 게임이 시작되고 나서 카메라가 움직이는 방향
    Vector2 camMoveSinceStart 
    {
        get
        {
            return (Vector2)camera.transform.position - startingPosition;
        }
    } 

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startingPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        // startingPosition : 0,0
        // camMoveSinceStart : 5,0
        // parallaxFactor : 1.1
        Vector2 newPosition = startingPosition + camMoveSinceStart / -parallaxFactor;

        transform.position = new Vector3(newPosition.x, transform.position.y, transform.position.z);

        Debug.Log($"startingPosition : {startingPosition} camMoveSinceStart : {camMoveSinceStart} parallaxFactor : {parallaxFactor}");
        Debug.Log($"newPosition : {newPosition} transform.position : {transform.position}");
    }
}
