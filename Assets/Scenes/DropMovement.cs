using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropMovement : MonoBehaviour
{

    [SerializeField] public float Speed = 1.0f;

    private Vector3 movement;

    // Start is called before the first frame update
    void Start()
    {
        movement = new Vector3(Speed, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        this.gameObject.transform.Translate(movement * Time.deltaTime * 5);
        LeftRight();
    }

    private void LeftRight()
    {
        if(this.gameObject.transform.localPosition.x > 45)
        {
            movement = new Vector3(-Speed, 0, 0);
        }
        if (this.gameObject.transform.localPosition.x < -45)
        {
            movement = new Vector3(Speed, 0, 0);
        }
    }

}
