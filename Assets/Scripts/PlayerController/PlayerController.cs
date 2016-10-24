using UnityEngine;
using UnityEditor;
using System.Collections;

public class Options : EditorWindow
{
    float _maxSpeed = 0.8f, _speedMod = 120f, _jumpMod = 2.5f, _turnMod = 300f, _mass = 0.5f, _lift = 2f, _drag = 6f;
    //reset values
    float maxSpeedp = 0.8f, speedModp = 120f, jumpModp = 2.5f, turnModp = 300f, massp = 0.5f, liftp = 2f, dragp = 6f;
    Texture tex;
    bool XONLY = true, XANDY = false, YONLY = false, canApply = true;
    int bool1 = 1, bool2 = 0, bool3 = 0, axis = 0, axisp = 0;
    bool groupEnabled;
    string errorBool = "";
    [MenuItem("DragonController/Options")]
    static void Init()
    {
        //get existing or if none, create one.
        Options window = (Options)EditorWindow.GetWindow(typeof(Options));
        window.Show();
        window.minSize = new Vector2(500, 500);
    }

    void Start()
    {
        PlayerController pc = FindObjectOfType<PlayerController>();
        _maxSpeed = pc.maxSpeed; _speedMod = pc.speedModifier;
        _jumpMod = pc.jumpSpeedModifier; _turnMod = pc.turnSpeedModifier;
        _mass = pc.mass; _lift = pc.lift; _drag = pc.drag;
    }

    //private Texture2D logo = null;
    //void OnEnable()
    //{
    //    logo = (Texture2D)Resources.Load("dragon_pc_logo.jpg", typeof(Texture2D));
    //}
    void OnGUI()
    {


        GUILayout.Box("Dragon Player Controller | Options");
        GUILayout.Label("Modifiers", EditorStyles.boldLabel);
        //s = EditorGUILayout.TextField("Text Field", s);
        groupEnabled = EditorGUILayout.BeginToggleGroup("Customize Modifiers", groupEnabled);
        
        _maxSpeed = EditorGUILayout.Slider("Max Speed", _maxSpeed, 0.1f, 10f);
        _speedMod = EditorGUILayout.Slider("Speed Modifier", _speedMod, 0, 200f);
        _jumpMod = EditorGUILayout.Slider("Jump Modifier", _jumpMod, 0, 10f);
        _turnMod = EditorGUILayout.Slider("Turn Sensitivity Modifier", _turnMod, 0, 1000f);
        EditorGUILayout.Separator();
        
        _mass = EditorGUILayout.Slider("Mass", _mass, 0, 10f);
        _drag = EditorGUILayout.Slider("Drag", _drag, 0, 20f);
        _lift = EditorGUILayout.Slider("Lift", _lift, 0, 10f);

        EditorGUILayout.EndToggleGroup();

        EditorGUILayout.Separator();
        GUILayout.Label("Axis Selection", EditorStyles.boldLabel);
        EditorGUILayout.LabelField(errorBool);
        XONLY = EditorGUILayout.Toggle("X-Axis Rotation ONLY", XONLY);
        YONLY = EditorGUILayout.Toggle("Y-Axis Rotation ONLY", YONLY);
        XANDY = EditorGUILayout.Toggle("X-Axis AND Y-Axis", XANDY);

        EditorGUILayout.Separator();
        //reset all
        if (GUILayout.Button("Reset All Settings"))
        {
            PlayerController pc = FindObjectOfType<PlayerController>();

            XONLY = true;
            YONLY = false;
            XANDY = false;
            pc.maxSpeed = maxSpeedp;
            pc.speedModifier = speedModp;
            pc.turnSpeedModifier = turnModp;
            pc.jumpSpeedModifier = jumpModp;
            pc.lift = liftp;
            pc.drag = dragp;
            pc.mass = massp;
            pc.axis = axisp;
            _maxSpeed = maxSpeedp;
            _speedMod = speedModp;
            _turnMod = turnModp;
            _jumpMod = jumpModp;
            _lift = liftp;
            _drag = dragp;
            _mass = massp;
            axis = axisp;
        }

        EditorGUILayout.Separator();

        //apply all
        if (GUILayout.Button("Apply Settings") && canApply)
        {
            PlayerController pc = FindObjectOfType<PlayerController>();

            pc.speedModifier = _speedMod;
            pc.turnSpeedModifier = _turnMod;
            pc.jumpSpeedModifier = _jumpMod;
            pc.lift = _lift;
            pc.drag = _drag;
            pc.mass = _mass;
            pc.axis = axis;
        }
        
    }
    void Update()
    {
        if (XONLY)
        {
            axis = 0;
            bool1 = 1;
        }else { bool1 = 0; }
        if(YONLY)
        {
            axis = 1;
            bool2 = 1;
        }
        else { bool2 = 0; }
        if (XANDY)
        {
            axis = 2;
                bool3 = 1;
            }else { bool3 = 0; }
        
        if(bool1 + bool2 + bool3 > 1 || bool1 + bool2 + bool3 < 1)
        {
            canApply = false;
            errorBool = "Must have exactly one selection.";
        }
        else { errorBool = ""; canApply = true; }

        

    }
}


