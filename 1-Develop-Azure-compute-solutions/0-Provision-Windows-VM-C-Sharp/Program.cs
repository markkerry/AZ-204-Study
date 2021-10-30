using System;
using Microsoft.Azure.Management.Compute.Fluent;
using Microsoft.Azure.Management.Compute.Fluent.Models;
using Microsoft.Azure.Management.Network.Fluent;
using Microsoft.Azure.Management.Fluent;
using Microsoft.Azure.Management.ResourceManager.Fluent;
using Microsoft.Azure.Management.ResourceManager.Fluent.Core;
using Microsoft.Azure.Management.Network.Fluent.Models;

namespace provisionVM
{
    class Program
    {
        static void Main(string[] args)
        {
            // Create the management client. This will be used for all the operations
            // that we will perform in Azure
            var credentials = SdkContext.AzureCredentialsFactory.FromFile("./azureauth.properties");

            var azure = Azure.Configure()
                .WithLogLevel(HttpLoggingDelegatingHandler.Level.Basic)
                .Authenticate(credentials)
                .WithDefaultSubscription();
            
            // Set the variables
            var groupName = "az204-rg";
            var vmName = "az204vm1";
            var location = Region.UKSouth;
            var vNetName = "az204-vnet1";
            var vNetAddress = "10.1.0.0/16";
            var subnetName = "az204-subnet1";
            var subnetAddress = "10.1.1.0/24";
            var nicName = "az204vm1-nic";
            var adminUser = "azureuser";
            var adminPassword = "";
            var publicIPName = "az204vm1-pip";
            var nsgName = "az204-nsg";

            // Create the resource group
            Console.WriteLine($"Creating resource group {groupName} ...");
            var resourceGroup = azure.ResourceGroups.Define(groupName)
                .WithRegion(location)
                .Create();
            
            // Create the vNet
            Console.WriteLine($"Creating virtual network {vNetName} ...");
            var network = azure.Networks.Define(vNetName)
                .WithRegion(location)
                .WithExistingResourceGroup(groupName)
                .WithAddressSpace(vNetAddress)
                .WithSubnet(subnetName, subnetAddress)
                .Create();
            
            // Create the publicIP
            Console.WriteLine($"Creating public IP {publicIPName} ...");
            var publicIP = azure.PublicIPAddresses.Define(publicIPName)
                .WithRegion(location)
                .WithExistingResourceGroup(groupName)
                .Create();
            
            // Create the nsg
            Console.WriteLine($"Creating Network Security Group {nsgName} ...");
            var nsg = azure.NetworkSecurityGroups.Define(nsgName)
                .WithRegion(location)
                .WithExistingResourceGroup(groupName)
                .Create();

            // Create nsg rule to allow rdp
            Console.WriteLine($"Creating security rule to allow RDP access ...");
            nsg.Update()
                .DefineRule("Allow-RDP")
                    .AllowInbound()
                    .FromAnyAddress()
                    .FromAnyPort()
                    .ToAnyAddress()
                    .ToPort(3389)
                    .WithProtocol(SecurityRuleProtocol.Tcp)
                    .WithPriority(100)
                    .WithDescription("Allow-RDP")
                    .Attach()
                .Apply();

            // Create the nic
            Console.WriteLine($"Creating network interface {nicName} ...");
            var nic = azure.NetworkInterfaces.Define(nicName)
                .WithRegion(location)
                .WithExistingResourceGroup(groupName)
                .WithExistingPrimaryNetwork(network)
                .WithSubnet(subnetName)
                .WithPrimaryPrivateIPAddressDynamic()
                .WithExistingPrimaryPublicIPAddress(publicIP)
                .WithExistingNetworkSecurityGroup(nsg)
                .Create();
            
            // Create the virtual machine
            Console.WriteLine($"Creating virtual machine {vmName} ...");
            azure.VirtualMachines.Define(vmName)
                .WithRegion(location)
                .WithExistingResourceGroup(groupName)
                .WithExistingPrimaryNetworkInterface(nic)
                .WithLatestWindowsImage("MicrosoftWindowsServer", "WindowsServer", "2019-Datacenter")
                .WithAdminUsername(adminUser)
                .WithAdminPassword(adminPassword)
                .WithSize(VirtualMachineSizeTypes.StandardDS2V2)
                .Create();
        }
    }
}
