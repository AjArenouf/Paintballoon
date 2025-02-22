using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoyMovement : MonoBehaviour
{
    public Transform target;
    public Transform target2;
    public Transform target3;
    public Transform target4;
    private Transform currentTarget;

    public Transform cameraTransform;
    Animator animator;
  
    public GameObject projectilePrefab;
    public float projectileSpeed = 10f;
    public Transform shootPoint;
    public GameObject arCamera;

    private bool hasPlayerReachedPoint;

    // Start is called before the first frame update
    void Start()
    {
        currentTarget = target;
        animator = GetComponent<Animator>();
        arCamera = GameObject.FindGameObjectWithTag("MainCamera");
    }

    // Update is called once per frame
    void Update()
    {
        if (hasPlayerReachedPoint)
        {
            return;
        }

        float speed = 3f;
        float step = speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, currentTarget.position, step);

        if (transform.position == currentTarget.position)
        {
            if (currentTarget == target)
                currentTarget = target2;
            else if (currentTarget == target2)
                currentTarget = target3;
            else if (currentTarget == target3)
                currentTarget = target4;
            else if (currentTarget == target4)
                currentTarget = target;

        }

        Vector3 directionToTarget = currentTarget.position - transform.position;

        if (directionToTarget != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 0.1f);
            
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "ThrowState")
        {
            animator.SetTrigger("isColliding");
            hasPlayerReachedPoint = true;
            StartCoroutine(pauseAndLookAtPlayer());
        }
    }

    IEnumerator pauseAndLookAtPlayer()
    {
        Vector3 directionToCamera = cameraTransform.position - transform.position;
        directionToCamera.y = 0;
        Quaternion targetRotation = Quaternion.LookRotation(directionToCamera);
        transform.rotation = targetRotation;
        Shoot();
        yield return new WaitForSeconds(0.5f);
        hasPlayerReachedPoint = false;
        yield break;
    }

    private void Shoot()
    {
        GameObject BoyBalloon = Instantiate(projectilePrefab, shootPoint.position, shootPoint.rotation);

        Vector3 direction = (arCamera.transform.position - transform.position).normalized;

        Rigidbody BoyBalloonRigidbody = BoyBalloon.GetComponent<Rigidbody>();
        BoyBalloonRigidbody.velocity = direction * projectileSpeed;
    }

}


