using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DragDropable : MonoBehaviour 
{
	[SerializeField] private InputAction press, screenPos;

	public Vector3 curScreenPos;
    public Camera gameCamera;
	public bool isDragging;

	private Vector3 WorldPos
	{
		get
		{
			float z = gameCamera.WorldToScreenPoint(transform.position).z;
			return gameCamera.ScreenToWorldPoint(curScreenPos + new Vector3(0, 0, z));
		}
	}
	private bool isClickedOn
	{
		get
		{
			Ray ray = gameCamera.ScreenPointToRay(curScreenPos);
			RaycastHit hit;
			if(Physics.Raycast(ray, out hit))
			{
				//Debug.Log(hit.transform.name);
				//return hit.transform == transform;
			}
			return false;
		}
	}
	private void Awake() 
	{
		screenPos.Enable();
		press.Enable();
		screenPos.performed += context => { curScreenPos = context.ReadValue<Vector2>(); };
		press.performed += _ => { if(isClickedOn) StartCoroutine(Drag()); };
		press.canceled += _ => { isDragging = false; };

	}

	private IEnumerator Drag()
	{
		isDragging = true;
		Vector3 offset = transform.position - WorldPos;
		// grab
		GetComponent<Rigidbody>().useGravity = false;
		while(isDragging)
		{
			// dragging
			transform.position = WorldPos + offset;
			yield return null;
		}
		// drop
		GetComponent<Rigidbody>().useGravity = true;
	}
}
