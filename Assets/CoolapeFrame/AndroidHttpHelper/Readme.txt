http://luz.because-why-not.com/unity-android-and-ssl-sslhandshakeexceptioncertpathvalidatorexception/

================================================================================================
Disclaimer: This post won’t make any sense to you if you don’t understand what Unity, Android or SSL is 😉

Most of the time I enjoy working with Unity as most tasks on it are fast and easy. But if something doesn’t work immediately then it subsequently gets messy and involves a lot of own work.

My current example is the WWW class. It is used to receive data from web servers using HTTP or HTTPS. I am using HTTPS to make sure that the data transfer is encrypted and people can’t cheat (as easily).  This worked fine until I created an Android version.  My app reported it can’t connect to the server. The reason was:


javax.net.ssl.SSLHandshakeException: java.security.cert.CertPathValidatorException: Trust anchor for certification path not found.
1
javax.net.ssl.SSLHandshakeException: java.security.cert.CertPathValidatorException: Trust anchor for certification path not found.
The exception means that Android rejected my server’s SSL certificate as I signed it myself instead of buying one from a big trusted certificate authority.

And here is where Unity gets annoying. Instead of ensuring that the WWW class behaves the same on all platforms they simply go with the default behavior of each platform without documenting anything. While my application works fine on other platforms it can’t work on Android as the WWW class uses the Android class “HTTPSUrlConnection” which enforces by default that every certificate has to be from a trustworthy CA. Fortunately,  there is a way to solve this problem.

 

The WWW class uses the default SSLContext/TrustManager to check if the certificate can be trusted. Android allows you to change the default system thus you can change the behaviour of Unitys class by changing the underlying HTTPSUrlConnection.

The following method replaces the default TrustManager  with a new one that will accept the given certificate. It is written in java and has to be included in unity as an Android Plugin.


    public static void trust(byte[] crtFileContent)
    {
        try
        {
            // Load CAs from an InputStream
            CertificateFactory cf = CertificateFactory.getInstance("X.509");

            InputStream caInput = new BufferedInputStream(new ByteArrayInputStream(crtFileContent));
            Certificate ca;
            try {
                ca = cf.generateCertificate(caInput);
                Log.d("JavaSSLHelper", "ca=" + ((X509Certificate) ca).getSubjectDN());
                Log.d("JavaSSLHelper", "Certificate successfully created");
            } finally
            {
                caInput.close();
            }

            // Create a KeyStore containing our trusted CAs
            String keyStoreType = KeyStore.getDefaultType();
            KeyStore keyStore = KeyStore.getInstance(keyStoreType);
            keyStore.load(null, null);
            keyStore.setCertificateEntry("ca", ca);

            // Create a TrustManager that trusts the CAs in our KeyStore
            String tmfAlgorithm = TrustManagerFactory.getDefaultAlgorithm();
            TrustManagerFactory tmf = TrustManagerFactory.getInstance(tmfAlgorithm);
            tmf.init(keyStore);

            try
            {
                // Create an SSLContext that uses our TrustManager
                SSLContext context = SSLContext.getInstance("TLS");
                context.init(null, tmf.getTrustManagers(), null);

                //this is important: unity will use the default ssl socket factory we just created
                HttpsURLConnection.setDefaultSSLSocketFactory(context.getSocketFactory());
                Log.d("JavaSSLHelper", "Default SSL Socket set.");
            } catch (NoSuchAlgorithmException e) {
                throw new RuntimeException(e);
            } catch (KeyManagementException e) {
                throw new RuntimeException(e);
            }
        }catch(Exception e)
        {
            throw new RuntimeException(e);
        }
    }
}

Now, it can be used directly in C# with the following lines:


    string cert = @"-----BEGIN CERTIFICATE-----
    MIICIzCCAYwCCQDMITtroXuUfzANBgkqhkiG9w0BAQUFADBWMQswCQYDVQQGEwJO
    WjESMBAGA1UECAwJU291dGhsYW5kMQ0wCwYDVQQHDARHb3JlMREwDwYDVQQKDAg0
    c2NpZW5jZTERMA8GA1UEAwwINHNjaWVuY2UwHhcNMTUwMjIyMDczMzI1WhcNMTYw
    MjIyMDczMzI1WjBWMQswCQYDVQQGEwJOWjESMBAGA1UECAwJU291dGhsYW5kMQ0w
    CwYDVQQHDARHb3JlMREwDwYDVQQKDAg0c2NpZW5jZTERMA8GA1UEAwwINHNjaWVu
    Y2UwgZ8wDQYJKoZIhvcNAQEBBQADgY0AMIGJAoGBAO5VE64yagrFbU2PTL6b9aQD
    vGJc5Ks5tm4Ipkm1+MPhjUv2h/RyoihrkkLnhm85KkNZbNSEib98XWb6iooJaTj3
    LgBvr071nhgq1s0q/PYClTFwOvgbh40gvE8PsVLum/vRj0t+lq+jI4nlMER9qkCw
    0Hy6DyTaL2DVXZTkAL5tAgMBAAEwDQYJKoZIhvcNAQEFBQADgYEAL4KfbLqKh62X
    3QLEeBtvzhqPQJ6gmJSIi4wxzk582UmnXI5iwLCCIvM62MiBfk6wlD5yqkD3VWwz
    fzhyyspGWZHjJn/AkCj4/7f28qsMtx1OId+hcomd3QGxBDd6jXa5Wq4gjun5yb/A
    qbKmnda0rnh9P7lWbcLwwhqIFUQUuis=
    -----END CERTIFICATE-----
    ";
 
    AndroidJavaClass clsJavaSSLHelper = new AndroidJavaClass("co.fourscience.ulib.JavaSSLHelper");
    byte[] certBytes = System.Text.Encoding.ASCII.GetBytes(cert);
    clsJavaSSLHelper.CallStatic("trust", certBytes);
The variable “cert” is simply the content of the “crt” file of the certificate you use.  The last line calls the static java method shown above. After this call all calls of WWW will use the new TrustManager and thus accept the given certificate.

 

Edit:
You can download the finished Android plugin here. It is a jar file containing the JavaSSLHelper class. Simply put the file into your Asset/Plugins/Android folder and use the C# code above with your own certificate to allow android to access it. (use the file at your own risk! )

Edit2: I created a proper plugin based on this solution. It will allow you to declare your own certificate as trustworthy without removing the default Android behaviour involving other HTTPS pages. (The old version allowed ONLY the given certificate and blocked all other pages)

You can download the newest version here.

Update 25-01-2016: Changed the example to my new url + new certificate

Source: github unity-android-ssl

BTW: Since December 2015 Mozilla’s project “lets encrypt ” is in open beta. There you can get real trusted certificates for free.  At least my current Android version recognizes the certificate as trustworthy! So you don’t need the plugin anymore.

More at https://letsencrypt.org.

