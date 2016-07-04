using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class FinalConditionParent : ConditionParent {
	private Image _thermometer;

	[SerializeField]
	private Sprite _goodTempSprite;

	[SerializeField]
	private Sprite _warmTempSprite;

	[SerializeField]
	private Sprite _coldTempSprite;

	[SerializeField]
	private Sprite _veryWarmTempSprite;

	[SerializeField]
	private Sprite _veryColdTempSprite;

	public override void Despawn () {
		base.Despawn();

		_animationPlaying = true;
	}

	protected override void Start () {
		base.Start();

		_animationPlaying = false;

		_thermometer = FindObjectOfType<Thermometer>().GetComponent<Image>();
	}

	protected override void Update () {
		base.Update();

		if (_animationPlaying) {
			GameObject.Destroy(this.gameObject);
		}

		switch (CalcTemperature()) {
			case 2:
				_thermometer.sprite = _veryWarmTempSprite;
				break;

			case 1:
				_thermometer.sprite = _warmTempSprite;
				break;

			case 0:
				_thermometer.sprite = _goodTempSprite;
				break;

			case -1:
				_thermometer.sprite = _coldTempSprite;
				break;

			case -2:
				_thermometer.sprite = _veryColdTempSprite;
				break;
		}
	}

	private int CalcTemperature () {
		int _temperature = 0;

		List<ConditionObject> allCos = new List<ConditionObject>(transform.GetComponentsInChildren<ConditionObject>());
		ConditionObject nonChangeableCo = allCos.Find(x => x.Changeable == false);
		List<ConditionObject> changeableCos = allCos.FindAll(x => x.Changeable == true);

		if (nonChangeableCo.State) {
			_temperature = -changeableCos.Count;
		} else {
			_temperature = changeableCos.Count;
		}

		foreach (ConditionObject co in changeableCos) {
			if (nonChangeableCo.State) {
				if (co.State) {
					_temperature++;
				}
			} else {
				if (!co.State) {
					_temperature--;
				}
			}
		}

		return _temperature;
	}
}
