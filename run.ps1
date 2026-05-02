$api = Start-Process powershell -ArgumentList "-NoExit", "-Command", "dotnet watch run --project src/GoodHamburger.Api" -PassThru
$web = Start-Process powershell -ArgumentList "-NoExit", "-Command", "dotnet watch run --project src/GoodHamburger.Web" -PassThru

Write-Host "API e Blazor rodando. Pressione CTRL+C para parar."
Wait-Process -Id $api.Id, $web.Id