# *Aplicação .NET Web Api*
## *Aplicação responsável por simular uma locadora de veículos.*
- *Endpoints da aplicação*
- *Podemos realizar o cadastro de veículos*
- *Cadastras eventos que aconteceram com os veículos*
- *Bucas eventos que ocorreram com os veículos*
- *Buscar carros por Placas,Modelos e Status do veículo*

<img width="948" alt="image" src="https://github.com/joaupaulo/CarCompanies/assets/61383712/607f2818-d2b3-445c-b8d3-4746d8ab70c2">


# *Tecnologias Utilizadas*

- .NET
- MongoDB
- Docker
- Kubernets
- DigitalOcean
- GitActions CI/CD
- Container Registry
- DDD
- Swagger

# Oque foi feito em Devops ?  
 [*Realizada configuração do CI/CD para o DigitalOcean*](https://github.com/joaupaulo/CarCompanies/blob/master/.github/workflows/main.yml)
 *Após ser concluido o workflow a imagem da aplicação é publicada no CONTAINER REGISTRY na DigitalOcean.*

 
 *Ademais, quando públicamos uma imagem dockerizada da aplicação no container registry, ele é automaticamente upada no Kubernets e com isso fazemos o deploy da aplicação no Ambiente, até então só possuimis Production.*

Vejamos uma imagem do Container Registry :

<img width="642" alt="image" src="https://github.com/joaupaulo/CarCompanies/assets/61383712/ba560ae9-5e43-4d17-9a86-723aebdb55d5">

Vejamos uma imagem do Kubernetes Cluster:

<img width="453" alt="image" src="https://github.com/joaupaulo/CarCompanies/assets/61383712/1f68460d-c52a-4834-8d5b-cafd5922d5a5">

Status dos  Workloads : 


<img width="676" alt="image" src="https://github.com/joaupaulo/CarCompanies/assets/61383712/5386b3e5-2be5-4597-b71a-2dc911a9956b">


Pods : 


<img width="588" alt="image" src="https://github.com/joaupaulo/CarCompanies/assets/61383712/9efa4439-ac62-466e-9149-cb1e37f76507">


Services : 


<img width="589" alt="image" src="https://github.com/joaupaulo/CarCompanies/assets/61383712/21cc1d38-e4eb-4f03-a7fa-aca0a6bd3fa7">



Ingresses  :

<img width="814" alt="image" src="https://github.com/joaupaulo/CarCompanies/assets/61383712/8fcedcb4-283b-4d69-9330-05567bbb22d2">



