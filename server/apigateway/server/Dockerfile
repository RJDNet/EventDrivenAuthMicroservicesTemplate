FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /source

COPY *.sln .
COPY /*.csproj ./
RUN dotnet restore

COPY . ./
RUN dotnet publish -c release -o /publish --no-restore
COPY entrypoint.sh /publish

FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
COPY --from=build /publish ./publish
WORKDIR /app/publish

CMD /bin/bash ./entrypoint.sh
