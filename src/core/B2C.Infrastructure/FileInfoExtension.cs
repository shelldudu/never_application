using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace B2C.Infrastructure
{
    /// <summary>
    /// 文件读取
    /// </summary>
    public static class FileInfoExtension
    {
        /// <summary>
        /// 转成base64内容
        /// </summary>
        public static string ToBase64(this string file)
        {
            return ToBase64(new System.IO.FileInfo(file));
        }

        /// <summary>
        /// 转成base64内容
        /// </summary>
        public static string ToBase64(this System.IO.FileInfo fileInfo)
        {
            using (var file = fileInfo.OpenRead())
            using (var st = new MemoryStream())
            {
                file.CopyTo(st);
                file.Flush();
                st.Position = 0;
                var base64 = Convert.ToBase64String(st.ToArray());
                return base64;
            }
        }
    }
}
