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


    /* ��Ʈ��ũ ���� Static Ŭ���� */
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


        #region FTP ����

        /// <summary>
        /// ftp���� db��������
        /// </summary>
        /// <param name="p_address">ftp�ּ�</param>
        /// <param name="p_id">ftp���̵�</param>
        /// <param name="p_pw">ftp���</param>
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
                return "������ ������ ���� �ٿ� ���� : \r\n" + e.ToString();
            }
        }

        /// <summary>
        /// ���ε�
        /// </summary>
        /// <param name="p_address">ftp�ּ�</param>
        /// <param name="p_id">ftp���̵�</param>
        /// <param name="p_pw">ftp���</param>
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
              
                // �׳� ���ε��ϰų� �ٿ�ε� �� ���, ���ڵ� ������ ����ε� ������ �ȵ�
                // ���� BinaryReader�� �о��ְ�, ����� ��. 
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
                return "������ ���ε� ���� : \r\n" + e.ToString();
            }
        }

        /// <summary>
        /// FTP ������ ���� ��¥ ��������. 
        /// ó�� �ٿ�Sync�� ���߱� ����.      --> ������� ����
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
        /// FTP �����̸� ����
        /// </summary>
        /// <param name="p_address">�ּ�</param>
        /// <param name="p_id">���̵�</param>
        /// <param name="p_pw">�н�����</param>
        /// <param name="p_fileName">���������̸�</param>
        /// <param name="p_fileReName">���������̸�</param>
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
                if (e.ToString().Contains("������ ã�� �� ���ų�"))
                    return "";
                else
                    return "FTP �����̸� ���� ���� : \r\n" + e.ToString();
            }
        }

        /// <summary>
        /// FTP ���ϻ���
        /// </summary>
        /// <param name="p_address">�ּ�</param>
        /// <param name="p_id">���̵�</param>
        /// <param name="p_pw">�н�����</param>
        /// <param name="p_fileName">���������̸�</param>
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
                if (e.ToString().Contains("������ ã�� �� ���ų�"))
                    return "";
                else
                    return "FTP ���ϻ��� ���� : \r\n" + e.ToString();
            }
        }



        #endregion FTP ����_END


        #region ��ȣȭ
        /// <summary>
        /// ��ȣȭ
        /// </summary>
        /// <param name="textToDecrypt"></param>
        /// <param name="key">��ȣȭŰ</param>
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
        /// ��ȣȭ
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
        /// ���� �ؽ� ������ 
        /// </summary>
        /// <param name="path">������ ���</param>
        /// <returns>�ؽ� string��</returns>
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
        #endregion ��ȣȭ_End


        #region ini��Ʈ��

        /* .ini���� ���� �Լ�
         * string section : [section]
         * string key : ���� Ű(val�� key)
         * string val : Ű�� ��(key�� val)
         * filePath : �� ini ���ϰ��
         */
        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);

        /* .ini���� �д� �Լ�
         * string section : ������ ���� Ű�� �����ִ� �����̸�
         * string key : ������ ���� Ű �̸�
         * string def : Ű�� ���� ���� ��� �⺻��(default)
         * StringBuilder retVal : ������ ��
         * int size : ������ ���� ����
         * string filePath : �о�� ini ���ϰ��
         * 
         * return value : �о�鿩�� ������ ����
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
        
        /* ������뿹�� 
        private void btnLogin_Click(object sender, EventArgs e)
        {
            //ini ����
            WritePrivateProfileString("LOGIN", "ID", "test", "C:\\login.ini");
            WritePrivateProfileString("LOGIN", "PW", "1234", "C:\\login.ini");
            StringBuilder retId = new StringBuilder();
            StringBuilder retPw = new StringBuilder();

            //ini �б�
            GetPrivateProfileString("LOGIN", "ID", "(NONE)", retId, 32, "C:\\login.ini");
            GetPrivateProfileString("LOGIN", "PW", "(NONE)", retPw, 32, "C:\\login.ini");
          
            MessageBox.Show("ID :"+ retId.ToString()+",PW: "+ retPw.ToString());
        }
        */
        #endregion ini��Ʈ��_End

        #region db����
        /// <summary>
        /// db���� Select�ϱ� 
        /// </summary>
        /// <param name="p_Sql">���� SQL</param>
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
        /// db���� Select�ϱ� 
        /// </summary>
        /// <param name="p_Sql">���� SQL</param>
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
        /// sql �����ϱ�
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
        /// db���� Select�ϱ� 
        /// </summary>
        /// <param name="p_Sql">���� SQL</param>
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
        /// db���� Select�ϱ� 
        /// </summary>
        /// <param name="p_Sql">���� SQL</param>
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
        /// sql �����ϱ�
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
