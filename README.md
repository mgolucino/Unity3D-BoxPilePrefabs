# Unity3D-BoxPilePrefabs
![Preview](https://user-images.githubusercontent.com/46628480/102295015-1fc59080-3f10-11eb-9c37-c6e4221dce2f.png)
This is a package of box pile prefabs for Unity Engine. You may use them as background props to add variety to your scenes. The prefabs consist of primitive cubes that can be replaced with any box-shaped 3D model. The scripts provided allow you to replace multiple cubes at once with the click of one button - no need for replacing cubes manually.

>This package does not come with 3D models to replace the primitive cubes - you must download or create these on your own. Links to third party 3D models have been provided in the assets folder.

# Instructions
Download the assets folder in this repository, then import the contents into your Unity 3D project. Open up the demo scene to view all the prefabs, then try replacing the primitive cubes with another 3D object.

# Replacing a Box Pile
![Box Pile](https://user-images.githubusercontent.com/46628480/102311824-2b767e80-3f33-11eb-8ec0-6c1d586f4ad1.png)
Click the drop down arrow next to **Box Pile Group (1)** to expose the children game objects. Click on any of the child objects to examine the **Box Pile (Script)** component in the inspector window. Drag the **Rounded Cube** object (or any other box object of your choice) into the **Replacement** field, then click on **Replace Boxes**. You may click on **Reset Boxes** to revert the object back into primitive cubes.

# Replacing a Box Pile Group
![Box Pile Group](https://user-images.githubusercontent.com/46628480/102311871-3a5d3100-3f33-11eb-978a-b7a3ed0bb41f.png)
Click on **Box Pile Group (1)** to examine the **Box Pile Group (Script)** component in the inspector window. Drag any box object into the **Replacement** field and click on **Replace Boxes**. If successful, all four box piles in the group will change at once.

# The "Normalize Scale" Option
The box pile scripts take into account the original size of your 3D model when replacing boxes. Each time you click on **Replace Boxes**, the resulting box pile will reflect the original size of your 3D model. If your 3D model has sides that are not 1 unit long, you may check the **Normalize Scale** field to make your boxes the same size as a primitive cube the next time you click on **Replace Boxes**.
