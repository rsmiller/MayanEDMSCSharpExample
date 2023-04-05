////////////////////////////////////////////////////////////////////////////////
/// This is an example of how to use C# to connect to Mayan EDMS via v4 API
/// I make no warrenties. Do what you want with this code.
/// What I have provided you here should be more than enough to develop on

using MayanEDMSCSharpExample.MayanEDMS;
using Microsoft.Extensions.Configuration;
using MayanEDMSCSharpExample.MayanEDMS.ApiRequests;

var config = new ConfigurationBuilder().AddJsonFile("appsettings.json", optional: false).Build();

var mayanConnection = new MayanConnectionSettings();

config.GetSection("MayanEDMS").Bind(mayanConnection);

var client = new MayanApiClient(mayanConnection);


////////////////////////////////////////////////////////////////////////////////
/// Perform Search
/// 
var search_results = await client.Search("SOME ID");

if(search_results.Success)
{
    foreach (var result in search_results.Data.results)
    {
        Console.WriteLine(result.file_latest.file); // Latest file donwload link
    }
}


////////////////////////////////////////////////////////////////////////////////
/// Post File
/// 
var file_info = new FileInfo(@"C:\SomeFileAndJunk.pdf");

var created_response = await client.CreateDocument(file_info.Name, file_info.FullName, MayanDocumentTypeId.Sales_Order);

////////////////////////////////////////////////////////////////////////////////
/// Add meta data to the created file
/// 
var metadata_response = await client.SetDocumentMetadata(created_response.Data.id, new MetadataRequest()
                        {
                            metadata_type_id = MayanDocumentMetadataTypeId.Order_Num,
                            value = "152637"
                        });


////////////////////////////////////////////////////////////////////////////////
/// Delete the file
/// 
var deleted_response = await client.DeleteDocument(created_response.Data.id);