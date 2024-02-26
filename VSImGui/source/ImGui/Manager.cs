using System;
using System.Collections.Generic;
using System.Linq;

namespace VSImGui.src.ImGui;

public enum VSDialogStatus
{
    /// <summary>
    /// All ImGUI windows in callback are closed
    /// </summary>
    Closed,
    /// <summary>
    /// Require cursor to shot and lock camera
    /// </summary>
    GrabMouse,
    /// <summary>
    /// Do not require cursor to shot and lock camera (works only with immersive mouse mode)
    /// </summary>
    DontGrabMouse
}


public delegate VSDialogStatus ImGuiDialogDrawCallback(float deltaSeconds);

public class VSImGuiManager
{
    public VSImGuiManager()
    {

    }

    public event ImGuiDialogDrawCallback DrawCallback
    {
        add
        {
            mCallbacks.Add(mNextId, value);
            mWasClosed.Add(mNextId++, false);
        }
        remove
        {
            int id = mCallbacks.First(entry => entry.Value == value).Key;
            mCallbacks.Remove(id);
            mWasClosed.Remove(id);
        }
    }

    private int mNextId = 1;
    private readonly Dictionary<int, ImGuiDialogDrawCallback> mCallbacks = new();
    private readonly Dictionary<int, bool> mWasClosed = new();

    internal (bool open, bool grab, bool close) Draw(float deltaSeconds)
    {
        bool open = false;
        bool grab = false;
        bool close = false;
        bool anyOpened = false;

        foreach ((int id, ImGuiDialogDrawCallback callback) in mCallbacks)
        {
            VSDialogStatus result = callback.Invoke(deltaSeconds);

            switch (result)
            {
                case VSDialogStatus.Closed:
                    if (!mWasClosed[id])
                    {
                        mWasClosed[id] = true;
                        close = true;
                    }
                    break;
                case VSDialogStatus.GrabMouse:
                    if (mWasClosed[id])
                    {
                        open = true;
                        mWasClosed[id] = false;
                    }
                    grab = true;
                    anyOpened = true;
                    break;
                case VSDialogStatus.DontGrabMouse:
                    if (mWasClosed[id])
                    {
                        open = true;
                        mWasClosed[id] = false;
                    }
                    anyOpened = true;
                    break;
            }

            if (open || close) Console.WriteLine($"{id}: {result} - open: {open}, grab: {grab}");
        }

        return (open, grab, !anyOpened && close);
    }
}
