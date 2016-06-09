using UnityEngine;
using System.Collections;

public class T_SpawnObject : MonoBehaviour
{
	[SerializeField]
	private float _maxMouseDistance;
	[SerializeField]
	protected float _throwingPower = 4f;
	[SerializeField]
	protected Vector3 _spawnOffset;
	
	protected ThrowMinigame _manager;
	protected T_ThrowObject _grabbedObject;
	protected Vector3 projectileVector;
	private Vector3 _oldMousePos;
	private Vector3 _currentVelocity;
	private Vector3 worldMousePos;
	private LineRenderer _lineRenderer;

	//Control tuneVariables
	
	//private float _lerpMod = 4; //how fast the old velocity lerp toward the new velocity

	protected virtual void Spawn() { }

	protected virtual void Start()
	{
		_manager = FindObjectOfType<ThrowMinigame>();
		_oldMousePos = _manager.MinigameCamera.ScreenToWorldPoint(Input.mousePosition + new Vector3(0.0f, 0.0f, Mathf.Abs((transform.position - _manager.transform.position).z)));
		_lineRenderer = GetComponent<LineRenderer>();
	}

	protected virtual void Update()
	{
		if (_manager.GetActive())
		{
			worldMousePos = _manager.MinigameCamera.ScreenToWorldPoint(Input.mousePosition + new Vector3(0.0f, 0.0f, Mathf.Abs((transform.position - _manager.transform.position).z)));
			
			projectileVector = worldMousePos - (this.transform.position + _spawnOffset);
			float distance = Vector3.Distance(worldMousePos, this.transform.position + _spawnOffset);
			distance = Mathf.Clamp(distance, 0, _maxMouseDistance);
			projectileVector = projectileVector.normalized * distance;
			//TODO Replace by Touchinput
			if (Input.GetMouseButtonDown(0))
			{
				Ray ray = _manager.MinigameCamera.ScreenPointToRay(Input.mousePosition);
				RaycastHit[] hits = Physics.RaycastAll(ray, Mathf.Infinity, _manager.MinigameLayers);
				_lineRenderer.SetPosition(0, this.transform.position + _spawnOffset);
				foreach (RaycastHit hit in hits)
				{
					if (hit.collider.GetComponent<T_SpawnObject>() != null)
					{
						GameObject go = (GameObject)Instantiate(_manager.ThrowObjectPrefab, worldMousePos, Quaternion.identity);
						go.GetComponent<Rigidbody>().useGravity = false;
						GameObject.Destroy(go, 10.0f);
						_grabbedObject = go.GetComponent<T_ThrowObject>();
						_grabbedObject.Grabbed = true;
						Spawn();
					}
				}
			}

			//TODO Replace by Touchinput
			if (Input.GetMouseButtonUp(0))
			{
				if (_grabbedObject != null)
				{
					Release();
				}
			}

			if (_grabbedObject != null)
			{
				_grabbedObject.transform.position = (this.transform.position + _spawnOffset); //+ projectileVector;//projectileVector;
				_lineRenderer.SetPosition(1, (this.transform.position + _spawnOffset) + projectileVector);
				//_currentVelocity = Vector3.Lerp(_currentVelocity, (worldMousePos - _oldMousePos) * (_throwingPower/Time.deltaTime), Time.deltaTime*4);
				//_grabbedObject.GetComponent<Rigidbody>().velocity = _currentVelocity;

				OnHold();

				if (Vector3.Distance(_grabbedObject.transform.position, transform.position) >= _maxMouseDistance)
				{
					//Release();
				}
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
	private void Release(){
		OnRelease();
		_grabbedObject.GetComponent<Rigidbody>().useGravity = true;
		_grabbedObject = null;
		
	}
	protected virtual void OnHold(){

	}
	protected virtual void OnRelease(){
		//_currentVelocity = Vector3.Lerp(_currentVelocity, (worldMousePos - _oldMousePos) * (_throwingPower/Time.deltaTime), Time.deltaTime*4);

		_grabbedObject.GetComponent<Rigidbody>().velocity = -projectileVector * _throwingPower;
	}
}
