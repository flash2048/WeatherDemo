dotnet dev-certs https --clean
New-SelfSignedCertificate -certstorelocation cert:\CurrentUser\my -dnsname @("localhost")  -TextExtension @("1.3.6.1.4.1.311.84.1.1=2") -Subject "localhost" -FriendlyName "ASP.NET Core HTTPS development certificate"
dotnet dev-certs https --trust