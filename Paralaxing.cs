using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paralaxing : MonoBehaviour {

    public Transform[] backgrounds; //Array of all the back and foregrounds to be parallaxed
    public float smoothing = 1f;    //How smooth the parallx is going to be.  Set above 0

    private float[] parallaxScales; //The proportion of the camera's movement to move the backgrounds by
    private Transform cam;          //Refrence to the main camera transform
    private Vector3 previousCamPos; //Position of the camera in the previous frame

    //called before Start().  Used for refrences.
    void Awake()
    {
        //set up camera refrence
        cam = Camera.main.transform;
    }

    // Use this for initialization
    void Start ()
    {
        //The previous frame had the current frame's camera positon
        previousCamPos = cam.position;

        //Assingning corresponding parallaxScales
        parallaxScales = new float[backgrounds.Length];
        for (int i = 0; i < backgrounds.Length; i++)
        {
            parallaxScales[i] = backgrounds[i].position.z * -1;
        }
	}
	
	// Update is called once per frame
	void Update ()
    {
        //The parallax is the opposite of the camera movement because the previous frame is multiplied by the scale
		for (int i = 0; i < backgrounds.Length; i++)
        {
            float parallax = (previousCamPos.x - cam.position.x) * parallaxScales[i];

            //set a target x position which is the current position plus the parallax
            float backgroundTargetPosX = backgrounds[i].position.x + parallax;

            //Create a target position which is the background's current position with it's target x position
            Vector3 backgroundTargetPos = new Vector3(backgroundTargetPosX, backgrounds[i].position.y, backgrounds[i].position.z);

            //Lerp between background and target positions
            backgrounds[i].position = Vector3.Lerp(backgrounds[i].position, backgroundTargetPos, smoothing * Time.deltaTime);
        }

        //set previous camera positon to the camera's position at end of frame
        previousCamPos = cam.position;
	}
}
