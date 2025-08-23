param location string
param namePrefix string
param accountType string
param kind string

resource stg 'Microsoft.Storage/storageAccounts@2023-05-01' = {
  name: '${namePrefix}storageaccountinstance'
  location: location
  kind: kind
  sku: {
    name: accountType
  }
  tags: {}
  properties: {
    supportsHttpsTrafficOnly: true
    allowBlobPublicAccess: true
    allowSharedKeyAccess: true
    defaultToOAuthAuthentication: false
    allowCrossTenantReplication: false
    accessTier: 'Cool'
    publicNetworkAccess: 'Enabled'
    minimumTlsVersion: 'TLS1_2'
  }
  dependsOn: []
}

resource stg_default 'Microsoft.Storage/storageAccounts/fileservices@2023-05-01' = {
  parent: stg
  name: 'default'
  properties: {
    protocolSettings: null
    shareDeleteRetentionPolicy: {
      enabled: false
    }
  }
}

resource blobContainer 'Microsoft.Storage/storageAccounts/blobServices/containers@2021-04-01' = {
  name: '${stg.name}/default/blob-store'
  properties: {
    publicAccess: 'Blob'
  }
}

resource tableService 'Microsoft.Storage/storageAccounts/tableServices@2023-05-01' = {
  parent: stg
  name: 'default'
}

resource boardTable 'Microsoft.Storage/storageAccounts/tableServices/tables@2023-05-01' = {
  parent: tableService
  name: 'board'
  properties: {}
}

output blobUri string = stg.properties.primaryEndpoints.blob
