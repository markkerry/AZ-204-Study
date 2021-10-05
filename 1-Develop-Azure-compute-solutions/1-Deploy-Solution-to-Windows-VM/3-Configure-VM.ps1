# Install and configure IIS
Install-WindowsFeature -Name web-server, web-app-dev, web-net-ext, web-net-ext45, web-appinit, web-asp, web-asp-net, web-asp-net45, web-isapi-ext, web-isapi-filter, web-includes, web-websockets, web-mgmt-service -IncludeManagementTools 

# Disable IE Enhanced Security Configuration
$AdminKey = "HKLM:\SOFTWARE\Microsoft\Active Setup\Installed Components\{A509B1A7-37EF-4b3f-8CFC-4F3A74704073}"
$UserKey = "HKLM:\SOFTWARE\Microsoft\Active Setup\Installed Components\{A509B1A8-37EF-4b3f-8CFC-4F3A74704073}"
Set-ItemProperty -Path $AdminKey -Name "IsInstalled" -Value 0
Set-ItemProperty -Path $UserKey -Name "IsInstalled" -Value 0

# Install Chocolatey
Set-ExecutionPolicy Bypass -Scope Process -Force; iex ((New-Object System.Net.WebClient).DownloadString('https://chocolatey.org/install.ps1'))

# install Microsoft Web Deployment Tool
choco install webdeploy -y