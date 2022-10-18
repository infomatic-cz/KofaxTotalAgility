

$base64FilePath = "C:\Temp\samplefile.base64";

$finalFilePath = "C:\Temp\samplefile.pdf";

$base64string = Get-Content -Path $base64FilePath -Raw

[IO.File]::WriteAllBytes($finalFilePath, [Convert]::FromBase64String($base64string))








