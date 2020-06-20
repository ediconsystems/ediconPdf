using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace EdiconPdf
{
    class EdiconXml
    {
        public string SourceData;

        private bool _isDecoded = false;
        private string _documentType;
        private string _fileFormat;
        private string _decodedContent;

        public EdiconXml(string sourceData)
        {
            SourceData = sourceData;
        }

        public string GetFileFormat()
        {
            if (!_isDecoded) Decode();
            return _fileFormat;
        }

        public string GetDocumentType()
        {
            if (!_isDecoded) Decode();
            return _documentType;
        }

        public string GetDecodedContent()
        {
            if (!_isDecoded) Decode();
            return _decodedContent;
        }

        private void Decode()
        {
            try
            {
                XmlDocument xml = new XmlDocument();
                xml.LoadXml(SourceData);
                XmlNode edicon = xml["edicon"];
                XmlNode document = edicon["document"];
                _documentType = document.Attributes["type"].InnerText;
                _fileFormat = document.Attributes["format"].InnerText;
                //
                string b64data = document.InnerText;
                _decodedContent = Encoding.UTF8.GetString(Convert.FromBase64String(b64data));
                _isDecoded = true;
            }
            catch (Exception)
            {
                _isDecoded = false;
                _documentType = null;
                _decodedContent = null;
                _fileFormat = null;
                throw;
            }
        }
    }
}
