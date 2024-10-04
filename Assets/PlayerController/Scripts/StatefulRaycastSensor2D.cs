using System;
using UnityEngine;

public class StatefulRaycastSensor2D: MonoBehaviour
{
	//Enum for different running modes defined inside the class to not clutter intellisense
	public enum RunMode { OnUpdate, OnInterval, OnRequest }

	//C# event used for automatic running modes to notify when the state changes if needed
	public event Action<bool> OnSensorStateChange;

	//Seriliazed fields for the designer to setup behaviour in editor - kept private for local manipulation
	[SerializeField] private RunMode m_RunMode;
	[SerializeField] private Vector2 m_SensorLocalVector;
	[SerializeField] private LayerMask m_LayerMask = 255;
	[SerializeField, Min(0f)] private float m_Interval;

	//tracker fields for realtime gathered data
	private float m_TimeSinceInterval;
	private RaycastHit2D m_HitInfo;

	//Always use awake to set up elements within this class - Start is used for second stage initialisation if needed (registering other classes)
	private void Awake()
	{
		//if the designer has selected interval anbd not supplied a valid interval then switch to update to not break things
		if(m_RunMode == RunMode.OnInterval && m_Interval <= 0f)
		{
			m_RunMode = RunMode.OnUpdate;
		}
	}

	private void Update()
	{
		//mutually exclusive behaviour based on enum value (simple state machine)
		switch (m_RunMode)
		{
			case RunMode.OnUpdate:
				RunCheck();
				break;
			case RunMode.OnInterval: //run this behaviour like a clock which ticks every interval
				m_TimeSinceInterval += Time.deltaTime;
				if(m_TimeSinceInterval >= m_Interval )
				{
					RunCheck();
					m_TimeSinceInterval -= m_Interval; //-= used so that the clock doesnt lose time from overshooting
				}
				break;
		}
	}

	//fucntion for running the check returns a bool and is public in case the user wants to run it manually
	public bool RunCheck()
	{
		//Get teh world-space versions of the ray start and vector
		Vector3 start = transform.position;
		Vector3 worldDir = transform.TransformVector(m_SensorLocalVector);

		//Run the raycast and store the result in a temporary variable
		RaycastHit2D newHitInfo = Physics2D.Raycast(start, worldDir.normalized, worldDir.magnitude + Mathf.Epsilon, m_LayerMask);

		if(newHitInfo.collider != m_HitInfo.collider)
		{ //the object being detected has changed but that doesnt mean the state has changed. We may still be detecting a hit just on a different object
			if((newHitInfo.collider != null) != (m_HitInfo.collider != null))
			{ //if we get here then the state has changed and the event will need to notify listeners
				OnSensorStateChange?.Invoke(newHitInfo.collider != null);
			}
			m_HitInfo = newHitInfo; //structs copy value in C# they arent pointers
		}

		return m_HitInfo.collider != null;
	}

	//getters done as the c# version of inline to save on needless stack allocations
	public bool HasDetectedHit() => m_HitInfo.collider != null;
	public float GetDistance() => m_HitInfo.distance;
	public Vector2 GetNormal() => m_HitInfo.normal;
	public Vector2 GetPosition() => m_HitInfo.point;
	public Collider2D GetCollider() => m_HitInfo.collider;

	//setters done in the same way as no extra pre-processing necessary on input values
	public void SetSensorLocalVector(Vector3 newVector) => m_SensorLocalVector = newVector;
	public void SetLayerMask(LayerMask newMask) => m_LayerMask = newMask;
	public void SetInterval(float newInterval) => m_Interval = newInterval;
}
