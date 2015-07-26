using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Linqify
{
    public interface ILinqifyExecutor : IDisposable
    {
        /// <summary>
        /// Gets or sets the http client handler. This helps to customise the HTTP request/petitios
        /// </summary>
        HttpClientHandler HttpClientHandler { get; }

        /// <summary>
        /// Allows callers to cancel operation (where applicable)
        /// </summary>
        CancellationToken CancellationToken { get; set; }

        /// <summary>
        /// Gets the most recent URL executed
        /// </summary>
        /// <remarks>
        /// This is very useful for debugging
        /// </remarks>
        Uri LastUrl { get; }

        /// <summary>
        /// list of response headers from query
        /// </summary>
        IDictionary<string, string> ResponseHeaders { get; set; }

        /// <summary>
        /// Gets and sets HTTP UserAgent header
        /// </summary>
        string UserAgent { get; set; }

        /// <summary>
        /// Timeout (milliseconds) for writing to request
        /// stream or reading from response stream
        /// </summary>
        int ReadWriteTimeout { get; set; }

        /// <summary>
        /// Timeout (milliseconds) to wait for a server response
        /// </summary>
        int Timeout { get; set; }

        /// <summary>
        /// makes HTTP call to the REST API
        /// </summary>
        /// <param name="url">URL with all query info</param>
        /// <param name="req">The request</param>
        /// <param name="reqProc">Request Processor for Async Results</param>
        /// <returns>JSON Results from the REST API</returns>
        Task<string> QueryApiAsync<T>(Request req, IRequestProcessor<T> reqProc);

        /// <summary>
        /// performs HTTP POST to the REST API
        /// </summary>
        /// <param name="url">URL of request</param>
        /// <param name="postData">parameters to post</param>
        /// <param name="getResult">callback for handling async Json response - null if synchronous</param>
        /// <param name="cancelToken">The cancellation token for Tasks</param>
        /// <returns>Json Response from the REST API - empty string if async</returns>
        Task<string> PostToApiAsync<T>(string url, IDictionary<string, string> postData, CancellationToken cancelToken);
    }
}