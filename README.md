# Passwords Backend 🔐

API Zero-Knowledge para geração e armazenamento seguro de senhas

![.NET](https://img.shields.io/badge/.NET-9.0-blue)
![EF Core](https://img.shields.io/badge/EF%20Core-7.0-green)
![MySQL](https://img.shields.io/badge/Database-MySQL-orange)
![JWT](https://img.shields.io/badge/Auth-JWT-yellow)
![License](https://img.shields.io/badge/License-MIT-lightgrey)

---

## 📌 Sobre o Projeto

Este é um backend **zero-knowledge** para armazenamento seguro de senhas.  
O servidor **não conhece o conteúdo real das senhas**: tudo é criptografado no cliente usando uma senha-mestra informada pelo usuário.

A API fornece:

- ✔ Cadastro e autenticação de usuários
- ✔ Login com JWT
- ✔ Solicitação de senha-mestra para embaralhar/desembaralhar dados
- ✔ Armazenamento de senhas criptografadas
- ✔ Rotas protegidas por Token Bearer
- ✔ Documentação automática com Swagger (Swashbuckle)

Ideal para uso com apps ou front-ends que precisam manipular senhas com máxima segurança.

---

## 🧱 Tecnologias Utilizadas

- **.NET 9**
- **ASP.NET Core Web API**
- **Entity Framework Core 7**
- **MySQL (Pomelo)**
- **JWT (Microsoft.IdentityModel.Tokens)**
- **Swashbuckle / OpenAPI**
- **User Secrets para chaves e segredos**

# 1. Clone o repositório

```bash
git clone https://github.com/Emerson2342/passwords_backend.git
```

# 2. Entre na pasta do projeto

```bash
cd passwords_backend
```

# 3. Restaure os pacotes

```bash
dotnet restore
```

# 4. Configure suas credenciais via User Secrets (somente primeira vez)

```bash
dotnet user-secrets set "PrivateKey" "SUA_PRIVATE_KEY"
dotnet user-secrets set "EncryptionKey" "SUA_ENCRYPTION_KEY"
dotnet user-secrets set "ConnectionStrings:AppDbConnectionsString" "server=localhost;Port=3306;database=passwords;User=root;Password=XXXXXX"
```

# 5. (Opcional) Crie ou atualize o banco de dados

```bash
dotnet ef migrations add InitialCreate
dotnet ef database update
```

# 6. Execute o backend em modo desenvolvimento

```bash
dotnet run

```
