# MVCAspNet.DataTables
DataTables (Jquery) no MVC Asp.Net.

Função para facilitar a utlização do Plugin DataTables Jquery.

Criado por: Charles Mendes de Macedo <br />

<h2>Como utilizar:</h2>
     Basicamente para utilizar é necessário que a Table(<table>) tenha a classe "_tbDataTables". 
     Essa table tem que ter a separação de <thead> <tbody>.
     ** Forma de atribuir o Plugin:
      * Básica: 
          Não adicionando nenhum dos atribudos para sua table, o função (PageDataTables.Load()) vai 
          atribuir o plugin depois que a table já foi preechida.
            Ex.: 
              <table class="_tbDataTables">
                  <thead>
                      <tr>
                          <th>IdCliente</th>
                          <th>Cliente</th>
                      </tr>
                  </thead>
                  <tbody>
                  </tbody>
              </table>
                
      * Carregamento Ajax somente na Renderização:
          Adicionando na sua tabela (<table>) o atributo "ajaxsource" (WebService do Ajax) com o 
          caminho do WebService, o atributo "destroy" tem que ser "true" e colocar o atribulo 
          processing (opcional). Na tag <thead> nos <th> o atributo "column" precisa ser preenchido 
          com o nome dos atributos do json que será resultado do WebService consumido.
            Ex.: 
              <table class="_tbDataTables" ajaxsource="Aluno/ProcessarAoIniciar" destroy="true" processing="true">
               <thead>
                  <tr>
                      <th column="idCliente">ID Cliente</th>
                      <th column="cliente">Nome do Cliente</th>
                  </tr>
               </thead>
               <tbody></tbody>
             </table>
                   
      * Processamento 100% Ajax:
          Adicionando na tabela (<table>) o atributo "ajaxsource" (WebService do Ajax) com o caminho do 
          WebService, o atributo "serverside" tem que ser colocodo como "true".  
          Na tag <thead> nos <th> o atributo "column" precisa ser preenchido com o nome dos atributos 
          do json que será resultado do WebService consumido.
            Ex.: 
              <table class="_tbDataTables" ajaxsource="Aluno/ProcessarDataTables">
               <thead>
                  <tr>
                      <th column="IdCliente">ID Cliente</th>
                      <th column="Cliente">Nome do Cliente</th>
                  </tr>
               </thead>
               <tbody></tbody>
             </table>
     ** Atributos:
       Como descrito acima, para utilizar esse .js é necessário colocar atributos na tabela (<table>), 
       tentei pegar as principais features do DataTables Jquery na função de carregar (PageDataTables.Load()), 
       mas nada impede de implementar mais no decorrer de novas necessidade!
       Os atributos que a função deste JS vai procura na tabela <table>:
          - ajaxSource: WebService do Ajax
          - serverSide: Processamento no servidor? (boolean)
          - jqueryUI: Utiliza o estilo (style) do JqueryUI? (boolean)
          - filter: Se vai ter o filtro do cabeçalho ou não (boolean)
          - lengthChange: Se vai ter o menu que contém quantidade de linhas por página (boolean)
          - serverMethod: Método do Servidor (Web Servicer) será "GET" ou "POST"
          - destroy: Se vai ser Destruído TUDO ao carregar (boolean)
          - processing: Se irá mostrar o Show Processando (boolean)
    
       Os atributos que a função deste JS vai procura no Head (<thead><tr><th>) da tabela (<table>):
          - column: Nome do atributo que vai vindo no json do WebService consumido.
          - sortable: Se a coluna que possui esse atributo vai ser ordenada ou não (boolean)
          - visible: Se a coluna que possui esse atributo vai ser visível ou não (boolean).
