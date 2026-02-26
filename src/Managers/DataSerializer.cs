using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace OutwardBasicChatCommands.Managers
{
    public class DataSerializer
    {
        private static DataSerializer _instance;

        private DataSerializer()
        {
        }

        public static DataSerializer Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new DataSerializer();

                return _instance;
            }
        }

        public static bool TryLoadFromXml<T>(string filePath, out T result)
        {
            result = default;

            if (!File.Exists(filePath))
                return false;

            try
            {
                var serializer = new XmlSerializer(typeof(T));
                var settings = new XmlReaderSettings { IgnoreComments = true };
                using var reader = XmlReader.Create(filePath, settings);
                result = (T)serializer.Deserialize(reader);
                return result != null;
            }
            catch (Exception ex)
            {
                OBCC.LogMessage($"DataSerializer@TryLoadFromXml Error: \"{ex.Message}\"");
                return false;
            }
        }

        public static bool SaveToXml<T>(T data, string filePath)
        {
            try
            {
                var directory = Path.GetDirectoryName(filePath);
                if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                var serializer = new XmlSerializer(typeof(T));

                var settings = new XmlWriterSettings
                {
                    Indent = true,
                    Encoding = Encoding.UTF8,
                    NewLineOnAttributes = false
                };

                using var writer = XmlWriter.Create(filePath, settings);
                serializer.Serialize(writer, data);

                return true;
            }
            catch (Exception ex)
            {
                OBCC.LogMessage($"DataSerializer@SaveToXml Error: \"{ex}\"");
                return false;
            }
        }
    }
}
