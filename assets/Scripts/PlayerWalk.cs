using UnityEngine;
using System.Collections;

[RequireComponent(typeof(NavMeshAgent))]
public class PlayerWalk : MonoBehaviour {
	private NavMeshAgent _agent;
	private GameObject _selectedObject;

	[SerializeField] private float _interactionDistance = 1.5f;
	[SerializeField] private bool _canWalk = true;

	[SerializeField]
	GameObject naviArrow;

	private void Start() {
		_agent = GetComponent<NavMeshAgent>();
	}

	private void Update()
	{
		if (_canWalk)
		{
			if (Input.GetMouseButton(0) && !FindObjectOfType<CameraMove>().IsMoving)
			{
				_selectedObject = null;

				Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
				RaycastHit[] hits = Physics.RaycastAll(ray, 200.0f);

				foreach (RaycastHit hit in hits)
				{
					if (hit.collider.tag == "Walkable")
					{
						
					}
					else if (hit.collider.tag == "Interactable")
					{
						_selectedObject = hit.collider.gameObject;
						NavMeshHit meshHit;
						NavMesh.SamplePosition(hit.point, out meshHit, 4.0f, 1 << NavMesh.GetAreaFromName("Walkable"));
						_agent.SetDestination(meshHit.position);
					}
				}
			}

			if (_selectedObject != null && Vector3.Distance(this.transform.position, _selectedObject.transform.position) <= _interactionDistance)
			{
				_selectedObject.GetComponent<MinigameStarter>().StartMiniGame();
				_selectedObject = null;
			}
		}

		//NAVI ARROW
		GameObject target = FindClosestInteractable();
		if (FindClosestInteractable () == null) {
			naviArrow.SetActive (false);
		} else {
			naviArrow.SetActive (true);
			naviArrow.transform.LookAt (target.transform.position);
		}
	}

	GameObject FindClosestInteractable() {
		GameObject[] interactable;
		interactable = GameObject.FindGameObjectsWithTag("Interactable");
		GameObject closest = null;
		float distance = Mathf.Infinity;
		Vector3 position = transform.position;
		foreach (GameObject target in interactable) {
			Vector3 diff = target.transform.position - position;
			float curDistance = diff.sqrMagnitude;
			if (curDistance < distance && target.GetComponent<MinigameStarter>().Active) {
				closest = target;
				distance = curDistance;
			}
		}
		return closest;
	}


	public GameObject SelectedObject
	{
		get { return _selectedObject; }
	}

	public bool CanWalk
	{
		set
		{
			if (value) {
				_agent.Resume();
			} else {
				_agent.Stop();
				_agent.ResetPath();
			}

			_canWalk = value;
		}
	}
}