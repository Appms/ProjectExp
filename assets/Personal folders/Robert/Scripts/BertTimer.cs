using UnityEngine;
using System.Collections;

public class BertTimer {

	private float _timer;
	private float _endTime;

	private float _interval;
	public float Interval{
		get
		{
			return _interval;
		}
		set
		{
			_interval = value;
			Reset();
		}
	}

	//check if timer expired if autoReset = true the method automatically calls reset if it is expired
	public bool Run(bool autoReset = false){
		_timer += Time.deltaTime;
		if(_timer > _endTime){
			//Timer passed
			if(autoReset){
				Reset();
			}
			return false;
		}else{
			//Timer not passed
			return true;
		}
	}

	//Reset timer to interval value
	public void Reset(){
		_endTime = Time.time + _interval;
	}

	//Reset timer to custom value
	public void Reset(float interval){
		_endTime = Time.time + interval;
	}
}
