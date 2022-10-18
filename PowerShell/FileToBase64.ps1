

$filePath = "C:\Temp\samplefile.pdf";

$documentBase64 = [convert]::ToBase64String((Get-Content -path $filePath -Encoding byte));

$resultPath = (Get-Item $filePath ).DirectoryName + "\" + (Get-Item $filePath ).Basename + ".base64"

Write-Host "Result file path is: " $resultPath

New-Item -Path $resultPath -ItemType File -Value $documentBase64 -Force