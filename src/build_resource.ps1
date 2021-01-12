Write-Host -ForegroundColor Blue -BackgroundColor Green "Building server..."
$proc = Start-Process "dotnet" -ArgumentList "build --configuration Release" -WorkingDirectory "Minerva.Server" -PassThru -NoNewWindow
$proc.WaitForExit()
Write-Host -ForegroundColor Blue -BackgroundColor Green " finished!"

Write-Host -ForegroundColor Blue -BackgroundColor Green "Installing client dependencies..."
$proc = Start-Process "npm" -ArgumentList "ci" -WorkingDirectory "Minerva.Client" -PassThru -NoNewWindow
$proc.WaitForExit()
Write-Host -ForegroundColor Blue -BackgroundColor Green " finished!"

Write-Host -ForegroundColor Blue -BackgroundColor Green "Building client..."
$proc = Start-Process "npm" -ArgumentList "run release" -WorkingDirectory "Minerva.Client" -PassThru -NoNewWindow
$proc.WaitForExit()
Write-Host -ForegroundColor Blue -BackgroundColor Green " finished!"
