# Introduction 
Externalize .NET Framework version 4.6.2 application session with Redis.

# Getting Started

### Installing Redis:
Install Redis for your Microsoft Window supported platform at https://chocolatey.org/packages/redis-64.

### Starting Redis:
- Run the following command at your command prompt to start the Redis server locally:
  - redis-server

### Software dependencies
   - Microsoft.Extensions.Hosting, version 2.2.0
   - Microsoft.Extensions.DependencyInjection, version 2.2.0
   - Microsoft.Extensions.Configuration.Json, version 2.2.0
   - Steeltoe.CloudFoundry.ConnectorCore, version 2.2.0
   - StackExchange.Redis, version 2.0.601