using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor.Compilation;
#endif

namespace Roguelike.Core
{
    public static class Extension
    {
        public static string ToDebugString(this byte[] buffer)
        {
            StringBuilder builder = new StringBuilder();
            int count = buffer.Length;
            builder.Append("[");
            for (int i = 0; i < count; i++)
            {
                builder.Append(buffer[i].ToString());
                builder.Append(" ");
            }
            builder.Append("]");

            return builder.ToString();
        }

        public static bool IsBuiltInType(this Type type)
        {
            if (type == typeof(bool))
            {
                return true;
            }
            else if (type == typeof(byte))
            {
                return true;
            }
            else if (type == typeof(sbyte))
            {
                return true;
            }
            else if (type == typeof(char))
            {
                return true;
            }
            else if (type == typeof(decimal))
            {
                return true;
            }
            else if (type == typeof(double))
            {
                return true;
            }
            else if (type == typeof(float))
            {
                return true;
            }
            else if (type == typeof(int))
            {
                return true;
            }
            else if (type == typeof(uint))
            {
                return true;
            }
            else if (type == typeof(nint))
            {
                return true;
            }
            else if (type == typeof(nuint))
            {
                return true;
            }
            else if (type == typeof(long))
            {
                return true;
            }
            else if (type == typeof(ulong))
            {
                return true;
            }
            else if (type == typeof(short))
            {
                return true;
            }
            else if (type == typeof(ushort))
            {
                return true;
            }
            else if (type == typeof(object))
            {
                return true;
            }
            else if (type == typeof(string))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static object Parse(this string value, Type type)
        {
            if (string.IsNullOrEmpty(value)) return null;
            if (type.IsBuiltInType() == false) return null;

            if (type == typeof(bool))
            {
                if (bool.TryParse(value, out bool result))
                {
                    return result;
                }
                else
                {
                    return null;
                }
            }
            else if (type == typeof(byte))
            {
                if (byte.TryParse(value, out byte result))
                {
                    return result;
                }
                else
                {
                    return null;
                }
            }
            else if (type == typeof(sbyte))
            {
                if (sbyte.TryParse(value, out sbyte result))
                {
                    return result;
                }
                else
                {
                    return null;
                }
            }
            else if (type == typeof(char))
            {
                if (char.TryParse(value, out char result))
                {
                    return result;
                }
                else
                {
                    return null;
                }
            }
            else if (type == typeof(decimal))
            {
                if (decimal.TryParse(value, out decimal result))
                {
                    return result;
                }
                else
                {
                    return null;
                }
            }
            else if (type == typeof(double))
            {
                if (double.TryParse(value, out double result))
                {
                    return result;
                }
                else
                {
                    return null;
                }
            }
            else if (type == typeof(float))
            {
                if (float.TryParse(value, out float result))
                {
                    return result;
                }
                else
                {
                    return null;
                }
            }
            else if (type == typeof(int))
            {
                if (int.TryParse(value, out int result))
                {
                    return result;
                }
                else
                {
                    return null;
                }
            }
            else if (type == typeof(uint))
            {
                if (uint.TryParse(value, out uint result))
                {
                    return result;
                }
                else
                {
                    return null;
                }
            }
            else if (type == typeof(long))
            {
                if (long.TryParse(value, out long result))
                {
                    return result;
                }
                else
                {
                    return null;
                }
            }
            else if (type == typeof(ulong))
            {
                if (ulong.TryParse(value, out ulong result))
                {
                    return result;
                }
                else
                {
                    return null;
                }
            }
            else if (type == typeof(short))
            {
                if (short.TryParse(value, out short result))
                {
                    return result;
                }
                else
                {
                    return null;
                }
            }
            else if (type == typeof(ushort))
            {
                if (ushort.TryParse(value, out ushort result))
                {
                    return result;
                }
                else
                {
                    return null;
                }
            }
            else if (type == typeof(string))
            {
                return value;
            }
            else
            {
                return null;
            }
        }

        public static object Parse(this Type type, string value)
        {
            if (string.IsNullOrEmpty(value)) return null;
            if (type.IsBuiltInType() == false) return null;

            if (type == typeof(bool))
            {
                if (bool.TryParse(value, out bool result))
                {
                    return result;
                }
                else
                {
                    return null;
                }
            }
            else if (type == typeof(byte))
            {
                if (byte.TryParse(value, out byte result))
                {
                    return result;
                }
                else
                {
                    return null;
                }
            }
            else if (type == typeof(sbyte))
            {
                if (sbyte.TryParse(value, out sbyte result))
                {
                    return result;
                }
                else
                {
                    return null;
                }
            }
            else if (type == typeof(char))
            {
                if (char.TryParse(value, out char result))
                {
                    return result;
                }
                else
                {
                    return null;
                }
            }
            else if (type == typeof(decimal))
            {
                if (decimal.TryParse(value, out decimal result))
                {
                    return result;
                }
                else
                {
                    return null;
                }
            }
            else if (type == typeof(double))
            {
                if (double.TryParse(value, out double result))
                {
                    return result;
                }
                else
                {
                    return null;
                }
            }
            else if (type == typeof(float))
            {
                if (float.TryParse(value, out float result))
                {
                    return result;
                }
                else
                {
                    return null;
                }
            }
            else if (type == typeof(int))
            {
                if (int.TryParse(value, out int result))
                {
                    return result;
                }
                else
                {
                    return null;
                }
            }
            else if (type == typeof(uint))
            {
                if (uint.TryParse(value, out uint result))
                {
                    return result;
                }
                else
                {
                    return null;
                }
            }
            else if (type == typeof(long))
            {
                if (long.TryParse(value, out long result))
                {
                    return result;
                }
                else
                {
                    return null;
                }
            }
            else if (type == typeof(ulong))
            {
                if (ulong.TryParse(value, out ulong result))
                {
                    return result;
                }
                else
                {
                    return null;
                }
            }
            else if (type == typeof(short))
            {
                if (short.TryParse(value, out short result))
                {
                    return result;
                }
                else
                {
                    return null;
                }
            }
            else if (type == typeof(ushort))
            {
                if (ushort.TryParse(value, out ushort result))
                {
                    return result;
                }
                else
                {
                    return null;
                }
            }
            else if (type == typeof(string))
            {
                return value;
            }
            else
            {
                return null;
            }
        }

        public static string Spacing(this string value)
        {
            StringBuilder stringBuilder = new StringBuilder();
            int length = value.Length;
            for (int i = 0; i < length; i++)
            {
                if (i != 0 && char.IsUpper(value[i]))
                {
                    stringBuilder.Append(" ");
                }
                stringBuilder.Append(value[i]);
            }

            return stringBuilder.ToString();
        }

        public static object Parse(this Automata.DataType type, string value)
        {
            object result = null;
            switch (type)
            {
                case Automata.DataType.Bool:
                    if (bool.TryParse(value, out bool resultBool))
                    {
                        result = resultBool;
                    }
                    break;
                case Automata.DataType.Int:
                    if (int.TryParse(value, out int resultInt))
                    {
                        result = resultInt;
                    }
                    break;
                case Automata.DataType.Float:
                    if (float.TryParse(value, out float resultFloat))
                    {
                        result = resultFloat;
                    }
                    break;
                case Automata.DataType.Timer:
                    if (float.TryParse(value, out float resultTimer))
                    {
                        result = resultTimer;
                    }
                    break;
                default:
                    break;
            }
            return result;
        }

        public static void PushFront<T>(this List<T> list, T item)
        {
            list.Insert(0, item);
        }
        public static void PushBack<T>(this List<T> list, T item)
        {
            list.Add(item);
        }
        public static T PopFront<T>(this List<T> list)
        {
            T temp = list[0];
            list.Remove(temp);
            return temp;
        }
        public static T PopBack<T>(this List<T> list)
        {
            T temp = list[list.Count - 1];
            list.Remove(temp);
            return temp;
        }

        public static void DestroyAllChildren(this GameObject gameObject)
        {
            Transform[] children = gameObject.GetComponentsInChildren<Transform>();
            int count = children.Length;
            for (int i = 0; i < count; i++)
            {
                var child = children[i];
                GameObject.Destroy(child.gameObject);
            }
        }

        public static void GenerateEnum<T>(string fullPath, string enumName, IEnumerable<T> values)
        {
#if UNITY_EDITOR

            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine("namespace TheSalt.Core");
            stringBuilder.AppendLine("{");
            stringBuilder.AppendLine($"\tpublic enum {enumName}");
            stringBuilder.AppendLine("\t{");
            foreach (var item in values)
            {
                stringBuilder.AppendLine($"\t\t{item},");
            }
            stringBuilder.AppendLine("\t}");
            stringBuilder.AppendLine("}");

            string code = stringBuilder.ToString();
            File.WriteAllText(fullPath, code);
            Debug.Log($"Genetated {fullPath}");
            CompilationPipeline.RequestScriptCompilation();
#endif
        }

        public static void GenerateEnumWithEnd<T>(string fullPath, string enumName, IEnumerable<T> values)
        {
#if UNITY_EDITOR

            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine("namespace TheSalt.Core");
            stringBuilder.AppendLine("{");
            stringBuilder.AppendLine($"\tpublic enum {enumName}");
            stringBuilder.AppendLine("\t{");
            foreach (var item in values)
            {
                stringBuilder.AppendLine($"\t\t{item},");
            }
            stringBuilder.AppendLine("\t\tEnd,");
            stringBuilder.AppendLine("\t}");
            stringBuilder.AppendLine("}");

            string code = stringBuilder.ToString();
            File.WriteAllText(fullPath, code);
            Debug.Log($"Genetated {fullPath}");
            CompilationPipeline.RequestScriptCompilation();
#endif
        }

        public static void GenerateEnumWithNone<T>(string fullPath, string enumName, IEnumerable<T> values)
        {
#if UNITY_EDITOR

            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine("namespace TheSalt.Core");
            stringBuilder.AppendLine("{");
            stringBuilder.AppendLine($"\tpublic enum {enumName}");
            stringBuilder.AppendLine("\t{");
            stringBuilder.AppendLine("\t\tNone,");
            foreach (var item in values)
            {
                stringBuilder.AppendLine($"\t\t{item},");
            }
            stringBuilder.AppendLine("\t}");
            stringBuilder.AppendLine("}");

            string code = stringBuilder.ToString();
            File.WriteAllText(fullPath, code);
            Debug.Log($"Genetated {fullPath}");
            CompilationPipeline.RequestScriptCompilation();
#endif
        }

        public static string KiloFormat(this int num)
        {
            if (num >= 100000000)
                return (num / 1000000).ToString("#,0M");

            if (num >= 10000000)
                return (num / 1000000).ToString("0.#") + "M";

            if (num >= 100000)
                return (num / 1000).ToString("#,0K");

            if (num >= 10000)
                return (num / 1000).ToString("0.#") + "K";

            return num.ToString("#,0");
        }

        public static string KiloFormat(this float num)
        {
            if (num >= 100000000)
                return (num / 1000000).ToString("#,0M");

            if (num >= 10000000)
                return (num / 1000000).ToString("0.#") + "M";

            if (num >= 100000)
                return (num / 1000).ToString("#,0K");

            if (num >= 10000)
                return (num / 1000).ToString("0.#") + "K";

            return num.ToString("#,0");
        }

        public static bool IsDigit(this char c)
        {
            if (c < 0x30 || c > 0x39) return false;
            return true;
        }

        public static int ToInt(this char c)
        {
            return c - 0x30;
        }

        public static T ParseEnum<T>(string s)
        {
            return (T)Enum.Parse(typeof(T), s);
    }
    }
}
