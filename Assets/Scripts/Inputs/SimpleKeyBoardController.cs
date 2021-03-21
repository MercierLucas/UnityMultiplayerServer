using UnityEngine;

public class SimpleKeyBoardController : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float speed;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        //rb.AddForce(new Vector3());
        if(rb.velocity.sqrMagnitude < speed)
        {
            Vector3 velocity = new Vector3(Input.GetAxis("Horizontal"),0f,Input.GetAxis("Vertical"));

            velocity*= Time.fixedDeltaTime*1000f;

            if(velocity.magnitude != 0f)
            {
                rb.AddForce(velocity);
            }
        }
    }
}