# Vintage Story ImGui - Debug Tools
This package is meant to be used alongside installed VSImGui mod for Vintage Story.
## Implemented tools
- **Debug widgets**: static methods for adding ImGui widgets to display and edit data anywhere in codebase.
## Usage
### Examples
After called at least once these lines of code will draw window with title `Test window`, a tab with title `test tab` and inside this tab: a line of text `test text` followed by separator and slider that will change value of `TestValue` property or field.
```csharp
DebugWidgets.Text(domain: "Test window", category: "test tab", id: 0, text: "test text");
DebugWidgets.Draw(domain: "Test window", category: "test tab", id: 1, () => ImGui.Separator());
DebugWidgets.IntSlider(domain: "Test window", category: "test tab", label: "test slider", min: 0, max: 10, getter: () => testValue, setter: value => TestValue = value);
```
### Guide
_will be added later_
