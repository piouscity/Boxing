using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Windows.Kinect;
using Microsoft.Kinect.VisualGestureBuilder;

public class GestureSourceManager : MonoBehaviour
{
    public struct EventArgs
    {
        public string name;
        public float confidence;

        public EventArgs(string _name, float _confidence)
        {
            name = _name;
            confidence = _confidence;
        }
    }
    public Material BoneMaterial;
    public GameObject Root;
    public GameObject Hip_Center;
    public GameObject Spine;
    public GameObject Shoulder_Center;
    public GameObject Head;
    public GameObject Collar_Left;
    public GameObject Shoulder_Left;
    public GameObject Elbow_Left;
    public GameObject Wrist_Left;
    public GameObject Hand_Left;
    public GameObject Collar_Right;
    public GameObject Shoulder_Right;
    public GameObject Elbow_Right;
    public GameObject Wrist_Right;
    public GameObject Hand_Right;
    public GameObject Hip_Left;
    public GameObject Thigh_Left;
    public GameObject Knee_Left;
    public GameObject Ankle_Left;
    public GameObject Foot_Left;
    public GameObject Hip_Right;
    public GameObject Thigh_Right;
    public GameObject Knee_Right;
    public GameObject Ankle_Right;
    public GameObject Foot_Right;
    public GameObject Neck;

    public BodySourceManager _BodySource;
    public GameObject Player;
    public string databasePath;
    public double confidence = 0.4;
    private KinectSensor _Sensor;
    private VisualGestureBuilderFrameSource _Source;
    private VisualGestureBuilderFrameReader _Reader;
    private VisualGestureBuilderDatabase _Database;
    public KinectQueue KinectQueue;
    private Body currentBody;
    private GameObject bodyObject;
    private bool isMonitor = false;

    private Dictionary<JointType, JointType> _BoneMap = new Dictionary<JointType, JointType>()
    {
        { JointType.FootLeft, JointType.AnkleLeft },
        { JointType.AnkleLeft, JointType.KneeLeft },
        { JointType.KneeLeft, JointType.HipLeft },
        { JointType.HipLeft, JointType.SpineBase },

        { JointType.FootRight, JointType.AnkleRight },
        { JointType.AnkleRight, JointType.KneeRight },
        { JointType.KneeRight, JointType.HipRight },
        { JointType.HipRight, JointType.SpineBase },

        { JointType.HandTipLeft, JointType.HandLeft },
        { JointType.ThumbLeft, JointType.HandLeft },
        { JointType.HandLeft, JointType.WristLeft },
        { JointType.WristLeft, JointType.ElbowLeft },
        { JointType.ElbowLeft, JointType.ShoulderLeft },
        { JointType.ShoulderLeft, JointType.SpineShoulder },

        { JointType.HandTipRight, JointType.HandRight },
        { JointType.ThumbRight, JointType.HandRight },
        { JointType.HandRight, JointType.WristRight },
        { JointType.WristRight, JointType.ElbowRight },
        { JointType.ElbowRight, JointType.ShoulderRight },
        { JointType.ShoulderRight, JointType.SpineShoulder },

        { JointType.SpineBase, JointType.SpineMid },
        { JointType.SpineMid, JointType.SpineShoulder },
        { JointType.SpineShoulder, JointType.Neck },
        { JointType.Neck, JointType.Head },
    };

    public bool IsMonitor
    {
        get { return isMonitor; }
        set {
            _BodySource.IsMonitor = value;
            isMonitor = value;
            /*if (!value)
            {
                _Source.TrackingId = 0;
                _Reader.IsPaused = true;
            }*/
        }
    }

    // Use this for initialization
    void Start()
    {
        bodyObject = new GameObject("body");
        CreateBodyObject();

        _Sensor = KinectSensor.GetDefault();
        if (_Sensor != null)
        {

            if (!_Sensor.IsOpen)
            {
                _Sensor.Open();
            }

            // Set up Gesture Source
            _Source = VisualGestureBuilderFrameSource.Create(_Sensor, 0);

            // open the reader for the vgb frames
            _Reader = _Source.OpenReader();
            if (_Reader != null)
            {
                _Reader.IsPaused = true;
                _Reader.FrameArrived += GestureFrameArrived;
            }

            // load the gestures from the gesture database
            string path = System.IO.Path.Combine(Application.streamingAssetsPath, databasePath);
            //Debug.Log(path);
            _Database = VisualGestureBuilderDatabase.Create(path);
            //Debug.Log(_Database);

            // Load all gestures
            IList<Gesture> gesturesList = _Database.AvailableGestures;
            for (int g = 0; g < gesturesList.Count; g++)
            {
                Gesture gesture = gesturesList[g];
                _Source.AddGesture(gesture);
            }

        }
    }

