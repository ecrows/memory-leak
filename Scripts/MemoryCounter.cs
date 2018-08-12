using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

namespace Gamekit2D
{
	public class MemoryCounter : MonoBehaviour
	{
		private bool firedAlready = false;
		private bool firstTime = true;
		private float errorDelayTime = 3.5f;
		private int resetMemoryValue = 256;
		public int memory = 2498;
		public bool leaking = false;
		public GameObject errorCanvas;
		public GameObject errorText;
		Text txtOut;
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
			"YOUR LITTLE BROTHER'S LAME SECRET HANDSHAKE",
			"YOUR MOTHER'S FACE",
			"THE SMELL OF YOUR GRANDPARENTS' HOUSE",
			"WHAT GETS YOU OUT OF BED IN THE MORNING",
			"YOUR FATHER'S FUNERAL",
		};

		private List<string> memories;
		bool endingGag = false;

		IEnumerator WaitPunish(float seconds) {
			//yield return new WaitForSeconds (playerDelayTime);
			yield return new WaitForSeconds (seconds);
			PlayerInput.Instance.GainControl ();
			errorCanvas.SetActive (false);
			memory = resetMemoryValue;
			firedAlready = false;
		}

		// Use this for initialization
		void Start ()
		{
			memories = new List<string>();
			txtOut = errorText.GetComponent<UnityEngine.UI.Text>();

			foreach (string mem in funmems) {
				memories.Add (mem);
			}

			foreach (string mem in sadmems) {
				memories.Add (mem);
			}

			InvokeRepeating ("CountDown", 0.0f, 0.04f);
		}

		void CountDown () {
			if (leaking) {
				if (memory > 0) {
					memory -= 1;
				} else {
					if (firedAlready) {
						return;
					}

					firedAlready = true;

					if (firstTime) {
						firstTime = false;

						if (memories.Count == 0) {
							txtOut.text = preface + "YOU'RE NOT SURE, BUT IT FEELS IMPORTANT";
						} else {
							int index = Random.Range (0, memories.Count);
							txtOut.text = preface + memories [index];
							memories.RemoveAt(index);
						}

						errorCanvas.SetActive (true);
						PlayerInput.Instance.ReleaseControl (true);
						StartCoroutine (WaitPunish(8.0f));
						return;
					}
					else if (endingGag) {
						txtOut.text = shortpreface + "BUILDING CODE";
					} else {
						if (memories.Count == 0) {
							txtOut.text = shortpreface + "YOU'RE NOT SURE, BUT IT FEELS IMPORTANT";
						} else {
							int index = Random.Range (0, memories.Count);
							txtOut.text = shortpreface + memories [index];
							memories.RemoveAt(index);
						}
					}

					errorCanvas.SetActive (true);
					PlayerInput.Instance.ReleaseControl (true);
					StartCoroutine (WaitPunish(errorDelayTime));
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
