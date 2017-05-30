using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FiringScriptTest : MonoBehaviour {
	
	[Header ("Play Tweaks")]

	public float chargeTime 				= 0.2f;
	public float timeBetweenShots 	= 2f;
	public float shotDuration 				= 1f;

	[Header ("Audio Clips Array")]
	public AudioClip[] audioClips;

	[Header ("Populate the gun barrel position!")]
	public Transform gunBarrel;

	[Header ("Debug information")]
	public bool ShowDebugInfo;
	[SerializeField]private int EnemiesInRangeCount; // just for debug
	[SerializeField]private Transform currentTarget;

	private Dictionary<int, Transform> EnemiesInRange = new Dictionary<int, Transform>();
	private List<int> enemyKeysList = new List<int>();
	private bool isFiring = false;
	private AudioSource myAudioSource_SFX;
	private LineRenderer lineRenderer;



	void Start () {
		myAudioSource_SFX = GetComponent<AudioSource>();
		lineRenderer = GetComponent<LineRenderer>();
	}
	

	void Update () {
		// for debug
		EnemiesInRangeCount = EnemiesInRange.Count; // just for debug

		if (EnemiesInRange.Count >0 && !isFiring)
		{
			enemyKeysList.Clear();
			foreach( int key in EnemiesInRange.Keys)
			{
				enemyKeysList.Add(key);
			}
	
			int selectedKey = enemyKeysList[Random.Range(0, enemyKeysList.Count)];
			currentTarget =  EnemiesInRange[selectedKey];
			#if UNITY_EDITOR
				if(ShowDebugInfo)
				Debug.Log(currentTarget);
			#endif
			StartCoroutine(FireAtEnemy(currentTarget));
		}
	}



	void OnTriggerEnter(Collider col)
	{
		if(!col.CompareTag("Damagable"))
		{ return; }
		int id = (col.gameObject.GetHashCode());
		#if UNITY_EDITOR
		if(ShowDebugInfo)
			Debug.Log(id + " Entered range");
		#endif
		if(!EnemiesInRange.ContainsKey(id))
		{
			EnemiesInRange.Add(id, col.gameObject.transform);
		}
	}


	void OnTriggerExit(Collider col)
	{
		if(!col.CompareTag("Damagable"))
		{ return; }
		int id = (col.gameObject.GetHashCode());
		#if UNITY_EDITOR
		if(ShowDebugInfo)
			Debug.Log(id + " left range");
		#endif
		if(!EnemiesInRange.ContainsKey(id))
			EnemiesInRange.Remove(id);
	}


	IEnumerator FireAtEnemy(Transform target)
	{
		isFiring = true;
		myAudioSource_SFX.clip = audioClips[0];
		myAudioSource_SFX.Play();
		while(myAudioSource_SFX.isPlaying)
		{
			yield return null;
		}
		// start the firing sound
		myAudioSource_SFX.clip = audioClips[1];
		myAudioSource_SFX.Play();

		// create the line now that firing sound is playing
		lineRenderer.enabled = true;
		lineRenderer.SetPosition(0, gunBarrel.position);

		// need to keep track of target as it moves
		while(myAudioSource_SFX.isPlaying)
		{
			lineRenderer.SetPosition(1, currentTarget.position);
			yield return null;
		}
		lineRenderer.enabled = false;
		yield return new WaitForSeconds(timeBetweenShots);
		isFiring = false;
	}
}
