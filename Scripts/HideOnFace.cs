using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideOnFace : MonoBehaviour {

	public GameObject girl;
	public bool flipX;
	private SpriteRenderer girlRenderer, dogRenderer;
	private Animator girlAnimator;

	// Use this for initialization
	void Start () {
		dogRenderer = gameObject.GetComponent (typeof(SpriteRenderer)).GetComponent<Renderer>() as SpriteRenderer;
		girlRenderer = girl.GetComponent (typeof(SpriteRenderer)).GetComponent<Renderer>() as SpriteRenderer;
		girlAnimator = girl.GetComponent (typeof(Animator)).GetComponent<Animator>() as Animator;
	}
	
	// Update is called once per frame
	void Update () {
		AnimatorClipInfo[] an = girlAnimator.GetCurrentAnimatorClipInfo (0);
		string clipName = "";

		if (an.Length > 0) {
			clipName = an [0].clip.name;
		}

		if (clipName != "Ellen_Crouch") {
			if (girlRenderer.flipX == flipX) {
				dogRenderer.enabled = false;
			} else {
				dogRenderer.enabled = true;

				//transform.position = basePosition + new Vector3(0.2f, 0.00f, 0.0f);
			}
		} else {
			if (girlRenderer.flipX != flipX) {
				dogRenderer.enabled = false;
			} else {
				dogRenderer.enabled = true;
			}
		}
	}
}
