# **Projeto DaVinci Energy**

# 👨‍👨‍👧‍👧 Equipe DaVinci

- RM550341 - Allef Santos (2TDSPV)
- RM97836 - Débora Dâmaso Lopes (2TDSPN)
- RM551491 - Cassio Yuji Hirassike Sakai (2TDSPN)
- RM550323 - Paulo Barbosa Neto (2TDSPN)
- RM552314 - Yasmin Araujo Santos Lopes (2TDSPN)

# ✍️ **Descrição Geral da Proposta da DaVinci Energy**

A DaVinci Energy visa ajudar residências e pequenos comércios a monitorar, controlar e otimizar o consumo de energia elétrica, com base na tabela de eficiência energética do Inmetro. Utilizando dispositivos de medição e inteligência de dados, a solução oferece dados importantes sobre o consumo de cada aparelho, auxilia na escolha de dispositivos mais eficientes e orienta o usuário a reduzir desperdícios, promovendo um uso consciente, econômico e sustentável de energia.

🎯 Nossa premissa é: 
- Controlar a vida útil dos aparelhos por meio da análise de eficiência energética, permitindo que o usuário saiba quando deve substituir um aparelho ou se há necessidade de troca devido a ineficiências.
- Detectar possíveis defeitos em aparelhos que estejam consumindo mais energia do que o esperado.
- Validar a veracidade das classificações de eficiência energética fornecidas por órgãos como o Inmetro.

📲 O usuário poderá cadastrar dispositivos e equipamentos através do nosso aplicativo Mobile. Com base nos dados recebidos, a solução calcula o consumo de energia elétrica de cada dispositivo, compara os valores com os padrões fornecidos pelo fabricante e por órgãos reguladores, e fornece informações úteis para os usuários.
Por exemplo, um usuário pode acompanhar em um aplicativo móvel informações como:
- Energia consumida (em watts) e estimativa de consumo mensal.
- Comparação do consumo real com o informado pelo fabricante e o Inmetro.
- Alertas para possíveis defeitos ou uso excessivo.

# 🖥️ **Projeto .NET: API para Monitoramento e Eficiência Energética**


## **Índice**

