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
    public BodySourceManager _BodySource;
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
}
