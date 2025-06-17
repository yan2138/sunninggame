using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    [Header("Follow Settings")]
    public Transform target;              

    [Header("Position Settings")]
    public float distance = 5.0f;         
    public float height = 2.0f;           
    public float positionDamping = 3.0f;  

    [Header("Rotation Settings")]
    public float rotationDamping = 3.0f;  
    public float defaultAngle = 15.0f;    

    [Header("Collision Settings")]
    public bool enableCollision = true;   
    public LayerMask collisionMask;       
    public float minDistance = 1.0f;      

    private Vector3 _targetPosition;      
    private float _currentDistance;      

    void Start()
    {
        if (target == null)
        {
            Debug.LogError("No follow target set!");
            enabled = false;
        }

        _currentDistance = distance;
        collisionMask = ~0; 
    }

    void LateUpdate()
    {
        if (target == null) return;

        Vector3 wantedPosition = target.position
                               - target.forward * distance
                               + Vector3.up * height;

        if (enableCollision)
        {
            RaycastHit hit;
            Vector3 rayDirection = wantedPosition - target.position;

            if (Physics.Raycast(target.position, rayDirection.normalized, out hit,
                               distance, collisionMask))
            {
                _currentDistance = Mathf.Clamp(hit.distance * 0.9f, minDistance, distance);
            }
            else
            {
                _currentDistance = distance;
            }

            wantedPosition = target.position
                          - target.forward * _currentDistance
                          + Vector3.up * height;
        }

        transform.position = Vector3.Lerp(transform.position,
                                         wantedPosition,
                                         positionDamping * Time.deltaTime);

        Quaternion wantedRotation = Quaternion.LookRotation(
            target.position - transform.position +
            (target.up * Mathf.Tan(defaultAngle * Mathf.Deg2Rad)),
            target.up);

        transform.rotation = Quaternion.Slerp(transform.rotation,
                                            wantedRotation,
                                            rotationDamping * Time.deltaTime);
    }
}