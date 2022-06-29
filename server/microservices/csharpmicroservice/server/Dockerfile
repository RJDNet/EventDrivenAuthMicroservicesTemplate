# https://hub.docker.com/_/microsoft-dotnet
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /source

# copy csproj and restore as distinct layers
COPY *.sln .
COPY /*.csproj ./
RUN dotnet restore

# copy everything else and build app
COPY . ./
RUN dotnet publish -c release -o /publish --no-restore
COPY entrypoint.sh /publish

# final stage/image
FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
COPY --from=build /publish ./publish
WORKDIR /app/publish

CMD /bin/bash ./entrypoint.sh
#ENTRYPOINT ["dotnet", "csharpmicroservice.dll"]