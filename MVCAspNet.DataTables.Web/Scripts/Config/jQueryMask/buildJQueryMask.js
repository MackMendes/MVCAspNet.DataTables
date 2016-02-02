// Carregar o DataTable após carregar tudo
// Author: Charles Mendes de Macedo
$(document).ready(function () { BuildJQueryMask.Load() });

var BuildJQueryMask = {
    Load: function () {
        // Carregar todos as Máscaras
        BuildJQueryMask.FactoryJQueryMask();
    },
    FactoryJQueryMask: function () {
        var config = {
            '.mask-date': '00/00/0000',
            '.mask-time': '00:00:00',
            '.mask-date-time': '00/00/0000 00:00:00',
            '.mask-cep': '00000-000',
            '.mask-phone': '0000-00009',
            '.mask-phone-ddd': '(00) 0000-00009',
            '.mask-cpf': ['000.000.000-00', { reverse: true }]
        };

        var configSelectorActual;
        for (var selector in config) {
            configSelectorActual = config[selector];
            if (typeof (configSelectorActual) === "string")
                $(selector).mask(configSelectorActual);
            else
                $(selector).mask(configSelectorActual[0], configSelectorActual[1]);
        }
    }
};