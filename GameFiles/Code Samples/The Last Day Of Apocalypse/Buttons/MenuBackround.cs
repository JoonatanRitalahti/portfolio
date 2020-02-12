using UnityEngine;

public class MenuBackround : MonoBehaviour
{
	// Fade the color from red to green
	// back and forth over the defined duration

	Color colorStart = new Color(0.1960784f,0,0);
	Color colorEnd = new Color(0.5f,0,0);
	float duration = 5.0f;
	Renderer rend;
	void Start()
	{
		rend = GetComponent<Renderer> ();
	}

	void Update()
	{
		
		float lerp = Mathf.PingPong(Time.time, duration) / duration;
		rend.material.color = Color.Lerp(colorStart, colorEnd, lerp);
	}
}