using ImGuiNET;
using System;
using System.Numerics;
using Vintagestory.API.Client;
using Vintagestory.API.Common;

namespace VSImGui;

static public class TilingUtils
{
    public enum TextureRenderLevel
    {
        Background,
        Window,
        Foreground
    }
    static public void TileWindowWithTexture(ICoreClientAPI api, AssetLocation texturePath, Vector2 textureSize, TextureRenderLevel layer = TextureRenderLevel.Window)
    {
        int texture = api.Render.GetOrLoadTexture(texturePath);
        TileWindowWithTexture(texture, textureSize, layer);
    }
    static public void TileWindowWithTexture(int texture, Vector2 textureSize, TextureRenderLevel layer = TextureRenderLevel.Window)
    {
        Vector2 dimensions = new(ImGui.GetWindowWidth(), ImGui.GetWindowHeight());

        (int widthCount, float widthSize) = CalcTextures(textureSize.X, dimensions.X);
        (int heightCount, float heightSize) = CalcTextures(textureSize.Y, dimensions.Y);

        Vector2 offset = new(0, 0);
        for (int x = 0; x < widthCount; x++)
        {
            for (int y = 0; y < heightCount; y++)
            {
                AddTextureToLayer(texture, layer, offset, 1, 1, textureSize);
                offset.Y += textureSize.Y;
            }
            AddTextureToLayer(texture, layer, offset, 1, heightSize, textureSize);

            offset.Y = 0;
            offset.X += textureSize.X;
        }

        for (int y = 0; y < heightCount; y++)
        {
            AddTextureToLayer(texture, layer, offset, widthSize, 1, textureSize);
            offset.Y += textureSize.Y;
        }
        AddTextureToLayer(texture, layer, offset, widthSize, heightSize, textureSize);
    }

    static private void AddTextureToLayer(int texture, TextureRenderLevel layer, Vector2 offset, float width, float height, Vector2 textureSize)
    {
        textureSize.X *= width;
        textureSize.Y *= height;

        switch (layer)
        {
            case TextureRenderLevel.Background:
                ImGui.GetBackgroundDrawList().AddImage(
                    texture,
                    ImGui.GetWindowPos() + offset,
                    ImGui.GetWindowPos() + offset + textureSize,
                    new Vector2(0, 0),
                    new Vector2(width, height)
                    );
                break;
            case TextureRenderLevel.Window:
                ImGui.GetWindowDrawList().AddImage(
                    texture,
                    ImGui.GetWindowPos() + offset,
                    ImGui.GetWindowPos() + offset + textureSize,
                    new Vector2(0, 0),
                    new Vector2(width, height)
                    );
                break;
            case TextureRenderLevel.Foreground:
                ImGui.GetForegroundDrawList().AddImage(
                    texture,
                    ImGui.GetWindowPos() + offset,
                    ImGui.GetWindowPos() + offset + textureSize,
                    new Vector2(0, 0),
                    new Vector2(width, height)
                    );
                break;
        }
    }

    static private(int count, float lastSize) CalcTextures(float textureSize, float windowSize)
    {
        int count = (int)MathF.Floor(windowSize / textureSize);
        float lastSize = (windowSize - count * textureSize) / textureSize;
        return (count, lastSize);
    }
}

static internal class ComparisonUtils
{
    public const float Epsilon = 1E-9f;
    static public bool CompareFloats(float first, float second) => MathF.Abs(first - second) < Epsilon;
}
