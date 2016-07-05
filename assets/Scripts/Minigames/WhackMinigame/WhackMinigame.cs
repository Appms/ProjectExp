using UnityEngine;
using System.Collections.Generic;

public class WhackMinigame : AbstractMinigame {
	[SerializeField]
	[Tooltip("The minimum time until an on object switches off")]
	private AnimationCurve _minTurnOffTime = new AnimationCurve(new Keyframe(0.0f, 3.0f), new Keyframe(1.0f, 0.3f));

	[SerializeField]
	[Tooltip("The delta between min and max time untile the object Switches off")]
	private AnimationCurve _maxTurnOffTimeDelta = new AnimationCurve(new Keyframe(0.0f, 3.0f), new Keyframe(1.0f, 0.5f));

	[SerializeField]
	[Tooltip("The minimum time until an active object switches on")]
	private float _minTurnOnTime = 2.0f;

	[SerializeField]
	[Tooltip("The maximum time until an active object switches on")]
	private float _maxTurnOnTime = 4.5f;

	[SerializeField]
	[Tooltip("The amount of objects that can turn on at the same time")]
	private int _activeObjectAmount = 4;

	[SerializeField]
	[Tooltip("The minimum time until the first lightbulbs turn on")]
	private float _minFirstTurnOnTime = 0.5f;

	[SerializeField]
	[Tooltip("The maximum time until the first lightbulbs turn on")]
	private float _maxFirstTurnOnTime = 0.75f;

	//A list of all whack objects
	private List<WhackObject> _whackObjects = new List<WhackObject>();

	private bool _justStarted;

	//For changing difficulty
	private MetricsManager.AvarageManager _timeToHit = new MetricsManager.AvarageManager();
	private MetricsManager.AvarageManager _timeLeft = new MetricsManager.AvarageManager();
	private MetricsManager.CounterManager _missedCount = new MetricsManager.CounterManager();
	private MetricsManager.CounterManager _wrongCount = new MetricsManager.CounterManager();
	private MetricsManager.CounterManager _rightCount = new MetricsManager.CounterManager();

	[SerializeField]
	[Tooltip("The amount the metric score will be changed with")]
	private float _metricScoreIncrements = 0.05f;

	[SerializeField]
	[Tooltip("The amount of missed objects to decrement the metric score")]
	private int _missedCountThreshold = 4;

	[SerializeField]
	[Tooltip("The amount of right objects to increment the metric score")]
	private int _rightCountThreshold = 4;

	[SerializeField]
	[Tooltip("The amount of wrong obejcts to decrement the metric score")]
	private int _wrongCountThreshold = 4;

	protected override void Start () {
		base.Start();

		_whackObjects = new List<WhackObject>(FindObjectsOfType<WhackObject>());

		if (_whackObjects.Count == 0) {
			Debug.LogError("You have no WhackObjects in your Scene!");
		}

		_justStarted = true;
	}

	protected override void Update () {
		if (_active) {
			//Metric calculations
			if (_missedCount.Counter >= _missedCountThreshold) {
				_missedCount.Reset();
				MetricsManager.MetricScore = Mathf.Max(0.0f, MetricsManager.MetricScore - _metricScoreIncrements);
			}
			//End metric calculations

			//Determine wich object is ready for a state change
			foreach (WhackObject obj in _whackObjects) {
				if (obj.SwitchTime <= Time.time) {
					if (obj.State) {
						obj.SwitchTime = Mathf.Infinity;
						_missedCount.Add();
						EndCombo();
					} else if (!obj.State) {
						obj.SwitchTime = Random.Range(_minTurnOffTime.Evaluate(MetricsManager.MetricScore), _minTurnOffTime.Evaluate(MetricsManager.MetricScore) + _maxTurnOffTimeDelta.Evaluate(MetricsManager.MetricScore)) + Time.time;
					}

					obj.SwitchState();
				}
			}

			int counter = 0;
			bool temp = false;

			while (GetTurningOnObjectCount() < _activeObjectAmount) {
				counter++;

				int index = Random.Range(0, _whackObjects.Count);

				int safetyCounter = 0;

				while (_whackObjects[index].State && safetyCounter < _whackObjects.Count) {
					index++;
					safetyCounter++;

					if (index >= _whackObjects.Count) {
						index = 0;
					}
				}

				if (_justStarted) {
					_whackObjects[index].SwitchTime = Random.Range(_minFirstTurnOnTime, _maxFirstTurnOnTime) + Time.time;
					temp = true;
				} else {
					_whackObjects[index].SwitchTime = Random.Range(_minTurnOnTime, _maxTurnOnTime) + Time.time;
				}

				if (counter >= 100)
					break;
			}

			if (temp) {
				_justStarted = false;
			}


			// Replace with Touch Input
			if (Input.GetMouseButtonDown(0)) {
				Ray ray = GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
				List<RaycastHit> hits = new List<RaycastHit>(Physics.RaycastAll(ray, Mathf.Infinity, _layer));
				RaycastHit hit = hits.Find(x => x.collider.GetComponent<WhackObject>() != null);

				if (hit.collider != null) {
					WhackObject obj = hit.collider.GetComponent<WhackObject>();

					if (obj.State) {
						_timeLeft.AddValue(obj.LeftTime);
						_timeToHit.AddValue(obj.HitTime);

						_rightCount.Add();

						if (_rightCount.Counter >= _rightCountThreshold) {
							if (_timeToHit.Avarage > _minTurnOffTime.Evaluate(MetricsManager.MetricScore)) {
								MetricsManager.MetricScore = Mathf.Max(0.0f, MetricsManager.MetricScore - _metricScoreIncrements);
							} else {
								MetricsManager.MetricScore = Mathf.Min(1.0f, MetricsManager.MetricScore + _metricScoreIncrements);
							}

							_rightCount.Reset();
						}

						AddCombo();
					} else {
						_wrongCount.Add();

						if (_wrongCount.Counter >= _wrongCountThreshold) {
							MetricsManager.MetricScore = Mathf.Max(0.0f, MetricsManager.MetricScore - _metricScoreIncrements);

							_wrongCount.Reset();
						}

						EndCombo();
					}

					obj.SwitchTime = Mathf.Infinity;
					obj.Interact();
				}
			}
		}

		base.Update();
	}

	private int GetTurningOnObjectCount () {
		int count = 0;

		foreach (WhackObject obj in _whackObjects) {
			if (!obj.State && obj.SwitchTime < Mathf.Infinity) {
				count++;
			}
		}

		return count;
	}
}
