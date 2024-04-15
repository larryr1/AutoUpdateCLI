Write-Host "AutoUpdate CLI Bootstrapper"
Write-Host "Download or contribute: https://github.com/larryr1/AutoUpdateCLI/"

$repo = "larryr1/AutoUpdateCLI"
$file = "AutoUpdateCLI-alpha-1.exe"

$releases = "https://api.github.com/repos/larryr1/AutoUpdateCLI/releases"

Write-Host Finding latest release...
$tag = (Invoke-WebRequest $releases | ConvertFrom-Json)[0].tag_name

$download = "https://github.com/$repo/releases/download/$tag/$file"
$name = $file.Split(".")[0]
$file = "$name.exe"
$dir = "$name-$tag"

Write-Host Dowloading latest release...
Invoke-WebRequest $download -Out $file

Write-Host Running release...
Start-Process .\$file

