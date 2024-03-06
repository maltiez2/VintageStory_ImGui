# Dear ImGui integration into Vintage Story
This repository contains two projects:
- VSImGui - mod that integrates Dear ImGui into Vintage Story using fork of ImGui.NET
- VSImGui_DebugTools - collection of static methods for debugging and tweaking VS mods

## Debug tools

[![NuGet](https://img.shields.io/nuget/v/VSImGui_DebugTools.svg?logo=NuGet&logoColor=white)](https://www.nuget.org/packages/VSImGui_DebugTools)

Contains static class `DebugWidgets` that has methods for drawing debug widgets in debug windows,
that can be pasted anywhere in code for displaying data or editing it in real time.

To be able to use this tools you need to install
[this nugget package](https://www.nuget.org/packages/VSImGui_DebugTools)
(`dotnet add package VSImGui_DebugTools` or search for "Vintage Story" in Visual Studio packet manager),
and to install [ImGui mod](https://mods.vintagestory.at/imgui). Then you will be able to just call methods
from `DebugWidgets` class. To avoid debug windows and dependency on ImGui mod use these methods only in DEBUG
confifugration. You can remove methods before publishing mod or use `#if DEBUG ... #endif` construction to
leave these methods only in RELEASE configuration.

### Example

After called at least once these lines of code will draw window with title _"Test window"_,
a tab with title _"test tab"_ and inside this tab: a line of text _"test text"_ followed by separator
and slider that will change value of `TestValue` property or field.

    DebugWidgets.Text(domain: "Test window", category: "test tab", id: 0, text: "test text");
    DebugWidgets.Draw(domain: "Test window", category: "test tab", id: 1, () => ImGui.Separator());
    DebugWidgets.IntSlider(domain: "Test window", category: "test tab", label: "test slider", min: 0, max: 10, getter: () => testValue, setter: value => TestValue = value);

## VSImGui mod
[![NuGet](https://img.shields.io/nuget/v/VSImGui.svg?logo=NuGet&logoColor=white)](https://www.nuget.org/packages/VSImGui)

_Description and examples will be added later._
