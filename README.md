<h2>Projeto 1. Desenvolva uma aplicação para controle de filmes, locações
e clientes em uma locadora.</h2>
<h1>Requisitos gerais</h1>
<p> Todas as informações devem ser gravadas em arquivos .csv.
Quando houver mais de uma entidade, salvar cada lista em um
arquivo separado.</p>
<p>  Sempre que o programa abrir, essas informações devem ser
recuperadas.</p>
<p>  Sempre que alguma informação for alterada, essa
informação deve ser gravada no arquivo.</p>

<h2>Informações obrigatórias para cada entidade.</h2>
<p> Um filme deve conter Título, Gênero e unidades disponíveis na
locadora.</p>
<p>  Um cliente deve ter Nome e filmes alugados (os filmes que
atualmente estão com aquele cliente).
<p>  Uma locação deve conter Filme e Cliente.
Validações</p>
<p>  Um filme não pode ser alugado caso não tenha quantidade
disponível na locadora.</p>
<p>  Um cliente não pode alugar mais que 2 filmes.</p>
<h2>Requisitos</h2
 Deve existir um menu onde o usuário poderá:
- Cadastrar um novo filme
- Cadastrar um novo cliente
- Locar filme
- Devolver filme

- Listar filmes
- Opções:
- Listar todos
- Listar disponíveis
- Listar indisponíveis
- Para todas as opções deve ser possível ordenar por
nome ou gênero
- Listar clientes
- Recarregar informações (ler os arquivos novamente)
