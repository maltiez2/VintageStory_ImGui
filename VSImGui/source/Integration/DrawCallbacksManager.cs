using System;
using System.Collections.Generic;
using System.Linq;
using VSImGui.API;

namespace VSImGui;


/// <summary>
/// Stores all the draw callbacks, calls them and makes sure that VS GUI dialog, that is used to integrate ImGui, is opened and closed when needed, and mouse is grabbed when needed
/// </summary>
internal class DrawCallbacksManager
{
    /// <summary>
    /// Draw callbacks that draw all the ImGui windows, is invoked before rendering all the drawn ImGui windows and widgets
    /// </summary>
    public event DrawCallbackDelegate DrawCallback
    {
        add
        {
            _callbacks.Add(_nextId, value);
            _wasClosed.Add(_nextId++, false);
        }
        remove
        {
            int id = _callbacks.First(entry => entry.Value == value).Key;
            _callbacks.Remove(id);
            _wasClosed.Remove(id);
        }
    }

    /// <summary>
    /// Is used to manage removing callbacks from event and map callback to boolean that tracks it status
    /// </summary>
    private int _nextId = 1;
    /// <summary>
    /// All the callbacks registered via <see cref="DrawCallback"/>
    /// </summary>
    private readonly Dictionary<int, DrawCallbackDelegate> _callbacks = new();
    /// <summary>
    /// Tracks down callback status to decide whether to close or open VS GUI dialog used for integration
    /// </summary>
    private readonly Dictionary<int, bool> _wasClosed = new();

    /// <summary>
    /// Invokes all the registered callbacks to draw ImGui windows and widgets
    /// </summary>
    /// <param name="deltaSeconds">Time elapsed since last frame</param>
    /// <returns>Whether VS GUI dialog should be closed/opened, and should it grab mouse from camera</returns>
    public (bool open, bool grab, bool close) Draw(float deltaSeconds)
    {
        bool open = false;
        bool grab = false;
        bool close = false;
        bool anyOpened = false;

        foreach ((int id, DrawCallbackDelegate callback) in _callbacks)
        {
            CallbackGUIStatus result = callback.Invoke(deltaSeconds);

            switch (result)
            {
                case CallbackGUIStatus.Closed:
                    if (!_wasClosed[id])
                    {
                        _wasClosed[id] = true;
                        close = true;
                    }
                    break;
                case CallbackGUIStatus.GrabMouse:
                    if (_wasClosed[id])
                    {
                        open = true;
                        _wasClosed[id] = false;
                    }
                    grab = true;
                    anyOpened = true;
                    break;
                case CallbackGUIStatus.DontGrabMouse:
                    if (_wasClosed[id])
                    {
                        open = true;
                        _wasClosed[id] = false;
                    }
                    anyOpened = true;
                    break;
            }

            if (open || close) Console.WriteLine($"{id}: {result} - open: {open}, grab: {grab}");
        }

        return (open, grab, !anyOpened && close);
    }
}