- [Visão Geral da API](#visão-geral-da-api)
- [Arquitetura e Design](#arquitetura-e-design)
- [Funcionalidades](#funcionalidades)
- [Tecnologias e Ferramentas](#tecnologias-e-ferramentas)
- [Configuração](#configuração)
- [Testes](#testes)
- [Documentação das APIs](#documentação-das-apis)
- [Implementação de IA Generativa](#implementação-de-ia-generativa)

---

## 📝 **Visão Geral da API**

A **API para Monitoramento e Eficiência Energética** foi desenvolvida em .NET Core com objetivo de colaborar na melhoria dos processos de energia sustentável. A API oferece funcionalidades para monitorar e reportar sobre o consumo de energia elétrica em residências e pequenos comércios. 



## 🏛️**Arquitetura e Design**

A API foi projetada para seguir os princípios de **modularidade**, **escalabilidade** e **separação de responsabilidades**:

- **Repository Pattern**: A interação com o banco de dados é feita através do padrão Repository, que desacopla a lógica de negócios da persistência de dados.
- **Dependency Injection**: A injeção de dependências é utilizada para promover a reutilização e facilitar os testes automatizados.

## 🎯**Funcionalidades**

- **Cadastro e Gestão de Dispositivos e Medidores de energia**: Permite o cadastro, atualização e remoção de dispositivos com informações sobre seu consumo de energia.
- **Relatórios de consumo por localização (medidor)**: verificar eficiência energética com base no consumo dos dispositivos, para facilitar a identificação de dispositivos com consumo anômalo.
- **IA Generativa**: Implementação de uma IA capaz de responder perguntas sobre energia.


## 🚀**Tecnologias e Ferramentas**

- **.NET Core**: Framework principal utilizado para desenvolver a API.
- **MongoDB**: Banco de dados utilizado para armazenar os dados de dispositivos e consumo energético.
- **Swagger**: Para documentação da API.
- **xUnit** e **Moq**: Para testes unitários e integração.
- **IA Generativa**: Utilizada para fornecer recomendações sobre como melhorar a eficiência energética.

## **Configuração**

1. Clone o repositório:
   ```bash
   git clone https://github.com/debsdamaso/Energy-API.git
   ```

2. Configure as variáveis de ambiente no `appsettings.json`:
   - **MongoDB Connection**: Configure a string de conexão do MongoDB.
   - **HuggingFace API Key**: Para a integração de IA Generativa.

3. Execute a aplicação:
   ```bash
   dotnet run
   ```

## ✅ **Testes**

O processo de testes do projeto incluiu testes automatizados para garantir a qualidade, cobertura verificar requisições e confiabilidade do sistema.

### **Testes Unitários**

Os testes unitários foram realizados usando **xUnit** e **Moq** para simular as dependências e garantir que os métodos de cada camada da aplicação funcionem como esperado.

- **Testes de Controller**: 
  Testamos os endpoints da API, verificando se a resposta está correta e se o status HTTP é adequado. Utilizamos o **Moq** para simular chamadas aos serviços e garantir que o controlador está corretamente interagindo com as camadas inferiores.

- **Testes de Service**:
  Garantimos que a lógica de negócios esteja correta, testando os métodos dos serviços. Usamos **Moq** para simular as interações com os repositórios e outras dependências externas.

### **Testes de Integração**

Os testes de integração garantem que as camadas da aplicação (Controller, Service, Repository) funcionem corretamente quando integradas com o banco de dados.

- **Testes de API**:
  Testamos os endpoints da API para garantir que a aplicação esteja se comportando corretamente em um ambiente real.
  
- **Testes de Repositório com MongoDB Local**:
  Utilizamos uma **instância local do MongoDB** para testar as operações de leitura e escrita diretamente no banco. Isso garante que as operações de CRUD estão funcionando como esperado.

---

## 📚**Documentação das APIs**

A documentação das APIs é gerada automaticamente utilizando o **Swagger**.

1. Execute a aplicação.
2. Navegue para `https://localhost:7010/swagger/index.html` 

## **Endpoints da API**

A API oferece os seguintes **endpoints** para o gerenciamento de dispositivos, medidores e análise de eficiência energética:

### **AI (Inteligencia Artificial)**
- **POST** `/api/ai/generate-text`
  - Gera uma resposta com base no prompt enviado.

### **Device (Dispositivos)**
- **GET** `/api/Device`
  - Retorna a lista de todos os dispositivos registrados.
- **POST** `/api/Device`
  - Cria um novo dispositivo.
- **GET** `/api/Device/{id}`
  - Retorna os detalhes de um dispositivo específico pelo ID.
- **PUT** `/api/Device/{id}`
  - Atualiza um dispositivo existente pelo ID.
- **DELETE** `/api/Device/{id}`
  - Remove um dispositivo pelo ID.

### **Efficiency (Eficiencia)**
- **POST** `/api/efficiency/classify`
  - Classifica um dispositivo em relação à sua eficiência energética com base nos parâmetros fornecidos.

### **Meter (Medidores)**
- **GET** `/api/meters`
  - Retorna a lista de todos os medidores registrados.
- **POST** `/api/meters`
  - Cria um novo medidor.
- **GET** `/api/meters/{id}`
  - Retorna os detalhes de um medidor específico pelo ID.
- **PUT** `/api/meters/{id}`
  - Atualiza um medidor existente pelo ID.
- **DELETE** `/api/meters/{id}`
  - Remove um medidor pelo ID.

### **Report (Relatório)**
- **GET** `/api/Report/consumption-by-location`
  - Retorna um relatório de consumo por localização.
- **GET** `/api/Report/inefficient-devices`
  - Retorna um relatório de dispositivos ineficientes.
- **GET** `/api/Report/anomalous-consumption`
  - Retorna um relatório de consumo anômalo.



## 💡**Implementação de IA Generativa**

A IA generativa foi integrada através da API do **HuggingFace**. A funcionalidade permite que a aplicação responda perguntas, como por exemplo, sobre recomendações sobre como reduzir o consumo de energia. 


1. Para utilizar a IA, envie um **prompt** através do endpoint `/api/ai/generate-text`.

**Exemplo de Requisição**:
```
{
  "prompt": "Como são feitas as classificações energéticas do Inmetro?"
}
```

**Resposta Obtida**:
```json
{
  "text": "Como são feitas as classificações energéticas do Inmetro? O Inmetro utiliza como referência para classificar o nível de eficiência energética dos aparelhos as normas do Programa Brasileiro"
}
```