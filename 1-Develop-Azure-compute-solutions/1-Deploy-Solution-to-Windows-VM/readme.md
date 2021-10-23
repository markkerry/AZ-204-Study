# Deploy a .NET Solution to a Windows VM

## Create the VNet, VM and Other Relevant Resources

Create a resource group for the resources

```bash
az group create --name az204-rg --location uksouth
```

The following VNet will be used for other resources in the Develop Azure Compute Solutions section.

```bash
az network vnet create \
    --name az204-vnet1 \
    --resource-group az204-rg \
    --address-prefixes 10.1.0.0/16
```

Create a subnet and associate with the VNet

```bash
az network vnet subnet create \
    --name az204-subnet1 \
    --vnet-name az204-vnet1 \ 
    --resource-group az204-rg \
    --address-prefixes 10.1.1.0/24
```

Create a Public IP for the Windows VM (vm1)

```bash
az network public-ip create \
    --resource-group az204-rg \
    --name az204vm1-pip
```

Create an NSG

```bash
az network nsg create \
    --name az204-nsg \
    --resource-group az204-rg
```

Create three NSG rules. One for RDP, IIS Management Service, and web traffic.

```bash
az network nsg rule create \
    --resource-group az204-rg \
    --nsg-name az204-nsg \
    --name allowRDP \
    --protocol tcp \
    --priority 1000 \
    --destination-port-range 3389 \
    --access allow

az network nsg rule create \
    --resource-group az204-rg \
    --nsg-name az204-nsg \
    --name allowIISManagementService \
    --protocol tcp \
    --priority 1001 \
    --destination-port-range 8172 \
    --access allow

az network nsg rule create \
    --resource-group az204-rg \
    --nsg-name az204-nsg \
    --name allow80 \
    --protocol tcp \
    --priority 1002 \
    --destination-port-range 80 \
    --access allow
```

Create the Network Interface Card for vm1. Assign it to vnet1, subnet1, the NSG and the Public IP.

```bash
az network nic create \
    --resource-group az204-rg \
    --name az204vm1-nic \
    --vnet-name az204-vnet1 \
    --subnet az204-subnet1 \
    --network-security-group az204-nsg \
    --public-ip-address az204vm1-pip
```

Finally Create the VM

```bash
rgName="az204-rg"
vmName="az204vm1"
password=""

az vm create \
    --name $vmName\
    --resource-group $rgName \
    --location uksouth
    --image win2019Datacenter \
    --size Standard_D2s_v3 \
    --nics az204vm1-nic \
    --public-ip-address-dns-name az204vm1 \
    --os-disk-name az204vm1-osdisk \
    --admin-username azureuser \
    --admin-password $password
```

Now deploy a custom script extension to the VM, which will install IIS, disable the IE Enhanced Security Configuration, install Chocolatey, and finally install the Microsoft Web Deployment Tool

```bash
az vm extension set \
    --publisher Microsoft.Compute \
    --version 1.9 \
    --name CustomScriptExtension \
    --vm-name $vmName \
    --resource-group $rgName \
    --settings '{"fileUris":["https://raw.githubusercontent.com/markkerry/AZ-204-Study/main/1-Develop-Azure-compute-solutions/2-Deploy-Solution-to-Azure-VM/3-configure-vm.ps1"], "commandToExecute":"powershell.exe -ExecutionPolicy Unrestricted -File 3-configure-vm.ps1 > c:\powershell.txt"}'
```

The contents of the PowerShell script is below

```PowerShell
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
```

## Deploy the .NET Solution

Ensure the Web Management Service is running on the machine.

Publish the solution from Visual Studio by signing into your Azure Account, selecting Publish, and then select vm1
