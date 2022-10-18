# ----- Notes -----
# Template to call KTA API form PowerShell
# KTA 7.10 API Documentation https://docshield.kofax.com/KTA/en_US/7.10.0-vmhad0mru4/help/SDK_Documentation/latest/index.html
# Boolean is writen as $true and $false
# Line breaks in json might be replaced by \r\n. To make them line breaks again press Ctrl+h to open replace menu,
#   type \r\n to first row, in second row press Ctrl+Enter (it will wrine line break) and hit Replace All
# Most usual methods works with Post but some require Get

# ----- Setup -----
# Url to KTA API, set protocol http/https, hostname/ip:port, class in sdk/xxx.svc and method name at the end
$UrlBase = "https://winsrv-kta75/TotalAgility/services/sdk/capturedocumentservice.svc/json/"    
$method = "post"
$sessionId = "8C353BDB05C2E448A6B05DF78482FFD7"
$folderPath = "C:\KMC_Custom\Demo\Export\0000008E"
# Request object as JSON based on documentation, @{} is object, don't forget to update session id
$Body = @{
    sessionId = $sessionId
    insertIndex = -1
}

# https://stackoverflow.com/questions/36456104/invoke-restmethod-ignore-self-signed-certs
if (-not("dummy" -as [type])) {
    add-type -TypeDefinition @"
using System;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

public static class Dummy {
    public static bool ReturnTrue(object sender,
        X509Certificate certificate,
        X509Chain chain,
        SslPolicyErrors sslPolicyErrors) { return true; }

    public static RemoteCertificateValidationCallback GetDelegate() {
        return new RemoteCertificateValidationCallback(Dummy.ReturnTrue);
    }
}
"@
}
[System.Net.ServicePointManager]::ServerCertificateValidationCallback = [dummy]::GetDelegate()


$url = $UrlBase+"CreateFolder"
$JsonBody = $Body | ConvertTo-Json
$result = Invoke-RestMethod -ContentType application/json -Uri $Url -Method $method -Body $JsonBody -UseDefaultCredentials 

$resultJson = ConvertTo-Json $result
Write-Host $resultJson
Write-Host $result.d



Get-ChildItem $folderPath | 
Foreach-Object {
    Write-Host $_.FullName

        
    $Body = @{
        sessionId = $sessionId
        parentId = $result.d
        documentDataInput = @{ FilePath = $_.FullName }
        insertIndex = 0
    }
    $url = $UrlBase+"CreateDocument3"
    $JsonBody = $Body | ConvertTo-Json
    try {
        $result = Invoke-RestMethod -ContentType application/json -Uri $Url -Method $method -Body $JsonBody -UseDefaultCredentials
    } catch {
        # Dig into the exception to get the Response details.
        # Note that value__ is not a typo.
        Write-Host "StatusCode:" $_.Exception.Response.StatusCode.value__ 
        Write-Host "StatusDescription:" $_.Exception.Response.StatusDescription
        Write-Host "Message:" $_.Exception.Message
        Write-Host "InnerException:" $_.Exception.InnerException
        
    }
    


}








# # Credentials and output
# $method = "Post"
# $useCurrentUserCredentials = $true          # True = use current user, False = create popup to provide credentials
# $printToConsole = $true                     # Write result json to console, good for small objects
# $printToFile = $true                        # Write result json to file, good for big objects
# $printToFilePath = "C:\temp\result.json"    # Path to output file


# # ----- Execution -----
# $JsonBody = $Body | ConvertTo-Json
# if ($useCurrentUserCredentials) {
#     # Use credentials of current user
#     $result = Invoke-RestMethod -ContentType application/json -Uri $Url -Method $method -Body $JsonBody -UseDefaultCredentials
# }
# else {
#     # Request credentials to use in call
#     $Cred = Get-Credential
#     $result = Invoke-RestMethod -ContentType application/json -Uri $Url -Method Post -Body $JsonBody -Credential $Cred
# }

# $resultJson = ConvertTo-Json $result

# if ($printToConsole) {
#     Write-Host $resultJson
# }
# if ($printToFile) {
#     Out-File -FilePath $printToFilePath -InputObject $resultJson -Encoding utf8
# }