using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace Microsoft.MixedReality.Toolkit.PackageManager
{
    public static class Serializers
    {
        /// <summary>
        /// Opens a file containing a serialized object, to be modified and
        /// written on disposal
        /// </summary>
        /// <typeparam name="T">Type contained in the file</typeparam>
        /// <param name="filePath">Full path to the file to open and write</param>
        /// <returns><see cref="ScopedSerializer{T}"/></returns>
        public static ScopedSerializer<T> OpenForWrite<T>(string filePath, XmlObjectSerializer serializer = null) where T : class
        {
            return new ScopedSerializer<T>(filePath, serializer ?? GetSerializer<T>());
        }

        /// <summary>
        /// Reads a stream and deserializes it into an object
        /// </summary>
        /// <typeparam name="T">Type to deserialize to</typeparam>
        /// <param name="stream">Stream to read</param>
        /// <param name="serializer">Optional serializer. If none is provided, <see cref="GetSerializer{T}"/> will be used</param>
        /// <returns>Instance of <typeparamref name="T"/></returns>
        public static T Read<T>(Stream stream, XmlObjectSerializer serializer = null) where T : class
        {
            serializer = serializer ?? GetSerializer<T>();
            return (T)GetSerializer<T>().ReadObject(stream);
        }

        /// <summary>
        /// Reads a file and deserializes it into an object
        /// </summary>
        /// <typeparam name="T">Type to deserialize to</typeparam>
        /// <param name="filePath">Path to read</param>
        /// <param name="serializer">Optional serializer. If none is provided, <see cref="GetSerializer{T}"/> will be used</param>
        /// <returns>Instance of <typeparamref name="T"/></returns>
        public static T Read<T>(string filePath, XmlObjectSerializer serializer = null) where T : class
        {
            serializer = serializer ?? GetSerializer<T>();
            return (T)GetSerializer<T>().ReadObject(File.Open(filePath, FileMode.Open));
        }

        /// <summary>
        /// Writes the instance to a file
        /// </summary>
        /// <typeparam name="T">Type of object</typeparam>
        /// <param name="filePath">Path to write to</param>
        /// <param name="instance">Instance of the object to write</param>
        /// <param name="serializer">Optional serializer. If none is provided, <see cref="GetSerializer{T}"/> will be used</param>
        public static void Write<T>(string filePath, T instance, XmlObjectSerializer serializer = null) where T : class
        {
            serializer = serializer ?? GetSerializer<T>();
            serializer.WriteObject(File.Open(filePath, FileMode.Create), instance);
        }

        /// <summary>
        /// Generates the default serializer for a specified type
        /// </summary>
        /// <typeparam name="T">Type for the serializer</typeparam>
        /// <returns>Serializer instance</returns>
        public static XmlObjectSerializer GetSerializer<T>() where T : class
        {
            return new DataContractJsonSerializer(typeof(T));
        }

        /// <summary>
        /// Serializer encapuslation that writes on disposal.
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        public class ScopedSerializer<T> : IDisposable where T : class
        {
            public void Dispose()
            {
                Write(this.filePath, this.Value, this.serializer);
            }

            public T Value { get; }

            private readonly XmlObjectSerializer serializer;
            private readonly string filePath;

            public ScopedSerializer(string filePath, XmlObjectSerializer serializer)
            {
                this.serializer = serializer;
                this.filePath = filePath;
                this.Value = Read<T>(filePath, serializer);
            }
        }
    }
}
