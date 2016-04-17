using UnityEngine;
using System.Collections;

public class CaveController : MonoBehaviour {
	
	int state;
	int color;
	int[] workersNeed = { 0, 0, 0, 0 };
	public Sprite[] stateSprites;
	public SpriteRenderer stateSprite;
	// Use this for initialization
	void Start () {
		state = 0;
		stateSprite.sprite = stateSprites [state];
		System.Random rand = new System.Random ();
		for (int i = 0; i < 4; i++) {
			workersNeed[i] = rand.Next (4);
		}
		color = rand.Next (3);

	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
