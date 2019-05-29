using Microsoft.AspNetCore.Mvc.Formatters;
using Never.Serialization;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace B2C.Admin.Web
{
    /// <summary>
    /// json格式序列化
    /// </summary>
    public class JsonFormatter : MediaTypeFormatter, IOutputFormatter, IInputFormatter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="JsonFormatter"/> class.
        /// </summary>
        public JsonFormatter()
        {
            this.SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/json"));
            this.SupportedEncodings.Add(new UTF8Encoding(encoderShouldEmitUTF8Identifier: false, throwOnInvalidBytes: true));
            this.SupportedEncodings.Add(new UnicodeEncoding(bigEndian: false, byteOrderMark: true, throwOnInvalidBytes: true));
        }

        /// <summary>
        /// 查询此 <see cref="T:System.Net.Http.Formatting.MediaTypeFormatter" /> 是否可以反序列化指定类型的对象。
        /// </summary>
        /// <param name="type">要反序列化的类型。</param>
        /// <returns>
        /// 如果 <see cref="T:System.Net.Http.Formatting.MediaTypeFormatter" /> 可以反序列化该类型，则为 true；否则为 false。
        /// </returns>
        /// <exception cref="System.ArgumentNullException">type is null</exception>
        public override bool CanReadType(Type type)
        {
            if (type == null)
                throw new ArgumentNullException("type is null");

            return true;
        }

        /// <summary>
        /// 查询此 <see cref="T:System.Net.Http.Formatting.MediaTypeFormatter" /> 是否可以序列化指定类型的对象。
        /// </summary>
        /// <param name="type">要序列化的类型。</param>
        /// <returns>
        /// 如果 <see cref="T:System.Net.Http.Formatting.MediaTypeFormatter" /> 可以序列化该类型，则为 true；否则为 false。
        /// </returns>
        /// <exception cref="System.ArgumentNullException">type is null</exception>
        public override bool CanWriteType(Type type)
        {
            if (type == null)
                throw new ArgumentNullException("type is null");

            return true;
        }

        /// <summary>
        /// 以异步方式反序列化指定类型的对象。
        /// </summary>
        /// <param name="type">要反序列化的对象的类型。</param>
        /// <param name="readStream">要读取的 <see cref="T:System.IO.Stream" />。</param>
        /// <param name="content"><see cref="T:System.Net.Http.HttpContent" />（如果可用）。它可以为 null。</param>
        /// <param name="formatterLogger">要将事件记录到的 <see cref="T:System.Net.Http.Formatting.IFormatterLogger" />。</param>
        /// <returns>
        /// 一个 <see cref="T:System.Threading.Tasks.Task" />，其结果将是给定类型的对象。
        /// </returns>
        public override Task<object> ReadFromStreamAsync(Type type, Stream readStream, HttpContent content, IFormatterLogger formatterLogger)
        {
            using (var reader = new StreamReader(readStream))
            {
                return Task.FromResult(SerializeEnvironment.JsonSerializer.DeserializeObject(reader.ReadToEnd(), type));
            }
        }

        /// <summary>
        /// 返回可以为给定参数设置响应格式的 <see cref="T:System.Net.Http.Formatting.MediaTypeFormatter" /> 专用实例。
        /// </summary>
        /// <param name="type">要设置格式的类型。</param>
        /// <param name="request">请求。</param>
        /// <param name="mediaType">媒体类型。</param>
        /// <returns>
        /// 返回 <see cref="T:System.Net.Http.Formatting.MediaTypeFormatter" />。
        /// </returns>
        public override MediaTypeFormatter GetPerRequestFormatterInstance(Type type, HttpRequestMessage request, MediaTypeHeaderValue mediaType)
        {
            return base.GetPerRequestFormatterInstance(type, request, mediaType);
        }

        /// <summary>
        /// 以异步方式反序列化指定类型的对象。
        /// </summary>
        /// <param name="type">要反序列化的对象的类型。</param>
        /// <param name="readStream">要读取的 <see cref="T:System.IO.Stream" />。</param>
        /// <param name="content"><see cref="T:System.Net.Http.HttpContent" />（如果可用）。它可以为 null。</param>
        /// <param name="formatterLogger">要将事件记录到的 <see cref="T:System.Net.Http.Formatting.IFormatterLogger" />。</param>
        /// <param name="cancellationToken">用于取消操作的标记。</param>
        /// <returns>
        /// 一个 <see cref="T:System.Threading.Tasks.Task" />，其结果将是给定类型的对象。
        /// </returns>
        public override Task<object> ReadFromStreamAsync(Type type, Stream readStream, HttpContent content, IFormatterLogger formatterLogger, CancellationToken cancellationToken)
        {
            return base.ReadFromStreamAsync(type, readStream, content, formatterLogger, cancellationToken);
        }

        /// <summary>
        /// 以异步方式写入指定类型的对象。
        /// </summary>
        /// <param name="type">要写入的对象的类型。</param>
        /// <param name="value">要写入的对象值。它可以为 null。</param>
        /// <param name="writeStream">要写入到的 <see cref="T:System.IO.Stream" />。</param>
        /// <param name="content"><see cref="T:System.Net.Http.HttpContent" />（如果可用）。它可以为 null。</param>
        /// <param name="transportContext"><see cref="T:System.Net.TransportContext" />（如果可用）。它可以为 null。</param>
        /// <returns>
        /// 将执行写操作的 <see cref="T:System.Threading.Tasks.Task" />。
        /// </returns>
        public override Task WriteToStreamAsync(Type type, object value, Stream writeStream, HttpContent content, TransportContext transportContext)
        {
            var json = Encoding.UTF8.GetBytes(SerializeEnvironment.JsonSerializer.SerializeObject(value));
            return writeStream.WriteAsync(json, 0, json.Length);
        }

        /// <summary>
        /// 以异步方式写入指定类型的对象。
        /// </summary>
        /// <param name="type">要写入的对象的类型。</param>
        /// <param name="value">要写入的对象值。它可以为 null。</param>
        /// <param name="writeStream">要写入到的 <see cref="T:System.IO.Stream" />。</param>
        /// <param name="content"><see cref="T:System.Net.Http.HttpContent" />（如果可用）。它可以为 null。</param>
        /// <param name="transportContext"><see cref="T:System.Net.TransportContext" />（如果可用）。它可以为 null。</param>
        /// <param name="cancellationToken">用于取消操作的标记。</param>
        /// <returns>
        /// 将执行写操作的 <see cref="T:System.Threading.Tasks.Task" />。
        /// </returns>
        public override Task WriteToStreamAsync(Type type, object value, Stream writeStream, HttpContent content, TransportContext transportContext, CancellationToken cancellationToken)
        {
            return base.WriteToStreamAsync(type, value, writeStream, content, transportContext, cancellationToken);
        }

        bool IOutputFormatter.CanWriteResult(OutputFormatterCanWriteContext context)
        {
            return this.CanWriteType(context.ObjectType);
        }

        Task IOutputFormatter.WriteAsync(OutputFormatterWriteContext context)
        {
            using (var writer = context.WriterFactory(context.HttpContext.Response.Body, Encoding.UTF8))
            {
                EasyJsonSerializer.SerializeObject(context.Object, writer);
                return writer.FlushAsync();
            }
        }

        bool IInputFormatter.CanRead(InputFormatterContext context)
        {
            return this.CanReadType(context.ModelType);
        }

        Task<InputFormatterResult> IInputFormatter.ReadAsync(InputFormatterContext context)
        {
            using (var reader = new StreamReader(context.HttpContext.Request.Body))
            {
                var text = reader.ReadToEnd();
                try
                {
                    return InputFormatterResult.SuccessAsync(EasyJsonSerializer.DeserializeObject(text, context.ModelType));
                }
                catch
                {
                    return InputFormatterResult.SuccessAsync(Newtonsoft.Json.JsonConvert.DeserializeObject(text, context.ModelType));
                }
            }
        }
    }
}