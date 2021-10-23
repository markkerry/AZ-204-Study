# Create an Azure Container Registry

## Create the ACR

```bash
az acr create \
    -n az204mkacr \
    -g az204-rg \
    --location uksouth \
    --sku Standard \
    --admin-enabled true
```

From the Linux VM, deploy the container images created in 4-Containerise-DotNet-Solution to the ACR

```bash
curl -sL https://packages.microsoft.com/keys/microsoft.asc | \
    gpg --dearmor | \
    sudo tee /etc/apt/trusted.gpg.d/microsoft.asc.gpg > /dev/null

AZ_REPO=$(lsb_release -cs)
echo "deb [arch=amd64] https://packages.microsoft.com/repos/azure-cli/ $AZ_REPO main" | \
    sudo tee /etc/apt/sources.list.d/azure-cli.list

sudo apt-get update
sudo apt-get install azure-cli

sudo az login

sudo az acr login --name az204mkacr

sudo docker tag az204webapplication2 az204mkacr.azurecr.io/az204webapplication2

sudo docker push az204mkacr.azurecr.io/az204webapplication2
```