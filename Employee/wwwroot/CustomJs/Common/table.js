var tbl = {
    BindDataTable: function (table, url, data, columns, orderColumns) {
        if ($.fn.dataTable.isDataTable(table)) table.DataTable().destroy();

        var dataTable = table.DataTable({
            ajax: { url: url, type: 'POST', data: data },
            processing: true,
            serverSide: true,
            //stateSave: true,   
            pageLength: 10,
            lengthMenu: [5, 10, 20, 30, 50, 100],
            stateSaveParams: function (settings, data) {
                data.search.search = "";
            },
            language: {
                search: 'Search : ',
                infoFiltered: '',
                zeroRecords: 'No records found',
                oPaginate: { sNext: 'Next', sPrevious: 'Prev' }
            },
            order: [],
            columnDefs: [{ targets: orderColumns, orderable: false }],
            columns: columns,
            dom: `<"top">ct<"top"ipl><"clear">`,
            drawCallback: function (settings) {
                let wrapper = $(this).closest('.dataTables_wrapper');
                wrapper.find('.dataTables_paginate').toggle(this.api().page.info().pages > 1);
                wrapper.find('.dataTables_length').toggle(settings.fnRecordsTotal() > 0);
                wrapper.find('.dataTables_info').toggle(settings.fnRecordsTotal() > 0);
            }           
        });     
    }

}