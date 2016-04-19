using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class CaveController : MonoBehaviour {
	
	int state = 0;
	int color = -1;
	List<List<int>> workersHave = new List<List<int>> 
		{ new List<int> { 100, 0 }, new List<int> { 100, 0 },
		new List<int>  { 100, 0 },new List<int> { 100, 0 } };
	int[] workersNeed = { 0, 0, 0, 0 };
	public Sprite[] stateSprites;
	public SpriteRenderer stateSprite;
	public Text caveBlack, caveRed, caveGreen, caveWhite, manBlack, manRed, manGreen, manWhite;
	public Image rewardImg;
	public Animator[] animators;
	bool hitedObject = false;
	bool needRestart = false;
	public Button restartBtn;
	public ParticleSystem particle;
	bool isLocked = false;

	System.Random rand;


	// Use this for initialization
	void Start () {
		
		stateSprite.sprite = stateSprites [state];
		rand = new System.Random ();
		restartBtn.gameObject.SetActive (false);
		restartBtn.onClick.AddListener(() => { 
			SceneManager.LoadScene ("scene");
			/*
			color = -1;
			for (int i = 0; i < 4; i++){
				workersHave[i][0] = 100;
				workersHave[i][1] = 0;
				workersNeed[i] = 0;
			}
			state = 0;

			bool hitedObject = false;
			bool needRestart = false;
			Initialize ();
			Debug.Log("RESETED");*/
		});
		Initialize ();
	}

	void Initialize(){
		
		for (int i = 0; i < 4; i++) {
			int tmp = workersHave [i] [0];
			workersHave [i] [0] -= System.Math.Max((workersNeed [i] - workersHave [i] [1]), 0);
			if (tmp != workersHave [i] [0]) {
				animators [i].SetTrigger ("CaveClicked");
			}
		}

		if (color != -1) {
			workersHave [color] [1] += 1;
		}

		manBlack.text = workersHave[0][0].ToString() + "\\" + workersHave[0][1];
		manRed.text = workersHave[1][0].ToString() + "\\" + workersHave[1][1];
		manGreen.text = workersHave[2][0].ToString() + "\\" + workersHave[2][1];
		manWhite.text = workersHave[3][0].ToString() + "\\" + workersHave[3][1];

		state = 0;

		for (int i = 0; i < 4; i++) {
			workersNeed[i] = rand.Next (5);
		}

		color = rand.Next (4);

		caveBlack.text = workersNeed [0].ToString ();
		caveRed.text = workersNeed [1].ToString ();
		caveGreen.text = workersNeed [2].ToString ();
		caveWhite.text = workersNeed [3].ToString ();

		switch (color) {
		case 0:
			rewardImg.color = Color.black;
			break;
		case 1:
			rewardImg.color = Color.red;
			break;
		case 2:
			rewardImg.color = Color.green;
			break;
		case 3:
			rewardImg.color = Color.white;
			break;
		default:
			rewardImg.color = Color.white;
			break;
		}
	}

	// Update is called once per frame
	void Update () {
		if (isLocked) {
			return;
		}
		if (needRestart) {
			restartBtn.gameObject.SetActive (needRestart);
			return;
		} else {
			restartBtn.gameObject.SetActive (false);
		}
		foreach (Touch touch in Input.touches) {
			HandleTouch(touch.fingerId, touch.position, touch.phase);
		}

		// Simulate touch events from mouse events
		if (Input.touchCount == 0) {
			if (Input.GetMouseButtonDown(0) ) {
				HandleTouch(10, Input.mousePosition, TouchPhase.Began);
			}
			if (Input.GetMouseButton(0) ) {
				HandleTouch(10, Input.mousePosition, TouchPhase.Moved);
			}
			if (Input.GetMouseButtonUp(0) ) {
				HandleTouch(10, Input.mousePosition, TouchPhase.Ended);
			}
		}
	}
	IEnumerator Delayer()
	{
		isLocked = true;
		yield return new WaitForSeconds (2);
		stateSprite.sprite = stateSprites[1];
		yield return new WaitForSeconds (2);
		stateSprite.sprite = stateSprites[2];
		yield return new WaitForSeconds (2);
		stateSprite.sprite = stateSprites[0];
		isLocked = false;
		particle.gameObject.SetActive (false);
	}
	private void HandleTouch(int touchFingerId, Vector3 touchPosition, TouchPhase touchPhase) {
		Ray ray = Camera.main.ScreenPointToRay(touchPosition);
		RaycastHit2D hit = new RaycastHit2D();
		hit = Physics2D.Raycast (Camera.main.ScreenToWorldPoint(touchPosition) , -Vector2.up, 50000);
		if (hit) {
			if(hit.collider.gameObject == this.gameObject)//If the object we hit is this object!
			{


				if(touchPhase == TouchPhase.Began)//Down If we started touching
				{
					hitedObject = true;
				}
			} 
			if(hitedObject == true)//If the object got toucheв
			{  
				if(touchPhase == TouchPhase.Ended)//If the touch ended
				{                    
					//PLAY ANIMATION

					particle.gameObject.SetActive (true);

					StartCoroutine (Delayer ());

					for (int i = 0; i <4; i++){
						if (workersHave [i] [0] + workersHave [i] [1] - workersNeed [i] < 0) {
							needRestart = true;
							return;
							}
					}


					Initialize();

					hitedObject = false;
				}
			}
		}
	}
}
