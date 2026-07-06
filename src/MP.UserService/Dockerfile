# 1. 拿一个最干净的、只带底层的运行时依赖镜像（连 .NET 运行时和 SDK 都没有，极小）
FROM mcr.microsoft.com/dotnet/runtime-deps:10.0 AS final
WORKDIR /app

# 2. 纯粹的物理拷贝：把刚才在外面（GitHub 宿主机上）已经用高级编译器砸出来的原生可执行文件，直接拷进镜像里
COPY ./publish/ .

# 3. 告诉 Docker 启动时直接运行它
ENTRYPOINT ["./MP.UserService"]