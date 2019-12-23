using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

class ConsoleRsaCryp
{

    static void Main()
    {
        try
        {
            string oldData = "taiyonghai";
            CreateRSAKey();
            string ciphertext = RSAEncrypt(oldData);
            string newData = RSADecrypt(ciphertext);
        }
        catch (ArgumentNullException)
        {
            //Catch this exception in case the encryption did
            //not succeed.
            Console.WriteLine("Encryption failed.");

        }

        Console.ReadKey();
    }

    /// <summary>
    /// 建立RSA公鑰私鑰
    /// </summary>
    public static void CreateRSAKey()
    {
        //設定[公鑰私鑰]檔案路徑
        string privateKeyPath = @"d:\\PrivateKey.xml";
        string publicKeyPath = @"d:\\PublicKey.xml";

        //建立RSA物件
        RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
       
        //生成RSA[公鑰私鑰]
        string privateKey = rsa.ToXmlString(true);
        string publicKey = rsa.ToXmlString(false);
        
        //將金鑰寫入指定路徑
        File.WriteAllText(privateKeyPath, privateKey);//檔案內包含公鑰和私鑰
        File.WriteAllText(publicKeyPath, publicKey);//檔案內只包含公鑰
    }

    /// <summary>
    /// 使用RSA實現加密
    /// </summary>
    /// <param name="data">加密資料</param>
    /// <returns></returns>
    public static string RSAEncrypt(string data)
    {
        //C#預設只能使用[公鑰]進行加密(想使用[公鑰解密]可使用第三方元件BouncyCastle來實現)
        string publicKeyPath = @"d:\\PublicKey.xml";
        string publicKey = File.ReadAllText(publicKeyPath);

        //建立RSA物件並載入[公鑰]
        RSACryptoServiceProvider rsaPublic = new RSACryptoServiceProvider();
        rsaPublic.FromXmlString(publicKey);

        //對資料進行加密
        byte[] publicValue = rsaPublic.Encrypt(Encoding.UTF8.GetBytes(data), false);
        string publicStr = Convert.ToBase64String(publicValue);//使用Base64將byte轉換為string
        return publicStr;
    }

    /// <summary>
    /// 使用RSA實現解密
    /// </summary>
    /// <param name="data">解密資料</param>
    /// <returns></returns>
    public static string RSADecrypt(string data)
    {
        //C#預設只能使用[私鑰]進行解密(想使用[私鑰加密]可使用第三方元件BouncyCastle來實現)
        string privateKeyPath = @"d:\\PrivateKey.xml";
        string privateKey = File.ReadAllText(privateKeyPath);

        //建立RSA物件並載入[私鑰]
        RSACryptoServiceProvider rsaPrivate = new RSACryptoServiceProvider();
        rsaPrivate.FromXmlString(privateKey);

        //對資料進行解密
        byte[] privateValue = rsaPrivate.Decrypt(Convert.FromBase64String(data), false);//使用Base64將string轉換為byte
        string privateStr = Encoding.UTF8.GetString(privateValue);
        return privateStr;
    }
}