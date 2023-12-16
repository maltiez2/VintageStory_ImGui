using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Numerics;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;

namespace VSImGui
{

    public class ValueConverter<TValue> : JsonConverter
        where TValue : IFloatArrayValue, new()
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(TValue);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            TValue result = new TValue();

            List<float> values = new(); 
            while (reader.ReadAsDouble() is double value)
            {
                values.Add((float)value);
            }

            result.Array = values.ToArray();

            return result;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            TValue input = (TValue)value;

            writer.WriteStartArray();
            foreach (float item in input.Array)
            {
                writer.WriteValue(item);
            }
            writer.WriteEndArray();
        }
    }

    public interface IFloatArrayValue
    {
        float[] Array { get; set; }
    }
    [JsonConverter(typeof(ValueConverter<Value2>))]
    public struct Value2 : IFloatArrayValue
    {
        public Vector2 Value { get; set; }
        public float X => Value.X;
        public float Y => Value.Y;
        public float[] Array { get => new float[] { X, Y }; set => Value = new(value); }

        public Value2(Vector2 value) => Value = value;
        public Value2((float first, float second) values) => Value = new(values.first, values.second);
        public Value2(Vec2f value) => Value = new(value.X, value.Y);
        public Value2(float first, float second) => Value = new(first, second);

        public static implicit operator Value2(Vector2 value) => new(value);
        public static implicit operator Value2((float first, float second) values) => new(values);
        public static implicit operator Value2(Vec2f value) => new(value);
        public static implicit operator Vector2(Value2 value) => new(value.X, value.Y);

        public override string ToString() => Array.ToString();
    }
    [JsonConverter(typeof(ValueConverter<Value3>))]
    public struct Value3 : IFloatArrayValue
    {
        public Vector3 Value { get; set; }
        public float X => Value.X;
        public float Y => Value.Y;
        public float Z => Value.Z;
        public float R => Value.X;
        public float G => Value.Y;
        public float B => Value.Z;
        public Value2 XY => new(X, Y);
        public float[] Array { get => new float[] { X, Y, Z }; set => Value = new(value); }

        public Value3(Vector3 value) => Value = value;
        public Value3((float first, float second, float third) values) => Value = new(values.first, values.second, values.third);
        public Value3(Vec3f value) => Value = new(value.X, value.Y, value.Z);
        public Value3(float first, float second, float third) => Value = new(first, second, third);

        public static implicit operator Value3(Vector3 value) => new(value);
        public static implicit operator Value3((float first, float second, float third) values) => new(values);
        public static implicit operator Value3(Vec3f value) => new(value);
        public static implicit operator Vector3(Value3 value) => new(value.X, value.Y, value.Z);

        public override string ToString() => Array.ToString();
    }
    [JsonConverter(typeof(ValueConverter<Value4>))]
    public struct Value4 : IFloatArrayValue
    {

        public Vector4 Value { get; set; }
        public float X => Value.X;
        public float Y => Value.Y;
        public float Z => Value.Z;
        public float W => Value.W;
        public float R => Value.X;
        public float G => Value.Y;
        public float B => Value.Z;
        public float A => Value.W;
        public Value3 RGB => new(R, G, B);
        public Value3 XYZ => new(X, Y, Z);
        [JsonProperty]
        public float[] Array { get => new float[] { X, Y, Z, W }; set => Value = new(value); }

        public Value4(Vector4 value) => Value = value;
        public Value4((float first, float second, float third, float fourth) values) => Value = new(values.first, values.second, values.third, values.fourth);
        public Value4(Vec4f value) => Value = new(value.X, value.Y, value.Z, value.W);
        public Value4(float first, float second, float third, float fourth) => Value = new(first, second, third, fourth);
        public Value4((Value3 RGB, float alpha) values) => Value = new(values.RGB.X, values.RGB.Y, values.RGB.Z, values.alpha);

        public static implicit operator Value4(Vector4 value) => new(value);
        public static implicit operator Value4((float first, float second, float third, float fourth) values) => new(values);
        public static implicit operator Value4((Value3 RGB, float alpha) values) => new(values);
        public static implicit operator Value4(Vec4f value) => new(value);
        public static implicit operator Vector4(Value4 value) => new(value.X, value.Y, value.Z, value.W);

        public override string ToString() => Array.ToString();
    }
    [JsonObject(MemberSerialization.OptOut)]
    public struct FontData
    {
        public string Name { get; set; }
        public int Size { get; set; }

        public FontData(string path, int size)
        {
            Name = path;
            Size = size;
        }
        public FontData((string path, int size) values)
        {
            Name = values.path;
            Size = values.size;
        }
        public static implicit operator FontData((string path, int size) values) => new(values);
        public static implicit operator (string path, int size)(FontData values) => (values.Name, values.Size);

        public override string ToString() => $"{Name} {Size}px";
    }
}
