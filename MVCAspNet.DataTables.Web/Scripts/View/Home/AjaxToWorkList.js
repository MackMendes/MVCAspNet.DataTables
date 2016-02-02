var AjaxToWorkList = {
    Options : {
        scrollY: "300px",
        scrollX: true,
        scrollCollapse: true,
        fixedColumns: {
            leftColumns: 2
        }
    },
    ApplyFilter: function ($tableWithDataTables) {

        $tableWithDataTables.columns().every(function () {
            var that = this;
            $('input[indexcol="' + that[0] + '"]').on('keyup change', function () {
                if (that.search() !== this.value) {
                    that.search(this.value).draw();
                }
            });
        });
    }
};


$(document).ready(function () {
    var $table = $('#tbWorkList');
    $tableWithDataTables = $table.DataTable(BuildDataTables.GetAtributos($table, AjaxToWorkList.Options));

    AjaxToWorkList.ApplyFilter($tableWithDataTables);
});





