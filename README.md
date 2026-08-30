# Auto-repair

Sistema projetado para a gestão da oficina mecânica Auto Repair, com o objetivo de otimizar o fluxo operacional, resolver problemas de priorização de atendimentos, controlar estoques/insumos e gerenciar orçamentos e Ordens de Serviço (OS).

## Escopo do sistema
O projeto visa solucionar os principais gargalos operacionais, por meio de funcionalidades que iram apoiar nos seguintes tópicos:

- Priorização de Atendimentos: Organização e fila de veículos com base na capacidade produtiva;
- Gestão de Ordens de Serviço (OS): Acompanhamento completo do ciclo de vida das OS (Recebida, Em diagnóstico, Aguardando Aprovação, Em Execução, Finalizado, Entregue);
- Orçamentos e Autorizações: Geração autómatica dos orçamentos após o diagnóstico e aprovação direta de clientes;
- Controle de Insumos e Estoque: Baixa automática e controle de peças utilizadas nos serviços;
- Registro de clientes/veículos: Registro de veículo, cliente e serviços executados;

## Tecnologias Utilizadas
- .NET 10
- Entity Framework Core 10
- SQL Server 2022 (latest)
- Coverlet Collector 10
- Report Generator 5.5.11
- Mediatr

## Documentação
A concepção da solução seguiu a abordagem de Domain-Driven Design (DDD) para mapeamento do domínio e definição dos contextos delimitados (Bounded Contexts).

### Domain Storytelling
A oficina enfrentava desorganização no pátio, perda de prazos por falta de visibilidade da fila de trabalho e inconsistência no controle de peças utilizadas. A solução foi desenhada para criar rastreabilidade total desde a recepção do veículo até a entrega das chaves.

Os diagramas do Domain Storytelling, feitos no [egon.io](https://egon.io/app/), estão contindos na pasta `docs/storytelling` na raiz da projeto.

### Event Storming
O mapeamento do domínio foi realizado identificando os principais Domain Events, Commands e Aggregates.

Para acessar o diagrama do Event Storming basta clicar no link a baixo.

https://miro.com/app/board/uXjVH8ybQhI=/?share_link_id=708098463167

### C4
A estrutura do sistema está documentada utilizando a notação C4 Model:
- Contexto: Visão geral da interação entre Clientes, Funcionários e o Sistema da Oficina.
- Contêineres: Aplicação API .NET 10, Banco de Dados SQL Server e serviços auxiliares.
- Componentes: Divisão interna.

Para acessar o diagrama do Modelo C4 basta clicar no link a baixo.

https://miro.com/app/board/uXjVH5f85_o=/?share_link_id=444920103157

## Executando o projeto

### Via Docker
Na raiz do projeto existe a pasta `docker/` contendo o `docker-compose.yml` e o `DockerFile`. 

* Pré-requisitos
  * Docker Engine e Docker Compose instalados.
* Execução
  Para executar o `docker-coimpose.yml` basta abrir um terminal de comando na pasta raiz e executar o comando `docker compose -f .\TC1.RepairShop\docker\docker-compose.yml up`.

### Sem docker
Garanta que:
- SDK do .NET 10 esteja instalado corretamente;
- A string de conexão com o SQL Server 2022 correta esteja no `appsettings.json`;

Caso todos os tópicos estejam configurados corretamente, basta executar via sua IDE favorita ou pelo comando `dotnet run --project /src/TC1.RepairShop.Api`

## Relatório de Cobertura de Código

* Pré-requisitos
  * SDK do .NET 10 esteja instalado corretamente;

Há um ferramenta para facilitar a geração do relatório de cobertura de código, automatizando as etapas para gerar um relatório pelo Report Generator.

Para gerar o relatório basta executar o seguinte comando na raiz do projeto `dotnet run --project ./CoverageRunner`.
