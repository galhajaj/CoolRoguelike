using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;

public class Utils
{
    // =========================================================================================== //
    /// <summary>
    /// Writes the given object instance to a binary file.
    /// <para>Object type (and all child types) must be decorated with the [Serializable] attribute.</para>
    /// <para>To prevent a variable from being serialized, decorate it with the [NonSerialized] attribute; cannot be applied to properties.</para>
    /// </summary>
    /// <typeparam name="T">The type of object being written to the XML file.</typeparam>
    /// <param name="filePath">The file path to write the object instance to.</param>
    /// <param name="objectToWrite">The object instance to write to the XML file.</param>
    /// <param name="append">If false the file will be overwritten if it already exists. If true the contents will be appended to the file.</param>
    public static void WriteToBinaryFile<T>(string filePath, T objectToWrite, bool append = false)
    {
        using (Stream stream = File.Open(filePath, append ? FileMode.Append : FileMode.Create))
        {
            var binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
            binaryFormatter.Serialize(stream, objectToWrite);
        }
    }

    /// <summary>
    /// Reads an object instance from a binary file.
    /// </summary>
    /// <typeparam name="T">The type of object to read from the XML.</typeparam>
    /// <param name="filePath">The file path to read the object instance from.</param>
    /// <returns>Returns a new instance of the object read from the binary file.</returns>
    public static T ReadFromBinaryFile<T>(string filePath)
    {
        using (Stream stream = File.Open(filePath, FileMode.Open))
        {
            var binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
            return (T)binaryFormatter.Deserialize(stream);
        }
    }
    // =========================================================================================== //
    /// <summary>
    /// Writes the given object instance to an XML file.
    /// <para>Only Public properties and variables will be written to the file. These can be any type though, even other classes.</para>
    /// <para>If there are public properties/variables that you do not want written to the file, decorate them with the [XmlIgnore] attribute.</para>
    /// <para>Object type must have a parameterless constructor.</para>
    /// </summary>
    /// <typeparam name="T">The type of object being written to the file.</typeparam>
    /// <param name="filePath">The file path to write the object instance to.</param>
    /// <param name="objectToWrite">The object instance to write to the file.</param>
    /// <param name="append">If false the file will be overwritten if it already exists. If true the contents will be appended to the file.</param>
    public static void WriteToXmlFile<T>(string filePath, T objectToWrite, bool append = false) where T : new()
    {
        TextWriter writer = null;
        try
        {
            var serializer = new XmlSerializer(typeof(T));
            writer = new StreamWriter(filePath, append);
            serializer.Serialize(writer, objectToWrite);
        }
        finally
        {
            if (writer != null)
                writer.Close();
        }
    }

    /// <summary>
    /// Reads an object instance from an XML file.
    /// <para>Object type must have a parameterless constructor.</para>
    /// </summary>
    /// <typeparam name="T">The type of object to read from the file.</typeparam>
    /// <param name="filePath">The file path to read the object instance from.</param>
    /// <returns>Returns a new instance of the object read from the XML file.</returns>
    public static T ReadFromXmlFile<T>(string filePath) where T : new()
    {
        TextReader reader = null;
        try
        {
            var serializer = new XmlSerializer(typeof(T));
            reader = new StreamReader(filePath);
            return (T)serializer.Deserialize(reader);
        }
        finally
        {
            if (reader != null)
                reader.Close();
        }
    }
    // =========================================================================================== //
    // remove the (clone) addition that adds to an instantiate obj
    public static string GetCleanName(string rawName)
    {
        return rawName.Split('(')[0].Trim();
    }
    // =========================================================================================== //
    public static Direction GetDirectionByName(string name)
    {
        if (name == "North")
            return Direction.NORTH;
        if (name == "South")
            return Direction.SOUTH;
        if (name == "West")
            return Direction.WEST;
        if (name == "East")
            return Direction.EAST;
        if (name == "Up")
            return Direction.UP;
        if (name == "Down")
            return Direction.DOWN;
        if (name == "Origin")
            return Direction.ORIGIN;

        if (name == "NorthWest")
            return Direction.NORTH_WEST;
        if (name == "SouthWest")
            return Direction.SOUTH_WEST;
        if (name == "NorthEast")
            return Direction.NORTH_EAST;
        if (name == "SouthEast")
            return Direction.SOUTH_EAST;

        return Direction.NONE;
    }
    // =========================================================================================== //
    public static int Dice(int number, int sides)
    {
        int counter = 0;
        for (int i = 0; i < number; ++i)
            counter += Random.Range(1, sides + 1);
        return counter;
    }
    // =========================================================================================== //
    public static bool IsChanceOccured(int chancePercent)
    {
        int rand = Dice(1, 100);
        return (rand <= chancePercent);
    }
    // =========================================================================================== //
    public static T GetObjectUnderCursor<T>(params string[] layers)
    {
        if (layers.Length <= 0)
        {
            Debug.LogError("GetRaycastHitUnderCursor function should contain at least one layer");
            return default(T);
        }

        LayerMask layerMask = 0;
        foreach (string layer in layers)
            layerMask |= (1 << LayerMask.NameToLayer(layer));

        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, 0.0F, layerMask);

        if (!hit)
            return default(T);
        if (hit.collider == null)
            return default(T);
        return hit.collider.gameObject.GetComponent<T>();
    }
    // =========================================================================================== //
    // get the offset from central position for each socket
    public static Position GetSocketOffset(SocketType socketType)
    {
        switch (socketType)
        {
            case SocketType.HEAD:
                return new Position(-8, 75);
            case SocketType.NECK:
                return new Position(0, 40);
            case SocketType.TORSO:
                return new Position(-6, 16);
            case SocketType.BACK:
                return new Position(0, 0);
            case SocketType.WRIST:
                return new Position(0, -10);
            case SocketType.MAIN_HAND:
                return new Position(-41, 26);
            case SocketType.OFF_HAND:
                return new Position(20, 0);
            case SocketType.WAIST:
                return new Position(0, -20);
            case SocketType.FEET:
                return new Position(0, -60);
            /*case SocketType.FINGER:
                break;*/
            case SocketType.RANGED:
                return new Position(-19, 46);
            case SocketType.AMMO:
                return new Position(20, 35);
            /*case SocketType.POCKET:
                return new Position(0, -);*/
            default:
                Debug.LogError("Unknown socket type " + socketType.ToString());
                return Position.NullPosition;
        }
    }
    // =========================================================================================== //
}
