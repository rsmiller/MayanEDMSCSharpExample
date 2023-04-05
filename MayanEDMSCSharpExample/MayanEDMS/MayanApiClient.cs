using System;
using RestSharp.Authenticators;
using RestSharp;
using System.Threading.Tasks;
using MayanEDMSCSharpExample.MayanEDMS.ApiResponses;
using MayanEDMSCSharpExample.MayanEDMS.ApiRequests;
using MayanEDMSCSharpExample.MayanEDMS.Models;

namespace MayanEDMSCSharpExample.MayanEDMS
{
    public class MayanApiClient
    {
        private IMayanConnectionSettings _Settings;
        private RestClient _Client;

        public MayanApiClient(IMayanConnectionSettings settings)
        {
            _Settings = settings;

            var restClientOptions = new RestClientOptions(settings.api_url);
            restClientOptions.Authenticator = new HttpBasicAuthenticator(settings.username, settings.password);

            _Client = new RestClient(restClientOptions);
        }

        public async Task<MayanApiResponse<DocumentSearchResponse>> Search(string wildcard)
        {
            try
            {
                var request = new RestRequest("search/documents.documentsearchresult/", Method.Get);
                request.AddOrUpdateHeader("Content-Type", "multipart/form-data");
                request.AddParameter("q", wildcard);

                var response = await _Client.ExecuteAsync<DocumentSearchResponse>(request);

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    return new MayanApiResponse<DocumentSearchResponse>(response.Data);
                }
                else
                {
                    return new MayanApiResponse<DocumentSearchResponse>(null, response.StatusDescription);
                }
            }
            catch (Exception e)
            {
                return new MayanApiResponse<DocumentSearchResponse>(e);
            }
        }

        public async Task<MayanApiResponse<DocumentResult>> GetDocument(int document_id)
        {
            try
            {
                var request = new RestRequest("document/" + document_id + "/", Method.Get);
                request.AddOrUpdateHeader("Content-Type", "multipart/form-data");

                var response = await _Client.ExecuteAsync<DocumentResult>(request);

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    return new MayanApiResponse<DocumentResult>(response.Data);
                }
                else
                {
                    return new MayanApiResponse<DocumentResult>(null, response.StatusDescription);
                } 
            }
            catch (Exception e)
            {
                return new MayanApiResponse<DocumentResult>(e);
            }
        }

        public async Task<MayanApiResponse<CreatedDocumentResponse>> CreateDocument(string file_name, string file_path, int mayan_document_action_id)
        {
            try
            {
                var firstRequest = new RestRequest("documents/", Method.Post);
                firstRequest.AddJsonBody<CreateDocumentRequest>(new CreateDocumentRequest() { label = file_name });

                var firstResponse = await _Client.ExecuteAsync<CreatedDocumentResponse>(firstRequest);

                if (firstResponse.StatusCode == System.Net.HttpStatusCode.Created)
                {
                    var secondRequest = new RestRequest("documents/" + firstResponse.Data.id + "/files/", Method.Post);
                    secondRequest.AddOrUpdateHeader("Content-Type", "multipart/form-data");
                    secondRequest.AddParameter("action", mayan_document_action_id);
                    secondRequest.AddParameter("filename", file_name);
                    secondRequest.AddFile("file_new", file_path);

                    var secondResponse = await _Client.ExecuteAsync(secondRequest);

                    if (secondResponse.StatusCode == System.Net.HttpStatusCode.Accepted)
                    {
                        return new MayanApiResponse<CreatedDocumentResponse>(firstResponse.Data);
                    }
                    else
                    {
                        await this.DeleteDocument(firstResponse.Data.id);
                        return new MayanApiResponse<CreatedDocumentResponse>(null, secondResponse.StatusDescription);
                    }

                }

                return new MayanApiResponse<CreatedDocumentResponse>(null, firstResponse.StatusDescription);
            }
            catch(Exception e)
            {
                return new MayanApiResponse<CreatedDocumentResponse>(e);
            }
        }

        public async Task<MayanApiResponse<bool>> SetDocumentMetadata(int document_id, MetadataRequest metaModel)
        {
            try
            {
                var request = new RestRequest("documents/" + document_id + "/metadata/", Method.Post);
                request.AddJsonBody<MetadataRequest>(metaModel);

                var response = await _Client.ExecuteAsync(request);

                if (response.StatusCode == System.Net.HttpStatusCode.Created)
                    return new MayanApiResponse<bool>(true);
                else
                    return new MayanApiResponse<bool>(false, response.StatusDescription);
            }
            catch (Exception e)
            {
                return new MayanApiResponse<bool>(false, e);
            }
        }

        public async Task<MayanApiResponse<bool>> DeleteDocument(int document_id)
        {
            try
            {
                var request = new RestRequest("documents/" + document_id, Method.Delete);

                var response = await _Client.ExecuteAsync(request);

                if (response.StatusCode == System.Net.HttpStatusCode.Created)
                    return new MayanApiResponse<bool>(true);
                else
                    return new MayanApiResponse<bool>(false, response.StatusDescription);
            }
            catch(Exception e)
            {
                return new MayanApiResponse<bool>(false, e);
            }
        }
    }
}
