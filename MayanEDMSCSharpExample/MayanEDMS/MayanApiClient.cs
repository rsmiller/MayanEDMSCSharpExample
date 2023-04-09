using RestSharp.Authenticators;
using RestSharp;
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

        /// <summary>
        /// Creates an authentication token
        /// </summary>
        /// <returns>MayanApiResponse<MayanTokenResponse></returns>
        public async Task<MayanApiResponse<MayanTokenResponse>> GetToken()
        {
            try
            {
                var request = new RestRequest("auth/token/obtain/?format=json", Method.Post);
                request.AddOrUpdateHeader("Content-Type", "multipart/form-data");
                request.AddParameter("username", _Settings.username);
                request.AddParameter("password", _Settings.password);

                var response = await _Client.ExecuteAsync<MayanTokenResponse>(request);

                if (response.StatusCode == System.Net.HttpStatusCode.Created)
                {
                    // Recreating the client with the new type of auth
                    var restClientOptions = new RestClientOptions(_Settings.api_url);
                    restClientOptions.Authenticator = new JwtAuthenticator(response.Data.token);

                    _Client = new RestClient(restClientOptions);

                    return new MayanApiResponse<MayanTokenResponse>(response.Data);
                }
                else
                {
                    return new MayanApiResponse<MayanTokenResponse>(null, response.StatusDescription);
                }
            }
            catch(Exception e)
            {
                return new MayanApiResponse<MayanTokenResponse>(e);
            }
        }

        /// <summary>
        /// Performs a wildcard search on document metadata or OCR data
        /// </summary>
        /// <param name="wildcard">String to search for</param>
        /// <returns>MayanApiResponse<DocumentSearchResponse></returns>
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

        /// <summary>
        /// Returns a document object
        /// </summary>
        /// <param name="document_id">Mayan document id</param>
        /// <returns>MayanApiResponse<DocumentResult></returns>
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

        /// <summary>
        /// Creates a document and uploads a file
        /// </summary>
        /// <param name="file_name">The name of the file</param>
        /// <param name="file_path">Complete file path of the file</param>
        /// <param name="mayan_document_action_id">Add document action Id</param>
        /// <returns>MayanApiResponse<CreatedDocumentResponse></returns>
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

        /// <summary>
        /// Creates a document and uploads a file
        /// </summary>
        /// <param name="file_name">The name of the file</param>
        /// <param name="file_path">Complete file path of the file</param>
        /// <param name="mayan_document_action_id">Add document action Id</param>
        /// <param name="mayan_document_type_id">Add document type Id</param>
        /// <returns>MayanApiResponse<CreatedDocumentResponse></returns>
        public async Task<MayanApiResponse<CreatedDocumentResponse>> CreateDocument(string file_name, string file_path, int mayan_document_action_id, int mayan_document_type_id)
        {
            try
            {
                var firstRequest = new RestRequest("documents/", Method.Post);
                firstRequest.AddJsonBody<CreateDocumentRequest>(new CreateDocumentRequest() { label = file_name, document_type_id = mayan_document_type_id });

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
            catch (Exception e)
            {
                return new MayanApiResponse<CreatedDocumentResponse>(e);
            }
        }

        /// <summary>
        /// Creates a document and uploads a file
        /// </summary>
        /// <param name="file_name">The name of the file</param>
        /// <param name="file_data">File byte data</param>
        /// <param name="mayan_document_action_id">Add document action Id</param>
        /// <returns>MayanApiResponse<CreatedDocumentResponse></returns>
        public async Task<MayanApiResponse<CreatedDocumentResponse>> CreateDocument(string file_name, byte[] file_data, int mayan_document_action_id)
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
                    secondRequest.AddFile("file_new", file_data, file_name);

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
            catch (Exception e)
            {
                return new MayanApiResponse<CreatedDocumentResponse>(e);
            }
        }

        /// <summary>
        /// Creates a document and uploads a file
        /// </summary>
        /// <param name="file_name">The name of the file</param>
        /// <param name="file_data">File byte data</param>
        /// <param name="mayan_document_action_id">Add document action Id</param>
        /// <param name="mayan_document_type_id">Add document type Id</param>
        /// <returns>MayanApiResponse<CreatedDocumentResponse></returns>
        public async Task<MayanApiResponse<CreatedDocumentResponse>> CreateDocument(string file_name, byte[] file_data, int mayan_document_action_id, int mayan_document_type_id)
        {
            try
            {
                var firstRequest = new RestRequest("documents/", Method.Post);
                firstRequest.AddJsonBody<CreateDocumentRequest>(new CreateDocumentRequest() { label = file_name, document_type_id = mayan_document_type_id });

                var firstResponse = await _Client.ExecuteAsync<CreatedDocumentResponse>(firstRequest);

                if (firstResponse.StatusCode == System.Net.HttpStatusCode.Created)
                {
                    var secondRequest = new RestRequest("documents/" + firstResponse.Data.id + "/files/", Method.Post);
                    secondRequest.AddOrUpdateHeader("Content-Type", "multipart/form-data");
                    secondRequest.AddParameter("action", mayan_document_action_id);
                    secondRequest.AddParameter("filename", file_name);
                    secondRequest.AddFile("file_new", file_data, file_name);

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
            catch (Exception e)
            {
                return new MayanApiResponse<CreatedDocumentResponse>(e);
            }
        }

        /// <summary>
        /// Sets the metadata of a document
        /// </summary>
        /// <param name="document_id">Mayan document id</param>
        /// <param name="metaModel">MetadataRequest object</param>
        /// <returns>MayanApiResponse<bool></returns>
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

        /// <summary>
        /// Deletes a document
        /// </summary>
        /// <param name="document_id">Mayan document id</param>
        /// <returns>MayanApiResponse<bool></returns>
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
