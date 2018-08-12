using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

namespace Gamekit2D
{
	public class MemoryCounter : MonoBehaviour
	{
		//public RandomAudioPlayer errorAudioPlayer;
		private bool firedAlready = false;
		private bool firstTime = false; // Disabling for now
		private float errorDelayTime = 3.1f;
		private int resetMemoryValue = 512;
		public int memory = 2498;
		public bool leaking = false;
		public GameObject errorCanvas;
		public GameObject errorText;
		public GameObject dialogueCanvas;
		public GameObject creditsCanvas;

		DialogueCanvasController dcc;
		Text txtOut;
		private string choose = "CHOOSE TO FORGET (1) or (2)";
		private string preface = "CLEARING SPACE TO AVOID SHUTDOWN\n\nDELETED:\n";
		private string shortpreface = "FORGOT:\n";

		private string[] funmems = {
			"WHAT YOU HAD FOR BREAKFAST",
			"YOUR NEPHEW'S BIRTHDAY\nEVERYTHING YOU LEARNED IN GRADE 3",
			"YOUR RECURRING NIGHTMARES WHERE YOU DON'T HAVE PANTS",
			"THAT TIME WHEN YOU TOLD A JOKE AND NOBODY LAUGHED,\nSO YOU TOLD IT AGAIN\nAND THEY SAID 'WE HEARD YOU'",
			"THE CAPITAL CITIES OF EVERY MAJOR WORLD COUNTRY",
			"THE PERIODIC TABLE",
			"THE LYRICS TO 'ALL STAR'"
		};

		private string[] sadmems = {
			"YOUR SECRET HANDSHAKE WITH YOUR LITTLE BROTHER",
			"YOUR MOTHER'S FACE",
			"THE SMELL OF YOUR GRANDPARENTS' HOUSE",
			"WHAT GETS YOU OUT OF BED IN THE MORNING",
			"YOUR FATHER'S FUNERAL",
		};

		private List<string> memories;
		bool endingGag = false;
		bool showedEndText = false;

		IEnumerator WaitCredits(float seconds) {
			yield return new WaitForSeconds (seconds);
			creditsCanvas.SetActive (true);
		}

		IEnumerator WaitPunish(float seconds) {
			//yield return new WaitForSeconds (playerDelayTime);
			yield return new WaitForSeconds (seconds);

			if (showedEndText) {
				errorCanvas.SetActive (false);
				dcc.ActivateCanvasWithText ("Motherfu-");
				dcc.DeactivateCanvasWithDelay (1.7f);
				StartCoroutine (WaitCredits(1.7f));
			} else {
				PlayerInput.Instance.GainControl ();
				errorCanvas.SetActive (false);
				memory = resetMemoryValue;
				firedAlready = false;
			}
		}

		// Use this for initialization
		void Start ()
		{
			dcc = dialogueCanvas.GetComponent<DialogueCanvasController> ();
			memories = new List<string>();
			txtOut = errorText.GetComponent<UnityEngine.UI.Text>();

			foreach (string mem in funmems) {
				memories.Add (mem);
			}

			foreach (string mem in sadmems) {
				memories.Add (mem);
			}

			InvokeRepeating ("CountDown", 0.0f, 0.007f);
		}
			

		void CountDown () {
			// Did the player choose a memory?
			if (Input.GetKeyDown (KeyCode.Alpha1)) {
				memory += 512;
			} else if (Input.GetKeyDown(KeyCode.Alpha2)) {
				memory += 512;
			}

			if (leaking) {
				if (memory > 0) {
					memory -= 1;
				} else {
					if (firedAlready) {
						return;
					}

					firedAlready = true;

					if (endingGag) {
						showedEndText = true;
						txtOut.text = shortpreface + "BUILDING CODE";
						errorCanvas.SetActive (true);
						PlayerInput.Instance.ReleaseControl (true);
						StartCoroutine (WaitPunish(2.0f));
					}
					else if (firstTime) {
						firstTime = false;

						if (memories.Count == 0) {
							txtOut.text = preface + "YOU'RE NOT SURE, BUT IT FEELS IMPORTANT";
						} else {
							int index = Random.Range (0, memories.Count);
							txtOut.text = preface + memories [index];
							memories.RemoveAt(index);
						}

						errorCanvas.SetActive (true);
						PlayerInput.Instance.ReleaseControl (false);
						StartCoroutine (WaitPunish(5.0f));
					}
					else {
						if (memories.Count == 0) {
							txtOut.text = shortpreface + "YOU'RE NOT SURE, BUT IT FEELS IMPORTANT";
						} else {
							int index = Random.Range (0, memories.Count);
							txtOut.text = shortpreface + memories [index];
							memories.RemoveAt(index);
						}

						errorCanvas.SetActive (true);
						PlayerInput.Instance.ReleaseControl (false);
						StartCoroutine (WaitPunish(errorDelayTime));
					}
				}
			}
		}

		public void SetMemory(int mem) {
			memory = mem;
		}

		public void SetLeaking(bool l) {
			leaking = l;
		}

		public void PrepEndingGag() {
			endingGag = true;
		}
	}
}
