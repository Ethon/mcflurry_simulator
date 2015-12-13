using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace UFO.Commander {
    [Serializable()]
    public class Config {
        private const string FILENAME = "commander_config.xml";

        private static Config instance;

        public static Config Get() {
            return instance;
        }

        public static Config Load() {
            if(!File.Exists(FILENAME)) {
                instance = new Config();
            } else { 
                using(StreamReader inStream = File.OpenText(FILENAME)) {
                    XmlSerializer serializer = new XmlSerializer(typeof(Config));
                    instance = (Config)serializer.Deserialize(inStream);
                }
            }

            return instance;
        }

        public static void Save() {
            if(instance != null) {
                using(StreamWriter outStream = File.CreateText(FILENAME)) {
                    XmlSerializer serializer = new XmlSerializer(typeof(Config));
                    serializer.Serialize(outStream, instance);
                }
            }
        }

        public Config() {
            MediaRootPath = @"C:\";
            DatabaseServer = "localhost";
            DatabaseName = "ufo";
            DatabaseUser = "root";
            RequireCredentials = true;
        }

        public string MediaRootPath { get; set; }

        public string DatabaseServer { get; set; }

        public string DatabaseName { get; set; }

        public string DatabaseUser { get; set; }

        public bool RequireCredentials { get; set; }
    }
}
