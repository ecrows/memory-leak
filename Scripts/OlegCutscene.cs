using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gamekit2D
{
	public class OlegCutscene : MonoBehaviour {

		DialogueCanvasController dcc;
		public GameObject doggo;
		public GameObject dialogueCanvas;
		public Sprite happyDog;
		public GameObject olegGetter;
		private SpriteRenderer dogRenderer;


		// Use this for initialization
		void Start () {
			dcc = dialogueCanvas.GetComponent<DialogueCanvasController> ();
			dogRenderer = doggo.GetComponent (typeof(SpriteRenderer)).GetComponent<Renderer>() as SpriteRenderer;
		}
		
		// Update is called once per frame
		void Update () {
			
		}

		IEnumerator WaitPickupDoggo(float seconds) {
			yield return new WaitForSeconds (seconds);
			PlayerInput.Instance.GainControl ();
			olegGetter.SetActive (true);
			gameObject.SetActive (false);
		}

		IEnumerator WaitBark(float seconds) {
			yield return new WaitForSeconds (seconds);
			dogRenderer.sprite = happyDog;
			// TODO: Play happy bark
			// TODO: Make Rin smile
			dcc.ActivateCanvasWithText ("Alright, I guess you can come with me!");
			dcc.DeactivateCanvasWithDelay (3.5f);
			StartCoroutine (WaitPickupDoggo (4.5f));
		}

		public void RunCutscene() {
			PlayerInput.Instance.ReleaseControl (true);
			dcc.ActivateCanvasWithText ("Ha.  Who's a cute dog?  Who do you belong to?");
			dcc.DeactivateCanvasWithDelay (3.2f);
			StartCoroutine (WaitBark(6.0f));
		}
	}
}