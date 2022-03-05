using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{

    private float xVel;
    private float zVel;
    private Vector3 xPos;
    private Vector3 yPos;
    private Vector3 inputMovement;
    private float moveSpeed = 5;

    public bool canMove;
    private Vector3 currentPos;
    private Vector3 currentRot;

    public float thresholdChange = 0.05f;
    private Vector3 lastTransformPos;

    public AudioClip walkSound;
    public AudioClip jumpSound;

    public Animator cloudAnims;

    // Start is called before the first frame update
    void Start()
    {
        canMove = true;
        lastTransformPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.LookRotation(GetComponent<Rigidbody>().velocity);
        var diffVector = transform.position - lastTransformPos;
        if (diffVector.magnitude >= thresholdChange && transform.position.y <= 0.97f && canMove)
        {
            lastTransformPos = transform.position;
            if(!GetComponent<AudioSource>().isPlaying)
            {
                GetComponent<AudioSource>().PlayOneShot(walkSound);
                cloudAnims.SetTrigger("running");
            }
        }
        else
        {
            if(canMove)
            {
                cloudAnims.SetTrigger("idle");
            }
            
        }
        if (canMove == true)
        {
            inputMovement = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            inputMovement.Normalize();
            transform.LookAt(inputMovement + transform.position);
            transform.Translate(inputMovement * Time.deltaTime * moveSpeed, Space.World);

            if(Input.GetKeyDown(KeyCode.Space) && transform.position.y <= 0.97f)
            {
                this.GetComponent<Rigidbody>().AddForce(new Vector3(0, 10, 0), ForceMode.Impulse);
                GetComponent<AudioSource>().PlayOneShot(jumpSound);
                cloudAnims.SetTrigger("jump");
            }
        }
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Mower" && canMove == true)
        {
            canMove = false;
            StartCoroutine("RanOver");
        }
    }

    IEnumerator RanOver()
    {
        GetComponent<CapsuleCollider>().enabled = false;
        GetComponent<Rigidbody>().useGravity = false;
        cloudAnims.SetTrigger("fall");
        currentPos = transform.position;
        currentRot = transform.rotation.eulerAngles;
        //transform.rotation = Quaternion.Euler(0, 0, -90f);
        //transform.position = new Vector3(currentPos.x, currentPos.y - 0.5f, currentPos.z);
        yield return new WaitForSeconds(2f);
        cloudAnims.SetTrigger("getup");
        //transform.position = currentPos;
        //transform.rotation = Quaternion.Euler(0,0,0);
        GetComponent<CapsuleCollider>().enabled = true;
        GetComponent<Rigidbody>().useGravity = true;
        canMove = true;
    }

}
