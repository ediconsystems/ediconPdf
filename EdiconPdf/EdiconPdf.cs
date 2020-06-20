using iTextSharp.text.pdf;
using System;
using System.IO;
using System.Text;

namespace EdiconPdf
{
    public class EdiconPdf
    {
        const string EDICON_EMBED_FILENAME = "edicon.xml";

        public static void EmbedEdiconData(string srcPdfFileName, string ediconXmlData, string dstPdfFileName)
        {
            //
            PdfReader reader = null;
            PdfStamper stamper = null;
            FileStream destPdfFileFileStream = null;
            //
            try
            {
                reader = new PdfReader(srcPdfFileName);
                destPdfFileFileStream = new FileStream(dstPdfFileName, FileMode.OpenOrCreate);
                stamper = new PdfStamper(reader, destPdfFileFileStream);
                PdfFileSpecification pfs = PdfFileSpecification.FileEmbedded(stamper.Writer, null/*srcDataFileName*/, EDICON_EMBED_FILENAME, Encoding.UTF8.GetBytes(ediconXmlData));
                stamper.AddFileAttachment(EDICON_EMBED_FILENAME, pfs);
            }
            catch (Exception ex)
            {
                Console.WriteLine("EmbedEdiconData error: " + ex.Message);
                throw;
            }
            finally
            {
                if (reader != null)
                    reader.Close();
                //
                if (stamper != null)
                    stamper.Close();
                //
                if (destPdfFileFileStream != null)
                    destPdfFileFileStream.Close();
            }
        }

        public static string CreateEdiconXmlData(byte[] sourceData, string format)
        {
            string encodedData = EncodeSourceData(sourceData);
            string ediconXmlData = AddEdiconEnvelope(encodedData, format);
            return ediconXmlData;
        }

        public static string ExtractEdiconData(string srcEdiconPdfFileName)
        {
            PdfReader reader = null;
            //
            try
            {
                reader = new PdfReader(srcEdiconPdfFileName);
                //
                PdfDictionary root = reader.Catalog;
                PdfDictionary documentnames = root.GetAsDict(PdfName.NAMES);
                PdfDictionary embeddedfiles = documentnames.GetAsDict(PdfName.EMBEDDEDFILES);
                PdfArray filespecs = embeddedfiles.GetAsArray(PdfName.NAMES);
                //
                for (int i = 0; i < filespecs.Size;)
                {
                    filespecs.GetAsString(i++);
                    PdfDictionary filespec = filespecs.GetAsDict(i++);
                    PdfDictionary refs = filespec.GetAsDict(PdfName.EF);
                    //
                    foreach (PdfName key in refs.Keys)
                    {
                        PRStream stream = (PRStream)PdfReader.GetPdfObject(refs.GetAsIndirectObject(key));
                        //
                        var outName = filespec.GetAsString(key).ToString();
                        //
                        if (outName.Equals(EDICON_EMBED_FILENAME, StringComparison.OrdinalIgnoreCase))
                        {
                            byte[] outByteArray = PdfReader.GetStreamBytes(stream);
                            string ediconData = Encoding.UTF8.GetString(outByteArray);
                            return ediconData;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("ExtractEdiconData error: " + ex.Message);
                throw;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }
            return null;
        }

        public static string AddEdiconEnvelope(string encodedContent, string format)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("<edicon version=\"1.0\" xmlns=\"http://edicon.cz/namespace/2020\" licence=\"http://edicon.cz/licence\">");
            sb.AppendLine(string.Format("<document type=\"invoice\" format=\"{0}\">", format));
            sb.AppendLine(encodedContent);
            sb.AppendLine("</document>");
            sb.AppendLine("</edicon >");
            return sb.ToString();
        }

        public static string EncodeSourceData(byte[] sourceData) {
            return Convert.ToBase64String(sourceData, Base64FormattingOptions.InsertLineBreaks);
        }
    }
}
