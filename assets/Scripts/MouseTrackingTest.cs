using UnityEngine;
using System.Collections.Generic;

namespace DebuggingTools
{
    [RequireComponent(typeof(LineRenderer))]
    public class MouseTrackingTest : MonoBehaviour
    {
        private LineRenderer _mouseLine;

        [SerializeField]
        [Range(1,500)]
        private int _mouseFrames;
        private int _oldMouseFramesCount;

        private List<Vector2> _mousePositions = new List<Vector2>();

        [SerializeField]
        GameObject ball;

        [SerializeField]
        Camera _camera;

        void Awake()
        {
            _mouseLine = GetComponent<LineRenderer>();
            _oldMouseFramesCount = _mouseFrames;
        }

        void Update()
        {
            if(_mouseFrames != _oldMouseFramesCount)
            {
                _oldMouseFramesCount = _mouseFrames;
                _mousePositions.Clear();
            }

            if (Input.GetMouseButton(0))
            {
                if (_mousePositions.Count == _mouseFrames) _mousePositions.RemoveAt(0);
                _mousePositions.Add(Input.mousePosition);

                _mouseLine.SetVertexCount(_mousePositions.Count);
                for (int i = 0; i < _mousePositions.Count; i++)
                {
                    Vector3 mousePos = Input.mousePosition;
                    //mousePos.x /= Screen.width;
                    //mousePos.y /= Screen.height;
                    mousePos.z = 5;
                    mousePos = _camera.ScreenToWorldPoint(mousePos);

                    _mouseLine.SetPosition(i, mousePos);
                    ball.transform.position = mousePos;
                    //_mouseLine.SetPosition(i, new Vector3(cameraPos.x + _mousePositions[i].x / Screen.width * 20 - 10, cameraPos.y, cameraPos.z + _mousePositions[i].y / Screen.height * 11.25f - 5.75f));
                }
            }
        }
    }
}
