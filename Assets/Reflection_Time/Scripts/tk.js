//this script is attached to plane in intro scene

private var disappear:boolean=true;

function Update () {

if(disappear){
GetComponent.<Renderer>().material.color.a += 0.3 *Time.deltaTime; //increases plane's alpha
}

if(GetComponent.<Renderer>().material.color.a >= 0.99){ 
disappear=false; 
}

if(!disappear){
GetComponent.<Renderer>().material.color.a -= 0.3 *Time.deltaTime; //decreases plane's alpha
}

if(GetComponent.<Renderer>().material.color.a <= 0.01 && !disappear){
//when alpha <= 0.01 and dissapear is false,
//load level 1 (loading scene), defined in build settings in File menu
    Application.LoadLevel(1);
    // Deactivates the game object.
    gameObject.SetActive (false);
}


}