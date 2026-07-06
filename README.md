# MP / Mid Platform

游戏客户端 → 直接请求 中台 MP.UserService 进行登录，拿到 JWT 后，再带着 JWT 去访问各个 Game MicroService

# 迁移

cd src\MP.UserService
dotnet ef migrations add Init
dotnet ef database update

# 编译

## 本地

docker desktop / settings / Docker Engine / 中，添加：
```
  "registry-mirrors": [
    "https://docker.m.daocloud.io"
  ]
```

Visual Studio CLI
```
cd MP
docker build -f src/MP.UserService/Dockerfile -t mp-user-service .
```

## CI / Actions

Actions -> General -> Workflow permissions ✅


# 测试

## 注册
curl -X POST http://localhost:5001/api/users/register \
  -H "Content-Type: application/json" \
  -d '{"username":"testuser","email":"test@example.com","password":"123456"}'

## 登录拿 token
curl -X POST http://localhost:5001/api/users/login \
  -H "Content-Type: application/json" \
  -d '{"email":"test@example.com","password":"123456"}'

## 用 token 查自己
curl http://localhost:5001/api/users/me \
  -H "Authorization: Bearer <上一步拿到的token>"