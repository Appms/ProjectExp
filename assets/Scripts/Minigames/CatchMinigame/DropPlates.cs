using UnityEngine;
using System.Collections;

public class DropPlates : MonoBehaviour {

	[SerializeField]
	GameObject platePrefab;

	private int amountOfPlates;

	private void OnEnable () {

		amountOfPlates = Random.Range(3, 4);
		StartCoroutine(FallingPlates());
	}

	IEnumerator FallingPlates () {

		while (amountOfPlates > 0) {
			yield return new WaitForSeconds(0.1f);
			GameObject obj = (GameObject)GameObject.Instantiate(platePrefab, this.transform.position, Quaternion.identity);
			obj.GetComponent<C_DropObject>().SetValues(FindObjectOfType<CatchMinigame>(), 0.5f, 1.0f, true);
			amountOfPlates--;
		}
		this.gameObject.SetActive(false);
	}
}
