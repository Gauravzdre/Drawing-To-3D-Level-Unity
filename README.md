OpenCVSharp Unity Room Designer
This Unity project utilizes OpenCVSharp to generate 3D room and level designs from hand-drawn floor plan images. The generated designs consist of walls, floors, and ceilings based on the contours detected in the input floor plan image.

How to Use
Floor Plan Image

Open Sample Scene you will find WallCreation gameobject just pass in the drawing image and it will generate the room and corridors
Prepare a hand-drawn floor plan image as the input. This image should represent the layout of the rooms and corridors.

Attach Scripts

Attach the OpenCVSharpProcessor script to an empty GameObject in your Unity scene.
Attach the WallCreator script to another empty GameObject in the scene.
Assign Floor Plan Texture

In the Unity Editor, assign the hand-drawn floor plan image to the floorPlanTexture field of the OpenCVSharpProcessor script.
Configure Parameters

Adjust parameters like targetWidth and targetHeight in the OpenCVSharpProcessor script to match the dimensions of your floor plan image.
Set parameters like defaultWallHeight, wallThickness, and floorHeight in the WallCreator script to customize the generated room's appearance.
Generate Floor Plan

Run the Unity scene.
Call the GenerateFloorPlan() method in the OpenCVSharpProcessor script to initiate the floor plan processing.
View Generated Design

Once the processing is complete, the scene will display the generated 3D room and level design based on the input floor plan image.
Features
Automatic Contour Detection: Utilizes OpenCVSharp to detect contours in the input floor plan image.
Dynamic Wall and Floor Creation: Generates 3D walls, floors, and ceilings based on the detected contours, allowing for flexible room designs.
Customization Options: Adjustable parameters for wall height, thickness, and floor height enable customization of the generated designs.
Efficient Workflow: Automates the process of converting hand-drawn floor plans into 3D room designs, saving time and effort in level design creation.
Notes
Ensure that the input floor plan image is clear and properly scaled to achieve accurate contour detection.
Experiment with different parameter values to achieve the desired visual aesthetics for the generated room designs.
Example
Below is an example of a hand-drawn floor plan image and its corresponding 3D room design generated using the OpenCVSharp Unity Room Designer:



Author
This project was created by [Your Name]. Feel free to reach out with any questions or feedback!

License
This project is licensed under the MIT License.
