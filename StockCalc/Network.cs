using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using System.Net;
using System.IO;
using System.Runtime.InteropServices;
using System.Data;
using System.Data.SQLite;

namespace StockCalc
{


    /* 네트워크 관련 Static 클래스 */
    class Network
    {

        static string Key = "whckqtm";

        public static string PasKey
        {
            get
            {
                return Key;
            }
        }


        #region FTP 관련

        /// <summary>
        /// ftp에서 db가져오기
        /// </summary>
        /// <param name="p_address">ftp주소</param>
        /// <param name="p_id">ftp아이디</param>
        /// <param name="p_pw">ftp비번</param>
        /// <returns></returns>
        public static string FtpDown(string p_address, string p_id, string p_pw, string p_fileName)
        {
            string strAddress = Decrypt(p_address);
            string strId = Decrypt(p_id);
            string strPw = Decrypt(p_pw);
            string outputFile = @".\" + p_fileName;
            strAddress += @"/chops/DBServer/StockCalc/" + p_fileName;

            try
            {
                FtpWebRequest req = (FtpWebRequest)WebRequest.Create(strAddress);

                req.Method = WebRequestMethods.Ftp.DownloadFile;
                req.Credentials = new NetworkCredential(strId, strPw);

                using (FtpWebResponse resp = (FtpWebResponse)req.GetResponse())
                {
                    Stream stream = resp.GetResponseStream();

                    using (StreamReader reader = new StreamReader(stream))
                    {
                        using (BinaryWriter destStream = new BinaryWriter(new FileStream(outputFile, FileMode.Create)))
                        {
                            long length = resp.ContentLength;
                            int bufferSize = 2048;
                            int readCount;
                            byte[] buffer = new byte[2048];
                            readCount = stream.Read(buffer, 0, bufferSize);
                            while (readCount > 0)
                            {
                                destStream.Write(buffer, 0, readCount);
                                readCount = stream.Read(buffer, 0, bufferSize);
                            }
                            destStream.Close();
                            reader.Close();
                            resp.Close();
                        }
                    }

                }
                return "";
            }

            catch (Exception e)
            {
                return "서버로 부터의 파일 다운 실패 : \r\n" + e.ToString();
            }
        }

        /// <summary>
        /// 업로드
        /// </summary>
        /// <param name="p_address">ftp주소</param>
        /// <param name="p_id">ftp아이디</param>
        /// <param name="p_pw">ftp비번</param>
        /// <returns></returns>
        public static string FtpUp(string p_address, string p_id, string p_pw, string p_fileName)
        {
            string strAddress = Decrypt(p_address);
            string strId = Decrypt(p_id);
            string strPw = Decrypt(p_pw);
            string inputFile = System.Environment.CurrentDirectory + @"\" + p_fileName;
            //string strResult = String.Empty;
            strAddress += @"/chops/DBServer/StockCalc/" + p_fileName;
            try
            {
                FtpWebRequest req = (FtpWebRequest)WebRequest.Create(strAddress);

                req.Method = WebRequestMethods.Ftp.UploadFile;
                req.Credentials = new NetworkCredential(strId, strPw);
              
                // 그냥 업로드하거나 다운로드 할 경우, 인코딩 때문에 제대로된 파일이 안됨
                // 따라서 BinaryReader로 읽어주고, 써줘야 함. 
                byte[] data;
                using (BinaryReader binReader = new BinaryReader(File.OpenRead(inputFile)))
                {
                    FileInfo fi = new FileInfo(inputFile);
                    binReader.BaseStream.Position = 0;
                    data = binReader.ReadBytes((int)fi.Length);
                }
                req.ContentLength = data.Length;
                using (Stream requestStream = req.GetRequestStream())
                {
                    requestStream.Write(data, 0, data.Length);
                }

                using (FtpWebResponse response = (FtpWebResponse)req.GetResponse())
                {
                    response.Close();
                    //strResult = string.Format("Upload File Complete, status {0}", response.StatusDescription);
                }
                return "";
            }

            catch (Exception e)
            {
                return "서버로 업로드 실패 : \r\n" + e.ToString();
            }
        }

