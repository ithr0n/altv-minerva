Write-Host -ForegroundColor Blue -BackgroundColor Green "Building server..."
$proc = Start-Process "dotnet" -ArgumentList "build --configuration Debug" -WorkingDirectory "PlayGermany.Server" -PassThru -NoNewWindow
$proc.WaitForExit()
Write-Host -ForegroundColor Blue -BackgroundColor Green " finished!"

Write-Host -ForegroundColor Blue -BackgroundColor Green "Installing client dependencies..."
$proc = Start-Process "npm" -ArgumentList "install" -WorkingDirectory "PlayGermany.Client" -PassThru -NoNewWindow
$proc.WaitForExit()
Write-Host -ForegroundColor Blue -BackgroundColor Green " finished!"

Write-Host -ForegroundColor Blue -BackgroundColor Green "Building client..."
$proc = Start-Process "npm" -ArgumentList "run release" -WorkingDirectory "PlayGermany.Client" -PassThru -NoNewWindow
$proc.WaitForExit()
Write-Host -ForegroundColor Blue -BackgroundColor Green " finished!"

Write-Host -ForegroundColor Blue -BackgroundColor Green "Installing client UI dependencies..."
$proc = Start-Process "npm" -ArgumentList "install" -WorkingDirectory "PlayGermany.Client/html" -PassThru -NoNewWindow
$proc.WaitForExit()
Write-Host -ForegroundColor Blue -BackgroundColor Green " finished!"

Write-Host -ForegroundColor Blue -BackgroundColor Green "Starting client UI (localhost)..."
$proc = Start-Process "npm" -ArgumentList "run serve" -WorkingDirectory "PlayGermany.Client/html"
#$proc.WaitForExit()
Write-Host -ForegroundColor Blue -BackgroundColor Green " finished!"

Write-Host -ForegroundColor Blue -BackgroundColor Green "Starting server..."
$proc = Start-Process "altv-server.exe" -WorkingDirectory "../dist"
#$proc.WaitForExit()
Write-Host -ForegroundColor Blue -BackgroundColor Green " finished!"