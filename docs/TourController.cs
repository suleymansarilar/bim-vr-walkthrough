using UnityEngine;

public class TourController : MonoBehaviour
{
    [Header("Path")]
    public Transform[] waypoints;

    [Header("Movement")]
    public float moveSpeed = 2.5f;          
    public float rotateSpeed = 3.5f;        
    public KeyCode startKey = KeyCode.Space;

    [Header("Vertical/Physics")]
    public float gravity = 9.81f;           
    public float maxStepUpSpeed = 3.0f;     

    [Header("Stuck/Failsafe")]
    public float arriveDist = 0.30f;        
    public float arriveHorizDist = 0.20f;   
    public float arriveYDist = 0.20f;       
    public float stuckTimeout = 1.5f;       
    public float progressEps = 0.02f;       

    [Header("Camera")]
    Camera tourCamera;
    float originalFov;
    Quaternion originalCameraRotation;
    public float tourFov = 75f;              
    public float pitchOffsetDeg = 0f;        

    int index = -1;
    bool running;

    CharacterController characterController;
    MonoBehaviour fpsController;            
    float verticalVelocity;                 

    
    float lastDist = float.MaxValue;
    float stuckTimer = 0f;

    void Start()
    {
        characterController = GetComponent<CharacterController>();

       
        tourCamera = GetComponentInChildren<Camera>();
        if (tourCamera != null)
        {
            originalFov = tourCamera.fieldOfView;
            originalCameraRotation = tourCamera.transform.localRotation;
        }

        
        var typed = GetComponent<FPSController>();
        if (typed != null) fpsController = typed;
        else
        {
            var byName = GetComponent("FPSController") as MonoBehaviour;
            if (byName != null) fpsController = byName;
        }
    }

    void Update()
    {
        
        if (!running)
        {
            if (Input.GetKeyDown(startKey) && waypoints != null && waypoints.Length > 0)
            {
                running = true;
                index = 0;
                if (fpsController) fpsController.enabled = false;

               
                if (tourCamera != null)
                {
                    tourCamera.fieldOfView = tourFov;
                    tourCamera.transform.localRotation = Quaternion.Euler(pitchOffsetDeg, 0f, 0f);
                }

                
                lastDist = float.MaxValue;
                stuckTimer = 0f;
                verticalVelocity = 0f;
            }
            else
            {
                return;
            }
        }

        
        if (index < 0 || index >= waypoints.Length)
        {
            running = false;
            if (fpsController) fpsController.enabled = true;

            
            if (tourCamera != null)
            {
                tourCamera.fieldOfView = originalFov;
                tourCamera.transform.localRotation = originalCameraRotation;
            }

            return;
        }

        Transform target = waypoints[index];
        Vector3 to = target.position - transform.position;

        
        float horizDist = new Vector3(to.x, 0f, to.z).magnitude;
        float totalDist = to.magnitude;
        if (totalDist < arriveDist || (horizDist < arriveHorizDist && Mathf.Abs(to.y) < arriveYDist))
        {
            index++;
            if (index >= waypoints.Length)
            {
                running = false;
                if (fpsController) fpsController.enabled = true;

                
                if (tourCamera != null)
                {
                    tourCamera.fieldOfView = originalFov;
                    tourCamera.transform.localRotation = originalCameraRotation;
                }
            }
            
            lastDist = float.MaxValue;
            stuckTimer = 0f;
            return;
        }

       
        if (totalDist > lastDist - progressEps)
            stuckTimer += Time.deltaTime;
        else
            stuckTimer = 0f;

        lastDist = totalDist;

        if (stuckTimer > stuckTimeout)
        {
            index++;
            if (index >= waypoints.Length)
            {
                running = false;
                if (fpsController) fpsController.enabled = true;

               
                if (tourCamera != null)
                {
                    tourCamera.fieldOfView = originalFov;
                    tourCamera.transform.localRotation = originalCameraRotation;
                }
            }
            stuckTimer = 0f;
            lastDist = float.MaxValue;
            return;
        }

        
        Vector3 horiz = new Vector3(to.x, 0f, to.z);
        Vector3 horizDir = horiz.sqrMagnitude > 1e-6f ? horiz.normalized : Vector3.zero;

        
        float desiredDy = Mathf.Clamp(to.y, -maxStepUpSpeed * Time.deltaTime, maxStepUpSpeed * Time.deltaTime);

        
        if (characterController != null && !characterController.isGrounded)
            verticalVelocity -= gravity * Time.deltaTime;
        else if (Mathf.Abs(to.y) < 0.05f)
            verticalVelocity = 0f;

        
        Vector3 move =
            horizDir * moveSpeed * Time.deltaTime +
            new Vector3(0f, desiredDy, 0f) +
            new Vector3(0f, verticalVelocity * Time.deltaTime, 0f);

        
        if (characterController != null) characterController.Move(move);
        else transform.position += move;

        
        if (horizDir.sqrMagnitude > 1e-6f)
        {
            Quaternion look = Quaternion.LookRotation(horizDir, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, look, rotateSpeed * Time.deltaTime);
        }

        
        if (running && tourCamera != null)
        {
            
            tourCamera.transform.localRotation = Quaternion.Euler(pitchOffsetDeg, 0f, 0f);
        }
    }
}