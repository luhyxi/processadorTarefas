# Processador de Tarefas - Projeto Final Tec. de Programação DiverseDEV
Projeto final do módulo "Técnicas de Programação I (C#)" do bootcamp DiverseDEV 


## Overview
O projeto tem como objetivo simular o processamento assincrono de "Tarefas", aos quais o processamento de multiplas subtarefas é levado conforme a completude ou incompletude da subtarefa em questão, após todas as subtarefas forem processadas, a tarefa é dada como concluída e é removida do processamento, simulando por exemplo o "download" de um arquivo ou o recebimento de pacotes de internet.

![Projeto Processador de Tarefas - Overview](https://github.com/luhyxi/processadorTarefas/assets/125469882/a8435855-4fd7-47f2-b40a-f94e6f5c7f31)

## Requisitos
Foi utilizado interfaces para a produção do código, no objetivo de aplicar a inversão de controle no projeto, foram implementadas a criação, processamento, cancelamento e listagem de tarefas ativas e inativas.

![Projeto Processador de Tarefas - Requisitos](https://github.com/luhyxi/processadorTarefas/assets/125469882/aedebfa1-3fd5-4219-b8b8-bbd92af9f7da)
![Processador de Tarefas - Plano de execução sugerido](https://github.com/luhyxi/processadorTarefas/assets/125469882/38b16abe-a914-447a-9cf9-c1e767f2bd0e)

## Objetivos Futuros
- Implementar o uso de repositórios SQL
- Documentação e melhoria da readility geral do código
- Implementar uma melhor listagem de tarefas ativas e inativas
- Melhor implementar e abstrair subtarefas
- Implementação de visualização no modelo WebAPI
