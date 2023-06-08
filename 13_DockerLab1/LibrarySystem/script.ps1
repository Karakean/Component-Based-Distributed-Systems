docker network create -d bridge my-bridge

docker run -d -p 5672:5672 -p 15672:15672 --network my-bridge --name rabbit rabbitmq:management

sleep(20)
docker run -d -it --name notificationservice --network my-bridge -p 92:80 mcr.microsoft.com/dotnet/core/sdk:2.2
docker cp Library.NotificationService2/. notificationservice:/app
docker exec -d notificationservice /bin/bash -c "cd /app && dotnet restore && dotnet publish -c Release -o out && cd out && dotnet Library.NotificationService2.dll"
docker commit notificationservice notificationserviceimg
docker rm -f notificationservice
docker run -d --name notificationservice --network my-bridge -p 92:80 notificationserviceimg bash -c "cd /app/out && dotnet Library.NotificationService2.dll"

docker build -t librarywebapi Library.WebApi/
docker run -p 91:80 -e LibraryWebApiServiceHost=http://localhost:91 --network my-bridge --name api librarywebapi



