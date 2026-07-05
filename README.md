# MP / Mid Platform

# 注册
curl -X POST http://localhost:5001/api/users/register \
  -H "Content-Type: application/json" \
  -d '{"username":"testuser","email":"test@example.com","password":"123456"}'

# 登录拿 token
curl -X POST http://localhost:5001/api/users/login \
  -H "Content-Type: application/json" \
  -d '{"email":"test@example.com","password":"123456"}'

# 用 token 查自己
curl http://localhost:5001/api/users/me \
  -H "Authorization: Bearer <上一步拿到的token>"