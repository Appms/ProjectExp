using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;
using System;

public class ConditionMinigame : AbstractMinigame {
	[SerializeField]
	[Tooltip("The prefab that is shown to the player")]
	private GameObject _conditionPrefab;

	[SerializeField]
	[Tooltip("The time for the fade betweent condition sets")]
	private float _fadeLerpTime = 0.5f;

	private GameObject _currentConditionPrefab;
	private List<ConditionObject> _currentConditionObjects;
	private float _feedbackEndTime;
	private bool _evaluated;
	private bool blurBool;

	private Vector3 _startValues = new Vector3(0, 0, 0);
	private Vector3 _endValues = new Vector3(3, 4, 1);

	private float _currentLerpTime;

	/// <summary>
	/// Returns a list of all current condition objects
	/// </summary>
	public List<ConditionObject> CurrentConditionObjects {
		get { return _currentConditionObjects; }
	}

	protected override void Start () {
		base.Start();
		_currentLerpTime = 0.0f;
		newElement();
	}

	protected override void Update () {
		if (_active) {
			if (_evaluated && objectAnimationsFinished()) {
				_currentLerpTime += Time.deltaTime;
				if (_currentLerpTime > _fadeLerpTime) {
					_currentLerpTime = _fadeLerpTime;

					if (blurBool) {
						newElement();
						blurBool = false;
						_currentLerpTime = 0.0f;
					} else {
						FindObjectOfType<UnityStandardAssets.ImageEffects.BlurEffect>().enabled = false;
						_evaluated = false;
					}
				}

				float perc = _currentLerpTime / _fadeLerpTime;

				Vector3 temp;

				if (blurBool) {
					temp = Vector3.Lerp(_startValues, _endValues, perc);
				} else {
					temp = Vector3.Lerp(_endValues, _startValues, perc);
				}

				FindObjectOfType<UnityStandardAssets.ImageEffects.BlurEffect>().downsampling = Mathf.RoundToInt(temp.x);
				FindObjectOfType<UnityStandardAssets.ImageEffects.BlurEffect>().iterations = Mathf.RoundToInt(temp.y);
				FindObjectOfType<UnityStandardAssets.ImageEffects.BlurEffect>().blurSpread = temp.z;
			} else {
				_currentLerpTime = 0.0f;
			}
		}

		base.Update();
	}

	/// <summary>
	/// Evaluates if the current set of state is in the right combination
	/// </summary>
	public void Evaluate () {
		bool result = _currentConditionObjects.ToArray().All(x => x.State == true) || _currentConditionObjects.ToArray().All(x => x.State == false);

		if (result) {
			AddCombo();
			_evaluated = true;
			blurBool = true;
			FindObjectOfType<UnityStandardAssets.ImageEffects.BlurEffect>().enabled = true;
		} else {
			//EndCombo();
		}
	}

	private bool objectAnimationsFinished () {
		return FindObjectsOfType<ConditionObject>().All(x => x.AnimationPlaying == false);
	}

	private bool parentAnimationsFinished () {
		return FindObjectsOfType<ConditionParent>().All(x => x.AnimationPlaying == false);
	}

	private void newElement () {
		_tempStates.Clear();

		if (_currentConditionPrefab != null) {
			_currentConditionPrefab.GetComponent<ConditionParent>().Despawn();
		}

		//_evaluated = false;
		_currentConditionPrefab = (GameObject)GameObject.Instantiate(_conditionPrefab, Vector3.zero, Quaternion.identity);
		_currentConditionObjects = new List<ConditionObject>(_currentConditionPrefab.GetComponentsInChildren<ConditionObject>());
	}

	protected override void DestroyDynamicObjects () {
		base.DestroyDynamicObjects();
		GameObject.Destroy(_currentConditionPrefab);
	}

	private List<bool> _tempStates = new List<bool>();

	/// <summary>
	/// This requests a state for a conditionObject to start initialize in
	/// </summary>
	/// <returns>A correct state</returns>
	public bool RequestState () {
		bool result;

		if (_tempStates.Count > 0 && (_tempStates.All(x => x == false) || _tempStates.All(x => x == true))) {
			result = !_tempStates[0];
		} else {
			result = Convert.ToBoolean(UnityEngine.Random.Range(0, 2));
		}

		_tempStates.Add(result);

		return result;
	}
}