        /// <summary>
        /// FTP 서버의 파일 날짜 가져오기. 
        /// 처음 다운Sync를 맞추기 위함.      --> 사용하지 않음
        /// </summary>
        /// <param name="p_address"></param>
        /// <param name="p_id"></param>
        /// <param name="p_pw"></param>
        /// <returns></returns>
        public static DateTime FtpGetDate(string p_address, string p_id, string p_pw, out string strResult)
        {
            string strAddress = Decrypt(p_address);
            string strId = Decrypt(p_id);
            string strPw = Decrypt(p_pw);
            DateTime reDate = new DateTime();
            strAddress += @"/chops/DBServer/StockCalc/Contents.db";
            try
            {
                FtpWebRequest req = (FtpWebRequest)WebRequest.Create(strAddress);
                req.Method = WebRequestMethods.Ftp.GetDateTimestamp;
                req.Credentials = new NetworkCredential(strId, strPw);

                using (FtpWebResponse resp = (FtpWebResponse)req.GetResponse())
                {
                    reDate = DateTime.ParseExact(resp.StatusDescription.Substring(4).Trim(), "yyyyMMddHHmmss", null);
                    resp.Close();
                }
                strResult = "";
                return reDate;
            }
            catch (Exception e)
            {
                strResult = e.ToString();
                return reDate;
            }
        }

        /// <summary>
        /// FTP 파일이름 변경
        /// </summary>
        /// <param name="p_address">주소</param>
        /// <param name="p_id">아이디</param>
        /// <param name="p_pw">패스워드</param>
        /// <param name="p_fileName">원본파일이름</param>
        /// <param name="p_fileReName">변경파일이름</param>
        /// <returns></returns>
        public static string FtpRename(string p_address, string p_id, string p_pw, string p_fileName, string p_fileReName)
        {
            string strAddress = Decrypt(p_address);
            string strId = Decrypt(p_id);
            string strPw = Decrypt(p_pw);
            string strToFile = "";

            strAddress += @"/chops/DBServer/StockCalc/" + p_fileName;
            strToFile += @"/chops/DBServer/StockCalc/" + p_fileReName;

            try
            {
                FtpWebRequest req = (FtpWebRequest)WebRequest.Create(strAddress);

                req.Method = WebRequestMethods.Ftp.Rename;
                req.Credentials = new NetworkCredential(strId, strPw);
                req.RenameTo = strToFile;

                FtpWebResponse response = (FtpWebResponse)req.GetResponse();
                response.Close();
                return "";
            }

            catch (Exception e)
            {
                if (e.ToString().Contains("파일을 찾을 수 없거나"))
                    return "";
                else
                    return "FTP 파일이름 변경 실패 : \r\n" + e.ToString();
            }
        }

        /// <summary>
        /// FTP 파일삭제
        /// </summary>
        /// <param name="p_address">주소</param>
        /// <param name="p_id">아이디</param>
        /// <param name="p_pw">패스워드</param>
        /// <param name="p_fileName">삭제파일이름</param>
        /// <returns></returns>
        public static string FtpDel(string p_address, string p_id, string p_pw, string p_fileName)
        {
            string strAddress = Decrypt(p_address);
            string strId = Decrypt(p_id);
            string strPw = Decrypt(p_pw);

            strAddress += @"/chops/DBServer/StockCalc/" + p_fileName;
            try
            {
                FtpWebRequest req = (FtpWebRequest)WebRequest.Create(strAddress);

                req.Method = WebRequestMethods.Ftp.DeleteFile;
                req.Credentials = new NetworkCredential(strId, strPw);

                FtpWebResponse response = (FtpWebResponse)req.GetResponse();
                response.Close();
                return "";
            }

            catch (Exception e)
            {
                if (e.ToString().Contains("파일을 찾을 수 없거나"))
                    return "";
                else
                    return "FTP 파일삭제 실패 : \r\n" + e.ToString();
            }
        }



        #endregion FTP 관련_END


