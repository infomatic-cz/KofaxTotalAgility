
# ----- Setup -----
$Url = "https://localhost/TotalAgility/services/sdk/capturedocumentservice.svc/json/GetFolder"
$Body = @{
    sessionId = "7DAD08C4DDA4584B8E94A4D8B75ED872"
    folderId = "7dae6e61-947f-483b-a2c1-aee10076b6e0"
}

# Setup WS call and output
$useCurrentUserCredentials = $true  # current user or popup to provide credentials
$printToConsole = $true # Write result json to console. Good for small objects
$printToFile = $true    # Write result json to file. Good for big objects
$printToFilePath = "C:\temp\result.json"


# ----- Execution -----
$JsonBody = $Body | ConvertTo-Json
if ($useCurrentUserCredentials) {
    # Use credentials of current user
    $result = Invoke-RestMethod -ContentType application/json -Uri $Url -Method Post -Body $JsonBody -UseDefaultCredentials
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