using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Kinect = Windows.Kinect;

public class BodySourceView : MonoBehaviour 
{
    public Material BoneMaterial;
    public GameObject BodySourceManager;
    
    private Dictionary<ulong, GameObject> _Bodies = new Dictionary<ulong, GameObject>();
    private BodySourceManager _BodyManager;

    private float[] AngAttackL = new float[9];
    private float[] AngAttackR = new float[9];
    private float[] AngWalkL = new float[15];
    private float[] AngWalkR = new float[15];
    private float[] PosY = new float[9];
    private float[] HeightY = new float[9];

    private Dictionary<Kinect.JointType, Kinect.JointType> _BoneMap = new Dictionary<Kinect.JointType, Kinect.JointType>()
    {
        { Kinect.JointType.FootLeft, Kinect.JointType.AnkleLeft },
        { Kinect.JointType.AnkleLeft, Kinect.JointType.KneeLeft },
        { Kinect.JointType.KneeLeft, Kinect.JointType.HipLeft },
        { Kinect.JointType.HipLeft, Kinect.JointType.SpineBase },
        
        { Kinect.JointType.FootRight, Kinect.JointType.AnkleRight },
        { Kinect.JointType.AnkleRight, Kinect.JointType.KneeRight },
        { Kinect.JointType.KneeRight, Kinect.JointType.HipRight },
        { Kinect.JointType.HipRight, Kinect.JointType.SpineBase },
        
        { Kinect.JointType.HandTipLeft, Kinect.JointType.HandLeft },
        { Kinect.JointType.ThumbLeft, Kinect.JointType.HandLeft },
        { Kinect.JointType.HandLeft, Kinect.JointType.WristLeft },
        { Kinect.JointType.WristLeft, Kinect.JointType.ElbowLeft },
        { Kinect.JointType.ElbowLeft, Kinect.JointType.ShoulderLeft },
        { Kinect.JointType.ShoulderLeft, Kinect.JointType.SpineShoulder },
        
        { Kinect.JointType.HandTipRight, Kinect.JointType.HandRight },
        { Kinect.JointType.ThumbRight, Kinect.JointType.HandRight },
        { Kinect.JointType.HandRight, Kinect.JointType.WristRight },
        { Kinect.JointType.WristRight, Kinect.JointType.ElbowRight },
        { Kinect.JointType.ElbowRight, Kinect.JointType.ShoulderRight },
        { Kinect.JointType.ShoulderRight, Kinect.JointType.SpineShoulder },
        
        { Kinect.JointType.SpineBase, Kinect.JointType.SpineMid },
        { Kinect.JointType.SpineMid, Kinect.JointType.SpineShoulder },
        { Kinect.JointType.SpineShoulder, Kinect.JointType.Neck },
        { Kinect.JointType.Neck, Kinect.JointType.Head },
    };
    
