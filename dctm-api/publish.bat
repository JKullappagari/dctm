cd "C:\Users\babujag\OneDrive - Hewlett Packard Enterprise\J\DCTM\DCTMRestAPI\Publish"

del -f *.*

dotnet publish -c Release "C:\Users\babujag\OneDrive - Hewlett Packard Enterprise\J\DCTM\DCTMRestAPI\DCTMRestAPI\DCTMRestAPI.csproj" /p:CopyOutputSymbolsToPublishDirectory=false -o "C:\Users\babujag\OneDrive - Hewlett Packard Enterprise\J\DCTM\DCTMRestAPI\Publish"

dotnet publish -c Release "C:\Users\babujag\OneDrive - Hewlett Packard Enterprise\J\DCTM\DCTMRestAPI\EncryptDecrypt\EncryptDecrypt.csproj" /p:CopyOutputSymbolsToPublishDirectory=false -o "C:\Users\babujag\OneDrive - Hewlett Packard Enterprise\J\DCTM\DCTMRestAPI\Publish"