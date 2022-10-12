
## Get parts of path
https://stackoverflow.com/questions/12503871/removing-path-and-extension-from-filename-in-powershell
                                           ## Output:
$PSCommandPath                             ## C:\Users\user\Documents\code\ps\test.ps1
(Get-Item $PSCommandPath ).Extension       ## .ps1
(Get-Item $PSCommandPath ).Basename        ## test
(Get-Item $PSCommandPath ).Name            ## test.ps1
(Get-Item $PSCommandPath ).DirectoryName   ## C:\Users\user\Documents\code\ps
(Get-Item $PSCommandPath ).FullName        ## C:\Users\user\Documents\code\ps\test.ps1

$ConfigINI = (Get-Item $PSCommandPath ).DirectoryName+"\"+(Get-Item $PSCommandPath ).BaseName+".ini"

$ConfigINI                                 ## C:\Users\user\Documents\code\ps\test.ini

## Call .Net method
https://stackoverflow.com/questions/27768303/how-to-unzip-a-file-in-powershell

Add-Type -AssemblyName System.IO.Compression.FileSystem
function Unzip
{
    param([string]$zipfile, [string]$outpath)

    [System.IO.Compression.ZipFile]::ExtractToDirectory($zipfile, $outpath)
}

Unzip "C:\a.zip" "C:\a"

