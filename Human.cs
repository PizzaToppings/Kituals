using UnityEngine;
using System.Collections;

public class Human : MonoBehaviour {

	public Transform[] trWaypoints;     // Waypoints where the humans walk to
	public float flSpeed;               // movementspeed for the humans
	Transform tr;                       
	int currentArea;                    // The current location of the humans
	public int newArea;                 // the location the human is supposed to go
	int travelArea;                     // the location where the human will move toward (in order to reach newArea)                   
	Animator ator;                      
	AudioSource aus;
	public GameObject ExcMark;          // Exclamation mark above the head of the humans, which indicates that they are being called

	public string strCurrentRoom;       // name of the current room
	public string strPreviousRoom;      // name of the previous room 

	// Use this for initialization
	void Start ()
    {
		tr = transform;
		ator = GetComponent<Animator>();
		aus = GetComponent<AudioSource>();
		currentArea = 0;
		newArea = 0;
		travelArea = 0;
	}

    void Update()
    {
        if (currentArea != newArea)
        {
            ator.SetBool("isWalking", true);
            aus.volume = 1;
        }
        else
        {
            ator.SetBool("isWalking", false);
            aus.volume = 0;
        }

        if (tr.position.x < (trWaypoints[travelArea].position.x - 0.01f))
        {
            transform.localScale = Vector3.one - Vector3.right * 2;

        }
        else if (tr.position.x > (trWaypoints[travelArea].position.x + 0.01f))
        {
            transform.localScale = Vector3.one;
        }

        setCurrentArea();
    }

    // start moving to a new location
    public void doChangePos (int _intPos)
	{
		newArea = _intPos;

		ExcMark.SetActive (true);
		CancelInvoke ("setExcMark");
		Invoke ("setExcMark", 1);

		setTravelArea ();
		StopCoroutine ("moveToArea");
		StartCoroutine ("moveToArea");
	}

    // set the exclamation mark if the human is called by the cat
	void setExcMark()
	{
		ExcMark.SetActive (false);
	}

    /*
    Indicate where to move the human, based on the current location and final location
    if the location is reached and its not the final location, it will be called again, thus creating a path
    */
	void setTravelArea()
	{
        travelArea = trWaypoints[currentArea].GetComponent<Waypoints>().SetNextWayPoint(newArea);
	}

    // set the currentArea is the travelArea has been reached
	void setCurrentArea()
	{
		if (tr.position == trWaypoints [travelArea].position)
        {
			currentArea = travelArea;
            setTravelArea();
		}
	}

    // Move through the travelAreas untill newArea has been reached
	IEnumerator moveToArea()
	{
		while (tr.position != trWaypoints [newArea].position)
        {
			tr.position = Vector3.MoveTowards (tr.position, trWaypoints [travelArea].position, flSpeed * Time.deltaTime);
			yield return 0;
		}
	}

    // change the room the human is in. Prevents cats from doing actions in that area, and the human cant go to that room
	void OnTriggerEnter2D(Collider2D _col)
	{
		if (_col.gameObject.layer == 9)
        {
			doChangeCurrentRoom(_col.gameObject.name);
		}
	}

    // leave a room, setting the current room as the previous room. Used to 'free' the room from the human, allowing actions for cats and the other human
	void OnTriggerExit2D(Collider2D _col)
	{
		if (_col.gameObject.layer == 9 && _col.gameObject.name == strCurrentRoom)
        {
			doChangeCurrentRoom("");
		}
	}

    // set the current room, based on entering or leaving
	void doChangeCurrentRoom(string _strRoom)
    {
		if ( _strRoom == "" )
        {
			strCurrentRoom = strPreviousRoom;
		}
        else
        {
			strPreviousRoom = strCurrentRoom;
			strCurrentRoom = _strRoom;
		}
	}
}