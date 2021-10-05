#!/bin/bash

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

# Run the go-app container
sudo docker run -d --rm -p 3000:3000 --name go-app markkerry/go-app:v1