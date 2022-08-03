
# ----- Setup -----
$Url = "http://82.113.54.226:3722/TotalAgility/services/sdk/jobservice.svc/json/GetJobProperties2"
$Body = @{
    sessionId = "098C4A97B020294FB2E72AB1B9BC1E18"
    jobIdentity = @{Id = "71BE1A82CBA242E08D92760EDCC6A663"}
    filter = @{AssociatedJobs = $true}
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