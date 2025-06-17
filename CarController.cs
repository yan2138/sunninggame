using UnityEngine;

public class KinematicCarMove : MonoBehaviour
{
    public float moveSpeed = 15f;    
    public float turnSpeed = 40f;    
    public float wheelSpinSpeed = 30f; 
    public Transform[] wheels;       
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = false;
    }

    void FixedUpdate()
    {
        float verticalInput = Input.GetAxis("Vertical");  
        float horizontalInput = Input.GetAxis("Horizontal"); 

        Vector3 moveDirection = transform.forward * verticalInput;
        Vector3 targetPosition = transform.position + moveDirection * moveSpeed * Time.fixedDeltaTime;

        rb.MovePosition(targetPosition);

        float turnAngle = horizontalInput * turnSpeed * Time.fixedDeltaTime;
        transform.Rotate(0, turnAngle, 0);

        if (wheels != null && wheels.Length > 0)
        {
            float wheelSpin = Mathf.Abs(verticalInput) * moveSpeed * wheelSpinSpeed;
            foreach (Transform wheel in wheels)
            {
                wheel.Rotate(Vector3.right, wheelSpin * Time.fixedDeltaTime);
            }
        }
    }
}