    // Public setter for Body ID to track
    public void SetBody(ulong id)
    {
        if (id > 0)
        {
            _Source.TrackingId = id;
            _Reader.IsPaused = false;
        }
        else
        {
            _Source.TrackingId = 0;
            _Reader.IsPaused = true;
        }
    }

    // Update Loop, set body if we need one
    void Update()
    {
        if (isMonitor && !_Source.IsTrackingIdValid)
        {
            FindValidBody();
        }
        Mapping();
    }

    // Check Body Manager, grab first valid body
    void FindValidBody()
    {

        if (_BodySource != null)
        {
            Body[] bodies = _BodySource.GetData();
            if (bodies != null)
            {
                foreach (Body body in bodies)
                {
                    if (body.IsTracked)
                    {
                        currentBody = body;
                        SetBody(body.TrackingId);
                        break;
                    }
                }
            }
        }

    }

    private void CreateBodyObject()
    {
        for (JointType jt = JointType.SpineBase; jt <= JointType.ThumbRight; jt++)
        {
            GameObject jointObj = GameObject.CreatePrimitive(PrimitiveType.Cube);

            LineRenderer lr = jointObj.AddComponent<LineRenderer>();
            lr.SetVertexCount(2);
            lr.material = BoneMaterial;
            lr.SetWidth(0.05f, 0.05f);


            jointObj.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
            jointObj.name = jt.ToString();
            jointObj.transform.parent = bodyObject.transform;
        }
    }

    /// Handles gesture detection results arriving from the sensor for the associated body tracking Id
    private void GestureFrameArrived(object sender, VisualGestureBuilderFrameArrivedEventArgs e)
    {
        VisualGestureBuilderFrameReference frameReference = e.FrameReference;
        using (VisualGestureBuilderFrame frame = frameReference.AcquireFrame())
        {
            if (frame != null)
            {
                // get the discrete gesture results which arrived with the latest frame
                IDictionary<Gesture, DiscreteGestureResult> discreteResults = frame.DiscreteGestureResults;

                if (discreteResults != null)
                {
                    foreach (Gesture gesture in _Source.Gestures)
                    {
                        if (gesture.GestureType == GestureType.Discrete)
                        {
                            DiscreteGestureResult result = null;
                            discreteResults.TryGetValue(gesture, out result);
                            if (result != null)
                            {
                                // Fire Event
                                //Debug.Log("Detected Gesture " + gesture.Name + " with Confidence " + result.Confidence);
                                if (result.Detected == true && result.Confidence >= confidence)
                                {
                                    KinectQueue.GestureQueue.Enqueue(gesture.Name);
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    void Mapping()
    {
        CreateBodyObject();
        if (_Source.IsTrackingIdValid)
        {
            for (JointType jt = JointType.SpineBase; jt <= JointType.ThumbRight; jt++)
            {
                //Debug.Log("real " + jt.ToString());
                var sourceJoint = currentBody.Joints[jt];
                Windows.Kinect.Joint? targetJoint = null;

                if (_BoneMap.ContainsKey(jt))
                {
                    //Debug.Log(jt.ToString());
                    targetJoint = currentBody.Joints[_BoneMap[jt]];
                }

                Transform jointObj = bodyObject.transform.Find(jt.ToString());
                jointObj.localPosition = GetVector3FromJoint(sourceJoint);

                LineRenderer lr = jointObj.GetComponent<LineRenderer>();
                if (targetJoint.HasValue)
                {
                    lr.SetPosition(0, jointObj.localPosition);
                    lr.SetPosition(1, GetVector3FromJoint(targetJoint.Value));
                    lr.SetColors(Color.green, Color.black);
                }
                else
                {
                    lr.enabled = false;
                }
            }
      
        }
    }
    private static Vector3 GetVector3FromJoint(Windows.Kinect.Joint joint)
    {
        return new Vector3(joint.Position.X * 3 - 3, joint.Position.Y * 3 + 4, joint.Position.Z * 3 - 0.6f);
    }
    Vector3 VectorMap(CameraSpacePoint pos)
    {
        return new Vector3(pos.X, pos.Y, pos.Z);
    }
}
