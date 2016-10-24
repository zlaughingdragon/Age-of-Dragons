using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Camera))] //just to make sure you put this script ON THE CAMERA.
public class CameraLook : MonoBehaviour {
    

    public Vector3 offset, newPos;
    //target is the player object. Camera base point is the point in which the camera should snap back to once look around is disabled. 
    //An easy way to create the basepoint is to align the camera how you want, and then right click on the camera in the hierarchy (editor) and click "Create Empty."
    //this object will now be at the point you need. Make sure you drag it into the inspector for camerabasepoint.
    public Transform target, cameraBasePoint;
    public float x = 10, y = 20, z = 5, turnSpeedModifier;


	// Use this for initialization
	void Start () {

        
	
	}
	
	// Update is called once per frame
	void Update () {
        
        //The offset is provided by the floats x, y, and z. Change them in the inspector. Hit play for a live result, but save your numbers before you stop playing.
        offset = new Vector3(x, y, z);


        //-2.46, 0.48, -183.05

        
        if (Input.GetButtonUp("MoveCam") == true)
            transform.position = cameraBasePoint.position;
        else
        {
            
        }
    }

    void FixedUpdate()
    {
        
    }

    void LateUpdate()
    {
        transform.LookAt(target);
    }
}
