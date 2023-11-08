Imports System.IO
Imports System.Windows
Imports System.Xml.Serialization
Imports Skiclub.Entities

Module TestStreamreader

    Public Sub Main(filename As String)
        Dim serializerTL = New XmlSerializer(GetType(ParticipantCollection))
        Try
            ' Open the file using a stream reader.
            ' Read the stream as a string and write the string to the console.

            Using sr As New StreamReader(filename)
                ' Todo: Asynchrones Lesen
                ' https://learn.microsoft.com/de-de/dotnet/standard/io/how-to-read-text-from-a-file
                'ResultBlock.Text = Await sr.ReadToEndAsync()

                ' Todo: XML Serializer verstehen
                ' https://learn.microsoft.com/de-de/dotnet/api/system.xml.serialization.xmlserializer?view=netframework-4.7.2&f1url=%3FappId%3DDev16IDEF1%26l%3DDE-DE%26k%3Dk(System.Xml.Serialization.XmlSerializer)%3Bk(TargetFrameworkMoniker-.NETFramework%2CVersion%253Dv4.7.2)%3Bk(DevLang-VB)%26rd%3Dtrue

                Dim loadedTeilnehmerCollection = TryCast(serializerTL.Deserialize(sr), ParticipantCollection)
                Console.WriteLine(sr.ReadToEnd())
            End Using
        Catch e As IOException
            Console.WriteLine("The file could not be read:")
            Console.WriteLine(e.Message)
        End Try
    End Sub

End Module
