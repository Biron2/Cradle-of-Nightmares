using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementScript : MonoBehaviour
{
    [SerializeField] private RectTransform bugObj;
    [SerializeField] private RectTransform[] pathPoints;
    private int currentPointIndex = 0;
    public float rotationSpeed = 2f;
    public float moveSpeed = 2f;

    private bool isMoving = false;
    private Vector3 initialPosition;

    private void Start()
    {
        initialPosition = bugObj.transform.position;
    }

    private void Update()
    {
        if (!isMoving)
        {
            RotateTowardsNextPoint();
        }
        else
        {
            MoveToNextPoint();
        }
    }

    private void RotateTowardsNextPoint()
    {
        Vector3 targetPosition = pathPoints[currentPointIndex].position;
        Vector3 direction = (targetPosition - bugObj.transform.position).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(Vector3.forward, direction);

        float angle = Quaternion.Angle(bugObj.transform.rotation, targetRotation);
        if (angle <= 1f)
        {
            bugObj.transform.rotation = targetRotation;
            isMoving = true;
        }
        else
        {
            bugObj.transform.rotation = Quaternion.Lerp(bugObj.transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }

    private void MoveToNextPoint()
    {
        if (currentPointIndex < pathPoints.Length) // Check if currentPointIndex is within bounds
        {
            Vector3 targetPosition = pathPoints[currentPointIndex].position;
            float step = moveSpeed * Time.deltaTime;
            bugObj.transform.position = Vector3.MoveTowards(bugObj.transform.position, targetPosition, step);

            if (Vector3.Distance(bugObj.transform.position, targetPosition) <= 0.1f)
            {
                currentPointIndex++;
                if (currentPointIndex >= pathPoints.Length)
                {
                    StartCoroutine(TeleportToInitialPosition());
                }
                else
                {
                    isMoving = false;
                }
            }
        }
    }


    private IEnumerator TeleportToInitialPosition()
    {
        yield return new WaitForSeconds(1f);
        currentPointIndex = 0;
        bugObj.transform.position = initialPosition;
        isMoving = false;
    }
}