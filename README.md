A template for apps that benefit from containerisation and event driven microservices architectures.

How to run - Have docker desktop installed on your machine and from the root folder of this project, execute the command "docker compose up" in the terminal. Once everything has built, navigate to http://localhost:3000.

Note: Regarding communication and microservices, there are multiple ways to handle this depending on your usecase. Do they communicate internally? Are any of them public? What data needs to be available to the clients? Do some microservices need to talk to each other? 
So choices need to be made whether it is public and using synchronous http request/response, or asynchronous message based mechanisms using amqp, as examples. The various different techniques used (topics, routing, rpc, direct, fanout, synchonous, asynthcrounous, message broker, pubsub etc) depends on your requiremenets, and often times, multiple of these techniques are used within the same architecture.

Note: Often times an abstraction over a message broker like MassTransit might be used instead of just using RabbitMQ, like in this template, or a service that deals with messaging in the cloud using AWS or Azure for example.

Main benefits of this template:-

Useful for learning
-An example of a potential containerised microservices architecture that can be extended upon or adapted. The general infrastructure is in place.

Supports multiple clients using different technologies
-Allows for multiple types of clients providing they have the ability to utilise cookies (mobile app using the browser for auth, or a SPA or serverside web app in the browser). Uses a reverse proxy to direct requests and serve static content.

Api gateway to handle requests and Authentication/Authorisation
-Includes an api gateway which handles requests from the clients and handles Authentication/Authorisation. Uses ASP.NET Core Identity and Entity Framework Core to scaffold authentication/authorisation and includes common techniques to help defend against all the common attacks. Eg. XSS and CSRF, or the stealing of tokens.

Microservices, Containerisation
-Adopts a microservices architecture using containerisation with Docker, which has advantages over many other architectures such as the ability to develop services separately by dev teams (versioning can help here too), have them de-coupled, and create containers that utilise different technologies based on your projects needs such as different languages/frameworks. Also useful for horizontal scaling of the microservices depending on load and demand. When used with orchestration software such as Kubernetes, allows for health checks and redundency.

Event Driven
-Adopts an event driven model for internal network communication between microservices, allowing for excellent performance and helping with structuring request/messaging flows between internal microservices.   



### How to use ###

# Start Up
-Have Docker Desktop installed on your machine.
-From root folder (the folder with the docker-compose.yml file), run command: docker compose up
-Wait 1 minute for everything to startup (database population and services currently wait a period of time to start).
-Navigate to http://localhost:3000 in your browser. 

# Shut Down
-Ctrl-c the terminal which is running docker compose.
-Remove all images by running command: docker compose down -rmi all 

### Useful Commands ###

# General
cd reverseproxy/clients/reactclient
cd server/apigateway/server
cd server/microservices/csharpmicroservice
cd server/microservices/nodemicroservice
cd server/microservices/pythonmicroservice

yarn start
dotnet run

# Environment
dotnet run --environment Production
dotnet run --launch-profile "SampleApp"

# DB Migrations
dotnet ef migrations add InitialCreate
dotnet ef migrations remove

dotnet ef database update
dotnet ef database drop

# Docker
docker compose up
docker compose down --rmi all
