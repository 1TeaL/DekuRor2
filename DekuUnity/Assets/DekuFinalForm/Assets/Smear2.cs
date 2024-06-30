using System.Collections.Generic;
using UnityEngine;

public class Smear2 : MonoBehaviour
{
    Queue<Vector3> _recentPositions = new Queue<Vector3>();
    Vector3 motionVector;

    [SerializeField]
    int _frameLag = 0;
    [SerializeField]
    Transform targetTransform;


    Material _smearMat = null;
    public Material smearMat
    {
        get
        {
            if (!_smearMat)
                _smearMat = GetComponent<Renderer>().material;
            return _smearMat;
        }
    }

    void Start()
    {
        _smearMat = GetComponent<Renderer>().material;
    }


    void LateUpdate()
    {

        if (_recentPositions.Count > _frameLag)
        {
            motionVector = targetTransform.position - _recentPositions.Dequeue();
            smearMat.SetVector("_SmearDirection", motionVector);
        }


        _recentPositions.Enqueue(targetTransform.position);
    }

}
