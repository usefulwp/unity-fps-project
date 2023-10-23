using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    private CharacterController characterController;
    public float speed;
    // Start is called before the first frame update
    void Start()
    {
        characterController = this.GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        float x= Input.GetAxis("Horizontal");
        float y=Input.GetAxis("Vertical");
        float z = 0;
        if (Input.GetButton("Fire1"))
        {
            z = 1f;
        }
        else if (Input.GetButton("Fire2"))
        {
            z = -1f;
        }
        transform.Translate(new Vector3(x, y, z)*Time.deltaTime*speed, Space.Self);
    }
}
