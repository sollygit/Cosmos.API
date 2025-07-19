param location string
param namePrefix string
param accountType string
param kind string
param accessTier string
param supportsHttpsTrafficOnly bool
param allowBlobPublicAccess bool
param isShareSoftDeleteEnabled bool
param minimumTlsVersion string
param allowSharedKeyAccess bool
param defaultOAuth bool
param publicNetworkAccess string
param allowCrossTenantReplication bool
param networkAclsBypass string
param networkAclsDefaultAction string
param networkAclsIpRules array
param largeFileSharesState string
param keySource string
param encryptionEnabled bool
param infrastructureEncryptionEnabled bool

resource stg 'Microsoft.Storage/storageAccounts@2023-05-01' = {
  name: '${namePrefix}storageaccountinstance'
  location: location
  kind: kind
  sku: {
    name: accountType
  }
  tags: {}
  properties: {
    minimumTlsVersion: minimumTlsVersion
    supportsHttpsTrafficOnly: supportsHttpsTrafficOnly
    allowBlobPublicAccess: allowBlobPublicAccess
    allowSharedKeyAccess: allowSharedKeyAccess
    defaultToOAuthAuthentication: defaultOAuth
    accessTier: accessTier
    publicNetworkAccess: publicNetworkAccess
    allowCrossTenantReplication: allowCrossTenantReplication
    networkAcls: {
      bypass: networkAclsBypass
      defaultAction: networkAclsDefaultAction
      ipRules: networkAclsIpRules
    }
    largeFileSharesState: largeFileSharesState
    encryption: {
      keySource: keySource
      services: {
        blob: {
          enabled: encryptionEnabled
        }
        file: {
          enabled: encryptionEnabled
        }
        table: {
          enabled: encryptionEnabled
        }
        queue: {
          enabled: encryptionEnabled
        }
      }
      requireInfrastructureEncryption: infrastructureEncryptionEnabled
    }
  }
  dependsOn: []
}

resource stg_default 'Microsoft.Storage/storageAccounts/fileservices@2023-05-01' = {
  parent: stg
  name: 'default'
  properties: {
    protocolSettings: null
    shareDeleteRetentionPolicy: {
      enabled: isShareSoftDeleteEnabled
    }
  }
}

resource blobContainer 'Microsoft.Storage/storageAccounts/blobServices/containers@2021-04-01' = {
  name: '${stg.name}/default/blob-store'
  properties: {
    publicAccess: 'Blob'
  }
}

output blobUri string = stg.properties.primaryEndpoints.blob
