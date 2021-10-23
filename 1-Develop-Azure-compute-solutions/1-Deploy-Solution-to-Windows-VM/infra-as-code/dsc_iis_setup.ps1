configuration iis_setup {

    Import-DscResource -ModuleName PSDesiredStateConfiguration

    node ("localhost")
    {
        foreach ($feature in @("Web-Server","Web-Common-Http","Web-Static-Content",`
        "Web-Default-Doc","Web-Dir-Browsing","Web-Http-Errors",`
        "Web-Health","Web-Http-Logging","Web-Log-Libraries",`
        "Web-Request-Monitor","Web-Security","Web-Filtering",`
        "Web-Stat-Compression","Web-Http-Redirect","Web-Mgmt-Tools",`
        "WAS","WAS-Process-Model","WAS-NET-Environment","WAS-Config-APIs","Web-CGI"))
        {
            WindowsFeature $feature
            {
                Name = $feature
                Ensure = "Present"
            }
            DependsOn = "[Script]InstallWebDeploy"
        }

        Registry IEEnhancedSecurityAdmin
        {
            Ensure = "Present"
            Key = "HKLM:\SOFTWARE\Microsoft\Active Setup\Installed Components\{A509B1A7-37EF-4b3f-8CFC-4F3A74704073}"
            ValueName = "IsInstalled"
            ValueData = "0"
            ValueType = "Dword"
        }

        Registry IEEnhancedSecurityUser
        {
            Ensure = "Present"
            Key = "HKLM:\SOFTWARE\Microsoft\Active Setup\Installed Components\{A509B1A8-37EF-4b3f-8CFC-4F3A74704073}"
            ValueName = "IsInstalled"
            ValueData = "0"
            ValueType = "Dword"
        }

        Script InstallChoco
        {
            TestScript = { # the TestScript block runs first. If the TestScript block returns $false, the SetScript block will run
                if (Test-Path "C:\ProgramData\chocolatey\choco.exe" -ErrorAction SilentlyContinue) {return $True}
                else {return $False}
            }
            SetScript = {
                Set-ExecutionPolicy Bypass -Scope Process -Force; iex ((New-Object System.Net.WebClient).DownloadString('https://chocolatey.org/install.ps1'))
            }
            GetScript = { # should return a hashtable representing the state of the current node
                $result = Test-Path -Path "C:\ProgramData\chocolatey\choco.exe"
                @{
                    "Installed" = $result
                }
            }
        }

        Script InstallWebDeploy
        {
            TestScript = { # the TestScript block runs first. If the TestScript block returns $false, the SetScript block will run
                if (Test-Path "C:\Program Files\IIS\Microsoft Web Deploy V3\msdeploy.exe" -ErrorAction SilentlyContinue) {return $True}
                else {return $False}
            }
            SetScript = {
                choco install webdeploy -y
            }
            GetScript = { # should return a hashtable representing the state of the current node
                $result = Test-Path -Path "C:\Program Files\IIS\Microsoft Web Deploy V3\msdeploy.exe"
                @{
                    "Installed" = $result
                }
            }
            DependsOn = "[Script]InstallChoco"
        }

        Service WebMgmt
        {
            Name = "WMSVC"
            StartupType = "Automatic"
            State = "Running"
            DependsOn = "[WindowsFeature]Web-Mgmt-Tools"
        }
    }
}