[RequireComponent(typeof(Rigidbody))] //ensure the object has a RigidBody.

public class PlayerController :  MonoBehaviour {

    /* note: if [serializefield] is preventing you from getting a variable that you need in another script, please
    set the desired variable to public outside of where it currently sits in the code. the use of a comma (,)
    saves space, but confuses some people. Using commas like public float var, var1; is the same 
    as saying public float var; over and over again.*/

    
   
    //FLOATS:

    //maxSpeed is the maximum speed your player can go. Play with it if results are undesireable.
    //currentSpeed is the current speed the player is going. Reference only.
    //speedModifier is the modifier to control the player's speed.
    //turnSpeedModifier is to control the sensitivity of the camera rotation.
    //jumpSpeedModifier is to control both how high and how fast the player jumps into the air. Note that the DRAG of the rigidbody (below)
    //should be how you control the descent.
    //offset X, Y, and Z are unused variables.
    //The camera object may show a Green squiggly line because we are only using this as reference in the script.
    [Header("Modifiers")]
    public float maxSpeed; public float currentSpeed, 
        speedModifier, turnSpeedModifier, jumpSpeedModifier, defaultTurnSpeedMod = 300, 
        mass = 0.5f, lift = 2f, drag = 6f;
    public GameObject cameraOBJ;

    

    //BOOLS:

    //isMoving has not been implimented.
    //canMove is for reference only, so it will give you an error in the inspector.
    //grounded is for getting whether the player is on the ground or not. BE SURE TO SET YOUR FLOOR WITH A TAG OF "ground"!
    //freeCam determines whether or not the player is using the free look with the button "MoveCam" set in the editor.
    [Header("Booleans")]
    [SerializeField]
    private bool isMoving;
    public bool canMove = true;
    [SerializeField]
    private bool grounded, freeCam;


    
    [Header("Please Select Look Axis")]

    [Range(0, 2)] //slider with a range between 0 and 2 (inclusive)
    [Tooltip("Use the slider to select axis. 0=x,1=y,2=x&y")]
    public int axis; //set by the slider
    [Tooltip("Note: Changing these fields does nothing. They are for reference.")]
    public string[] axisIndex = { "X ONLY", "Y ONLY", "X AND Y" }; //reference for user in editor
    

    private bool XONLY, XANDY, YONLY;
    

    //Rigidbody and Vector3s:

    [Header("Required Data")]
    [SerializeField]
    private Rigidbody rb;
    private Vector3 tmpRotation;
    
    void Awake()
    {
        //get the rigidbody
        rb = GetComponent<Rigidbody>();
        //feel free to change the mass here if you need to.
        rb.mass = mass;
        
    }

	// Use this for initialization
	void Start () {
        

        canMove = true;
    }

