﻿$(document).ready(function () {
    $('#customerTable').DataTable({
        "processing": true,     // for show progress bar
        "serverSide": true,     // for process server side
        "filter": true,         // enable/disable search box
        "orderMulti": true,    // enable/disable multiple columns at once


        "ajax": {
            "url": "/Customers/GetData",
            "type": "GET",
            "datatype": "json",
            "async": true,
            "data": function (d) {
                return {
                    draw: d.draw,
                    start: d.start,
                    length: d.length,
                    orderBy: d.columns[d.order[0].column].data,
                    ascending: d.order[0].dir === "asc",
                    search: d.search.value
                };
            },
        },
        "columns": [
            { "data": "companyName", "name": "CompanyName" },
            { "data": "contactName", "name": "ContactName" },
            { "data": "contactTitle", "name": "ContactTitle" },
            { "data": "address", "name": "Address" },
            { "data": "city", "name": "City" },
            { "data": "region", "name": "Region" },
            { "data": "postalCode", "name": "PostalCode" },
            { "data": "country", "name": "Country" },
            { "data": "phone", "name": "Phone" },
            { "data": "fax", "name": "Fax" },
            {
                "data": "customerID",
                "render": function (data, type, row, meta) {
                    return '<a href="/Customers/Details/' + data + '">Details</a> | ' +
                        '<a href="/Customers/Edit/' + data + '">Edit</a> | ' +
                        '<a href="/Customers/Delete/' + data + '">Delete</a>';
                }
            }
        ],
        "serverSide": true,
        "order": [0, "asc"],
        "paging": true,
        "pageLength": 10,
        "lengthMenu": [10, 25, 50, 100],
        "dom": 'BlCfrtip',
        "buttons": [
            {
                extend: 'excelHtml5',					// Habilitar el botón de Excel
                filename: 'Listado Excel',
                title: 'Mi Título',
                extension: '.xlsx',
                exportOptions: {
                    columns: ':not(:last-child)'		// Excluir la última columna (botones de acción)
                    //columns: [0, 1, 2, 3, 4, 5, 6]	// Exportar solo las columnas
                },
                customize: function (xlsx) {
                    var sheet = xlsx.xl.worksheets['sheet1.xml'];

                }
            }
            ,
            {
                extend: 'csvHtml5',						// Habilitar el botón de CSV
                exportOptions: {
                    columns: ':not(:last-child)'		// Excluir la última columna (botones de acción)
                }
            },
            {
                extend: 'pdfHtml5',						// Habilitar el botón de PDF
                title: 'Listado PDF',
                orientation: 'landscape',				// 'portrait'
                pageSize: 'A4',							// A3 , A5 , A6 , legal , letter
                exportOptions: {
                    // columns: [0, 2]					// Exportar solo la primera y la tercera columna
                    columns: ':not(:last-child)'
                },
                customize: function (doc) {
                    doc.defaultStyle.fontSize = 9;
                    doc.styles.tableHeader.fontSize = 10;
                    doc.content[1].table.widths =
                        Array(doc.content[1].table.body[0].length + 1).join('*').split('');
                }
            },
            {
                extend: 'copy',							// Habilitar el botón 'copy'
                text: 'Copiar',							// Texto personalizado para el botón 'copy'
                exportOptions: {
                    columns: [0, 1, 2]					// Columnas que se copiarán
                }
            },
            'print'										// Habilitar el botón de impresión

        ],
        "language": {
            "url": "//cdn.datatables.net/plug-ins/1.13.6/i18n/es-AR.json"
        }
    });


    // Funcionalidad para exportar a Excel
    $("#exportExcelBtn").click(function () {
        window.location.href = '/Customers/ExportToExcel';
    });

});
