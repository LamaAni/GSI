using GSI.JSON;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

public static class __Global_JSON_EXTENTIONS
{
    #region Json serialization

    /// <summary>
    /// Convert the IJsonObject to json.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="obj"></param>
    /// <returns></returns>
    public static string ToJson<T>(this T obj) where T : IJsonObject
    {
        DataContractJsonSerializerSettings settings =
            new DataContractJsonSerializerSettings();
        settings.UseSimpleDictionaryFormat = true;
        settings.SerializeReadOnlyTypes = false;
        DataContractJsonSerializer serializer =
            new DataContractJsonSerializer(obj.GetType(), settings);
        MemoryStream strm = new MemoryStream();
        serializer.WriteObject(strm, obj);

        StreamReader reader = new StreamReader(strm);
        strm.Seek(0, SeekOrigin.Begin);
        string rslt = reader.ReadToEnd();
        strm.Close();
        strm.Dispose();
        return rslt;
    }

        /// <summary>
    /// Reads an IJson object from json.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="json"></param>
    /// <returns></returns>
    public static T FromJson<T>(this string json) where T : IJsonObject
    {
        return (T)json.FromJson(typeof(T));
    }


    /// <summary>
    /// Reads an IJson object from json.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="json"></param>
    /// <returns></returns>
    public static IJsonObject FromJson(this string json, Type t)
    {
        DataContractJsonSerializerSettings settings =
            new DataContractJsonSerializerSettings();
        settings.UseSimpleDictionaryFormat = true;
        settings.SerializeReadOnlyTypes = false;

        DataContractJsonSerializer serializer =
           new DataContractJsonSerializer(t, settings);
        MemoryStream stream = new MemoryStream();
        StreamWriter writer = new StreamWriter(stream);
        writer.Write(json);
        writer.Flush();
        stream.Position = 0;
        IJsonObject obj = (IJsonObject)serializer.ReadObject(stream);
        stream.Close();
        stream.Dispose();
        return obj;
    }

    #endregion
}
