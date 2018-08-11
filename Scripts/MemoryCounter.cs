using UnityEngine;
using System.Collections;

public class MemoryCounter : MonoBehaviour
{
	public int memory = 2498;
	public bool leaking = false;

	// Use this for initialization
	void Start ()
	{
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (leaking) {
			if (memory > 0) {
				memory -= 1;
			}
		}
	}
}
		