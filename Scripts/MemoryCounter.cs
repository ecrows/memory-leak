using UnityEngine;
using System.Collections;

public class MemoryCounter : MonoBehaviour
{
	public int memory = 2498;
	public bool leaking = false;

	// Use this for initialization
	void Start ()
	{
		InvokeRepeating ("CountDown", 0.0f, 0.5f);
	}

	void CountDown () {
		if (leaking) {
			if (memory > 0) {
				memory -= 1;
			}
		}
	}

	public void SetMemory(int mem) {
		memory = mem;
	}

	public void SetLeaking(bool l) {
		leaking = l;
	}
}
