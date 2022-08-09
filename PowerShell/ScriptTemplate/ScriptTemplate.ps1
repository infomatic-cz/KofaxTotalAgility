# ----- Notes -----
# Template to call KTA API form PowerShell
# KTA 7.10 API Documentation https://docshield.kofax.com/KTA/en_US/7.10.0-vmhad0mru4/help/SDK_Documentation/latest/index.html
# Boolean is writen as $true and $false
# Line breaks in json might be replaced by \r\n. To make them line breaks again press Ctrl+h to open replace menu,
#   type \r\n to first row, in second row press Ctrl+Enter (it will wrine line break) and hit Replace All
# Most usual methods works with Post but some require Get

# ----- Setup -----
# Url to KTA API, set protocol http/https, hostname/ip:port, class in sdk/xxx.svc and method name at the end
$Url = "http://82.113.54.226:3722/TotalAgility/services/sdk/jobservice.svc/json/GetJobVariableValue"    

# Request object as JSON based on documentation, @{} is object, don't forget to update session id
$Body = @{
    sessionId = "098C4A97B020294FB2E72AB1B9BC1E18"
    jobIdentity = @{ Id = "A9CA91CE4C6D11ECA4B700155D280A0F" }
    variableIdentity = @{ Id = "SPZ" }
}

# Credentials and output
$method = "Post"
$useCurrentUserCredentials = $true          # True = use current user, False = create popup to provide credentials
$printToConsole = $true                     # Write result json to console, good for small objects
$printToFile = $true                        # Write result json to file, good for big objects
$printToFilePath = "C:\temp\result.json"    # Path to output file


# ----- Execution -----

# Ignore selfsigned certificates
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

$JsonBody = $Body | ConvertTo-Json

# Try invoke method
try {
    if ($useCurrentUserCredentials) {
        
            

        # Use credentials of current user
        $result = Invoke-RestMethod -ContentType application/json -Uri $Url -Method $method -Body $JsonBody -UseDefaultCredentials
        
    }
    else {
        # Request credentials to use in call
        $Cred = Get-Credential
        $result = Invoke-RestMethod -ContentType application/json -Uri $Url -Method Post -Body $JsonBody -Credential $Cred
    }

    $resultJson = ConvertTo-Json $result

    if ($printToConsole) {
        Write-Host $resultJson
    }
    if ($printToFile) {
        Out-File -FilePath $printToFilePath -InputObject $resultJson -Encoding utf8
    }
} catch {
    # Dig into the exception to get the Response details.
    # Note that value__ is not a typo.
    Write-Host "StatusCode:" $_.Exception.Response.StatusCode.value__ 
    Write-Host "StatusDescription:" $_.Exception.Response.StatusDescription
    Write-Host "Message:" $_.Exception.Message
    Write-Host "InnerException:" $_.Exception.InnerException
    
}