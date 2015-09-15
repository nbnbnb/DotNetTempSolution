using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace CommonLib.Concrete
{
    public static class ObjectExtension
    {
        /// <summary>
        /// 将对象序列化成XML字符串
        /// </summary>
        /// <param name="obj">实体对象</param>
        /// <returns></returns>
        public static string Serialize(object obj)
        {
            return Serialize(obj, obj.GetType(), Encoding.UTF8);
        }

        /// <summary>
        /// 将对象序列化成XML字符串
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string Serialize(object obj, Type type)
        {
            return Serialize(obj, type, Encoding.UTF8);
        }

        /// <summary>
        /// 将对象序列化成XML字符串
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="type"></param>
        /// <param name="encode"></param>
        /// <returns></returns>
        public static string Serialize(object obj, Type type, Encoding encode)
        {
            try
            {
                var serializer = new XmlSerializer(type);
                var namespaces = new XmlSerializerNamespaces();
                namespaces.Add(string.Empty, string.Empty);
                using (var stream = new MemoryStream())
                {
                    serializer.Serialize(stream, obj, namespaces);
                    var str = encode.GetString(stream.GetBuffer());
                    return str.TrimEnd(new char[1]);
                }
            }
            catch
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// 序列化数据写入文件
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="type"></param>
        /// <param name="path"></param>
        public static void SerializeToFile(object obj, Type type, string path)
        {
            try
            {
                var serializer = new XmlSerializer(type);
                using (var writer = new StreamWriter(path))
                {
                    serializer.Serialize(writer, obj);
                }
            }
            catch
            {
            }
        }

        /// <summary>
        /// 将对象序列化成JSON字符串
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string SerializeJson(object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }

        /// <summary>
        /// 将XML字符串序列化成实体
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="xml">待转换字符串</param>
        /// <returns></returns>
        public static T Deserialize<T>(string xml) where T : class
        {
            return Deserialize<T>(xml, Encoding.UTF8, false);
        }

        /// <summary>
        /// 将XML字符串序列化成实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="xml"></param>
        /// <param name="encode"></param>
        /// <returns></returns>
        public static T Deserialize<T>(string xml, Encoding encode) where T : class
        {
            return Deserialize<T>(xml, encode, false);
        }

        /// <summary>
        /// 将XML字符串序列化成实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="xml"></param>
        /// <param name="encode"></param>
        /// <param name="isThrow"></param>
        /// <returns></returns>
        public static T Deserialize<T>(string xml, Encoding encode, bool isThrow) where T : class
        {
            try
            {
                var serializer = new XmlSerializer(typeof(T));
                using (var stream = new MemoryStream(encode.GetBytes(xml)))
                {
                    return serializer.Deserialize(stream) as T;
                }
            }
            catch
            {
                if (isThrow) throw;
                return null;
            }
        }

        /// <summary>
        /// 从文件序列化成实体
        /// </summary>
        /// <param name="path"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static object DeserializeFromFile(string path, Type type)
        {
            try
            {
                var serializer = new XmlSerializer(type);
                using (var stream = new FileStream(path, FileMode.Open))
                {
                    return serializer.Deserialize(stream);
                }
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 将JSON字符串序列化成实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="json"></param>
        /// <returns></returns>
        public static T DeserializeJson<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json);
        }

        /// <summary>
        /// 深度克隆对象
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="obj">要克隆的对象</param>
        /// <returns></returns>
        public static T Clone<T>(T obj)
        {
            var formatter = new BinaryFormatter(null, new StreamingContext(StreamingContextStates.Clone));
            using (var stream = new MemoryStream())
            {
                formatter.Serialize(stream, obj);
                stream.Position = 0;
                return (T)formatter.Deserialize(stream);
            }
        }

        /// <summary>
        /// 字符串数组是否包含指定字符串，不区分大小写
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public static bool IsContainsIgnoreCase(string[] source, string target)
        {
            return source.Any(str => str.Equals(target, StringComparison.InvariantCultureIgnoreCase));
        }

        /// <summary>
        /// 根据对象的属性名称得到对象属性的值
        /// </summary>
        /// <param name="obj">要获取的对象</param>
        /// <param name="attrName">对象的属性名</param>
        public static Object GetObjValueByPropert(object obj, string attrName)
        {
            if (obj == null) return null;

            var arrayAttr = obj.GetType().GetProperties();
            foreach (var pi in arrayAttr)
            {
                if (pi.Name != attrName) continue;

                var val = pi.GetValue(obj, null);
                if (val == null)
                {
                    return null;
                }

                if (pi.PropertyType.FullName == "System.DateTime")
                {
                    return Convert.ToDateTime(val);
                }

                return Convert.ToString(val);
            }

            return null;
        }

        /// <summary>
        /// 根据对象的属性名称得到对象属性类型
        /// </summary>
        /// <param name="obj">要获取的对象</param>
        /// <param name="attrName">对象的属性名</param>
        public static Type GetObjPropertyTypeByName(object obj, string attrName)
        {
            var arrayAttr = obj.GetType().GetProperties();
            return (from pi in arrayAttr where pi.Name == attrName select pi.PropertyType).FirstOrDefault();
        }
    }
}
