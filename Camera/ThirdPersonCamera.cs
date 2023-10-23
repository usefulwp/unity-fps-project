using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    public Transform fpsTransform;
    private Vector3 direction;
    private Camera theThirdCamera;
    // Start is called before the first frame update
    void Start()
    {
        theThirdCamera = GetComponent<Camera>();
        direction = theThirdCamera.transform.position-fpsTransform.position;
    }

    // Update is called once per frame
    void Update()
    {
        theThirdCamera.transform.position = fpsTransform.position + direction;
    }
}
