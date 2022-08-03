# ----- Notes -----
# Template to call KTA API form PowerShell
# KTA 7.10 API Documentation https://docshield.kofax.com/KTA/en_US/7.10.0-vmhad0mru4/help/SDK_Documentation/latest/index.html
# Boolean is writen as $true and $false, null -> $null


# ----- Setup -----
# Url to KTA API, set protocol http/https, hostname/ip:port, class in sdk/xxx.svc and method name at the end
$Url = "http://82.113.54.226:3722/TotalAgility/services/sdk/packageservice.svc/json/ExportPackageToBytes2"    

# Request object as JSON based on documentation, @{} is object, don't forget to update session id
$Body = @{
    sessionId = "098C4A97B020294FB2E72AB1B9BC1E18"
    packageIdentity = @{ Name = "BR_LPra" }
    #deploymentIdentity = $null
}

# Credentials and output
$method = "Get"
$useCurrentUserCredentials = $true          # True = use current user, False = create popup to provide credentials
$printToConsole = $false                     # Write result json to console, good for small objects
$printToFile = $false                        # Write result json to file, good for big objects
$printToFilePath = "C:\temp\result.json"    # Path to output file


# ----- Execution -----
$JsonBody = $Body | ConvertTo-Json
$result = Invoke-RestMethod -ContentType application/json -Uri $Url -Method $method -Body $JsonBody -UseDefaultCredentials

# $decoded = [System.Convert]::FromBase64CharArray($e, 0, $e.Length)
Set-Content -Path C:\temp\testpackage.zip -value (([char[]]$result.d) -join "")


# if ($useCurrentUserCredentials) {
#     # Use credentials of current user
#     $result = Invoke-RestMethod -ContentType application/json -Uri $Url -Method Post -Body $JsonBody -UseDefaultCredentials
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