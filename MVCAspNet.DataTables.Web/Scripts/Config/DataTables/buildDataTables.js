// ********* Arquivo .js para configuração e atribuição do Plugin DataTables. *********
// Criado por: Charles Mendes de Macedo
/** Como utilizar:
     Basicamente para utilizar é necessário que a Table(<table>) tenha a classe "_tbDataTables". Essa table tem que ter a separação de <thead> <tbody>!
     ** Forma de atribuir o Plugin:
      * Básica: 
          Não adicionando nenhum dos atribudos para sua table, o função (PageDataTables.Load()) vai atribuir o plugin depois que a table já foi preechida.
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
      * Carregamento Ajax somente no Inicio:
          Adicionando na sua tabela (<table>) o atributo "ajaxsource" (WebService do Ajax) com o caminho do WebService, 
          o atributo "destroy" tem que ser "true" e colocar o atribulo processing (opcional).
          Na tag <thead> nos <th> o atributo "column" precisa ser preenchido com o nome dos atributos 
          do json que será resultado do WebService consumido.
              Ex.: <table class="_tbDataTables" ajaxsource="Aluno/ProcessarAoIniciar" destroy="true" processing="true">
                     <thead>
                        <tr>
                            <th column="idCliente">ID Cliente</th>
                            <th column="cliente">Nome do Cliente</th>
                        </tr>
                     </thead>
                     <tbody></tbody>
                   </table>
      * Processamento 100% Ajax:
          Adicionando na tabela (<table>) o atributo "ajaxsource" (WebService do Ajax) com o caminho do WebService, o atributo "serverside" tem que ser 
          colocodo como "true". Na tag <thead> nos <th> o atributo "column" precisa ser preenchido com o nome dos atributos do json que será resultado do 
          WebService consumido.
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
       Como descrito acima, para utilizar esse .js é necessário colocar atributos na tabela (<table>), tentei pegar as principais features do DataTables 
       Jquery na função de carregar (PageDataTables.Load()), mas nada impede de implementar mais no decorrer de novas necessidade!
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
          - visible: Se a coluna que possui esse atributo vai ser visível ou não (boolean)
*/

// ************************************************* //
// ** Objecto para armazenar as functions criadas ** //
var PageDataTables = {};

// Carregar o DataTable após carregar tudo
// Author: Charles Mendes de Macedo
$(document).ready(function () { PageDataTables.Load() });

// Função que Carrega o DataTables em todos as table's que tiver com o classe _tbDataTables
// Author: Charles Mendes de Macedo
PageDataTables.Load = function () {
    var tables = $('._tbDataTables');
    for (var i = 0; i < tables.length; i++) {
        var table = $(tables[i]);
        table.dataTable(PageDataTables.GetAtributos(table));
    }
}

