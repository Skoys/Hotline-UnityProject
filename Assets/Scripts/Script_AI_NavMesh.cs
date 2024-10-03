using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Script_AI_NavMesh : MonoBehaviour
{
    [SerializeField] int nbrOfPoints = 3;
    [SerializeField] float navMeshSize = 1;
    [SerializeField] float navMeshHeight = 1;
    [SerializeField] float gizmosSize = 1;
    [SerializeField] GameObject target;
    [SerializeField] List<Quaternion> pointPositions;

    [SerializeField] Quaternion closest;
    [SerializeField] bool recalculate = true;
    [SerializeField] float distBtwPoints;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Keypad7))
        {
            recalculate = true;
        }

        if (recalculate)
        {
            CalculateNewNavmesh();
            CalculateClosest();
            CalculateTree(closest);
        }
    }

    private void CalculateNewNavmesh()
    {
        distBtwPoints = navMeshSize / nbrOfPoints;
        pointPositions = new List<Quaternion>();
        Vector3 start = new Vector3(transform.position.x - ((nbrOfPoints - 1) * distBtwPoints) * 0.5f, transform.position.y, transform.position.z + ((nbrOfPoints - 1) * distBtwPoints) * 0.5f);
        RaycastHit hit;
        Vector3 current = new Vector3();
        for (int i = 0; i < nbrOfPoints; i++)
        {
            for (int j = 0; j < nbrOfPoints; j++)
            {
                current = new Vector3(start.x + i * distBtwPoints, start.y + navMeshHeight * 0.5f, start.z - j * distBtwPoints);
                if (Physics.Raycast(current, -transform.up * navMeshHeight, out hit, 2))
                {
                    bool okToPlace = true;
                    Collider[] test = Physics.OverlapSphere(hit.point, gizmosSize);
                    foreach (Collider coll in test)
                    {
                        if (coll.tag == "Map")
                        {
                            okToPlace = false;
                            break;
                        }
                    }
                    if (okToPlace)
                    {
                        Quaternion QtHitPoint = new Quaternion(hit.point.x, hit.point.y, hit.point.z, 0);
                        pointPositions.Add(QtHitPoint);
                    }
                }
                Debug.DrawRay(current, -transform.up * navMeshHeight, Color.red, 10);
            }
        }
        recalculate = false;
    }

    private void CalculateClosest()
    {
        foreach (var point in pointPositions)
        {
            if (Vector3.Distance(new Vector3(point.x, point.y, point.z), target.transform.position) < Vector3.Distance(new Vector3(closest.x, closest.y, closest.z), target.transform.position))
            {
                closest = point;
            }
        }
        closest.w = 1;
    }

    private void CalculateTree(Quaternion current)
    {
        int place = pointPositions.IndexOf(current);
        int smallest = (int)current.w;
        List<Quaternion> next = new List<Quaternion>();
        if(place + nbrOfPoints > pointPositions.Count) //UP
        {
            if(Phys) //Add Ray
            if (pointPositions[place + nbrOfPoints].w == 0)
            {
                next.Add(pointPositions[place + nbrOfPoints]);
            }
            else if (pointPositions[place + nbrOfPoints].w < smallest)
            {
                smallest = (int)pointPositions[place + nbrOfPoints].w;
            }
        }
        if (place + nbrOfPoints > pointPositions.Count) //UP
        {
            if (pointPositions[place + nbrOfPoints].w == 0)
            {
                next.Add(pointPositions[place + nbrOfPoints]);
            }
            else if (pointPositions[place + nbrOfPoints].w < smallest)
            {
                smallest = (int)pointPositions[place + nbrOfPoints].w;
            }
        }


        current.w = smallest + 1;

        foreach (var point in next)
        {
            CalculateTree(point);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        foreach (var point in pointPositions)
        {
            Vector3 v3Point = new Vector3(point.x, point.y, point.z);
            Gizmos.DrawWireSphere(v3Point, gizmosSize);
        }
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(transform.position, new Vector3(navMeshSize, navMeshHeight, navMeshSize));
    }
}
