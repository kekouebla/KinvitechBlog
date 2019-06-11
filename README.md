# Introduction 
Externalize .NET Framework version 4.6.2 application configuration with Spring Cloud Config Server and GitHub repository.

# Getting Started

### Installing docker:
Install docker community edition for your Microsoft Window supported platform at https://docs.docker.com/install/.

Verify the version of the  installation success by running at your command prompt the command: 
- docker --version

### Configuring Spring Cloud Config Server docker image:
- Run a docker container named spring-cloud-config-server using the latest version of hyness/spring-cloud-config-server image at   https://hub.docker.com/r/hyness/spring-cloud-config-server/ with the following environment variables and port.
  - Set environment variables
    - SPRING_CLOUD_CONFIG_SERVER_GIT_URI
    - SPRING_PROFILES_ACTIVE
  - Publish or expose port
    - Bind port 8888 (host machine port:8888) of the container to port 8888 of the host (8888:container port)
      
- Run the following command at your command prompt (in one line) to pull down the docker image:
  - docker run -it --name=spring-cloud-config-server -p 8888:8888 -e SPRING_CLOUD_CONFIG_SERVER_GIT_URI=https://github.com/kekouebla/KinvitechConfig -e SPRING_PROFILES_ACTIVE=dev hyness/spring-cloud-config-server

- Run the following command at your command prompt to inspect the container for the environment variables and port:
  - docker ps -a (to view a list of all containers).
  - docker inspect CONTAINER_ID (where CONTAINER_ID is the ID of the spring cloud config server container image)
  
- Run the following command at your command prompt to a JSON response of the configuration file content if you have curl installed:
  - curl http://localhost:8888/appdotnet462/dev


### Software dependencies
   - Microsoft.Extensions.Hosting, version 2.2.0
   - Microsoft.Extensions.DependencyInjection, version 2.2.0
   - Microsoft.Extensions.Configuration.Json, version 2.2.0
   - Steeltoe.Extensions.Configuration.ConfigServerBase, version 2.2.0

### Interfacing Spring Cloud Config Server docker with .NET App
The communication between the Spring Cloud Config Server and the .NET App is made possible by the Steeltoe client library, the            spring configuration in the appsettings.json file and the configuration file from GitHub repository at https://github.com/kekouebla/KinvitechConfig.  The Spring Cloud Config Server located at http://localhost:8888 reads the configuration file from GitHub repository https://github.com/kekouebla/KinvitechConfig.  The configuration file name must much the name of the application (i.e. appdotnet462) without the profile (i.e. dev) so that the configuration provider, through the Steeltoe configuration provider, reads the right configuration settings.  You can confirm this important setting by taking a look at the name of the application and the active profiles in the appsettings.json and the configuration file in the https://github.com/kekouebla/KinvitechConfig.  
For more information on Spring Cloud Config settings please visit https://cloud.spring.io/spring-cloud-config/single/spring-cloud-config.html.

### Environment Variables With Docker
Run the following two docker commands to destroy the Spring Cloud Config container and re-run the process with or without environment changes:
- docker rm CONTAINER_ID (where CONTAINER_ID is the ID of the Spring Cloud Config Server container)
- docker images (to get a list of all images)
- docker rmi IMAGE_ID (where IMAGE_ID is the ID of the Spring Cloud Config Server image)

