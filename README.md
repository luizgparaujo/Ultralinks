# Projeto Ultrlinks

Este projeto utiliza o banco de dados PostgreSQL para armazenar informações relacionadas a implementação de um sistema para depósito e transferência de dinheiro entre contas.

## Pré-requisitos

Antes de executar o projeto, certifique-se de que você tem o seguinte instalado:

- [Docker](https://docs.docker.com/get-docker/) (versão 20.10 ou superior)

## Executando o Container do PostgreSQL

Para criar e executar um container do PostgreSQL, siga os passos abaixo:

### 1. Abrir o terminal

Abra o terminal (Prompt de Comando, PowerShell, Terminal no Linux ou Mac).

### 2. Executar o comando Docker

Execute o seguinte comando para criar e iniciar o container:

```bash
docker run --name ultralinks -e POSTGRES_DB=postgres -e POSTGRES_USER=admin -e POSTGRES_PASSWORD=admin -p 5432:5432 -d postgres:13