// Pegar todos os atributos da table para montar Json que o plugin (DataTables) utiliza
// Author: Charles Mendes de Macedo
PageDataTables.GetAtributos = function (objTable) {
    var atributos = {};
    //**********************************************************************************///
    /* Iniciando os atributos (Default) */
    atributos.aoColumns = [];                                           // Colunas
    atributos.sAjaxSource = null;                                       // WebService do Ajax
    atributos.bServerSide = false;                                      // Processamento no servidor? (boolean)
    atributos.sPaginationType = "full_numbers";                         // Tipo de Paginação
    atributos.aLengthRows = 10;                                         // Quantidade de Linhas ao carregar
    atributos.aLengthMenu = [[10, 25, 50, -1], [10, 25, 50, "Todos"]];  // Menu para mostrar quantidade de registros por página para o usuário
    atributos.oLanguage = getInternationalisation();                    // Tradução para pt-BR
    atributos.jQueryUI = false;                                         // Utiliza o estilo (style) do JqueryUI? (boolean)
    atributos.bFilter = true;                                           // Se vai ter o filtro do cabeçalho ou não (boolean)
    atributos.bLengthChange = true;                                     // Se vai ter o menu que contém quantidade de linhas por página  (boolean)
    atributos.sServerMethod = enumMethodServer.get;                     // Método do Servidor (Web Servicer) será GET ou POST
    atributos.bDestroy = false;                                         // Se vai ser Destruído TUDO ao carregar (boolean)
    atributos.bProcessing = false;                                      // Se irá mostrar o Show Processando (boolean)

    //**********************************************************************************//
    /* Pegando os atributos no Objeto Table (jquery) passado pelo parametro de entrada */

    // Pegando o nome das atributos do Json para cada coluna na ordem colocado no thead ///
    var tableTh = objTable.children('thead').children('tr').children('th');
    if (tableTh !== undefined) {
        for (var i = 0; i < tableTh.length; i++) {
            var attrColumn = {};
            var jquerTH = $(tableTh[i]);

            attrColumn.bSortable = (jquerTH.attr('sortable') !== 'false');
            attrColumn.bVisible = (jquerTH.attr('visible') !== 'false');
            var dtColumn = jquerTH.attr('column');
            if (dtColumn !== undefined) {
                attrColumn.mData = dtColumn.toString();
            }

            atributos.aoColumns.push(attrColumn);
        }
    }

    // Pegando o caminho do Ajax 
    var ajaxSource = objTable.attr('ajaxSource');
    if (ajaxSource !== undefined)
        atributos.sAjaxSource = ajaxSource.toString();

    // Pegando se o processamento será no servidor
    atributos.bServerSide = objTable.attr('serverSide') === 'true';

    // Pegando se será utilizado o estilo do JqueryUI
    atributos.jQueryUI = objTable.attr('jqueryUI') === 'true';

    // Pegando se vai ter o filtro do cabeçalho
    atributos.bFilter = objTable.attr('filter') !== 'false';

    // Pegando se vai poder alterar a quantidade de linhas por página
    atributos.bLengthChange = objTable.attr('lengthChange') !== 'false';

    // Tipo do Método no Servidor (POST ou GET)
    var serverMethod = objTable.attr('serverMethod');
    if (serverMethod !== undefined)
        atributos.sServerMethod = (serverMethod.toString() === enumMethodServer.post ? enumMethodServer.post : enumMethodServer.get);

    // Se vai ser Destruído TUDO ao carregar
    atributos.bDestroy = objTable.attr('destroy') === 'true';

    // Se vai ter o processamento ao carregar
    var processing = objTable.attr('processing');
    if (processing !== undefined)
        atributos.bProcessing = (processing === 'true');


    return atributos;
}

// Pegar a internalização do DataTables em Português (pt-BR)
// Author: Charles Mendes de Macedo
function getInternationalisation() {
    // Tradução do DataTables
    if (PageDataTables.ConfigLanguage === undefined) {
        PageDataTables.ConfigLanguage = {
            "oAria": {
                "sSortAscending": ": ativar para classificar coluna ascendente",
                "sSortDescending": ": ativar para classificar coluna descendente"
            },
            "oPaginate": {
                "sFirst": "Primeiro",
                "sLast": "&Uacute;ltimo",
                "sNext": "Pr&oacute;ximo",
                "sPrevious": "Anterior"
            },
            "sEmptyTable": "N&atilde;o h&aacute; dados dispon&iacute;veis na tabela",
            "sInfo": "Exibindo de _START_ a _END_ de _TOTAL_ registros",
            "sInfoEmpty": "Exibindo 0 a 0 de 0 registros",
            "sInfoFiltered": "(filtrado a partir de _MAX_ registros)",
            "sInfoPostFix": "",
            "sInfoThousands": ",",
            "sLengthMenu": "Mostrar _MENU_",
            "sLoadingRecords": "Carregando...",
            "sProcessing": "Processando...",
            "sSearch": "Pesquisar:",
            "sZeroRecords": "Nenhum registro encontrado com o filtro utilizado",
            /**
             * All of the language information can be stored in a file on the
             * server-side, which DataTables will look up if this parameter is passed.
             * It must store the URL of the language file, which is in a JSON format,
             * and the object has the same properties as the oLanguage object in the
             * initialiser object (i.e. the above parameters). Please refer to one of
             * the example language files to see how this works in action.
             *  @type string
             *  @default <i>Empty string - i.e. disabled</i>
             *  @dtopt Language
             * 
             *  @example
             *    $(document).ready( function() {
             *      $('#example').dataTable( {
             *        "oLanguage": {
             *          "sUrl": "http://www.sprymedia.co.uk/dataTables/lang.txt"
             *        }
             *      } );
             *    } );
             */
            "sUrl": ""
        };
    }

    return PageDataTables.ConfigLanguage;
}

// Enum do tipo de Método no sevidor
// Author: Charles Mendes de Macedo
var enumMethodServer = { get: "GET", post: "POST" };
