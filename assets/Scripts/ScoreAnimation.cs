using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScoreAnimation : MonoBehaviour {

	/*
    [SerializeField]
    private Color _normalColor;

    [SerializeField]
    private Color _animationColor;

    [SerializeField]
    private AnimationCurve _animationCurve;

    [SerializeField]
    private float _animationSpeed;

    private float _animationDelta;

    private int _scoreGame;
    private int _score;

    private int _originalFontSize;
	*/
	/*
    private float _acc;

    private AudioSource _audio;
	*/

	[SerializeField]
	private float _countUpTime = 2.5f;

	private float _displayTime;
	private int _displayScore;
	private int _totalScore;

	private Text _scoreLabel;
	private bool _counting;

	// Use this for initialization
	void Start () {
        _scoreLabel = GetComponent<Text>();
		//_originalFontSize = _scoreLabel.fontSize;

		//TODO Change in scene
		_countUpTime = 2.0f;

        //_audio = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
		if (_counting)
		{
			_displayTime += Time.deltaTime;
			_displayScore = (int)(_totalScore * (_displayTime / _countUpTime));

			if (_displayScore >= _totalScore)
			{
				_displayScore = _totalScore;
				_counting = false;
			}

			_scoreLabel.text = _displayScore.ToString();
		}

		/*
	    if(_scoreGame != _score && _animationDelta < 1)
        {
            _acc += Time.unscaledDeltaTime * 2;
            _animationDelta += Time.unscaledDeltaTime * (_animationSpeed + _acc);
            _scoreLabel.color = Color.Lerp(_normalColor, _animationColor, 1 - _animationCurve.Evaluate(_animationDelta));
            _scoreLabel.fontSize = (int)(_originalFontSize * _animationCurve.Evaluate(_animationDelta));
        }

        else if (_animationDelta >= 1)
        {
            _score++;
            _audio.Play(0);
            _animationDelta = 0;
            _scoreLabel.text = _score.ToString();
        }

        else if (_scoreGame == _score)
        {
            _acc = 0;
        }
		*/
	}

    public void UpdateScore(int newScore)
    {
		_totalScore = newScore;
		_displayTime = 0.0f;
		_counting = true;
    }
}
