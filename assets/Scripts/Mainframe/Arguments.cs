using UnityEngine;
using System.Collections;
using System;

public class Arguments {
	
	private int _userID;
	private int _gameID;
	private string _username;
	private int _gametime;
	private string _conURL;

    public Arguments()
    {
        //string[] arguments = Environment.GetCommandLineArgs();
        //_userID = Convert.ToInt32(arguments[2]);
        //_gameID = Convert.ToInt32(arguments[3]);
        //_username = arguments[4];
        //_gametime = Convert.ToInt32(arguments[5]);
        //_conURL = arguments[6];

        _userID = 2;
        _gameID = 6;
        _username = "YVONNE";
        _gametime = 120;
        _conURL = "http://www.serellyn.net/HEIM/php/";
    }

	public int getUserID() {
		return _userID;	
	}
	
	public int getGameID() {
		return _gameID;	
	}
	
	public string getUsername() {
		return _username;
	}
	
	public int getGameTime() {
		return _gametime;
	}

	public string getConURL() {
		return _conURL;
	}
}
