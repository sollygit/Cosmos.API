param location string = resourceGroup().location
param namePrefix string = 'az'
param accountType string = 'Standard_LRS'
param kind string = 'StorageV2'
param accessTier string = 'Cool'
param supportsHttpsTrafficOnly bool = true
param allowBlobPublicAccess bool = true
param isShareSoftDeleteEnabled bool = false
param minimumTlsVersion string = 'TLS1_2'
param allowSharedKeyAccess bool = true
param defaultOAuth bool = false
param publicNetworkAccess string = 'Enabled'
param allowCrossTenantReplication bool = false
param networkAclsBypass string = 'AzureServices'
param networkAclsDefaultAction string = 'Allow'
param networkAclsIpRules array = []
param largeFileSharesState string = 'Disabled'
param keySource string = 'Microsoft.Storage'
param encryptionEnabled bool = true
param infrastructureEncryptionEnabled bool = false

module stgDeployment 'storage.bicep' = {
  name: 'stgDeployment'
  params: {
    location: location
    namePrefix: namePrefix
    accountType: accountType
    kind: kind
    accessTier: accessTier
    supportsHttpsTrafficOnly: supportsHttpsTrafficOnly
    allowBlobPublicAccess: allowBlobPublicAccess
    isShareSoftDeleteEnabled: isShareSoftDeleteEnabled
    minimumTlsVersion: minimumTlsVersion
    allowSharedKeyAccess: allowSharedKeyAccess
    defaultOAuth: defaultOAuth
    publicNetworkAccess: publicNetworkAccess
    allowCrossTenantReplication: allowCrossTenantReplication
    networkAclsBypass: networkAclsBypass
    networkAclsDefaultAction: networkAclsDefaultAction
    networkAclsIpRules: networkAclsIpRules
    largeFileSharesState: largeFileSharesState
    keySource: keySource
    encryptionEnabled: encryptionEnabled
    infrastructureEncryptionEnabled: infrastructureEncryptionEnabled
  }
} 

output blobUri string = stgDeployment.outputs.blobUri

