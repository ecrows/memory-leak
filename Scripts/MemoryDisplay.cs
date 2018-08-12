using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Gamekit2D
{
	public class MemoryDisplay : MonoBehaviour
	{
		public GameObject girl;
		MemoryCounter mc;
		Text txt;
		public string units = "TB";

		// Use this for initialization
		void Start () {
			mc = girl.GetComponent <MemoryCounter>();
			txt = gameObject.GetComponent<UnityEngine.UI.Text>();
		}
		
		// Update is called once per frame
		void Update ()
		{
			txt.text = "Memory remaining: " + mc.memory + units;
		}

		public void setUnits(string u) {
			units = u;
		}
	}
}