#Getting set up
Lets implement this framework in your own project. [TODO: Details]

* TODO: What packages are and which ones we use
* TODO: OpenTK and windowing
* TODO: What the managers do

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

#Main file
Now that we have all the project dependencies in place it's time to implement the main file. This is the place where the window is created. Let's see how to implement a minimalistic version.

We will be using the following namespaces:

```
using System; // Important things
using OpenTK; // For windowing
using OpenTK.Input; // To get they Key enumeration
using System.Drawing; // For coors
using GameFramework; // For the 2D game framework
```

You probabaly want to develop your game in it's own meaningful namespace, and the main class should be in that namespace as well. Developing in the ```GameFramework``` namespace might be tempting, but try to avoid it. I'm going to be working in a namespace called ```SomeNamespace```. My main class is going to have a **static** reference to an OpenTK Window.



```
namespace SomeNamespace {
    class MainClass {
        public static OpenTK.GameWindow Window = null; // Reference to OpenTK window
```

Next, let's add some stock functions to Initialize, Update, Render and Shutdown the game. These functions will be called by the OpenTK Window as such they take event argumetns. For the most part you can ignore the arguments to Initialize and Shutdown. Both Update and Render take a ```FrameEventArgs``` argument. Each ```FrameEventArgs``` object has a ```.Time``` member, this variable is the delta time for the frame.

```
        public static void Initialize(object sender, EventArgs e) {
            // INITIALIZE GAME
        }

        public static void Update(object sender, FrameEventArgs e) {
            // UPDATE GAME
        }

        public static void Render(object sender, FrameEventArgs e) {
            // RENDER GAME
        }

        public static void Shutdown(object sender, EventArgs e) {
            // SHUTDOWN GAME
        }
```

Next, lets create the actual window. This needs to happen inside of the **Main** method. Because **Main** is going to create a window it needs to be marked as an STA Thread. Remember this is an [OpenTK Window](http://www.opentk.com/files/doc/class_open_t_k_1_1_game_window.html) it would be wise to read trough the documentation.

```
        [STAThread]
        public static void Main() {
            // Create static (global) window instance
            Window = new OpenTK.GameWindow();

            // Hook up the initialize callback
            Window.Load += new EventHandler<EventArgs>(Initialize);
            // Hook up the update callback
            Window.UpdateFrame += new EventHandler<FrameEventArgs>(Update);
            // Hook up the render callback
            Window.RenderFrame += new EventHandler<FrameEventArgs>(Render);
            // Hook up the shutdown callback
            Window.Unload += new EventHandler<EventArgs>(Shutdown);

            // Set window title and size
            Window.Title = "Game Name";
            Window.ClientSize = new Size(800, 600);

            // Run the game at 60 frames per second. This method will NOT return
            // until the window is closed.
            Window.Run(60.0f);

            // If we made it down here the window was closed. Call the windows
            // Dispose method to free any resources that the window might hold
            Window.Dispose();
        }
```

Not sure if it's even worth including here, but we just need to close the class and namespace

```
    }
}
```

If all is well, now you should be able to run your program. You should get a console and a window. The contents of that window depends on your graphics driver, it may be black, white or it may just contain garbage. This is what mine looks like:

![Win Sample](https://dl.dropboxusercontent.com/u/48598159/result.png)

#Next steps
We have a window, so what next? Go to the [repository wiki](https://github.com/gszauer/2DOpenTKFramework/wiki) for detailed instructions on how to set up and use the framework managers.

![WIKI](https://dl.dropboxusercontent.com/u/48598159/wiki_how.png)