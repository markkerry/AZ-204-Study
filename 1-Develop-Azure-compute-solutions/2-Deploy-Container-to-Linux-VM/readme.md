# Deploy a Container to a Linux VM

## Create the Linux VM and Relevant Resources

Using the same Resource Group, VNet, Subnet and NSG as before, create a new Public IP

```bash
az network public-ip create \
    --resource-group az204-rg \
    --name az204vm2-pip
```

Create a Network Interface Card for vm2

```bash
az network nic create \
    --resource-group az204-rg \
    --name az204vm2-nic \
    --vnet-name az204-vnet1 \
    --subnet az204-subnet1 \
    --network-security-group az204-nsg \
    --public-ip-address az204vm2-pip
```

Create two NSG rules. One allowing SSH, the other allowing port 3000 for the container

```bash
az network nsg rule create \
    --resource-group az204-rg \
    --nsg-name az204-nsg \
    --name allowRDP \
    --protocol tcp \
    --priority 1003 \
    --destination-port-range 22 \
    --access allow

az network nsg rule create \
    --resource-group az204-rg \
    --nsg-name az204-nsg \
    --name allowPort3000 \
    --protocol tcp \
    --priority 1004 \
    --destination-port-range 3000 \
    --access allow
```

Create the Ubuntu VM (vm2)

```bash
rgName="az204-rg"
vmName="az204vm2"
password=""

az vm create \
    --name $vmName\
    --resource-group $rgName \
    --location uksouth
    --image UbuntuLTS \
    --size Standard_D2s_v3 \
    --nics az204vm2-nic \
    --public-ip-address-dns-name az204vm2 \
    --os-disk-name az204vm2-osdisk \
    --admin-username azureuser \
    --admin-password $password
```

## Install Docker

```bash
# Update the package index
sudo apt-get update

# Install packages to allow apt to use the repository over HTTPS
sudo apt-get install \
    apt-transport-https \
    ca-certificates \
    curl \
    gnupg-agent \
    software-properties-common


# Add Docker's official GPG key
curl -fsSL https://download.docker.com/linux/ubuntu/gpg | sudo apt-key add -

# Setup a stable repository
sudo add-apt-repository \
   "deb [arch=amd64] https://download.docker.com/linux/ubuntu \
   $(lsb_release -cs) \
   stable"

# Update the package index
sudo apt-get update

# Install docker, containerd
sudo apt-get install docker-ce docker-ce-cli containerd.io
```

Run the go-app container from ` markkerry/go-app`. The container exposes local port 3000 to remote port 3000

```bash
# Run the go-app container
sudo docker run -d --rm -p 3000:3000 --name go-app markkerry/go-app:v1
```

Test the container 

```http
http://publicip:3000
```
