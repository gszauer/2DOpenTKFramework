# Getting set up

Visual studio has a built in [Package Manager](https://en.wikipedia.org/wiki/Package_manager) called [NuGet](https://www.nuget.org/). NuGet allows you to easily install 3rd party (Not written by you or microsoft) library code to help with common tasks. We are going to be utilizing two of these packages, **OpenTK** for hardware accelerated rendering and **NAudio** for loading MP3's. Once you add a package to your visual studio project it behaves exactly like adding a reference (Like we did for windows forms)

Because we are going to be using **OpenTK** for hardware accelerated rendering, we can no longer use **WinForms** to create our window. OpenTK actually comes with several ways to create and manage multiple windows, we wil be using it's built in functions.

While **OpenTK** and **NAudio** are needed for your project to compile, you will not be using either of them directly. This framework provides several managers that wrap the functionality of the libraries into easy to use functions for you, the point of the managers is to allow you to focus on making a game, without having to worry about the infrastructure code below it.

In essence, this is a lot like the support classes we wrote for **WinformsGames**, except you don't need to write the boiler plate code, i did that for you. If you are interested in what the code does, you do have full source access. Feel free to take a look and ask me any questions you may have.

Having said all that, let's see how to implement this and make a blank window pop up in your own project.

# OpenAL

The framework uses OpenAL to play audio. OpenAL is not something that is installed on your system by default, the installer is included in this repository as ```OpenALDriver.zip```, you can also download it from [the official OpenAL website](https://www.openal.org/downloads/)

Anyone playing your games will also need to have OpenAL installed. We will talk about how to bundle OpenAL with your installer at a later point, for now just have it installed on your machine.

# Create project

* Open up **Visual Studio** and make a new _"Console Application"_ project.

# Add packages

* Right click on the project name (PROJECT, not solution)
* Select the **Manage NuGet Packages...** option, you will be greated with this window

![Package Manager](https://dl.dropboxusercontent.com/u/48598159/howto_package.png)

* Search for and install **OpenTK**

![OpenTK](https://dl.dropboxusercontent.com/u/48598159/otk_inst.png)

* Search for and install **NAudio**

![NAudio](https://dl.dropboxusercontent.com/u/48598159/naudui_inst.png)

# Add references
We have to add a reference to System.Drawing to our project.

* Under your PROJECT right click **References**
* Search for **System.Drawing**
* Select the entry, and check the box next to it
* Then click ok

![Drawing](https://dl.dropboxusercontent.com/u/48598159/sysdraw.png)

# Add files
You need to add the following files from this repository to your project

* [2DFramework/Framework/GraphicsManager.cs](2DFramework/Framework/GraphicsManager.cs)
* [2DFramework/Framework/InputManager.cs](2DFramework/Framework/InputManager.cs)
* [2DFramework/Framework/SoundManager.cs](2DFramework/Framework/SoundManager.cs)
* [2DFramework/Framework/TextureManager.cs](2DFramework/Framework/TextureManager.cs)

How you get them into your project is up to you. I do suggest placing them all in a folder called **Framework** to keep these seperate from your files.

# Main file
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
			float deltaTime = (float)e.Time;
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
#if DEBUG
            Console.ReadLine();
#endif
        }
```

The #if DEBUG block is a bit unusual. It will make the console window stick around after the main window is closed. Usually this is not the behaviour we want, but it will make it easyer to catch errors later down the line.

Not sure if it's even worth including here, but we just need to close the class and namespace

```
    }
}
```

If all is well, now you should be able to run your program. You should get a console and a window. The contents of that window depends on your graphics driver, it may be black, white or it may just contain garbage. This is what mine looks like:

![Win Sample](https://dl.dropboxusercontent.com/u/48598159/result.png)

# Next steps
We have a window, so what next? Go to the [repository wiki](https://github.com/gszauer/2DOpenTKFramework/wiki) for detailed instructions on how to set up and use the framework managers.

![WIKI](https://dl.dropboxusercontent.com/u/48598159/wiki_how.png)

Follow the links on the sidebar in order, you will make a visual studio solution with many projects in it, this is all outlined on the "Introduction" page of the wii

![SIDEBAR](https://dl.dropboxusercontent.com/u/48598159/sidebar.png)
