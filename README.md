# Project Description
This is a C# wrapper for the GraphViz graph generator.

Pass in a dot string and an output type and voila, your graph is generated.

The output file is returned to you as a byte array to do as you please.

This library acts as a wrapper for the GraphViz command line tools. The graphviz command line tools are included in this project and need to be placed in the graphviz folder relative to the GraphVizWrapper dll upon deployment.

## Usage

```C#
// These three instances can be injected via the IGetStartProcessQuery, 
//                                               IGetProcessStartInfoQuery and 
//                                               IRegisterLayoutPluginCommand interfaces

var getStartProcessQuery = new GetStartProcessQuery();
var getProcessStartInfoQuery = new GetProcessStartInfoQuery();
var registerLayoutPluginCommand = new RegisterLayoutPluginCommand();

// GraphGeneration can be injected via the IGraphGeneration interface

var wrapper = new GraphGeneration(getStartProcessQuery, 
								  getProcessStartInfoQuery, 
								  registerLayoutPluginCommand);

byte[] output = wrapper.GenerateGraph("digraph{a -> b; b -> c; c -> a;}", Enums.GraphReturnType.Png);
```
## Getting started

### Clone the repository
```C#
git clone https://github.com/JamieDixon/GraphViz-C-Sharp-Wrapper.git
```
### In the terminal (command prompt), change directory to the build folder and run the build.bat file
```C#
cd build
build.bat
```

This will build the GraphVizWrapper project, configure the necessary GraphViz files 
and move them into the GraphvizWrapper bin folder and then build/configure the sample project(s) included.

### Copy files to your own project

Once you've run the build you're ready to move the necessary files to your own project.
You'll need the GraphVizWrapper.dll file from the bin folder along with the GraphViz folder 
(the dll and this folder must reside at the same level and be placed into the bin of your application at build time)

## Running the sample application

1. Make sure you've run build.bat
2. Navigate to sample-applications/MVC4
3. Open the GraphVizWrapper-MVC4Sample.sln in Visual Studio. F5 to run the project.

The sample takes a binary dependency on GraphVizWrapper.dll which is placed into the lib folder when you run build.bat

If you make modifications to the GraphVizWrapper you'll need to run build.bat again to make sure this sample application
has the latest version of GraphVizWrapper.dll

**This sample application demonstrated one way of displaying the returned graph byte array to a user in an MVC4 application:**

### Controller Action Method:
```C#
var bytes = this.graphVizWrapper.GenerateGraph("digraph{a -> b; b -> c; c -> a;}", Enums.GraphReturnType.Jpg);
            
// Alternatively you could save the image on the server as a file.
var viewModel = string.Format("data:image/jpg;base64,{0}", Convert.ToBase64String(bytes));
ViewBag.Data = viewModel;
return this.View();
```

### Razor View
```Razor
<img src="@ViewBag.Data" />
```
