using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(SpriteRenderer))]

public class Tiling : MonoBehaviour {

    public int offsetX = 2;                 
    public bool hasARightBuddy = false;     
    public bool hasALeftBuddy = false;
    public bool reverseScale = false;   // used if object is not tilable
    public Transform parents;//////////
    private float spriteWidth = 0f;
    private Camera cam;
    private Transform myTransform;

    private void Awake()
    {
        cam = Camera.main;
        myTransform = transform;
    }
    // Use this for initialization
    void Start ()
    {
        SpriteRenderer sRenderer = GetComponent<SpriteRenderer>();
        spriteWidth = sRenderer.sprite.bounds.size.x;
	}
	
	// Update is called once per frame
	void Update ()
    {
		if (hasALeftBuddy == false || hasARightBuddy == false)
        {
            //calculate the camera extend (half width what camera can see in world coords)
            float camHorizontalExtend = cam.orthographicSize * Screen.width / Screen.height;

            //calculate x position where the camera can see edge of sprite
            float edgeVisiblePositionRight = (myTransform.position.x + spriteWidth / 2) - camHorizontalExtend;
            float edgeVisiblePositonLeft = (myTransform.position.x - spriteWidth / 2) + camHorizontalExtend;

            //check if edge of sprite can be seen then call MakeNewBuddy if possible
            if (cam.transform.position.x >= edgeVisiblePositionRight - offsetX && hasARightBuddy == false)
            {
                MakeNewBuddy(1);
                hasARightBuddy = true;
            }
            else if (cam.transform.position.x <= edgeVisiblePositonLeft + offsetX && hasALeftBuddy == false)
            {
                MakeNewBuddy(-1);
                hasALeftBuddy = true;
            }
        }
	}

    //function that creates sprite on side required
    void MakeNewBuddy (int rightOrLeft)
    {
        //calulate position of new buddy
        //Vector3 newPosition = new Vector3(myTransform.position.x + spriteWidth * rightOrLeft, myTransform.position.y, myTransform.position.z);
        Vector3 newPosition = new Vector3(myTransform.position.x + myTransform.localScale.x * spriteWidth * rightOrLeft, myTransform.position.y, myTransform.position.z);
        //instantiating new buddy and storing it in avariable
        Transform newBuddy = Instantiate(myTransform, newPosition, myTransform.rotation) as Transform;

        //if not tilable reverse the x size to mitigate seams
        if (reverseScale == true)
        {
            newBuddy.localScale = new Vector3(newBuddy.localScale.x * -1, newBuddy.localScale.y, newBuddy.localScale.z);
        }

        //newBuddy.parent = myTransform.parent;
        newBuddy.transform.parent = parents.transform;//////////

        if (rightOrLeft > 0)
        {
            newBuddy.GetComponent<Tiling>().hasALeftBuddy = true;
        }
        else
        {
            newBuddy.GetComponent<Tiling>().hasARightBuddy = true;
        }
    }
}
