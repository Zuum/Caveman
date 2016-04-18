using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class CaveController : MonoBehaviour {
	
	int state;
	int color;
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

	System.Random rand;


	// Use this for initialization
	void Start () {
		stateSprite.sprite = stateSprites [state];
		rand = new System.Random ();

	
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

		manBlack.text = workersHave[0][0].ToString() + "\\" + workersHave[0][1];
		manRed.text = workersHave[1][0].ToString() + "\\" + workersHave[1][1];
		manGreen.text = workersHave[2][0].ToString() + "\\" + workersHave[2][1];
		manWhite.text = workersHave[3][0].ToString() + "\\" + workersHave[3][1];

		state = 0;

		for (int i = 0; i < 4; i++) {
			workersNeed[i] = rand.Next (4);
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
		/*
		Vector3 touchPos;
		bool aTouch;
		if (Application.platform == RuntimePlatform.WindowsEditor) {
			aTouch = Input.GetMouseButton (0);

			touchPos = Input.mousePosition;
		} else {
			aTouch = Input.touches.Length > 0;
			touchPos = Input.touches [0];
		}
		Debug.Log ("Is touching: ", aTouch.ToString());
			
			//Vector3 touchPos3D = Camera.main.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, Camera.main.nearClipPlane)); //Touch to world!
			Ray ray = Camera.main.ScreenPointToRay(touchPos);
			RaycastHit hit = new RaycastHit();
			bool hitedObject = false;

		if (Physics.Raycast (ray, out hit, 50)) {
			Debug.DrawRay (ray.origin, ray.origin + ray.direction, Color.red);
		}
				/*if(hit.collider.gameObject == this.gameObject)//If the object we hit is this object!
				{
					if(touch.phase == TouchPhase.Began)//Down If we started touching
					{
						hitedObject = true;
					}
				}
			}  
			if(hitedObject == true)//If the object got toucheв
			{  
				if(touch.phase == TouchPhase.Ended)//If the touch ended
				{                    
					//PLAY ANIMATION


					workersHave [color] [1] += 1;

					Initialize();

					hitedObject = false;
				}
			}*/

		foreach (Touch touch in Input.touches) {
			HandleTouch(touch.fingerId, Camera.main.ViewportToWorldPoint(touch.position), touch.phase);
		}

		// Simulate touch events from mouse events
		if (Input.touchCount == 0) {
			if (Input.GetMouseButtonDown(0) ) {
				Debug.Log (Camera.main.ScreenToWorldPoint(Input.mousePosition));
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

	private void HandleTouch(int touchFingerId, Vector3 touchPosition, TouchPhase touchPhase) {
		Debug.Log ("1");
		Debug.Log (Camera.main.ScreenPointToRay(touchPosition));
		Ray ray = Camera.main.ScreenPointToRay(touchPosition);
		RaycastHit2D hit = new RaycastHit2D();
		Debug.DrawRay(ray.origin, ray.direction*100, Color.red);
		//Debug.Log (Physics2D.Raycast (Camera.main.ScreenToWorldPoint(touchPosition) , out (Vector2) hit, 50000));
		hit = Physics2D.Raycast (Camera.main.ScreenToWorldPoint(touchPosition) , -Vector2.up, 50000);
		if (hit) {
			Debug.Log ("2");
			if(hit.collider.gameObject == this.gameObject)//If the object we hit is this object!
			{
				Debug.Log ("3");


				if(touchPhase == TouchPhase.Began)//Down If we started touching
				{
					Debug.Log ("4");
					hitedObject = true;
				}
			} 
			if(hitedObject == true)//If the object got toucheв
			{  
				Debug.Log ("5");
				if(touchPhase == TouchPhase.Ended)//If the touch ended
				{                    
					//PLAY ANIMATION
					Debug.Log ("6");

					Initialize();

					workersHave [color] [1] += 1;

					hitedObject = false;
				}
			}
		}
	}
}
