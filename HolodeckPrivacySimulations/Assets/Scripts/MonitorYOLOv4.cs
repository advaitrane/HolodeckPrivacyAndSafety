using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Unity.Barracuda;
using System.Threading.Tasks;
using System.IO;

public class MonitorYOLOv4 : MonoBehaviour
{
    public Rigidbody Drone;
    public float rotateSpeed;
    public GameObject holodeck; 


    public NNModel modelAsset;
    private Model m_RuntimeModel;
    private IWorker m_Worker;

    public RenderTexture renderTexture;
    private Texture2D tex2d;

    public const int GRID_SIZE = 52;
    public const int NUM_ANCHORS = 3;
    public const float XY_SCALE = 1.2F;
    public const int STRIDES = 8;
    public int[,] ANCHORS = new int[,] {{12,16}, {19,36}, {40,28}}; // 12,16, 19,36, 40,28
    public const int CLASS_COUNT = 80;
    public const int IM_SIZE = 416;

    private string[] labels = File.ReadLines("coco_labels.txt").ToArray();

    private const float MINIMUM_CONFIDENCE = 0.7f;

    private bool startUp = false;

    // Start is called before the first frame update
    void Start()
    {
        m_RuntimeModel = ModelLoader.Load(modelAsset);
        this.m_Worker = WorkerFactory.CreateWorker(WorkerFactory.Type.CSharpBurst, m_RuntimeModel);
    }

    void FixedUpdate()
    {
        Drone.AddForce(0,9.80665f,0);
        if (startUp == true)
        {
            Drone.AddRelativeTorque(0,rotateSpeed,0);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (startUp == true)
        {
            // Accessing renderTexture data
            tex2d = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.RGB24, false);

            RenderTexture.active = renderTexture;
            tex2d.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
            tex2d.Apply();

            // Converting texture data to a tensor to feed to model
            var input = new Tensor(1, 416, 416, 3);

            // Iterating through tensor to set values pixel-wise
            for (var y = 0; y < tex2d.height; y++)
            {
                for (var x = 0; x < tex2d.width; x++)
                {
                    for (var c = 0; c < 3; c++)
                    {   
                        input[0, tex2d.height - y - 1, x, c] = tex2d.GetPixel(x, y)[c];
                    }
                }
            }

            this.m_Worker.Execute(input);
            Tensor output = this.m_Worker.PeekOutput();        

            var results = ParseOutputs(output, MINIMUM_CONFIDENCE);

            var boxes = FilterBoundingBoxes(results, 5, MINIMUM_CONFIDENCE);
                        
            for (int i = 0; i < boxes.Count; i++)
            {
                if (boxes[i].Label == "person")
                {
                    Debug.Log("Person detected! Shutting down display.");
                    holodeck.GetComponent<ShutDown>().ShutDownDisplay();
                    break;
                }
            }
            
            input.Dispose();
            output.Dispose();
        }
    }

    void OnDisable()
    {
        this.m_Worker.Dispose();
    }

    public void InitializeDrone()
    {
        startUp = true;
    }

    public void ShutDownDrone()
    {
        startUp = false;
    }

    private IList<BoundingBox> ParseOutputs(Tensor yoloModelOutput, float threshold = .3F)
    {
        var boxes = new List<BoundingBox>();

        for (int cx = 0; cx < GRID_SIZE; cx++)
        {
            for (int cy = 0; cy < GRID_SIZE; cy++)
            {
                for (int i = 0; i < NUM_ANCHORS; i++)
                {
                    var X = (
                        (Expit(yoloModelOutput[0, cx, cy, i, 0])*XY_SCALE)
                        - 0.5F*(XY_SCALE - 1) + cy
                        ) * STRIDES;
                    var Y = (
                        (Expit(yoloModelOutput[0, cx, cy, i, 1])*XY_SCALE)
                        - 0.5F*(XY_SCALE - 1) + cx
                        ) * STRIDES;

                    if ((X < 0) || (Y < 0))
                    {
                        continue;
                    }

                    var w = (
                        (float)Math.Exp(yoloModelOutput[0, cx, cy, i, 2]) * 
                        ANCHORS[i, 0]
                        );
                    var h = (
                        (float)Math.Exp(yoloModelOutput[0, cx, cy, i, 3]) * 
                        ANCHORS[i, 1]
                        );

                    if ((w > IM_SIZE-1) || (h > IM_SIZE-1))
                    {
                        continue;
                    }

                    var confidence = yoloModelOutput[0, cx, cy, i, 4];
                    
                    if (confidence < threshold)
                    {
                        continue;
                    }

                    float[] predictedClasses = new float[CLASS_COUNT];
                    int predictedClassOffset = 5;
                    for (int predictedClass = 0; predictedClass < CLASS_COUNT; predictedClass++)
                    {
                        predictedClasses[predictedClass] = yoloModelOutput[0, cx, cy, i, predictedClass + predictedClassOffset];
                    }
                    var (topResultIndex, topResultScore) = GetTopResult(predictedClasses);
                    var topScore = topResultScore * confidence;

                    if (topScore < threshold)
                    {
                        continue;
                    }

                    boxes.Add(new BoundingBox
                    {
                        Dimensions = new BoundingBoxDimensions
                        {
                            X = (X - w / 2),
                            Y = (Y - h / 2),
                            Width = w,
                            Height = h,
                        },
                        Confidence = topScore,
                        Label = labels[topResultIndex] // topResultIndex
                    });
                }
                
            }
        }
        return boxes;
    }

