using System;
using System.Security.Cryptography;
using System.Text;

namespace Job.Framework.Common
{
    /// <summary>
    /// 加密类型格式
    /// </summary>
    public enum SecurityType
    {
        Defalut,
        Format
    }

    public sealed class SecurityHelper
    {
        private const string KEY_128 = "!0LINGLINGLING0!";
        private const string IV_128 = "!0ZEROBASELOVE0!";   //注意了，是16个字符，128位 

        /// <summary>
        /// 创建一个 MD5 加密字符
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private static string MD5(string input)
        {
            using (var md5 = System.Security.Cryptography.MD5.Create())
            {
                var data = md5.ComputeHash(Encoding.UTF8.GetBytes(input));

                var sb = new StringBuilder();

                for (var i = 0; i < data.Length; i++)
                {
                    sb.AppendFormat("{0:X2}", data[i]);
                }

                return sb.ToString();
            }
        }

        /// <summary>
        /// 创建一个安全的加盐 MD5 加密字符串
        /// </summary>
        /// <param name="input"></param>
        /// <param name="salt"></param>
        /// <returns></returns>
        public static string MD5(string input, string salt = KEY_128)
        {
            return SecurityHelper.MD5(string.Concat(SecurityHelper.MD5(input), salt));
        }

        public static string Encrypt(string input, SecurityType type = SecurityType.Defalut)
        {
            var byKey = Encoding.UTF8.GetBytes(KEY_128);
            var byIV = Encoding.UTF8.GetBytes(IV_128);
            var byData = Encoding.UTF8.GetBytes(input);

            switch (type)
            {
                case SecurityType.Format:
                    {
                        using (var des = new RijndaelManaged() { Key = byKey, IV = byIV, Mode = CipherMode.ECB, Padding = PaddingMode.PKCS7 })
                        {
                            using (var csEncrypt = des.CreateEncryptor())
                            {
                                var sb = new StringBuilder();

                                foreach (var item in csEncrypt.TransformFinalBlock(byData, 0, byData.Length))
                                {
                                    sb.AppendFormat("{0:X2}", item);
                                }

                                return sb.ToString();
                            }
                        }
                    }
                default:
                    {
                        using (var des = new RijndaelManaged() { Key = byKey, IV = byIV, Mode = CipherMode.ECB, Padding = PaddingMode.PKCS7 })
                        {
                            using (var csEncrypt = des.CreateEncryptor())
                            {
                                return Convert.ToBase64String(csEncrypt.TransformFinalBlock(byData, 0, byData.Length));
                            }
                        }
                    }
            }
        }

        public static string Decrypt(string input, SecurityType type = SecurityType.Defalut)
        {
            var byKey = Encoding.UTF8.GetBytes(KEY_128);
            var byIV = Encoding.UTF8.GetBytes(IV_128);

            switch (type)
            {
                case SecurityType.Format:
                    {
                        var len = input.Length / 2;
                        var byData = new byte[len];

                        for (int i = 0; i < len; i++)
                        {
                            byData[i] = (byte)Convert.ToInt32(input.Substring(i * 2, 2), 16);
                        }

                        using (var des = new RijndaelManaged() { Key = byKey, IV = byIV, Mode = CipherMode.ECB, Padding = PaddingMode.PKCS7 })
                        {
                            using (var csDecrypt = des.CreateDecryptor())
                            {
                                return Encoding.UTF8.GetString(csDecrypt.TransformFinalBlock(byData, 0, byData.Length));
                            }
                        }
                    }
                default:
                    {
                        var byData = Convert.FromBase64String(input);

                        using (var des = new RijndaelManaged() { Key = byKey, IV = byIV, Mode = CipherMode.ECB, Padding = PaddingMode.PKCS7 })
                        {
                            using (var csDecrypt = des.CreateDecryptor())
                            {
                                return Encoding.UTF8.GetString(csDecrypt.TransformFinalBlock(byData, 0, byData.Length));
                            }
                        }
                    }
            }
        }
    }
}
