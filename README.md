#Getting set up
Lets implement this framework in your own project. [TODO: Details]

#Create project
* Open up **Visual Studio** and make a new _"Console Application"_ project.

#Add packages
* Right click on the project name (PROJECT, not solution)
* Select the **Manage NuGet Packages...** option, you will be greated with this window

![Package Manager](https://dl.dropboxusercontent.com/u/48598159/howto_package.png)

* Search for and install **OpenTK**

![OpenTK](https://dl.dropboxusercontent.com/u/48598159/otk_inst.png)

* Search for and install **NAudio**

![NAudio](https://dl.dropboxusercontent.com/u/48598159/naudui_inst.png)

#Add references
We have to add a reference to System.Drawing to our project.

* Under your PROJECT right click **References**
* Search for **System.Drawing**
* Select the entry, and check the box next to it
* Then click ok

![Drawing](https://dl.dropboxusercontent.com/u/48598159/sysdraw.png)

#Add files
You need to add the following files from this repository to your project

* [2DFramework/Framework/GraphicsManager.cs](2DFramework/Framework/GraphicsManager.cs)
* [2DFramework/Framework/InputManager.cs](2DFramework/Framework/InputManager.cs)
* [2DFramework/Framework/SoundManager.cs](2DFramework/Framework/SoundManager.cs)
* [2DFramework/Framework/TextureManager.cs](2DFramework/Framework/TextureManager.cs)

How you get them into your project is up to you. I do suggest placing them all in a folder called **Framework** to keep these seperate from your files.

#Main
It's time to create a main window!