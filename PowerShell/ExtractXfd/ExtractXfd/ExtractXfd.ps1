
Write-Host "Start"
$rootFolderPath = Read-Host "Please provide folder path for .xfd extraction: "

$7zPath = $PSScriptRoot+"\7-ZipPortable\App\7-Zip\7z.exe"
Set-Alias 7zip $7zPath


#$ExtractedFolderName = "ExtractedFRP"

# Run in location of script
#foreach ($xfdFilePath in Get-Childitem -Path $PSScriptRoot -Include *.xfd -Recurse) {

# Run in provided path
foreach ($xfdFilePath in Get-Childitem -Path $rootFolderPath -Include *.xfd -Recurse) {

    Write-Host "xfdFilePath = "$xfdFilePath

    $xfdFolderPath = (Get-Item $xfdFilePath ).DirectoryName
    Write-Host "xfdFolderPath = "$xfdFolderPath

    $xfdFileName = (Get-Item $xfdFilePath ).Basename 
    Write-Host "xfdFileName = "$xfdFileName

    $extractFilePath = $xfdFolderPath+"\"+$xfdFileName
    Write-Host "extractFilePath = "$extractFilePath

    7zip e $xfdFilePath -o"$extractFilePath" -aoa
    


    # #$xfdFilePathWithoutExtension = join-path $xfdFilePath.DirectoryName  $xfdFilePath.BaseName
    # $xfdFolderPath = $xfdFilePath.DirectoryName
    # $xfdFileName = Split-Path $xfdFilePath -leaf
    # $xfdFileNameWithoutExtension = $xfdFilePath.BaseName
    # $extractFileFolderPath = $xfdFolderPath + "\"+$ExtractedFolderName
    # #$extractFilePath = $extractFileFolderPath+"\"+$xfdFileNameWithoutExtension
    # $finalFileName = $xfdFileNameWithoutExtension+".xml"
    # #Get-Childitem -Path C:\_GitHub\Tulip

    # #Write-Host $PSScriptRoot
    # Write-Host $xfdFilePath
    # #Write-Host $xfdFilePathWithoutExtension
    # Write-Host $xfdFileName
    # Write-Host $xfdFileNameWithoutExtension
    # #Write-Host $extractFilePath

    # #Rename-Item -

    # #Copy-Item -Path $xfdFilePath -Destination $PSScriptRoot"/ExtractedFRP/frp.zip"

    # #Expand-Archive -Path $PSScriptRoot"/ExtractedFRP/frp.zip" -DestinationPath $extractTargetPath"Extracted"

    # #Expand-Archive -Path $PSScriptRoot"/ExtractedFRP/frp.zip" -DestinationPath $extractTargetPath"Extracted"

    # New-Item -Path $xfdFolderPath -Name $ExtractedFolderName -ItemType "directory" -Force

    # $7zPath = "$env:ProgramFiles\7-Zip\7z.exe"

    # Set-Alias 7zip $7zPath

    # 7zip e $xfdFilePath -o"$extractFileFolderPath" -aoa

    # #Rename-Item -Path $xfdFilePathWithoutExtension -NewName $xfdFilePathWithoutExtension".xml"
    # #Rename-Item -Path $extractFileFolderPath"\"$xfdFileNameWithoutExtension -NewName $xfdFilePathWithoutExtension".xml"
    # #Get-Item $extractFilePath | Rename-Item -NewName $finalFileName -Force

    # Move-Item -Path $extractFilePath -Destination $extractFileFolderPath"\"$finalFileName -Force
    
    #Move-Item -Path $xfdFilePathWithoutExtension".xml" -Destination $extractFilePath -Force


    #7z e $PSScriptRoot"/ExtractedFRP/frp.zip"

}






Write-Host "End"