using UnityEngine;
using System.Collections;

public class T_SpawnObject : MonoBehaviour
{
	protected ThrowMinigame _manager;
	protected T_ThrowObject _grabbedObject;
	private Vector3 _oldMousePos;
	private Vector3 _currentVelocity;

	//Control tuneVariables
	private float _throwingPower = 0.5f; 
	private float _lerpMod = 4; //how fast the old velocity lerp toward the new velocity

	protected virtual void Start()
	{
		_manager = FindObjectOfType<ThrowMinigame>();
		_oldMousePos = _manager.MinigameCamera.ScreenToWorldPoint(Input.mousePosition + new Vector3(0.0f, 0.0f, Mathf.Abs((transform.position - _manager.transform.position).z)));
	}

	protected virtual void Update()
	{
		if (_manager.GetActive())
		{
			//Ray ray = _manager.MinigameCamera.ScreenPointToRay(Input.mousePosition + new Vector3(0.0f, 0.0f, Mathf.Abs((transform.position - _manager.transform.position).z)));
			Vector3 worldMousePos = _manager.MinigameCamera.ScreenToWorldPoint(Input.mousePosition + new Vector3(0.0f, 0.0f, Mathf.Abs((transform.position - _manager.transform.position).z)));

			//TODO Replace by Touchinput
			if (Input.GetMouseButtonDown(0))
			{
				Ray ray = _manager.MinigameCamera.ScreenPointToRay(Input.mousePosition);
				RaycastHit[] hits = Physics.RaycastAll(ray, Mathf.Infinity, _manager.MinigameLayers);

				foreach (RaycastHit hit in hits)
				{
					if (hit.collider.GetComponent<T_SpawnObject>() != null)
					{
						GameObject go = (GameObject)Instantiate(_manager.ThrowObjectPrefab, worldMousePos, Quaternion.identity);
						go.GetComponent<Rigidbody>().useGravity = false;
						GameObject.Destroy(go, 10.0f);
						_grabbedObject = go.GetComponent<T_ThrowObject>();
						_grabbedObject.Grabbed = true;
					}
				}
			}

			//TODO Replace by Touchinput
			if (Input.GetMouseButtonUp(0))
			{
				if (_grabbedObject != null)
				{
					_grabbedObject.GetComponent<Rigidbody>().useGravity = true;
					_grabbedObject = null;
				}
			}

			if (_grabbedObject != null)
			{
				_grabbedObject.transform.position = worldMousePos;
				_currentVelocity = Vector3.Lerp(_currentVelocity, (worldMousePos - _oldMousePos) * (_throwingPower/Time.deltaTime), Time.deltaTime*4);
				_grabbedObject.GetComponent<Rigidbody>().velocity = _currentVelocity;
			}

			/*
			if (worldMousePos.x < -5f || worldMousePos.x > 0 || worldMousePos.y < -1 || worldMousePos.y > 3f)
			{
				if (_grabbedObject != null)
				{
					_grabbedObject.GetComponent<Rigidbody>().useGravity = true;
					_grabbedObject = null;
				}
			}
			else
			{
				worldMousePos.x = Mathf.Clamp(worldMousePos.x, -5f, 0f);
				worldMousePos.y = Mathf.Clamp(worldMousePos.y, -1f, 3f);
			}
			*/

			_oldMousePos = worldMousePos;
		}
	}
}
