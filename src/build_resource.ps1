Write-Host -ForegroundColor Blue -BackgroundColor Green "Building server..."
$proc = Start-Process "dotnet" -ArgumentList "build --configuration Release" -WorkingDirectory "PlayGermany.Server" -PassThru -NoNewWindow
$proc.WaitForExit()
Write-Host -ForegroundColor Blue -BackgroundColor Green " finished!"

Write-Host -ForegroundColor Blue -BackgroundColor Green "Installing client dependencies..."
$proc = Start-Process "npm" -ArgumentList "ci" -WorkingDirectory "PlayGermany.Client" -PassThru -NoNewWindow
$proc.WaitForExit()
Write-Host -ForegroundColor Blue -BackgroundColor Green " finished!"

Write-Host -ForegroundColor Blue -BackgroundColor Green "Building client..."
$proc = Start-Process "npm" -ArgumentList "run release" -WorkingDirectory "PlayGermany.Client" -PassThru -NoNewWindow
$proc.WaitForExit()
Write-Host -ForegroundColor Blue -BackgroundColor Green " finished!"
