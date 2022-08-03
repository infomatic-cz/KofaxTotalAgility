
# ----- Setup -----
$Url = "http://82.113.54.226:3722//TotalAgility/services/sdk/jobservice.svc/json/GetJobVariableValue"
$Body = @{
    sessionId = "098C4A97B020294FB2E72AB1B9BC1E18"
    jobIdentity = @{ Id = "2D5B20CF124B11EDA4BB00155D280A0F" }
    variableIdentity = @{ Id = "ProcessingLog" }
}

# Setup WS call and output
$useCurrentUserCredentials = $true  # current user or popup to provide credentials
$printToConsole = $true # Write result json to console. Good for small objects
$printToFile = $false    # Write result json to file. Good for big objects
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