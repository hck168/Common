using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Job.Framework.Config
{
    public class AppConfigSection : ConfigurationSection
    {
        public virtual string File
        {
            get
            {
                throw new Exception("派生类需要重写 File 属性");
            }
        }

        protected override void DeserializeElement(XmlReader reader, bool serializeCollectionKey)
        {
            base.DeserializeElement(reader, serializeCollectionKey);

            if (!string.IsNullOrEmpty(File))
            {
                var file = string.Empty;
                var source = base.ElementInformation.Source;

                if (string.IsNullOrEmpty(source))
                {
                    file = this.File;
                }
                else
                {
                    file = Path.Combine(Path.GetDirectoryName(source), this.File);
                }

                if (System.IO.File.Exists(file))
                {
                    using (var xReader = XmlReader.Create(file, new XmlReaderSettings { IgnoreComments = true, IgnoreWhitespace = true }))
                    {
                        while (xReader.Read())
                        {
                            if (xReader.NodeType == XmlNodeType.Element)
                            {
                                if (xReader.Name == reader.Name)
                                {
                                    xReader.MoveToContent();

                                    base.DeserializeElement(xReader, serializeCollectionKey);

                                    break;
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
