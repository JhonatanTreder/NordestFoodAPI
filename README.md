# NordesteFoodAPI

API back-end para a rede de lanchonetes **Raízes do Nordeste**, construída para integrar múltiplos canais de venda (aplicativo, totem de autoatendimento, balcão e plataforma web) em uma operação multiunidade, com controle de estoque por unidade, fluxo de pedidos e pagamento simulado.

Projeto desenvolvido em **.NET 8** seguindo os princípios de **Clean Architecture**, organizado como um monolito modular por módulos de domínio.

---

## Índice

- [Sobre o projeto](#sobre-o-projeto)
- [Tecnologias utilizadas](#tecnologias-utilizadas)
- [Arquitetura](#arquitetura)
- [Estrutura do projeto](#estrutura-do-projeto)
- [Módulos e responsabilidades](#módulos-e-responsabilidades)
- [Perfis de usuário e autorização](#perfis-de-usuário-e-autorização)
- [Fluxo do pedido](#fluxo-do-pedido)
- [Pré-requisitos](#pré-requisitos)
- [Configuração](#configuração)
- [Executando o projeto](#executando-o-projeto)
- [Documentação da API (Swagger)](#documentação-da-api-swagger)
- [Coleção Postman](#coleção-postman)
- [Usuários de teste](#usuários-de-teste)
- [LGPD, privacidade e segurança](#lgpd-privacidade-e-segurança)
- [Itens conceituais / roadmap](#itens-conceituais--roadmap)
- [Autor](#autor)

---

## Sobre o projeto

A rede **Raízes do Nordeste** opera em modelo de franquia: uma matriz responsável pela gestão centralizada e diversas unidades espalhadas pela região. Cada unidade possui estoque próprio e um cardápio local, que pode variar conforme a demanda e a disponibilidade de insumos — os produtos são cadastrados globalmente, mas o preço e a ativação no cardápio são definidos por unidade.

O sistema atende simultaneamente múltiplos canais de venda (aplicativo, totem, balcão e web), registrando a origem de cada pedido (`OrderChannel`) para fins de rastreabilidade e consolidação operacional.

### Objetivos

- Levantar e priorizar requisitos de back-end.
- Modelar o domínio e a base de dados.
- Projetar e documentar uma API REST com contratos claros.
- Implementar o fluxo crítico de pedidos, da criação ao pagamento simulado e atualização de status.
- Garantir controle de estoque por unidade e rastreabilidade multicanal.
- Aplicar práticas de segurança (JWT, hash de senha, autorização por perfis) e conformidade mínima com a LGPD.

---

## Tecnologias utilizadas

| Categoria | Tecnologia |
|---|---|
| Runtime / Framework | .NET 8 / ASP.NET Core |
| Linguagem | C# |
| Arquitetura | Clean Architecture — monolito modular por módulos |
| Banco de dados | SQL Server |
| ORM | Entity Framework Core |
| Autenticação | ASP.NET Core Identity + JWT |
| Hash de senha | BCrypt (via ASP.NET Core Identity) |
| Documentação | Swagger / OpenAPI |
| Testes de API | Coleção Postman |

---

## Arquitetura

O projeto segue **Clean Architecture**, dividido por módulos de domínio (`Auth`, `Restaurants`, `Products`, `UnitProducts`, `Orders`, `Payments`, `Stocks`, `Feedbacks`), além de um projeto compartilhado (`Shared`). Cada módulo é dividido internamente nas seguintes camadas:

- **API** — Controllers responsáveis por receber as requisições HTTP e delegar para os *use cases*, tratando o retorno de status codes e o formato de resposta padronizado (`ApiResponse`).
- **Application** — *Use cases* que orquestram as regras de negócio, coordenando repositórios, serviços e entidades de domínio.
- **Domain** — Entidades, *value objects*, enums, contratos (interfaces de repositórios/serviços) e exceções de domínio. É a camada mais interna e não depende de nenhuma outra.
- **Infrastructure** — Implementações concretas: repositórios com Entity Framework Core, serviços mock (ex.: pagamento), configurações de mapeamento de entidades (`IEntityTypeConfiguration`) e injeção de dependência de cada módulo.

Cada módulo expõe um método de extensão (`Add<Módulo>Module`) que registra seus próprios serviços no `IServiceCollection`, mantendo baixo acoplamento entre módulos.

Padrão de retorno das operações: as camadas de aplicação e infraestrutura utilizam o objeto `Result` / `Result<T>` (em `Shared/Common/Results`) para representar sucesso ou falha de forma explícita, evitando o uso de exceções para controle de fluxo esperado (ex.: "não encontrado", "conflito").

---

## Estrutura do projeto

```
NordesteFoodAPI/
├── Migrations/                     # Migrations do Entity Framework Core
├── Modules/
│   ├── Auth/                       # Login, registro, geração de token JWT
│   ├── Feedbacks/                  # Avaliação de pedidos entregues
│   ├── Orders/                     # Pedidos, itens do pedido e status
│   ├── Payments/                   # Pagamento simulado (mock)
│   ├── Products/                   # Cadastro global de produtos
│   ├── Restaurants/                # Unidades da rede (restaurantes)
│   ├── Stocks/                     # Estoque por unidade/produto
│   └── UnitProducts/               # Vínculo produto ↔ unidade (preço, disponibilidade)
│       └── <Módulo>/
│           ├── API/                # Controllers
│           ├── Application/        # Use Cases
│           ├── Domain/             # Entities, DTOs, Enums, ValueObjects, Contracts, Exceptions
│           └── Infraestructure/    # Repositórios, EF Config, DI
├── Properties/
├── Shared/
│   ├── API/Responses/              # ApiResponse padrão
│   ├── Common/Results/             # Result, Result<T>, ErrorType
│   ├── Domain/Enums/Utils/         # UserRole, RegexPatterns
│   ├── Exceptions/                 # DomainLayerException
│   └── Infraestructure/
│       ├── Identity/                # ApplicationUser, RoleSeeder
│       └── Persistence/             # AppDbContext
├── appsettings.json
├── appsettings.Development.json
├── NordesteFoodAPI.postman_collection.json
├── Program.cs
└── NordesteFoodAPI.sln
```

---

## Módulos e responsabilidades

| Módulo | Responsabilidade |
|---|---|
| **Auth** | Cadastro e autenticação de usuários (`Client`, `Attendant`, `Kitchen`, `Admin`), emissão de token JWT. |
| **Restaurants** | Cadastro e consulta das unidades (restaurantes) da rede. |
| **Products** | Cadastro global de produtos (base para os cardápios de cada unidade). |
| **UnitProducts** | Vínculo entre produto e unidade, definindo preço local, disponibilidade no cardápio (`IsAvailable`) e criação automática do estoque inicial. |
| **Stocks** | Controle de estoque por unidade/produto (incremento e decremento de quantidade). |
| **Orders** | Criação de pedidos, itens, canal de origem (`OrderChannel`) e máquina de estados do status do pedido. |
| **Payments** | Processamento de pagamento simulado (`PaymentServiceMock`), com resultado variando conforme o provedor informado. |
| **Feedbacks** | Registro de avaliação (nota + comentário) para pedidos já entregues. |

---

## Perfis de usuário e autorização

O sistema utiliza políticas de autorização baseadas em roles (`UserRole`):

| Role | Descrição |
|---|---|
| `Client` | Cliente final — cria pedidos, consulta seus próprios pedidos, solicita pagamento e registra feedback. |
| `Attendant` | Atendente — entrega e cancela pedidos. |
| `Kitchen` | Cozinha — inicia o preparo e marca o pedido como pronto. |
| `Admin` | Administrador — gerencia restaurantes, produtos, vínculos de cardápio e estoque. |

Políticas utilizadas nos endpoints: `AdminOnly`, `ClientOnly`, `KitchenOnly`, `AttendantOrAdmin`, `AuthenticatedUsers`.

---

## Fluxo do pedido

```
AguardandoPagamento → PagamentoConfirmado → EmPreparo → Pronto → Entregue
                                                                 ↘
                                                              Cancelado
```

Resumo do fluxo crítico (Pedido → Pagamento → Entrega):

1. O cliente cria o pedido (`POST /Order/create`) informando `restaurantId`, `orderChannel` e a lista de itens.
2. O sistema valida o restaurante, o cardápio da unidade (`UnitProduct`) e a disponibilidade em estoque (`Stock`) de cada item.
3. O pedido é criado com status `AguardandoPagamento`.
4. O cliente solicita o pagamento (`POST /Payment/create`), que é processado pelo `PaymentServiceMock`.
5. Se aprovado: o pedido passa para `PagamentoConfirmado` e o estoque de cada item é decrementado.
6. Se recusado ou falhou: o pedido permanece em `AguardandoPagamento`, permitindo nova tentativa.
7. A cozinha inicia o preparo (`PATCH /Order/start-preparation/{id}` → `EmPreparo`) e depois marca como pronto (`PATCH /Order/mark-ready/{id}` → `Pronto`).
8. O atendente (ou admin) registra a entrega (`PATCH /Order/deliver/{id}` → `Entregue`).
9. A qualquer momento antes da entrega, o pedido pode ser cancelado (`PATCH /Order/cancel/{id}` → `Cancelado`), desde que não esteja já entregue ou cancelado.

Regras de negócio relevantes:
- **Idempotência de pagamento**: não é permitido criar mais de um pagamento para o mesmo pedido (erro `409`).
- **Transições de status inválidas** geram `DomainLayerException` e retornam `409`.
- **Cancelamento** não reverte estoque (a baixa só ocorre após o pagamento ser confirmado).
- **Segurança de acesso**: um cliente só pode consultar seus próprios pedidos (`GET /Order/search/id/{id}` retorna `404` também quando o pedido existe mas não pertence ao usuário, para não vazar informação).

---

## Pré-requisitos

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- SQL Server (local ou instância acessível)
- [Postman](https://www.postman.com/) (para executar a coleção de testes)

---

## Configuração

No arquivo `appsettings.json` (ou `appsettings.Development.json`), configure:

```json
{
  "ConnectionStrings": {
    "LocalConnection": "Server=localhost\\SQLEXPRESS;DataBase=NordesteFoodDb;Integrated Security=True;Persist Security Info=False;Pooling=False;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=True;Trusted_Connection=True;Command Timeout=0"
  },
  "JWT": {
    "SecretKey": "COLOQUE_A_SUA_CHAVE_AQUI_VEJA_O_README",
    "ValidIssuer": "NordesteFoodAPI",
    "ValidAudience": "NordesteFoodAPI"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*"
}
```

Descrição das variáveis:

| Chave | Descrição |
|---|---|
| `ConnectionStrings:LocalConnection` | String de conexão com o SQL Server. O exemplo acima aponta para uma instância local `SQLEXPRESS` com autenticação integrada do Windows. Ajuste `Server` conforme a sua instância. |
| `JWT:SecretKey` | Chave secreta usada para assinar os tokens JWT. **Substitua** `COLOQUE_A_SUA_CHAVE_AQUI_VEJA_O_README` por uma chave própria, longa e aleatória, antes de rodar o projeto. |
| `JWT:ValidIssuer` | Emissor válido do token (`NordesteFoodAPI`). |
| `JWT:ValidAudience` | Audiência válida do token (`NordesteFoodAPI`). |
| `Logging:LogLevel` | Níveis de log padrão da aplicação e do ASP.NET Core. |
| `AllowedHosts` | Hosts permitidos para a aplicação (`*` libera qualquer host). |

> ⚠️ Nunca versione chaves e strings de conexão reais em repositórios públicos. Utilize variáveis de ambiente, *user-secrets* (`dotnet user-secrets`) ou um `appsettings.Development.json` fora do controle de versão para valores sensíveis.

---

## Executando o projeto

```bash
# 1. Restaurar dependências
dotnet restore

# 2. Aplicar as migrations no banco configurado
dotnet ef database update

# 3. Executar a aplicação
dotnet run
```
>**OBS: Será necessário rodar o projeto localmente na porta 7200, caso contrário o projeto vai ter problemas para ser inicializado (principalmente na inicialização do Swagger/OpenAPI)** 

A API ficará disponível em `https://localhost:7200` (conforme `launchSettings.json`).

Ao iniciar, o `RoleSeeder` garante a criação das roles (`Client`, `Attendant`, `Kitchen`, `Admin`) caso ainda não existam no banco.

---

## Documentação da API (Swagger)

Com a aplicação em execução, a documentação interativa completa está disponível em:

```
https://localhost:7200/swagger
```

---

## Coleção Postman

O arquivo `NordesteFoodAPI.postman_collection.json`, na raiz do repositório, contém os cenários de teste organizados em pastas numeradas, na ordem de execução:

1. **`00-Setup`** — prepara o ambiente com dados iniciais e preenche automaticamente as variáveis do ambiente Postman:
   - Registro/login de administrador (`adminToken`);
   - Criação de restaurante (`restaurantId`);
   - Criação de produto (`productId`);
   - Criação de `UnitProduct` (`unitProductId`) e ativação no cardápio;
   - Login de cliente (`token`) e de cozinha (`kitchenToken`).
2. **Pastas `01` a `04`** — cenários de teste do fluxo de pedidos, pagamento, autenticação/autorização e regras de negócio.

Para executar:
1. Importe a coleção no Postman.
2. Configure um *environment* com as variáveis utilizadas pelos scripts (`token`, `adminToken`, `kitchenToken`, `restaurantId`, `productId`, `unitProductId`, `orderId`).
3. Execute as pastas na ordem, começando por `00-Setup`.

---

## Usuários de teste

O registro (`POST /Auth/register`) sempre cria o usuário com a role `Client` por padrão. Para testar os perfis `Admin` e `Kitchen`, é necessário promover manualmente o usuário no banco de dados após o registro.

### 1) Administrador

Registre o usuário:

```json
{
    "username": "admin",
    "email": "admin@exemple.com",
    "password": "Admin@123",
    "phoneNumber": null
}
```

Em seguida, execute **uma única vez** no banco de dados para promover o usuário à role `Admin`:

```sql
UPDATE AspNetUserRoles
SET RoleId = (SELECT Id FROM AspNetRoles WHERE Name = 'Admin')
WHERE UserId = (SELECT Id FROM AspNetUsers WHERE Email = 'admin@exemple.com');
```

> Alternativamente, você pode simplesmente deixar um usuário já com a role `Admin` cadastrado diretamente no banco e pular o registro — nesse caso, atente-se a usar as mesmas credenciais no login.

### 2) Cliente

```json
{
    "username": "cliente",
    "email": "cliente@exemple.com",
    "password": "Senha@123",
    "phoneNumber": null
}
```

O cliente já é criado com a role `Client` automaticamente pelo registro — não é necessário nenhum ajuste manual.

### 3) Cozinha (Kitchen)

Registre o usuário:

```json
{
    "username": "cozinha",
    "email": "cozinha@exemple.com",
    "password": "Cozinha@123",
    "phoneNumber": null
}
```

Em seguida, execute **uma única vez** no banco de dados para promover o usuário à role `Kitchen`:

```sql
UPDATE AspNetUserRoles
SET RoleId = (SELECT Id FROM AspNetRoles WHERE Name = 'Kitchen')
WHERE UserId = (SELECT Id FROM AspNetUsers WHERE Email = 'cozinha@exemple.com');
```

### Login

Todos os perfis utilizam o mesmo endpoint de login (`POST /Auth/login`). Exemplo com o usuário cliente:

```json
{
    "email": "cliente@exemple.com",
    "password": "Senha@123"
}
```

O token JWT retornado já contém a role do usuário e deve ser enviado no header `Authorization: Bearer {token}` nas requisições autenticadas.

---

## LGPD, privacidade e segurança

- **Dados coletados**: nome, e-mail e telefone (opcional), com finalidade de autenticação e contato sobre pedidos.
- **Base legal**: execução de contrato para os pedidos. Para funcionalidades futuras de fidelização, o consentimento explícito do titular é registrado através do campo `LgpdConsentGiven` na entidade `ApplicationUser`.
- **Minimização de dados**: os endpoints retornam apenas os campos necessários; senhas nunca são expostas nas respostas.
- **Segurança**:
  - Hash de senha via ASP.NET Core Identity (BCrypt);
  - Autenticação JWT com tempo de expiração configurável;
  - Autorização por roles/políticas (`AdminOnly`, `ClientOnly`, `KitchenOnly`, `AttendantOrAdmin`);
  - Validação de pertencimento: um usuário só acessa seus próprios pedidos.

---

## Itens conceituais / roadmap

Os itens abaixo foram planejados e discutidos, mas não fazem parte do MVP implementado — priorização feita para concentrar esforço no fluxo crítico de pedido, pagamento, estoque e multicanalidade:

- Programa de fidelização (pontos e resgate).
- Promoções e campanhas (regras de desconto).
- Logs e auditoria de ações sensíveis (ex.: via Serilog).
- Política de retenção/anonimização de dados pessoais.

---

## Autor

**Jhonatan Treder de Oliveira Pereira**
Projeto Multidisciplinar — Trilha Back-end — Centro Universitário Internacional Uninter
