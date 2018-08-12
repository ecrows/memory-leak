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
		// private bool firstTime = false; // Disabling for now
		private float errorDelayTime = 4.2f;
		private int resetMemoryValue = 512;
		public int memory = 2498;
		public bool leaking = false;

		private int choice1index = -1;
		private int choice2index = -1;

		public GameObject bsodCanvas;
		public GameObject bsodText;
		public GameObject errorCanvas;
		public GameObject errorText;
		public GameObject dialogueCanvas;
		public GameObject creditsCanvas;
		public GameObject choice1Obj;
		public GameObject choice2Obj;

		DialogueCanvasController dcc;
		Text txtOut;
		Text txtBSOD;

		Text txtChoice1;
		Text txtChoice2;
		private string preface = "CLEARING SPACE TO AVOID SHUTDOWN\n\nDELETED:\n";
		private string punishpreface = "OUT OF SPACE!\nDELETING RANDOM MEMORY...\nFORGOT:\n";
		private string shortpreface = "FORGOT:\n";

		// TODO: I really wanted to add gameplay affecting memories here
		// HOW TO JUMP
		// HOW TO TELL LEFT FROM RIGHT
		// HOW TO READ NUMBERS
		// etc...

		private string[] funmems = {
			"WHAT YOU HAD FOR BREAKFAST",
			"YOUR NEPHEW'S BIRTHDAY",
			"THAT NIGHTMARE WHERE YOU DON'T HAVE PANTS",
			"EVERYTHING YOU LEARNED IN GRADE 3",
			"THE CAPITAL CITIES OF EVERY MAJOR WORLD COUNTRY",
			"THE PERIODIC TABLE",
			"THE LYRICS TO 'ALL STAR'",
			"WHO BENOIT MANDELBROT IS",
			"HOW TO SPELL 'SCHADENFREUDE'",
			"YOUR FIRST KISS",
			"YOUR FAVOURITE COLOUR",
			"THE NAME OF YOUR FIRST PET",
			"YOUR BANK ACCOUNT PIN",
			"YOUR NEW YEARS' RESOLUTION",
			"WHAT YEAR IT IS",
			"ALL THE JOKES YOU'VE MEMORIZED",
			"HOW TO CROCHET",
			"WHY YOU MOVED TO HONG KONG"
		};

		private string[] sadmems = {
			"YOUR SECRET HANDSHAKE WITH YOUR LITTLE BROTHER",
			"YOUR MOTHER'S FACE",
			"THE SMELL OF YOUR GRANDPARENTS' HOUSE",
			"WHAT GETS YOU OUT OF BED IN THE MORNING",
			"YOUR FATHER'S FUNERAL",
			"WHEN OLEG WAS A PUPPY",
			"THE LAST TIME YOU SPOKE TO DAD",
			"BREAKING YOUR ARM WHEN YOU WERE SIX"
		};

		private List<string> memories;
		bool endingGag = false;
		bool showedEndText = false;

		IEnumerator WaitCredits(float seconds) {
			yield return new WaitForSeconds (seconds);
			creditsCanvas.SetActive (true);
		}

		IEnumerator WaitPunish(float seconds) {
			txtChoice1.text = "";
			txtChoice2.text = "";

			yield return new WaitForSeconds (seconds);

			if (showedEndText) {
				bsodCanvas.SetActive (false);
				dcc.ActivateCanvasWithText ("Motherfu-");
				dcc.DeactivateCanvasWithDelay (1.7f);
				StartCoroutine (WaitCredits(1.7f));
			} else {
				// Signal that we need new choices
				choice1index = -1;
				choice2index = -1;
				PlayerInput.Instance.GainControl ();
				bsodCanvas.SetActive (false);
				memory = resetMemoryValue;
				firedAlready = false;
			}
		}

		IEnumerator WaitNice(float seconds) {
			//yield return new WaitForSeconds (playerDelayTime);
			yield return new WaitForSeconds (seconds);

			// Signal that we need new choices
			choice1index = -1;
			choice2index = -1;

			// errorCanvas.SetActive (false);
			// memory = resetMemoryValue;
			firedAlready = false;
		}


		// Use this for initialization
		void Start ()
		{
			dcc = dialogueCanvas.GetComponent<DialogueCanvasController> ();
			memories = new List<string>();
			txtOut = errorText.GetComponent<UnityEngine.UI.Text>();
			txtBSOD = bsodText.GetComponent<UnityEngine.UI.Text>();
			txtChoice1 = choice1Obj.GetComponent<UnityEngine.UI.Text>();
			txtChoice2 = choice2Obj.GetComponent<UnityEngine.UI.Text>();

			foreach (string mem in funmems) {
				memories.Add (mem);
			}

			foreach (string mem in sadmems) {
				memories.Add (mem);
			}

			InvokeRepeating ("CountDown", 0.0f, 0.009f);
		}

		void Update()
		{
			if (txtChoice1.text != "" && !endingGag) {
				if (Input.GetKeyDown (KeyCode.Alpha1)) {
					memory += 512;

					txtOut.text = "FORGOT: " + txtChoice1.text;
					txtChoice1.text = "";
					txtChoice2.text = "";
					StartCoroutine (WaitNice (2.0f));
				} else if (Input.GetKeyDown (KeyCode.Alpha2)) {
					memory += 512;

					txtOut.text = "FORGOT: " + txtChoice2.text;
					txtChoice1.text = "";
					txtChoice2.text = "";

					StartCoroutine (WaitNice (2.0f));
				}
			}
		}

		void CountDown () {
			if (leaking) {

				// Reset choices here
				if (choice1index == -1) {
					txtOut.text = "CHOOSE A MEMORY TO DELETE:";

					if (memories.Count < 2) {
						choice1index = 1337;
						choice2index = 1337;
						txtChoice1.text = "1. YOU'RE NOT SURE BUT IT FEELS IMPORTANT";
						txtChoice2.text = "2. ANOTHER TREASURED CHILDHOOD MEMORY";
					} else {
						choice1index = Random.Range (0, memories.Count);
						txtChoice1.text = "1. " + memories [choice1index];
						memories.RemoveAt (choice1index);

						choice2index = Random.Range (0, memories.Count);
						txtChoice2.text = "2. " + memories [choice2index];
						memories.RemoveAt (choice2index);
					}
				}


				if (memory > 0) {
					memory -= 1;
				} else {
					if (firedAlready) {
						return;
					}

					firedAlready = true;

					if (endingGag) {
						showedEndText = true;
						txtBSOD.text = shortpreface + "BUILDING CODE";
						bsodCanvas.SetActive (true);
						PlayerInput.Instance.ReleaseControl (true);
						StartCoroutine (WaitPunish(2.4f));
					}
					else {
						if (memories.Count == 0) {
							txtBSOD.text = shortpreface + "YOU'RE NOT SURE, BUT IT FEELS IMPORTANT";
						} else {
							int index = Random.Range (0, memories.Count);
							txtBSOD.text = punishpreface + memories [index];
							memories.RemoveAt(index);
						}

						bsodCanvas.SetActive (true);
						PlayerInput.Instance.ReleaseControl (true);
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
