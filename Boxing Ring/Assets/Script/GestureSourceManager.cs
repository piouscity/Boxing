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
    private bool isMonitor = false;
    public bool IsMonitor
    {
        get { return isMonitor; }
        set {
            _BodySource.IsMonitor = value;
            isMonitor = value;
            if (!value)
            {
                _Source.TrackingId = 0;
                _Reader.IsPaused = true;
            }
        }
    }

    // Use this for initialization
    void Start()
    {
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
            Debug.Log(path);
            _Database = VisualGestureBuilderDatabase.Create(path);
            Debug.Log(_Database);

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
                        SetBody(body.TrackingId);
                        break;
                    }
                }
            }
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
        //GameObject tanımlanmış mı kontrolü yapılır.
        if (_BodySource == null)
        {
            return;

        }
        //BodySourceManager scripti içerisindeki fonksiyon çağırılarak body değerleri alınır.
        Windows.Kinect.Body[] data = _BodySource.GetData();

        //Data değerleri başarıyla atanmışmı kontrolü yapılır.
        if (data == null)
        {
            return;
        }
        //Takip edilebilir kaç kişi var ise onun id numarası kayıt altına alınır.
        List<ulong> trackedIds = new List<ulong>();
        foreach (var body in data)
        {
            if (body == null)
            {
                continue;
            }
            if (body.IsTracked)
            {
                trackedIds.Add(body.TrackingId);
                var pos = body.Joints[JointType.Head].Position;
                Head.transform.position = new Vector3(pos.X, pos.Y, Player.transform.position.z);
                pos = body.Joints[JointType.Neck].Position;
                Neck.transform.position = new Vector3(pos.X, pos.Y, Player.transform.position.z);
                pos = body.Joints[JointType.ShoulderLeft].Position;
                Shoulder_Left.transform.position = new Vector3(pos.X, pos.Y, Player.transform.position.z);
                pos = body.Joints[JointType.ShoulderRight].Position;
                Shoulder_Right.transform.position = new Vector3(pos.X, pos.Y, Player.transform.position.z);
                pos = body.Joints[JointType.SpineBase].Position;
                Spine.transform.position = new Vector3(pos.X, pos.Y, Player.transform.position.z);
            }
        }
    }
}
