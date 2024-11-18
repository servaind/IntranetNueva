using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace Servaind.Intranet.Web.Helpers
{
    public static class XmlHelper
    {
        public static XmlDocument ToXmlDocument<T>(this T obj)
        {
            var result = new XmlDocument();
            using (var writer = new StringWriter())
            {
                var serializer = new XmlSerializer(typeof(T), new[] { obj.GetType() });

                serializer.Serialize(writer, obj);

                result.LoadXml(writer.ToString());
            }

            return result;
        }

        public static T FromXmlString<T>(this string xml, Type[] extraTypes = null)
        {
            var _serializer = new XmlSerializer(typeof(T), extraTypes);
            using (var _reader = new StringReader(xml))
            {
                return (T)_serializer.Deserialize(_reader);
            }
        }
    }
}