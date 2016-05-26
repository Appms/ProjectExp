using UnityEngine;
using System.Collections;

public class MinigameTransititon : MonoBehaviour {
	[SerializeField] private float _changePerSecond = 0.5f;
	[SerializeField] private float _heightWidthEndValue = 0.9f;
	[SerializeField] private float _xyEndValue = 0.05f;

	private Camera _camera;

	private void Start()
	{
		_camera = GetComponent<Camera>();

		_camera.rect = new Rect(0.5f, 0.5f, 0.0f, 0.0f);
	}

	private void Update()
	{
		Rect viewPort = _camera.rect;

		viewPort.width += _changePerSecond * Time.deltaTime;
		viewPort.height += _changePerSecond * Time.deltaTime;

		if (viewPort.height >= _heightWidthEndValue || viewPort.width >= _heightWidthEndValue)
		{
			viewPort.width = _heightWidthEndValue;
			viewPort.height = _heightWidthEndValue;
		}

		viewPort.x -= _changePerSecond / 2.0f * Time.deltaTime;
		viewPort.y -= _changePerSecond / 2.0f * Time.deltaTime;

		if (viewPort.x <= _xyEndValue || viewPort.y <= _xyEndValue)
		{
			viewPort.x = _xyEndValue;
			viewPort.y = _xyEndValue;
		}

		_camera.rect = viewPort;
	}
}
