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

	public float GetTime(){
		return (_endTime - _timer);
	}
	//check if timer expired if autoReset = true the method automatically calls reset if it is expired
	public bool Run(bool autoReset = false){
		_timer += Time.deltaTime;
		if(_timer > _endTime){
			//Time passed
			if(autoReset){
				Reset();
			}
			return false;
		}else{
			//Time not passed
			return true;
		}
	}

	//Reset timer to interval value
	public void Reset(){
		_endTime = Time.time + _interval;
		_timer = Time.time;
	}
	//Reset timer to custom value
	public void Reset(float interval){
		_endTime = Time.time + interval;
		_timer = Time.time;
	}
}
