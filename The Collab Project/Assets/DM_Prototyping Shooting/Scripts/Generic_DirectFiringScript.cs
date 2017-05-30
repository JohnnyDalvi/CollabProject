using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
[RequireComponent(typeof(CapsuleCollider))]

public class Generic_DirectFiringScript : MonoBehaviour {


	[Header ("Play Tweaks")]
	public float chargeTime 				= 	1f;
	public float timeBetweenShots 		= 	3f;
	public float shotDuration 				= 	2f;
	public float cooldownDuration		= 2f;

	[Header ("Audio Clips Array")]
	public AudioClip[] audioClips;
	[Header ("Populate the gun barrel position!")]
	public Transform gunBarrel;


	[Header ("Debug information")]
	public bool viewDebugInfo = false;
	[SerializeField]private int EnemiesInRangeCount;
	List<int> enemyKeysList = new List<int>();
	[SerializeField]private int currentTarget;
//	private enum FiringStateEnum	{ Charging, Firing, CoolingDown, Idle };
//	[SerializeField] private FiringStateEnum 	FiringState;

	// This dictionary has the unique instance ID as the key, and the Transform as the value
	private Dictionary<int, Transform> EnemiesInRange = new Dictionary<int, Transform>();

	// flag that is used within firing coroutine for tracking state.
	private bool 								isFiring = false;
	private AudioSource 					myAudioSource_SFX;
	private LineRenderer 					lineRenderer;
	private float								internalTimer;



	void Start () {
		myAudioSource_SFX 			= GetComponent<AudioSource>();
		lineRenderer 						= GetComponent<LineRenderer>();
//		FiringState 							= 	FiringStateEnum.Idle;
	}


	void Update () {
		EnemiesInRangeCount = EnemiesInRange.Count; // just for debug

		// so each update check that we are not currently firing, and also if there is anything in range to select from.
		if (EnemiesInRange.Count >0 && !isFiring)
		{
			// need to create a list of all the keys since we cant randomly pick a dictionary entry, just used to get new target

			enemyKeysList.Clear();
			foreach( int key in EnemiesInRange.Keys) //add all the keys in the dictionary to a list
			{	enemyKeysList.Add(key);		}
			// now select a key in the list at random
			int selectedKey = enemyKeysList[Random.Range(0, enemyKeysList.Count)];

			currentTarget = selectedKey;
			// and start firing at that target using the coroutine, passing in the Transform of the target selected
			StartCoroutine(FireAtEnemy());
		}
	}


	// When an enemy enters the firing range, add it to the dictionary with its unique ID and its Transform
	void OnTriggerEnter(Collider col)
	{
		if(!col.CompareTag("Damagable"))
		{ return; }

		int id = (col.gameObject.GetHashCode());
		if(!EnemiesInRange.ContainsKey(id))
		{
			EnemiesInRange.Add(id, col.gameObject.transform);
		}
	}

	// when an emeny leaves the firing range, ensure its ID is in the dictionary and remove it.
	void OnTriggerExit(Collider col)
	{
		if(!col.CompareTag("Damagable"))
		{ return; }
		int id = (col.gameObject.GetHashCode());


		if(EnemiesInRange.ContainsKey(id))
			{ EnemiesInRange.Remove(id);}

		if(currentTarget == id) 
			{currentTarget = 0;}
	}

//
	IEnumerator FireAtEnemy()
	{
		isFiring = true;
		lineRenderer.enabled = true;
		while (currentTarget !=0)
		{
			DrawFiringGraphic(gunBarrel, EnemiesInRange[currentTarget]);
			yield return null;
		}
		lineRenderer.enabled = false;
		yield return new WaitForSeconds(timeBetweenShots);
		isFiring = false;
	}





	//------------------------------------------------------------------
	//                             private methods 
	//------------------------------------------------------------------

	private void PlaySFX(AudioClip clipToPlay)
	{
		myAudioSource_SFX.clip = clipToPlay;
		myAudioSource_SFX.Play();
	}

	void DrawFiringGraphic(Transform LR_From, Transform LR_To)
	{
		lineRenderer.SetPosition(0, LR_From.position);
		lineRenderer.SetPosition(1, LR_To.position);
	}


	// just to show the range sphere as a gizmo in play mode.
	void OnDrawGizmos()
	{
		#if UNITY_EDITOR
			if(viewDebugInfo)
				Gizmos.DrawWireSphere(transform.position,GetComponent<CapsuleCollider>().radius*2);
		#endif
	}
}