        #region 암호화
        /// <summary>
        /// 복호화
        /// </summary>
        /// <param name="textToDecrypt"></param>
        /// <param name="key">복호화키</param>
        /// <returns></returns>
        public static string Decrypt(string textToDecrypt)
        {
            if (textToDecrypt == "")
                return "";

            string key = Key;

            RijndaelManaged rijndaelCipher = new RijndaelManaged();

            rijndaelCipher.Mode = CipherMode.CBC;

            rijndaelCipher.Padding = PaddingMode.PKCS7;



            rijndaelCipher.KeySize = 128;

            rijndaelCipher.BlockSize = 128;

            byte[] encryptedData = Convert.FromBase64String(textToDecrypt);

            byte[] pwdBytes = Encoding.UTF8.GetBytes(key);

            byte[] keyBytes = new byte[16];

            int len = pwdBytes.Length;

            if (len > keyBytes.Length)
            {

                len = keyBytes.Length;

            }

            Array.Copy(pwdBytes, keyBytes, len);

            rijndaelCipher.Key = keyBytes;

            rijndaelCipher.IV = keyBytes;

            byte[] plainText = rijndaelCipher.CreateDecryptor().TransformFinalBlock(encryptedData, 0, encryptedData.Length);

            return Encoding.UTF8.GetString(plainText);

        }


        /// <summary>
        /// 암호화
        /// </summary>
        /// <param name="textToEncrypt"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string Encrypt(string textToEncrypt)
        {
            string key = Key;
            RijndaelManaged rijndaelCipher = new RijndaelManaged();

            rijndaelCipher.Mode = CipherMode.CBC;

            rijndaelCipher.Padding = PaddingMode.PKCS7;



            rijndaelCipher.KeySize = 128;

            rijndaelCipher.BlockSize = 128;

            byte[] pwdBytes = Encoding.UTF8.GetBytes(key);

            byte[] keyBytes = new byte[16];

            int len = pwdBytes.Length;

            if (len > keyBytes.Length)
            {

                len = keyBytes.Length;

            }

            Array.Copy(pwdBytes, keyBytes, len);

            rijndaelCipher.Key = keyBytes;

            rijndaelCipher.IV = keyBytes;

            ICryptoTransform transform = rijndaelCipher.CreateEncryptor();

            byte[] plainText = Encoding.UTF8.GetBytes(textToEncrypt);

            return Convert.ToBase64String(transform.TransformFinalBlock(plainText, 0, plainText.Length));
           
        }

        /// <summary>
        /// 파일 해쉬 구하자 
        /// </summary>
        /// <param name="path">파일의 경로</param>
        /// <returns>해쉬 string값</returns>
        public static string GetMD5(string path)
        {
            if (!File.Exists(path))
                return null;

            using (FileStream stream = File.OpenRead(path))
            {
                MD5 md5 = new MD5CryptoServiceProvider();
                byte[] byteChecksum = md5.ComputeHash(stream);
                return BitConverter.ToString(byteChecksum).Replace("-", String.Empty);
            }
        }
        #endregion 암호화_End


        #region ini컨트롤

        /* .ini파일 쓰는 함수
         * string section : [section]
         * string key : 값의 키(val의 key)
         * string val : 키의 값(key의 val)
         * filePath : 쓸 ini 파일경로
         */
        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);

