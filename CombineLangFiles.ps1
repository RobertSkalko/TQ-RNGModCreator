$folderPath = "D:\TQExtract\text"

$target = "ALL_TEXT_COMBINED.txt"

$targetPath = Join-Path -Path $folderPath -ChildPath $target

$files = Get-ChildItem -Path $folderPath -Filter *.txt | Where-Object { $_.Name -ne $target } | Sort-Object Name

foreach ($file in $files) {
    $content = Get-Content -Raw -Encoding Unicode -Path $file.FullName
    Add-Content -Encoding Unicode -Path $targetPath -Value $content
    Add-Content -Encoding Unicode -Path $targetPath -Value "`r`n"
}

Write-Host "All files added to $target"