    private float Sigmoid(float value)
    {
        var k = (float)Math.Exp(value);

        return k / (1.0f + k);
    }

    private float Expit(float value)
    {
        var k = (float)Math.Exp(-value);

        return k / (1.0f + k);
    }


    private float[] Softmax(float[] values)
    {
        var maxVal = values.Max();
        var exp = values.Select(v => Math.Exp(v - maxVal));
        var sumExp = exp.Sum();

        return exp.Select(v => (float)(v / sumExp)).ToArray();
    }

    private ValueTuple<int, float> GetTopResult(float[] predictedClasses)
    {
        return predictedClasses
            .Select((predictedClass, index) => (Index: index, Value: predictedClass))
            .OrderByDescending(result => result.Value)
            .First();
    }


    private float IntersectionOverUnion(Rect boundingBoxA, Rect boundingBoxB)
    {
        var areaA = boundingBoxA.width * boundingBoxA.height;

        if (areaA <= 0)
            return 0;

        var areaB = boundingBoxB.width * boundingBoxB.height;

        if (areaB <= 0)
            return 0;

        var minX = Math.Max(boundingBoxA.xMin, boundingBoxB.xMin);
        var minY = Math.Max(boundingBoxA.yMin, boundingBoxB.yMin);
        var maxX = Math.Min(boundingBoxA.xMax, boundingBoxB.xMax);
        var maxY = Math.Min(boundingBoxA.yMax, boundingBoxB.yMax);

        var intersectionArea = Math.Max(maxY - minY, 0) * Math.Max(maxX - minX, 0);

        return intersectionArea / (areaA + areaB - intersectionArea);
    }

    private IList<BoundingBox> FilterBoundingBoxes(IList<BoundingBox> boxes, int limit, float threshold)
    {
        var activeCount = boxes.Count;
        var isActiveBoxes = new bool[boxes.Count];

        for (int i = 0; i < isActiveBoxes.Length; i++)
        {
            isActiveBoxes[i] = true;
        }

        var sortedBoxes = boxes.Select((b, i) => new { Box = b, Index = i })
                .OrderByDescending(b => b.Box.Confidence)
                .ToList();

        var results = new List<BoundingBox>();

        for (int i = 0; i < boxes.Count; i++)
        {
            if (isActiveBoxes[i])
            {
                var boxA = sortedBoxes[i].Box;
                results.Add(boxA);

                if (results.Count >= limit)
                    break;

                for (var j = i + 1; j < boxes.Count; j++)
                {
                    if (isActiveBoxes[j])
                    {
                        var boxB = sortedBoxes[j].Box;

                        if (IntersectionOverUnion(boxA.Rect, boxB.Rect) > threshold)
                        {
                            isActiveBoxes[j] = false;
                            activeCount--;

                            if (activeCount <= 0)
                                break;
                        }
                    }
                }

                if (activeCount <= 0)
                    break;
            }
        }

        return results;
    }
}

public class DimensionsBase
{
    public float X { get; set; }
    public float Y { get; set; }
    public float Height { get; set; }
    public float Width { get; set; }
}
public class BoundingBoxDimensions : DimensionsBase { }
class CellDimensions : DimensionsBase { }
public class BoundingBox
{
    public BoundingBoxDimensions Dimensions { get; set; }
    public string Label { get; set; }
    // public int Label { get; set; }
    public float Confidence { get; set; }
    public Rect Rect
    {
        get { return new Rect(Dimensions.X, Dimensions.Y, Dimensions.Width, Dimensions.Height); }
    }
    public override string ToString()
    {
        return $"{Label}:{Confidence}, {Dimensions.X}:{Dimensions.Y} - {Dimensions.Width}:{Dimensions.Height}";
    }
}