        /* .ini파일 읽는 함수
         * string section : 가져올 값의 키가 속해있는 섹션이름
         * string key : 가져올 값의 키 이름
         * string def : 키의 값이 없을 경우 기본값(default)
         * StringBuilder retVal : 가져올 값
         * int size : 가져올 값의 길이
         * string filePath : 읽어올 ini 파일경로
         * 
         * return value : 읽어들여온 데이터 길이
         */
        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);


        public static void WriteIniFile(string section, string key, string value, string path)
        {
            WritePrivateProfileString(section, key, value, path);
        }

        public static string ReadIniFile(string section, string key, string path)
        {
            StringBuilder sb = new StringBuilder(255);
            GetPrivateProfileString(section, key, "", sb, sb.Capacity, path);
            return sb.ToString();
        }
        
        /* 정석사용예시 
        private void btnLogin_Click(object sender, EventArgs e)
        {
            //ini 쓰기
            WritePrivateProfileString("LOGIN", "ID", "test", "C:\\login.ini");
            WritePrivateProfileString("LOGIN", "PW", "1234", "C:\\login.ini");
            StringBuilder retId = new StringBuilder();
            StringBuilder retPw = new StringBuilder();

            //ini 읽기
            GetPrivateProfileString("LOGIN", "ID", "(NONE)", retId, 32, "C:\\login.ini");
            GetPrivateProfileString("LOGIN", "PW", "(NONE)", retPw, 32, "C:\\login.ini");
          
            MessageBox.Show("ID :"+ retId.ToString()+",PW: "+ retPw.ToString());
        }
        */
        #endregion ini컨트롤_End

        #region db관련
        /// <summary>
        /// db에서 Select하기 
        /// </summary>
        /// <param name="p_Sql">보낼 SQL</param>
        /// <returns>Return DataTable</returns>
        public static DataTable GetDBTable(string p_Sql)
        {
            SQLiteConnection Conn = new SQLiteConnection(Variables.DBROUTE);
            DataTable dt = new DataTable();
            Conn.Open();
            SQLiteDataAdapter Ap = new SQLiteDataAdapter(p_Sql, Conn);
            Ap.Fill(dt);
            Ap.Dispose();
            Conn.Dispose();
            return dt;
        }

        /// <summary>
        /// db에서 Select하기 
        /// </summary>
        /// <param name="p_Sql">보낼 SQL</param>
        /// <returns>Return DataSet</returns>
        public static DataSet GetDBSet(string p_Sql)
        {
            SQLiteConnection Conn = new SQLiteConnection(Variables.DBROUTE);
            DataSet ds = new DataSet();
            Conn.Open();
            SQLiteDataAdapter Ap = new SQLiteDataAdapter(p_Sql, Conn);
            Ap.Fill(ds);
            Ap.Dispose();
            Conn.Dispose();
            return ds;
        }

        /// <summary>
        /// sql 실행하기
        /// </summary>
        /// <param name="p_Sql"></param>
        public static int ExecDB(string p_Sql)
        {
            SQLiteConnection Conn = new SQLiteConnection(Variables.DBROUTE);
            Conn.Open();
            SQLiteCommand sqlcmd = new SQLiteCommand(p_Sql, Conn);
            int intReturn = sqlcmd.ExecuteNonQuery();
            sqlcmd.Dispose();
            Conn.Dispose();
            return intReturn;
        }



        /// <summary>
        /// db에서 Select하기 
        /// </summary>
        /// <param name="p_Sql">보낼 SQL</param>
        /// <returns>Return DataTable</returns>
        public static DataTable GetDBTable(string p_Sql, string p_DbRoute)
        {
            SQLiteConnection Conn = new SQLiteConnection(p_DbRoute);
            DataTable dt = new DataTable();
            Conn.Open();
            SQLiteDataAdapter Ap = new SQLiteDataAdapter(p_Sql, Conn);
            Ap.Fill(dt);
            Ap.Dispose();
            Conn.Dispose();
            return dt;
        }

        /// <summary>
        /// db에서 Select하기 
        /// </summary>
        /// <param name="p_Sql">보낼 SQL</param>
        /// <returns>Return DataSet</returns>
        public static DataSet GetDBSet(string p_Sql, string p_DbRoute)
        {
            SQLiteConnection Conn = new SQLiteConnection(p_DbRoute);
            DataSet ds = new DataSet();
            Conn.Open();
            SQLiteDataAdapter Ap = new SQLiteDataAdapter(p_Sql, Conn);
            Ap.Fill(ds);
            Ap.Dispose();
            Conn.Dispose();
            return ds;
        }

        /// <summary>
        /// sql 실행하기
        /// </summary>
        /// <param name="p_Sql"></param>
        public static int ExecDB(string p_Sql, string p_DbRoute)
        {
            SQLiteConnection Conn = new SQLiteConnection(p_DbRoute);
            Conn.Open();
            SQLiteCommand sqlcmd = new SQLiteCommand(p_Sql, Conn);
            int intReturn = sqlcmd.ExecuteNonQuery();
            sqlcmd.Dispose();
            Conn.Dispose();
            return intReturn;
        }
        #endregion
    }


}
