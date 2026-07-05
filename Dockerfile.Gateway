FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["src/MP.Gateway/MP.Gateway.csproj", "src/MP.Gateway/"]
RUN dotnet restore "src/MP.Gateway/MP.Gateway.csproj"

COPY src/MP.Gateway/ src/MP.Gateway/
WORKDIR /src/src/MP.Gateway

RUN dotnet publish -c Release -o /app/publish -p:PublishAot=true

FROM mcr.microsoft.com/dotnet/runtime-deps:10.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

ENV ASPNETCORE_HTTP_PORTS=8080
EXPOSE 8080

ENTRYPOINT ["./MP.Gateway"]
