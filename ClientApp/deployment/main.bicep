param location string = resourceGroup().location
param namePrefix string = 'az'
param accountType string = 'Standard_LRS'
param kind string = 'StorageV2'

module stgDeployment 'storage.bicep' = {
  name: 'stgDeployment'
  params: {
    location: location
    namePrefix: namePrefix
    accountType: accountType
    kind: kind
  }
} 

output blobUri string = stgDeployment.outputs.blobUri