    void FixedUpdate()
    {
        #region movement -- GET AXIS provides a positive or negative 1 if player is pressing. Direction is based on this.

        //get current speed for reference
        currentSpeed = rb.velocity.magnitude;

        //if not free looking
        if (freeCam == false)
        {

            if (canMove)
            {
                //test for input and max speed 
                if (Input.GetAxis("Vertical") != 0 && rb.velocity.magnitude <= maxSpeed)
                    rb.AddForce(Input.GetAxis("Vertical") * (transform.forward * speedModifier), ForceMode.Acceleration); //add force for forward/backward

                //test for input and max speed
                if (Input.GetAxis("Horizontal") != 0 && rb.velocity.magnitude <= maxSpeed)
                    rb.AddForce(Input.GetAxis("Horizontal") * (transform.right * speedModifier), ForceMode.Acceleration); //add force for sideways


                // Jump is based on mass immensely. Play with the drag values below for best results.
                if (Input.GetButtonDown("Jump") && grounded == true)
                {

                    rb.AddForce(transform.up * jumpSpeedModifier, ForceMode.Impulse);
                }
                if (grounded == false)
                {
                    //speed modifier is multiplied by the default of 2 in the variable drag. Use this to adjust drag.
                    rb.drag -= (Time.deltaTime * jumpSpeedModifier * drag);

                    //Ensure drag is not negative
                    if (rb.drag <= 0) rb.drag = 0f;

                }// change the lift value for the amount of drag on lift-off
                else { rb.drag = lift; }

            }
        }

        else if((Input.GetAxis("Vertical") != 0 || Input.GetAxis("Horizontal") != 0) && !canMove) //if tries to move and canmove = false
        { Debug.Log("Please wait until action is finished."); }
        #endregion

        //old script packet I saved... ignore unless you're curious.
        #region mouselook DEPRICATED

        //if (Input.GetAxis("Mouse Y") != 0 || Input.GetAxis("Mouse X") != 0)
        //{
        //    Plane playerPlane = new Plane(Vector3.up, transform.position);

        //    Ray ray = camera.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);

        //    float hitdist = 0.0f;
        //    if (playerPlane.Raycast(ray, out hitdist))
        //    {
        //        Vector3 target = ray.GetPoint(hitdist);

        //            Quaternion targetRotation = Quaternion.LookRotation(target - transform.position);
        //            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 2f * (Time.deltaTime));
        //            camera.transform.rotation = Quaternion.Slerp(camera.transform.rotation, targetRotation, .00001f * (Time.deltaTime));


        //    }

        //}
        #endregion


        //script for the freelook camera
        #region freeCam
        if (Input.GetButtonDown("MoveCam")) //test for "MoveCam" button
        {
            //freeCam bool is used for movement bool as well
            freeCam = true;
            //this is just for reference
            canMove = false;

            //get the previous rotation. This is not being used, currently.
            tmpRotation = cameraOBJ.transform.eulerAngles;
            
        }
        else if(Input.GetButtonUp("MoveCam")) //if movecam button is released, do something.
        {
            canMove = true; freeCam = false;
            //position is set to the previous location
            //camera.transform.position = tmpLocation;
            cameraOBJ.transform.eulerAngles = tmpRotation;
        }

        #endregion

        #region        //Test which axis the user selected
        switch (axis)
        {
            case 0 :
                XONLY = true;
                YONLY = false;
                XANDY = false;
                    break;
            case 1 :
                XONLY = false;
                YONLY = true;
                XANDY = false;
                break;
            case 2 :
                XONLY = false;
                YONLY = false;
                XANDY = true;
                break;
        }
        #endregion

        #region X-ONLY
        if (XONLY)
        {
            //if the mouse is moving on the X axis and free look is NOT enabled
            if (Input.GetAxis("Mouse X") != 0 && freeCam == false)
            {
                //Rotate the player relative to mouse.  
                transform.Rotate(transform.up, turnSpeedModifier * Time.deltaTime * (Input.GetAxis("Mouse X")));



            }
            else
            {
                //if the mouse is moving on the X axis and free look IS enabled

                if (Input.GetAxis("Mouse X") != 0 && freeCam == true) //If you need to rotate vertically as well, please add the snippet below to the if statement.
                {//Optional Snippet: (|| Input.GetAxis("Mouse Y") != 0 ||) <-- Please insert between "("Mouse X") != 0" and "freecam == true"
                 //Rotate the camera AROUND the player (transform)
                    cameraOBJ.transform.RotateAround(transform.position, transform.up, (turnSpeedModifier * (Input.GetAxis("Mouse X")) * Time.deltaTime));
                    //Optional Snippet: (|| Input.GetAxis("Mouse Y") != 0 ||) <-- Please insert between "("Mouse X") != 0" and "Time.deltaTime"
                }
            }
        }
        #endregion

        #region Y-ONLY
        if (YONLY)
        {
            //if the mouse is moving on the X axis and free look is NOT enabled
            if ((Input.GetAxis("Mouse Y") != 0 || Input.GetAxis("Mouse X") != 0) && freeCam == false)
            {
                //Rotate the player relative to mouse.  
                transform.Rotate(transform.up, turnSpeedModifier * Time.deltaTime * (Input.GetAxis("Mouse X")));



            }
            else
            {
                //if the mouse is moving on the X axis and free look IS enabled

                if (Input.GetAxis("Mouse Y") != 0 && freeCam == true)
                {
                 //Rotate the camera AROUND the player (transform)
                    cameraOBJ.transform.RotateAround(transform.position, transform.right, (turnSpeedModifier * (Input.GetAxis("Mouse Y")) * Time.deltaTime));
                    
                }
            }
        }
        #endregion

        #region XANDY
        if (XANDY)
        {
            //if the mouse is moving on the X axis and free look is NOT enabled
            if ((Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0) && freeCam == false)
            {
                //Rotate the player relative to mouse.  
                transform.Rotate(transform.up, turnSpeedModifier * Time.deltaTime * (Input.GetAxis("Mouse X")));
                
            }
            else           //Optional Snippet: (|| Input.GetAxis("Mouse Y") != 0 ||) <-- Please insert between "("Mouse X") != 0" and "freecam == true"
            {
                //if the mouse is moving on the X axis and free look IS enabled

                if ((Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0) && freeCam == true) 
                {
                 //Rotate the camera AROUND the player (transform)
                    cameraOBJ.transform.RotateAround(transform.position, transform.up, (turnSpeedModifier * (Input.GetAxis("Mouse X")) * Time.deltaTime));
                    cameraOBJ.transform.RotateAround(transform.position, transform.right, (turnSpeedModifier * (Input.GetAxis("Mouse Y")) * Time.deltaTime));
                }
            }
        }
        #endregion
    }



    //test for ground
    void OnCollisionEnter(Collision other)
    {
        grounded = other.collider.CompareTag("ground");
    }
    void OnCollisionExit(Collision other)
    {
        grounded = !other.collider.CompareTag("ground");
    }

}