    void Update () 
    {
        if (BodySourceManager == null)
        {
            return;
        }
        
        _BodyManager = BodySourceManager.GetComponent<BodySourceManager>();
        if (_BodyManager == null)
        {
            return;
        }
        
        Kinect.Body[] data = _BodyManager.GetData();
        if (data == null)
        {
            return;
        }
        
        List<ulong> trackedIds = new List<ulong>();
        foreach(var body in data)
        {
            if (body == null)
            {
                continue;
              }
                
            if(body.IsTracked)
            {
                trackedIds.Add (body.TrackingId);
            }
        }
        
        List<ulong> knownIds = new List<ulong>(_Bodies.Keys);
        
        // First delete untracked bodies
        foreach(ulong trackingId in knownIds)
        {
            if(!trackedIds.Contains(trackingId))
            {
                Destroy(_Bodies[trackingId]);
                _Bodies.Remove(trackingId);
            }
        }

        foreach(var body in data)
        {
            if (body == null)
            {
                continue;
            }
            
            if(body.IsTracked)
            {
                if(!_Bodies.ContainsKey(body.TrackingId))
                {
                    _Bodies[body.TrackingId] = CreateBodyObject(body.TrackingId);
                }
                
                RefreshBodyObject(body, _Bodies[body.TrackingId]);

                //Get all body part position
                Vector3 BodySpineBase = _Bodies[body.TrackingId].transform.GetChild(0).transform.position;
                Vector3 BodySpineMid = _Bodies[body.TrackingId].transform.GetChild(1).transform.position;
                Vector3 BodyHead = _Bodies[body.TrackingId].transform.GetChild(3).transform.position;
                Vector3 BodyShoulderLeft = _Bodies[body.TrackingId].transform.GetChild(4).transform.position;
                Vector3 BodyElbowLeft = _Bodies[body.TrackingId].transform.GetChild(5).transform.position;
                Vector3 BodyWristLeft = _Bodies[body.TrackingId].transform.GetChild(6).transform.position;
                Vector3 BodyHipLeft = _Bodies[body.TrackingId].transform.GetChild(12).transform.position;
                Vector3 BodyKneeLeft = _Bodies[body.TrackingId].transform.GetChild(13).transform.position;
                Vector3 BodyFootLeft = _Bodies[body.TrackingId].transform.GetChild(15).transform.position;
                Vector3 BodyHipRight = _Bodies[body.TrackingId].transform.GetChild(16).transform.position;
                Vector3 BodyKneeRight = _Bodies[body.TrackingId].transform.GetChild(17).transform.position;
                Vector3 BodyFootRight = _Bodies[body.TrackingId].transform.GetChild(19).transform.position;

                float BodyYPos = 0f;
                for (int i = 0; i < 25; i++)
                    BodyYPos += _Bodies[body.TrackingId].transform.GetChild(i).transform.position.y;

                //Get "Attack" angular velocity
                for (int i = AngAttackL.Length - 1; i > 0 ; i--)
                    AngAttackL[i] = AngAttackL[i - 1];
                AngAttackL[0] = Vector3.Angle(BodyShoulderLeft - BodyElbowLeft, BodyElbowLeft - BodyWristLeft);
                if (Mathf.Abs(AngAttackL[0] - AngAttackL[8]) > 20f)
                {
                    Global.SIGNAL_ATTACK_LEFT = true;
                    for (int i = AngAttackL.Length - 1; i > 0; i--)
                        AngAttackL[i] = AngAttackL[0];
                }
                else
                    Global.SIGNAL_ATTACK_LEFT = false;

                //Get "Walk Left" angular velocity
                for (int i = AngWalkL.Length - 1; i > 0; i--)
                    AngWalkL[i] = AngWalkL[i - 1];
                AngWalkL[0] = Vector3.Angle(BodySpineBase - BodySpineMid, BodySpineMid - BodyKneeLeft);
                if (Mathf.Abs(AngWalkL[0] - AngWalkL[14]) > 1.8f)
                    Global.SIGNAL_WALK_LEFT = true;
                else
                    Global.SIGNAL_WALK_LEFT = false;

                //Get "Walk Right" angular velocity
                for (int i = AngWalkR.Length - 1; i > 0; i--)
                    AngWalkR[i] = AngWalkR[i - 1];
                AngWalkR[0] = Vector3.Angle(BodySpineBase - BodySpineMid, BodySpineMid - BodyKneeRight);
                if (Mathf.Abs(AngWalkR[0] - AngWalkR[14]) > 1.8f)
                    Global.SIGNAL_WALK_RIGHT = true;
                else
                    Global.SIGNAL_WALK_RIGHT = false;

                //Get body "Normal Vector" angle
                Global.SIGNAL_STAND_ANGLE = Vector3.Dot((BodyHipLeft - BodyHipRight).normalized, new Vector3(0f, 0f, -1f));

                //Get "Jump" Y position change
                for (int i = PosY.Length - 1; i > 0; i--)
                    PosY[i] = PosY[i - 1];
                PosY[0] = BodyYPos;
                if (PosY[0] - PosY[8] > 5f)
                {
                    Debug.Log("Jump!! Val = " + (PosY[0] - PosY[8]));
                    Global.SIGNAL_JUMP = true;
                    for (int i = PosY.Length - 1; i > 0; i--)
                        PosY[i] = PosY[0];
                }
                else
                    Global.SIGNAL_JUMP = false;
            }
        }
    }
    
    private GameObject CreateBodyObject(ulong id)
    {
        GameObject body = new GameObject("Body:" + id);
        
        for (Kinect.JointType jt = Kinect.JointType.SpineBase; jt <= Kinect.JointType.ThumbRight; jt++)
        {
            GameObject jointObj = GameObject.CreatePrimitive(PrimitiveType.Cube);

            LineRenderer lr = jointObj.AddComponent<LineRenderer>();
            lr.SetVertexCount(2);
            lr.material = BoneMaterial;
            lr.SetWidth(0.05f, 0.05f);

            jointObj.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            jointObj.name = jt.ToString();
            jointObj.transform.parent = body.transform;
        }

        body.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);

        return body;
    }
    
    private void RefreshBodyObject(Kinect.Body body, GameObject bodyObject)
    {
        for (Kinect.JointType jt = Kinect.JointType.SpineBase; jt <= Kinect.JointType.ThumbRight; jt++)
        {
            Kinect.Joint sourceJoint = body.Joints[jt];
            Kinect.Joint? targetJoint = null;
            
            if(_BoneMap.ContainsKey(jt))
            {
                targetJoint = body.Joints[_BoneMap[jt]];
            }
            
            Transform jointObj = bodyObject.transform.Find(jt.ToString());
            jointObj.localPosition = GetVector3FromJoint(sourceJoint);
            
            LineRenderer lr = jointObj.GetComponent<LineRenderer>();
            if(targetJoint.HasValue)
            {
                lr.SetPosition(0, jointObj.localPosition * 0.5f);
                lr.SetPosition(1, GetVector3FromJoint(targetJoint.Value) * 0.5f);
                lr.SetColors(GetColorForState (sourceJoint.TrackingState), GetColorForState(targetJoint.Value.TrackingState));
            }
            else
            {
                lr.enabled = false;
            }
        }
    }
    
    private static Color GetColorForState(Kinect.TrackingState state)
    {
        switch (state)
        {
        case Kinect.TrackingState.Tracked:
            return Color.green;

        case Kinect.TrackingState.Inferred:
            return Color.red;

        default:
            return Color.black;
        }
    }
    
    private static Vector3 GetVector3FromJoint(Kinect.Joint joint)
    {
        return new Vector3(joint.Position.X * 10, joint.Position.Y * 10, joint.Position.Z * 10);
    }
}
