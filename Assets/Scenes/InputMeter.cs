using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputMeter : MonoBehaviour
{
    [SerializeField] public GameObject Meter;
    [SerializeField] public GameObject Drop;
    [SerializeField] public int MeterFill;

    private float DropX;
    private bool SpacePressed;

    // Start is called before the first frame update
    void Start()
    {
        Meter = this.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        this.InputListen();
        this.CheckHit();
    }

    private void ChangeMeter()
    {
        RectTransform rt = this.gameObject.GetComponent<RectTransform>();
        rt.sizeDelta = new Vector2(rt.sizeDelta.x, MeterFill);
    }

    private void InputListen()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            DropX = Drop.transform.localPosition.x;
            
        }
        if(Input.GetKeyUp(KeyCode.Space))
        {
            SpacePressed = true;
        }
    }

    private void CheckHit()
    {
        if(SpacePressed)
        {
            if(DropX > -20 && DropX < 15)
            {
                MeterFill += 20;

                if(MeterFill > 100)
                {
                    MeterFill = 100;
                }
            }
            else
            {
                MeterFill -= 10;

                if(MeterFill < 0)
                {
                    MeterFill = 0;
                }
            }

            ChangeMeter();

            SpacePressed = false;

        }
    }

}
