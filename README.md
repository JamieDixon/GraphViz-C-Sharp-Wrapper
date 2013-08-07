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
