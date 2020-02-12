﻿// ***********************************************************************
// Assembly         : dotNetTips.Utility.Standard.Extensions
// Author           : David McCarter
// Created          : 06-01-2018
//
// Last Modified By : David McCarter
// Last Modified On : 02-12-2020
// ***********************************************************************
// <copyright file="HttpRequestExtensions.cs" company="dotNetTips.com - David McCarter">
//     dotNetTips.com - David McCarter
// </copyright>
// <summary></summary>
// ***********************************************************************
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace dotNetTips.Utility.Standard.Extensions
{
    /// <summary>
    /// Class HttpRequestExtensions.
    /// </summary>
    public static class HttpRequestExtensions
    {

        /// <summary>
        /// Retrieve the raw body as a string from the Request.Body stream.
        /// </summary>
        /// <param name="request">Request instance to apply to.</param>
        /// <param name="encoding">Optional - Encoding, defaults to UTF8.</param>
        /// <returns>Task&lt;System.String&gt;.</returns>
        /// <exception cref="System.ArgumentNullException">request</exception>
        public static async Task<string> GetRawBodyStringAsync(this HttpRequest request, Encoding encoding)
        {
            if (request is null)
            {
                throw new System.ArgumentNullException(nameof(request));
            }

            if (encoding == null)
            {
                encoding = Encoding.UTF8;
            }

            using (var reader = new StreamReader(request.Body, encoding))
            {
                return await reader.ReadToEndAsync().ConfigureAwait(true);
            }
        }

        /// <summary>
        /// Retrieves the raw body as a byte array from the Request.Body stream
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>Task&lt;System.Byte[]&gt;.</returns>
        /// <exception cref="ArgumentNullException">request</exception>
        public static async Task<byte[]> GetRawBodyBytesAsync(this HttpRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request), $"{nameof(request)} is null.");
            }

            using (var ms = new MemoryStream(2048))
            {
                await request.Body.CopyToAsync(ms).ConfigureAwait(true);
                return ms.ToArray();
            }
        }

        /// <summary>
        /// Tries the get HttpRequest body.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="request">The HTTPRequest object.</param>
        /// <param name="value">The return value.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        /// <remarks>Orginal code by Jerry Nixon</remarks>
        public static bool TryGetBody<T>(this HttpRequest request, out T value)
        {
            if (!request.TryGetBody(out var bytes))
            {
                value = default;
                return false;
            }

            try
            {
                var json = BitConverter.ToString(bytes);
                value = JsonConvert.DeserializeObject<T>(json);

                return true;
            }
            catch
            {
                value = default;
                return false;
            }
        }

        /// <summary>
        /// Tries the get HTTPRequest body.
        /// </summary>
        /// <param name="request">The HTTPRequest object.</param>
        /// <param name="value">The return value.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        /// <exception cref="Exception">HttpRequest has no body.</exception>
        /// <remarks>Orginal code by Jerry Nixon</remarks>

        public static bool TryGetBody(this HttpRequest request, out byte[] value)
        {
            try
            {
                if ((request.Body?.Length ?? 0) == 0)
                {
                    throw new Exception("HttpRequest has no body.");
                }

                value = GetBytes();

                return true;
            }
            catch
            {
                value = default;
                return false;
            }

            byte[] GetBytes()
            {
                byte[] bytes;
                using (var ms = new MemoryStream())
                {
                    request.Body.CopyTo(ms);
                    bytes = ms.ToArray();
                }
                return bytes;
            }
        }
    }
}
