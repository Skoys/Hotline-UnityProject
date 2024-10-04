using System;
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
    [SerializeField] GameObject other;
    [SerializeField] List<Vector4> pointPositions;

    [SerializeField] Vector4 closest;
    [SerializeField] int selected;
    [SerializeField] bool recalculate = true;
    [SerializeField] float distBtwPoints;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Keypad7))
        {
            recalculate = true;
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow) && selected - 1 >= 0)
        {
            selected -= 1;
            Debug.Log(selected % nbrOfPoints);
        }
        if (Input.GetKeyDown(KeyCode.RightArrow) && selected + 1 < pointPositions.Count)
        {
            selected += 1;
            Debug.Log(selected % nbrOfPoints);
        }
        if (Input.GetKeyDown(KeyCode.UpArrow) && selected + nbrOfPoints < pointPositions.Count)
        {
            selected += nbrOfPoints;
        }
        if (Input.GetKeyDown(KeyCode.DownArrow) && selected - nbrOfPoints >= 0)
        {
            selected -= nbrOfPoints;
        }

        if (recalculate)
        {
            CalculateNewNavmesh();
            CalculateClosest();
            CalculateTree(closest);
            Debug.DrawRay(GetTheClosest(other), new Vector3(0, 10, 0), Color.green, 10)
        }
    }

    private void CalculateNewNavmesh()
    {
        distBtwPoints = navMeshSize / nbrOfPoints;
        pointPositions = new List<Vector4>();
        Vector3 start = new Vector3(transform.position.x - ((nbrOfPoints - 1) * distBtwPoints) * 0.5f, transform.position.y, transform.position.z + ((nbrOfPoints - 1) * distBtwPoints) * 0.5f);
        RaycastHit hit;
        Vector3 current = new Vector3();
        for (int i = 0; i < nbrOfPoints; i++)
        {
            for (int j = 0; j < nbrOfPoints; j++)
            {
                current = new Vector3(start.x + i * distBtwPoints, start.y + navMeshHeight * 0.5f, start.z - j * distBtwPoints);
                if (Physics.Raycast(current, -transform.up * navMeshHeight, out hit, 2, 7))
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
                        Vector4 QtHitPoint = new Vector4(hit.point.x, hit.point.y, hit.point.z, 0);
                        pointPositions.Add(QtHitPoint);
                    }
                    else
                    {
                        pointPositions.Add(Vector4.positiveInfinity);
                    }
                }
                Debug.DrawRay(current, -transform.up * navMeshHeight, Color.red, 1);
            }
        }
        recalculate = false;
    }

    private void CalculateClosest()
    {
        closest = pointPositions[0];
        foreach (var point in pointPositions)
        {
            if (Vector3.Distance(new Vector3(point.x, point.y, point.z), target.transform.position) < Vector3.Distance(new Vector3(closest.x, closest.y, closest.z), target.transform.position))
            {
                closest = point;
            }
        }
        selected = pointPositions.IndexOf(closest);
    }

    private void CalculateTree(Vector4 current)
    {
        Vector3 currentPos = new Vector3(current.x, current.y, current.z);
        Vector3 nextPos = Vector3.zero;
        int place = pointPositions.IndexOf(current);
        int smallest = Convert.ToInt32(current.w);
        List<int> next = new List<int>();
        RaycastHit hit;

        if(place + nbrOfPoints < pointPositions.Count) //UP
        {
            int nextInd = place + nbrOfPoints;
            nextPos = new Vector3(pointPositions[nextInd].x, pointPositions[nextInd].y, pointPositions[nextInd].z);
            if (pointPositions[nextInd] != Vector4.positiveInfinity)
            {
                if (!Physics.Raycast(currentPos, (nextPos - currentPos).normalized, out hit, distBtwPoints, 7))
                {
                    if (pointPositions[nextInd].w == 0)
                    {
                        next.Add(nextInd);
                    }
                    else if (pointPositions[nextInd].w < smallest)
                    {
                        smallest = (int)pointPositions[nextInd].w + 1;
                    }
                }
            }
        }
        if (place - nbrOfPoints >= 0) //DOWN
        {
            int nextInd = place - nbrOfPoints;
            nextPos = new Vector3(pointPositions[nextInd].x, pointPositions[nextInd].y, pointPositions[nextInd].z);
            if (pointPositions[nextInd] != Vector4.positiveInfinity) 
            {
                if (!Physics.Raycast(currentPos, (nextPos - currentPos).normalized, out hit, distBtwPoints, 7))
                {
                    if (pointPositions[nextInd].w == 0)
                    {
                        next.Add(nextInd);
                    }
                    else if (pointPositions[nextInd].w < smallest)
                    {
                        smallest = (int)pointPositions[nextInd].w + 1;
                    }
                }
            }
            
        }
        if ((place - 1) % nbrOfPoints != 19 && (place - 1) % nbrOfPoints >= 0) //Left
        {
            int nextInd = place - 1;
            nextPos = new Vector3(pointPositions[nextInd].x, pointPositions[nextInd].y, pointPositions[nextInd].z);
            if (pointPositions[nextInd] != Vector4.positiveInfinity)
            {
                if (!Physics.Raycast(currentPos, (nextPos - currentPos).normalized, out hit, distBtwPoints, 7))
                {
                    if (pointPositions[nextInd].w == 0)
                    {
                        next.Add(nextInd);
                    }
                    else if (pointPositions[nextInd].w < smallest)
                    {
                        smallest = (int)pointPositions[nextInd].w;
                    }
                }
            }
        }
        if ((place + 1) % nbrOfPoints != 0 && (place + 1) % nbrOfPoints < pointPositions.Count) //RIGHT
        {
            int nextInd = place + 1;
            nextPos = new Vector3(pointPositions[nextInd].x, pointPositions[nextInd].y, pointPositions[nextInd].z);
            if (pointPositions[nextInd] != Vector4.positiveInfinity)
            {
                if (!Physics.Raycast(currentPos, (nextPos - currentPos).normalized, out hit, distBtwPoints, 7))
                {
                    if (pointPositions[nextInd].w == 0)
                    {
                        next.Add(nextInd);
                    }
                    else if (pointPositions[nextInd].w < smallest)
                    {
                        smallest = (int)pointPositions[nextInd].w;
                    }
                }
            }
        }

        current.w = smallest + 1;
        pointPositions[place] = current;
        foreach (var point in next)
        {
            pointPositions[point] = new Vector4(pointPositions[point].x, pointPositions[point].y, pointPositions[point].z, current.w + 1);
        }
        foreach (var point in next)
        {
            CalculateTree(pointPositions[point]);
        }
    }

    public Vector3 GetTheClosest(GameObject seeker)
    {
        RaycastHit hit;
        Vector3 v3Point = Vector3.zero;
        Vector4 ret = Vector4.positiveInfinity;
        foreach (var point in pointPositions)
        {
            v3Point = new Vector3(point.x, point.y, point.z);
            if(Physics.Raycast(seeker.transform.position, (v3Point - seeker.transform.position).normalized, out hit, 30))
            {
                if(hit.collider.gameObject == seeker.gameObject)
                {
                    if(point.w < ret.w && point.w != 0)
                    {
                        ret = point;
                    }
                }
            }
        }
        return new Vector3(ret.x, ret.y, ret.z);
    }

    private void OnDrawGizmos()
    {
        Vector3 v3Point = Vector3.zero;
        foreach (var point in pointPositions)
        {
            Gizmos.color = Color.HSVToRGB((0 + point.w) * 0.01f, 1, 1);
            v3Point = new Vector3(point.x, point.y, point.z);
            Gizmos.DrawWireSphere(v3Point, gizmosSize);
        }

        if(pointPositions.Count > 0)
        {
            Gizmos.color = Color.blue;
            v3Point = new Vector3(pointPositions[selected].x, pointPositions[selected].y, pointPositions[selected].z);
            Gizmos.DrawWireSphere(v3Point, gizmosSize * 1.5f);
        }

        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(transform.position, new Vector3(navMeshSize, navMeshHeight, navMeshSize));
    }
}
