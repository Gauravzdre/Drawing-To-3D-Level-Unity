using UnityEngine;
using OpenCvSharp;
using System.Collections.Generic;
using System.Linq;

public class OpenCVSharpProcessor : MonoBehaviour
{
    public Texture2D floorPlanTexture;
    public float targetWidth = 30.0f;
    public float targetHeight = 40.0f;


    void GenerateFloorPlan()
    {
        ProcessFloorPlan();
    }

    private void ProcessFloorPlan()
    {
        Mat image = OpenCvSharp.Unity.TextureToMat(floorPlanTexture);
        Mat grayImage = image.CvtColor(ColorConversionCodes.BGR2GRAY);
        Mat blurred = grayImage.GaussianBlur(new Size(3, 3), 0);
        Mat processedImage = blurred.AdaptiveThreshold(255, AdaptiveThresholdTypes.MeanC, ThresholdTypes.BinaryInv, 11, 5);

        Cv2.FindContours(processedImage, out Point[][] contours, out HierarchyIndex[] hierarchy, RetrievalModes.List, ContourApproximationModes.ApproxSimple);

        List<Vector2[]> wallPoints = new List<Vector2[]>();
        Vector2[] outerWallContour = null;

        foreach (var contour in contours)
        {
            double area = Cv2.ContourArea(contour);

            if (area > 20.0) 
            {
                wallPoints.Add(contour.Select(p => new Vector2(p.X, p.Y)).ToArray());
            }
        }

        // Add the outer wall after processing all contours
        if (outerWallContour != null)
        {
            wallPoints.Add(outerWallContour);
        }
        float scaleX = (targetWidth * 2) / floorPlanTexture.width;
        float scaleY = (targetHeight *2)/ floorPlanTexture.height;
        WallCreator wallCreator = GetComponent<WallCreator>();
        wallCreator.CreateWalls(wallPoints, scaleX, scaleY);
    }
}
