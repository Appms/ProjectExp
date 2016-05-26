using UnityEngine;
using System.Collections;

public class MinigameStarter : MonoBehaviour {
	[SerializeField] bool _active;
	[SerializeField] private string _minigameScene;

	private Color _defaultColor;

	private void Start()
	{
		_defaultColor = GetComponent<Renderer>().sharedMaterial.GetColor("_Color");
	}

	public void Update()
	{
		if (_active)
		{
			GetComponent<Renderer>().material.SetColor("_Color", Color.red);
		}
		else
		{
			GetComponent<Renderer>().material.SetColor("_Color", _defaultColor);
		}
	}

	public void StartMiniGame()
	{
		if (_active)
		{
			FindObjectOfType<GameManager>().StartMinigame(_minigameScene);
			_active = false;
		}
	}

	public bool Startable
	{
		set { _active = value; }
	}

	public bool Active
	{
		get { return _active; }
	}
}
