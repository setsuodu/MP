# build context 是仓库根目录（解决方案根），不是 src/MP.UserService/
# 这样以后 UserService 要引用 MP.Shared 时，COPY 范围不用改
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# 先只拷 csproj 再 restore，能利用 Docker 层缓存：
# 代码改了但依赖没变时，restore 这层不用重跑
COPY ["src/MP.UserService/MP.UserService.csproj", "src/MP.UserService/"]
RUN dotnet restore "src/MP.UserService/MP.UserService.csproj"

COPY src/MP.UserService/ src/MP.UserService/
WORKDIR /src/src/MP.UserService

# PublishAot=true 已经在 csproj 里配置，这里不需要重复传参
# 但显式带上更保险，防止以后有人在别的环境跑这个 Dockerfile 时csproj配置被覆盖
RUN dotnet publish -c Release -o /app/publish -p:PublishAot=true

# ---- 最终阶段：只要 runtime-deps，不要 aspnet ----
# AOT 产物是原生二进制，自带 CLR，不需要托管运行时。
# 用 mcr.microsoft.com/dotnet/aspnet 这一步就白做了 AOT。
FROM mcr.microsoft.com/dotnet/runtime-deps:10.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

ENV ASPNETCORE_HTTP_PORTS=8080
EXPOSE 8080

ENTRYPOINT ["./MP.UserService"]
